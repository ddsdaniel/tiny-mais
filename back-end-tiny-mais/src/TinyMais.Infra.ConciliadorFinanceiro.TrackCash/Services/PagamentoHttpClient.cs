using Microsoft.Extensions.Logging;
using TinyMais.Domain.Abstractions.Models;
using TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Abstractions;
using TinyMais.Infra.ConciliadorFinanceiro.TrackCash.DTOs.Pagamentos;
using TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Extensions;

namespace TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Services
{
    public class PagamentoHttpClient : TrachCashHttpClient, IPagamentoHttpClient
    {
        private const string URL_PAGAMENTO = "payments";

        public PagamentoHttpClient(
            HttpClient httpClient,
            IAppSettings appSettings,
            ILogger<PagamentoHttpClient> logger) : base(httpClient, appSettings, logger)
        {
        }

        public Task<PaymentResultDTO?> ConsultarAsync(DateTime dataInicial, DateTime dataFinal)
        {
            /*
             Filtros	Descrição
            date	Data do pedido
            date_start	Data inicial do pedido
            date_end	Data final do pedido
            status	(Consulte a tabela)
            point_sale	(Consulte a tabela)
            order	Numero do pedido na Loja
            mkp_order	Numero do pedido no Marketplace
             */

            var filtros = $"date_start={dataInicial.ToTrackCashDate()}";
            filtros += $"&date_end={dataFinal.ToTrackCashDate()}";

            var url = $"{URL_BASE}/{URL_PAGAMENTO}?{filtros}";

            return GetAsync<PaymentResultDTO>(url);
        }
    }
}
