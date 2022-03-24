using Microsoft.Extensions.Logging;
using TinyMais.Domain.Abstractions.Models;
using TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Abstractions;
using TinyMais.Infra.ConciliadorFinanceiro.TrackCash.DTOs.Pedidos;
using TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Enums;
using TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Extensions;

namespace TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Services
{
    public class PedidoHttpClient : TrachCashHttpClient, IPedidoHttpClient
    {
        private const string URL_PEDIDO = "orders";

        public PedidoHttpClient(
            HttpClient httpClient,
            IAppSettings appSettings,
            ILogger<PedidoHttpClient> logger) : base(httpClient, appSettings, logger)
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
