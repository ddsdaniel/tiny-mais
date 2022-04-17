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

        protected virtual void PrevinirConsumoExcessivo() { }

        protected virtual async Task<TRetorno?> PostAsync<TBody, TRetorno>(string rota, TBody body)
           where TRetorno : class
           where TBody : class

        {
            PrevinirConsumoExcessivo();

            var jsonRequest = JsonConvert.SerializeObject(body);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-post+json");

            using HttpResponseMessage response = await _httpClient.PostAsync(rota, content);

            return await ObterRetorno<TRetorno>(response);
        }

        protected virtual async Task<TRetorno?> PatchAsync<TBody, TRetorno>(string rota, TBody body)
            where TRetorno : class
            where TBody : class

        {
            PrevinirConsumoExcessivo();

            var jsonRequest = JsonConvert.SerializeObject(body);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            using HttpResponseMessage response = await _httpClient.PatchAsync(rota, content);

            return await ObterRetorno<TRetorno>(response);
        }

        protected virtual async Task<TViewModel> ObterRetorno<TViewModel>(HttpResponseMessage response)
            where TViewModel : class
        {
            await Criticar(response);

            return Valido
                ? await response.Content.ReadAsAsync<TViewModel>()
                : null;
        }

        protected virtual async Task<TViewModel?> GetAsync<TViewModel>(string rota)
          where TViewModel : class
        {
            PrevinirConsumoExcessivo();

            using var response = await _httpClient.GetAsync(rota);

            return await ObterRetorno<TViewModel>(response);
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
