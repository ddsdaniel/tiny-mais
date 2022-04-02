using Infra.HttpClients.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Tiny.Infra.HttpClients.Abstractions.HttpClients
{
    public abstract class TinyHttpClient : InfraHttpClient
    {
        protected const string URL_BASE = "https://api.tiny.com.br/api2";

        protected TinyHttpClient(
            HttpClient httpClient,
            ILogger<TinyHttpClient> logger
            ) : base(httpClient, logger)
        {
        }

        protected override async Task<TViewModel?> GetAsync<TViewModel>(string rota)
            where TViewModel : class
        {
            var response = await _httpClient.GetAsync(rota);
            await Criticar(response);

            if (Invalido) return null;

            var json = await response.Content.ReadAsStringAsync();
            
            var viewModel = JsonSerializer.Deserialize<TViewModel>(json);

            return viewModel;
        }
    }
}
