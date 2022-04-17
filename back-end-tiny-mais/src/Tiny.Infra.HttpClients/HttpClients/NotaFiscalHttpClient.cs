using Microsoft.Extensions.Logging;
using Tiny.Infra.HttpClients.Abstractions.AppServices;
using Tiny.Infra.HttpClients.Abstractions.HttpClients;
using Tiny.Infra.HttpClients.DTOs.NotaFiscal;
using TinyMais.Domain.Abstractions.Models;

namespace Tiny.Infra.HttpClients.HttpClients
{
    public class NotaFiscalHttpClient : TinyHttpClient, INotaFiscalHttpClient
    {
        private const string URL_NOTA_FISCAL = "nota.fiscal.obter.php";
        private readonly IAppSettings _appSettings;

        public NotaFiscalHttpClient(
            HttpClient httpClient,
            ILogger<NotaFiscalHttpClient> logger,
            IAppSettings appSettings,
            IPrevineConsumoExcessivoAppService previneConsumoExcessivoAppService
            ) : base(httpClient, logger, previneConsumoExcessivoAppService)
        {
            _appSettings = appSettings;
        }

        public Task<NotaFiscalRootDTO?> ConsultarPorIdAsync(string id)
        {
            var filtros = "formato=json";
            filtros += $"&token={_appSettings.Tiny.ApiToken}";
            filtros += $"&id={id}";

            var url = $"{URL_BASE}/{URL_NOTA_FISCAL}?{filtros}";

            return GetAsync<NotaFiscalRootDTO>(url);
        }
    }
}
