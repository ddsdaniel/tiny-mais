using Microsoft.Extensions.Logging;
using TinyMais.Domain.Abstractions.Models;
using TrackCash.Infra.HttpClients.Abstractions.HttpClients;
using TrackCash.Infra.HttpClients.DTOs.Payments;
using TrackCash.Infra.HttpClients.Extensions;

namespace TrackCash.Infra.HttpClients.HttpClients
{
    public class PaymentHttpClient : TrachCashHttpClient, IPaymentHttpClient
    {
        private const string URL_PAGAMENTO = "payments";

        public PaymentHttpClient(
            HttpClient httpClient,
            IAppSettings appSettings,
            ILogger<PaymentHttpClient> logger
            ) : base(httpClient, logger, appSettings)
        {

        }

        public Task<RootDTO?> ConsultarPorPedidoAsync(string idPedidoMarketPlace)
        {
            var filtros = $"mkp_order={idPedidoMarketPlace}";
            var url = $"{URL_BASE}/{URL_PAGAMENTO}?{filtros}";
            return GetAsync<RootDTO>(url);
        }

        public Task<RootDTO?> ConsultarPorDataAsync(DateTime dataInicial, DateTime dataFinal, int paginaAtual)
        {
            var filtros = $"date_start={dataInicial.ToTrackCashDate()}";
            filtros += $"&date_end={dataFinal.ToTrackCashDate()}";
            filtros += $"&page={paginaAtual}";

            var url = $"{URL_BASE}/{URL_PAGAMENTO}?{filtros}";

            return GetAsync<RootDTO>(url);
        }
    }
}
