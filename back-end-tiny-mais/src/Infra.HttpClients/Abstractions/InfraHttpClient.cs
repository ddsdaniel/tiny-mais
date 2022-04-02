using Microsoft.Extensions.Logging;
using TinyMais.Domain.Abstractions.Services;

namespace Infra.HttpClients.Abstractions
{
    public abstract class InfraHttpClient : Service
    {
        protected readonly HttpClient _httpClient;
        protected readonly ILogger<InfraHttpClient> _logger;

        public InfraHttpClient(
            HttpClient httpClient,
            ILogger<InfraHttpClient> logger
            )
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        protected async Task<TViewModel?> GetAsync<TViewModel>(string rota)
          where TViewModel : class
        {
            var response = await _httpClient.GetAsync(rota);
            await Criticar(response);

            return Valido
                ? await response.Content.ReadAsAsync<TViewModel>()
                : null;
        }

        private async Task Criticar(HttpResponseMessage response)
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
