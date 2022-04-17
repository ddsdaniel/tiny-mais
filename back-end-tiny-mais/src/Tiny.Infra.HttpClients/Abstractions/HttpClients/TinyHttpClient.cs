using Infra.HttpClients.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Tiny.Infra.HttpClients.Abstractions.AppServices;
using TinyMais.Domain.Abstractions.Models;

namespace Tiny.Infra.HttpClients.Abstractions.HttpClients
{
    public abstract class TinyHttpClient : RestHttpClient
    {
        protected const string URL_BASE = "https://api.tiny.com.br/api2";
        private readonly IPrevineConsumoExcessivoAppService _previneConsumoExcessivoAppService;

        protected TinyHttpClient(
            HttpClient httpClient,
            ILogger<TinyHttpClient> logger,
            IPrevineConsumoExcessivoAppService previneConsumoExcessivoAppService
            ) : base(httpClient, logger)
        {
            _previneConsumoExcessivoAppService = previneConsumoExcessivoAppService;
        }

        protected override void PrevinirConsumoExcessivo()
        {
            _previneConsumoExcessivoAppService.Previnir();
        }

        protected override async Task<TViewModel> ObterRetorno<TViewModel>(HttpResponseMessage response)
            where TViewModel : class
        {
            if (Invalido) return null;

            var json = await response.Content.ReadAsStringAsync();

            try
            {
                var viewModel = JsonSerializer.Deserialize<TViewModel>(json);

                return viewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Falha ao obter retorno do Tiny: {ex.Message}. {json}");
                throw;
            }

        }
    }
}
