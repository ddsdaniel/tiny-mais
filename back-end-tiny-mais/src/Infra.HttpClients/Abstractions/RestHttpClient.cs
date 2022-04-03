using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using TinyMais.Domain.Abstractions.Services;

namespace Infra.HttpClients.Abstractions
{
    public abstract class RestHttpClient : Service
    {
        protected readonly HttpClient _httpClient;
        protected readonly ILogger<RestHttpClient> _logger;

        public RestHttpClient(
            HttpClient httpClient,
            ILogger<RestHttpClient> logger
            )
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        protected virtual async Task<TRetorno?> PatchAsync<TBody, TRetorno>(string rota, TBody body)
            where TRetorno : class
            where TBody : class

        {
            var jsonRequest = JsonConvert.SerializeObject(body);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");
            
            using HttpResponseMessage response = await _httpClient.PatchAsync(rota, content);

            await Criticar(response);

            return Valido
                ? await response.Content.ReadAsAsync<TRetorno>()
                : null;
        }

        protected virtual async Task<TViewModel?> GetAsync<TViewModel>(string rota)
          where TViewModel : class
        {
            using var response = await _httpClient.GetAsync(rota);
            await Criticar(response);

            return Valido
                ? await response.Content.ReadAsAsync<TViewModel>()
                : null;
        }

        protected async Task Criticar(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                if (jsonString.StartsWith("{\"erro\":"))
                {
                    _logger.LogError(jsonString);
                    Criticar(jsonString);
                }
            }
            else
            {
                var mensagem = await response.Content.ReadAsStringAsync();
                Criticar(mensagem);
            }
        }
    }
}
