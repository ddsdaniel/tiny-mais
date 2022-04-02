using Microsoft.Extensions.Logging;
using TinyMais.Domain.Abstractions.Models;
using TrackCash.Infra.HttpClients.Abstractions.HttpClients;
using TrackCash.Infra.HttpClients.DTOs.Orders;
using TrackCash.Infra.HttpClients.Enums;
using TrackCash.Infra.HttpClients.Extensions;

namespace TrackCash.Infra.HttpClients.HttpClients
{
    public class OrderHttpClient : TrachCashHttpClient, IOrderHttpClient
    {
        private const string URL_PEDIDO = "orders";

        public OrderHttpClient(
            HttpClient httpClient,
            IAppSettings appSettings,
            ILogger<OrderHttpClient> logger
            ) : base(httpClient, logger, appSettings)
        {
        }

        public Task<OrdersDTO?> ConsultarAsync(DateTime dataInicial, DateTime dataFinal, StatusPedido status)
        {
            var filtros = $"date_start={dataInicial.ToTrackCashDate()}";
            filtros += $"&date_end={dataFinal.ToTrackCashDate()}";
            filtros += $"&status={(int)status}";

            var url = $"{URL_BASE}/{URL_PEDIDO}?{filtros}";

            return GetAsync<OrdersDTO>(url);
        }
    }
}
