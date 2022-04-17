using Microsoft.Extensions.Logging;
using Tiny.Infra.HttpClients.Abstractions.AppServices;
using Tiny.Infra.HttpClients.Abstractions.HttpClients;
using Tiny.Infra.HttpClients.DTOs.Pedidos;
using TinyMais.Domain.Abstractions.Models;

namespace Tiny.Infra.HttpClients.HttpClients
{
    public class PedidosHttpClient : TinyHttpClient, IPedidosHttpClient
    {
        private const string URL_PEDIDOS = "pedidos.pesquisa.php";
        private readonly IAppSettings _appSettings;

        public PedidosHttpClient(
            HttpClient httpClient,
            ILogger<PedidosHttpClient> logger,
            IAppSettings appSettings,
            IPrevineConsumoExcessivoAppService previneConsumoExcessivoAppService
            ) : base(httpClient, logger, previneConsumoExcessivoAppService)
        {
            _appSettings = appSettings;
        }

        public Task<PedidosRootDTO?> ConsultarPorNumeroEcommerceAsync(string numeroEcommerce)
        {
            var filtros = "formato=json";
            filtros += $"&token={_appSettings.Tiny.ApiToken}";
            filtros += $"&numeroEcommerce={numeroEcommerce}";

            var url = $"{URL_BASE}/{URL_PEDIDOS}?{filtros}";

            return GetAsync<PedidosRootDTO>(url);
        }
    }
}
