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

        public Task<RootDTO?> ConsultarPorDataAsync(DateTime dataInicial, DateTime dataFinal)
        {
            var filtros = $"date_start={dataInicial.ToTrackCashDate()}";
            filtros += $"&date_end={dataFinal.ToTrackCashDate()}";

            var url = $"{URL_BASE}/{URL_PAGAMENTO}?{filtros}";

            return GetAsync<RootDTO>(url);
        }
    }
}
