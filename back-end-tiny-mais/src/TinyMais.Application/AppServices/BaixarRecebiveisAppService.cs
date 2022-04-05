using Microsoft.Extensions.Logging;
using Tiny.Infra.HttpClients.Abstractions.HttpClients;
using Tiny.Infra.HttpClients.Constantes;
using Tiny.Infra.HttpClients.DTOs.ContasReceber;
using Tiny.Infra.HttpClients.DTOs.ContasReceberBaixa.Request;
using Tiny.Infra.HttpClients.DTOs.NotaFiscal;
using Tiny.Infra.HttpClients.DTOs.Pedido;
using Tiny.Infra.HttpClients.DTOs.Pedidos;
using TinyMais.Application.Abstractions.AppServices;
using TrackCash.Infra.HttpClients.Abstractions.Formatters;
using TrackCash.Infra.HttpClients.Abstractions.HttpClients;
using TrackCash.Infra.HttpClients.DTOs.Payments;
using TrackCash.Infra.HttpClients.Extensions;

namespace TinyMais.Application.AppServices
{
    public class BaixarRecebiveisAppService : AppService, IBaixarRecebiveisAppService
    {
        private readonly ILogger<BaixarRecebiveisAppService> _logger;
        private readonly IPaymentHttpClient _paymentHttpClient;
        private readonly IMarketPlaceOrderIdFormatter _marketPlaceOrderIdFactory;
        private readonly IContaReceberHttpClient _contaReceberHttpClient;
        private readonly INotaFiscalHttpClient _notaFiscalHttpClient;
        private readonly IPedidoHttpClient _pedidoHttpClient;
        private readonly IPedidosHttpClient _pedidosHttpClient;

        public BaixarRecebiveisAppService(
            ILogger<BaixarRecebiveisAppService> logger,
            IPaymentHttpClient paymentHttpClient,
            IMarketPlaceOrderIdFormatter marketPlaceOrderIdFactory,
            IContaReceberHttpClient contaReceberHttpClient,
            INotaFiscalHttpClient notaFiscalHttpClient,
            IPedidoHttpClient pedidoHttpClient,
            IPedidosHttpClient pedidosHttpClient
            )
        {
            _paymentHttpClient = paymentHttpClient;
            _logger = logger;
            _marketPlaceOrderIdFactory = marketPlaceOrderIdFactory;
            _contaReceberHttpClient = contaReceberHttpClient;
            _notaFiscalHttpClient = notaFiscalHttpClient;
            _pedidoHttpClient = pedidoHttpClient;
            _pedidosHttpClient = pedidosHttpClient;
        }

        public async Task BaixarAsync(DateTime dataInicial, DateTime dataFinal)
        {
            //TODO: remover este teste
            dataInicial = Convert.ToDateTime("11/03/2022");
            dataFinal = Convert.ToDateTime("26/03/2022");

            _logger.LogInformation($"Iniciando {nameof(BaixarRecebiveisAppService)}.BaixarAsync({dataInicial:dd/MM/yyyy}, {dataFinal:dd/MM/yyyy})");

            var macroPagamentos = await ObterPayments(dataInicial, dataFinal);
            foreach (var macroPagamento in macroPagamentos)
            {
                foreach (var pedidoTrackCash in macroPagamento.order)
                {
                    var pedidoResumidoTiny = await ObterPedidoResumido(pedidoTrackCash);

                    if (pedidoResumidoTiny != null)
                    {
                        var pedidoCompletoTiny = await ObterPedidoCompleto(pedidoResumidoTiny);
                        var notaFiscalTiny = await ObterNotaFiscal(pedidoCompletoTiny);
                        var contasReceberTiny = (await ObterContasReceber(notaFiscalTiny))
                            .Select(c => c.conta);

                        foreach (var pagamentoTrackCash in macroPagamento.payments)
                        {
                            //TODO: encontrar a conta certa caso retorne mais de um registro aqui, exemplo: parcelado
                            var contaTiny = contasReceberTiny.FirstOrDefault();

                            //TODO: implementar baixa parcial?

                            if (contaTiny?.situacao == SituacaoContaReceber.ABERTO)
                            {
                                await BaixarContaReceber(pagamentoTrackCash, contaTiny);
                            }
                        }
                    }
                }
            }
            _logger.LogInformation($"Concluiu {nameof(BaixarRecebiveisAppService)}");
        }

        private async Task BaixarContaReceber(PaymentDTO pagamentoTrackCash, ContaDTO? contaTiny)
        {
            //TODO: remover campos opcionais da classe?
            //TODO: o histórico, categoria ou code indicam qual o campo de destino (juros, taxas, descontos, acréscimos, etc)?

            _logger.LogInformation($"Baixando conta a receber {contaTiny.id}...");

            var contaBaixaTiny = new ContaBaixaDTO
            {
                id = contaTiny.id,
                data = Convert.ToDateTime(pagamentoTrackCash.date).ToString("dd/MM/yyyy"),
                valorPago = pagamentoTrackCash.value.LerMoedaTrackCash()
                //contaDestino = "",//opcional
                //categoria = "",//opcional
                //historico = contaTiny.historico,//opcional
                //valorTaxas = 0,//opcional
                //valorJuros = 0,//opcional
                //valorDesconto = 0,//opcional
                //valorAcrescimo = 0,//opcional
            };
            var resultadoBaixa = await _contaReceberHttpClient.BaixarAsync(contaBaixaTiny);

            if (resultadoBaixa.retorno.status == RetornoMetodo.Ok)
            {
                _logger.LogInformation($"Sucesso ao baixar conta de receber: {contaBaixaTiny.id}");
            }
            else
            {
                var erro = resultadoBaixa.retorno.erros != null
                    ? string.Join(',', resultadoBaixa.retorno.erros.Select(e => e.erro))
                    : string.Join(',', resultadoBaixa.retorno.registros.SelectMany(r => r.registro.erros.Select(e => e.erro)));

                _logger.LogError($"Falha ao baixar conta de receber: {contaBaixaTiny.id}. {erro}");
            }
        }

        private async Task<List<ContaContainerDTO>> ObterContasReceber(NotaFiscalDTO notaFiscal)
        {
            var numeroNotaFiscal = $"{notaFiscal.numero}/01";//TODO: não sei se é sempre neste formato

            _logger.LogInformation($"Obtendo contas a receber {numeroNotaFiscal}...");

            var root = await _contaReceberHttpClient.ConsultarPorNumeroDocAsync(numeroNotaFiscal);

            var contasReceber = root.retorno.contas;

            return contasReceber;
        }

        private async Task<NotaFiscalDTO> ObterNotaFiscal(PedidoCompletoDTO pedido)
        {
            var idNotaFiscal = pedido.id_nota_fiscal;

            _logger.LogInformation($"Obtendo nota fiscal {idNotaFiscal}...");

            var notaFiscal = (await _notaFiscalHttpClient.ConsultarPorIdAsync(idNotaFiscal)).retorno.nota_fiscal;
            return notaFiscal;
        }

        private async Task<PedidoCompletoDTO> ObterPedidoCompleto(PedidosRootDTO pedidoResumido)
        {
            string idPedido = pedidoResumido.retorno.pedidos.FirstOrDefault().pedido.id;

            _logger.LogInformation($"Obtendo pedido completo {idPedido}...");

            var pedido = (await _pedidoHttpClient.ConsultarPorIdAsync(idPedido)).retorno.pedido;

            return pedido;
        }

        private async Task<PedidosRootDTO> ObterPedidoResumido(OrderDTO order)
        {
            var marketPlaceOrderId = _marketPlaceOrderIdFactory.Formatar(order.mkp_order_id, order.mkp_channel);

            _logger.LogInformation($"Obtendo pedidos (resumidos) {order.mkp_order_id} de {order.mkp_channel}...");

            var pedidos = await _pedidosHttpClient.ConsultarPorNumeroEcommerceAsync(marketPlaceOrderId);

            return pedidos;
        }

        private async Task<IEnumerable<PaymentListDTO>> ObterPayments(DateTime dataInicial, DateTime dataFinal)
        {
            _logger.LogInformation($"Obtendo pagamentos...");

            //TODO: implementar paginação em ObterPayments

            var payments = (await _paymentHttpClient.ConsultarPorDataAsync(dataInicial, dataFinal)).data.SelectMany(p => p.List);

            return payments.Where(p => p.order != null);
        }
    }
}
