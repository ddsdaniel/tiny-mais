using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using TinyMais.Domain.Abstractions.Models;
using TinyMais.Domain.Abstractions.Services;
using TinyMais.Domain.Models;

namespace TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Abstractions
{
    public abstract class TrachCashHttpClient : Service
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TrachCashHttpClient> _logger;
        protected const string URL_BASE = "https://sistema.trackcash.com.br/api";

        public TrachCashHttpClient(
            HttpClient httpClient,
            IAppSettings appSettings,
            ILogger<TrachCashHttpClient> logger
            )
        {
            _httpClient = httpClient;
            _logger = logger;
            Autenticar(appSettings);
        }

        private void Autenticar(IAppSettings appSettings)
        {
            var credenciaisBytes = Encoding.ASCII.GetBytes($"{appSettings.TrackCash.Credencial.Usuario}:{appSettings.TrackCash.Credencial.Senha}");

            var credenciaisBase64 = Convert.ToBase64String(credenciaisBytes);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credenciaisBase64);
            _httpClient.DefaultRequestHeaders.Add("token", appSettings.TrackCash.ApiToken);
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
