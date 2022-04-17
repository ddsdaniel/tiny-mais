using Microsoft.Extensions.Logging;
using Tiny.Infra.HttpClients.Abstractions.AppServices;
using Tiny.Infra.HttpClients.Abstractions.HttpClients;
using Tiny.Infra.HttpClients.DTOs.Pedido;
using TinyMais.Domain.Abstractions.Models;

namespace Tiny.Infra.HttpClients.HttpClients
{
    public class PedidoHttpClient : TinyHttpClient, IPedidoHttpClient
    {
        private const string URL_PEDIDO = "pedido.obter.php";
        private readonly IAppSettings _appSettings;

        public PedidoHttpClient(
            HttpClient httpClient,
            ILogger<PedidoHttpClient> logger,
            IAppSettings appSettings,
            IPrevineConsumoExcessivoAppService previneConsumoExcessivoAppService
            ) : base(httpClient, logger, previneConsumoExcessivoAppService)
        {
            _appSettings = appSettings;
        }

        public Task<PedidoRootDTO?> ConsultarPorIdAsync(string id)
        {
            var filtros = "formato=json";
            filtros += $"&token={_appSettings.Tiny.ApiToken}";
            filtros += $"&id={id}";

            var url = $"{URL_BASE}/{URL_PEDIDO}?{filtros}";

            return GetAsync<PedidoRootDTO>(url);
        }
    }
}
