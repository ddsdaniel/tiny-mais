using Infra.HttpClients.Extensions;
using Microsoft.Extensions.Logging;
using Tiny.Infra.HttpClients.Abstractions.HttpClients;
using Tiny.Infra.HttpClients.Constantes;
using Tiny.Infra.HttpClients.DTOs.ContasReceber;
using Tiny.Infra.HttpClients.DTOs.ContasReceberBaixa.Request;
using Tiny.Infra.HttpClients.DTOs.NotaFiscal;
using Tiny.Infra.HttpClients.DTOs.Pedido;
using Tiny.Infra.HttpClients.DTOs.Pedidos;
using TinyMais.Application.Abstractions.AppServices;
using TinyMais.Application.Constantes;
using TrackCash.Infra.HttpClients.Abstractions.Factories;
using TrackCash.Infra.HttpClients.Abstractions.Formatters;
using TrackCash.Infra.HttpClients.Abstractions.HttpClients;
using TrackCash.Infra.HttpClients.DTOs.Payments;

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
        private readonly IMarketPlaceConfigFactory _marketPlaceConfigFactory;

        public BaixarRecebiveisAppService(
            ILogger<BaixarRecebiveisAppService> logger,
            IPaymentHttpClient paymentHttpClient,
            IMarketPlaceOrderIdFormatter marketPlaceOrderIdFactory,
            IContaReceberHttpClient contaReceberHttpClient,
            INotaFiscalHttpClient notaFiscalHttpClient,
            IPedidoHttpClient pedidoHttpClient,
            IPedidosHttpClient pedidosHttpClient,
            IMarketPlaceConfigFactory marketPlaceConfigFactory
            )
        {
            _paymentHttpClient = paymentHttpClient;
            _logger = logger;
            _marketPlaceOrderIdFactory = marketPlaceOrderIdFactory;
            _contaReceberHttpClient = contaReceberHttpClient;
            _notaFiscalHttpClient = notaFiscalHttpClient;
            _pedidoHttpClient = pedidoHttpClient;
            _pedidosHttpClient = pedidosHttpClient;
            _marketPlaceConfigFactory = marketPlaceConfigFactory;
        }

        public async Task BaixarAsync(DateTime dataInicial, DateTime dataFinal)
        {
            _logger.LogInformation($"Iniciando {nameof(BaixarRecebiveisAppService)}.BaixarAsync({dataInicial:dd/MM/yyyy}, {dataFinal:dd/MM/yyyy})");

            var macroPagamentos = await ObterPayments(dataInicial, dataFinal);

            await BaixarMacroPagamentosAsync(macroPagamentos);
        }

        public async Task BaixarAsync(string idPedidoMarketPlace)
        {
            _logger.LogInformation($"Iniciando {nameof(BaixarRecebiveisAppService)}.BaixarAsync({idPedidoMarketPlace})");

            var macroPagamentos = await ObterPayments(idPedidoMarketPlace);

            await BaixarMacroPagamentosAsync(macroPagamentos);
        }

        private async Task BaixarMacroPagamentosAsync(IEnumerable<PaymentListDTO> macroPagamentos)
        {
            if (!macroPagamentos.Any())
            {
                _logger.LogWarning("Nenhum pagamento foi retornado");
            }

            foreach (var macroPagamento in macroPagamentos)
            {
                _logger.LogInformation(new string('-', 50));

                foreach (var pedidoTrackCash in macroPagamento.order)
                {
                    var pedidoResumidoTiny = await ObterPedidoResumido(pedidoTrackCash);

                    if (pedidoResumidoTiny != null)
                    {
                        if (pedidoResumidoTiny.retorno.pedidos != null)
                        {
                            var pedidoCompletoTiny = await ObterPedidoCompleto(pedidoResumidoTiny);
                            if (pedidoCompletoTiny != null)
                            {
                                var notaFiscalTiny = await ObterNotaFiscal(pedidoCompletoTiny);

                                if (notaFiscalTiny != null)
                                {
                                    PaymentDTO ultimoPagamento = null;
                                    foreach (var pagamentoTrackCash in macroPagamento.payments)
                                    {
                                        if (ultimoPagamento != null &&
                                            ultimoPagamento.mkp_order_id == pagamentoTrackCash.mkp_order_id &&
                                            ultimoPagamento.current_installment == pagamentoTrackCash.current_installment)
                                            continue;

                                        if (pagamentoTrackCash.id_code != TipoPagamento.VENDA)
                                            continue;

                                        ultimoPagamento = pagamentoTrackCash;                                    

                                        var contasTiny = await ObterContasReceber(notaFiscalTiny, pagamentoTrackCash);

                                        if (contasTiny == null)
                                            continue;

                                        var contasReceberTiny = contasTiny.Select(c => c.conta);

                                        var contaTiny = contasReceberTiny.FirstOrDefault();

                                        if (contaTiny?.situacao == SituacaoContaReceber.ABERTO)
                                        {
                                            await BaixarContaReceber(pagamentoTrackCash, contaTiny, pedidoTrackCash, macroPagamento);
                                        }
                                        else
                                        {
                                            _logger.LogInformation($"Conta a receber já estava com o status {contaTiny?.situacao}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            _logger.LogInformation(new string('-', 50));
            _logger.LogInformation("Finalizou BaixarAsync");
        }

        private async Task BaixarContaReceber(PaymentDTO pagamentoTrackCash, ContaDTO? contaTiny, OrderDTO order, PaymentListDTO macroPagamento)
        {
            _logger.LogInformation($"Baixando conta a receber {contaTiny.id}...");

            var taxas = Math.Abs(macroPagamento.payments
                                                .Where(p => p.current_installment == pagamentoTrackCash.current_installment)
                                                .Where(p => p.id_code == TipoPagamento.COMISSAO)
                                                .Sum(p => p.value.LerMoedaJson()));

            var valorBruto = macroPagamento.payments
                                        .Where(p => p.mkp_order_id == pagamentoTrackCash.mkp_order_id &&
                                                    p.current_installment == pagamentoTrackCash.current_installment &&
                                                    (p.id_code == TipoPagamento.VENDA || p.id_code == TipoPagamento.FIXED_VALUE)
                                              )
                                        .Sum(p => p.value.LerMoedaJson());

            var valorDesconto = Math.Abs(macroPagamento.payments
                                        .Where(p => p.mkp_order_id == pagamentoTrackCash.mkp_order_id &&
                                                    p.current_installment == pagamentoTrackCash.current_installment &&
                                                    p.id_code == TipoPagamento.FIXED_VALUE
                                              )
                                        .Sum(p => p.value.LerMoedaJson()));

            var diferenca = Math.Round(contaTiny.valor.LerMoedaJson() - valorBruto, 2, MidpointRounding.AwayFromZero);

            if (Math.Abs(diferenca) <= 0.03)
            {
                valorBruto += diferenca;
            }

            var valorLiquido = Math.Round(valorBruto - taxas, 2, MidpointRounding.AwayFromZero);

            var contaBaixaTiny = new ContaBaixaDTO
            {
                id = contaTiny.id,
                data = Convert.ToDateTime(pagamentoTrackCash.date).ToString("dd/MM/yyyy"),
                valorPago = valorLiquido,
                valorTaxas = taxas,
                valorDesconto = valorDesconto
            };

            var marketPlace = _marketPlaceConfigFactory.Obter(order.mkp_channel, order.mkp_id_channel);
            if (marketPlace != null)
                contaBaixaTiny.contaDestino = marketPlace.ContaDestino;

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

        private async Task<List<ContaContainerDTO>> ObterContasReceber(NotaFiscalDTO notaFiscal, PaymentDTO pagamentoTrackCash)
        {
            var parcela = pagamentoTrackCash.current_installment.PadLeft(2, '0');
            var numeroNotaFiscal = $"{notaFiscal.numero}/{parcela}";

            _logger.LogInformation($"Obtendo contas a receber {numeroNotaFiscal}...");

            var root = await _contaReceberHttpClient.ConsultarPorNumeroDocAsync(numeroNotaFiscal);

            var contasReceber = root.retorno.contas;

            if (contasReceber == null)
                _logger.LogWarning("Conta a receber não encontrada no Tiny");

            return contasReceber;
        }

        private async Task<NotaFiscalDTO> ObterNotaFiscal(PedidoCompletoDTO pedido)
        {
            var idNotaFiscal = pedido.id_nota_fiscal;

            _logger.LogInformation($"Obtendo nota fiscal {idNotaFiscal}...");

            var notaFiscal = (await _notaFiscalHttpClient.ConsultarPorIdAsync(idNotaFiscal)).retorno.nota_fiscal;

            if (notaFiscal == null)
                _logger.LogWarning("Nota fiscal não encontrada");

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
            var marketPlaceOrderId = _marketPlaceOrderIdFactory.Formatar(order.mkp_order_id, order.mkp_channel, order.mkp_id_channel);

            _logger.LogInformation($"Obtendo pedidos (resumidos) {marketPlaceOrderId} de {order.mkp_channel}...");

            var pedidos = await _pedidosHttpClient.ConsultarPorNumeroEcommerceAsync(marketPlaceOrderId);

            if (pedidos.retorno.pedidos == null)
                _logger.LogWarning($"Pedido não encontrado no Tiny.");

            return pedidos;
        }

        private async Task<IEnumerable<PaymentListDTO>> ObterPayments(string idPedidoMarketPlace)
        {
            _logger.LogInformation($"Obtendo pagamentos da Track Cash...");

            var pagamentos = new List<PaymentListDTO>();

            RootDTO root = await _paymentHttpClient.ConsultarPorPedidoAsync(idPedidoMarketPlace);

            pagamentos.AddRange(root.data.SelectMany(p => p.List));

            return pagamentos.Where(p => p.order != null);
        }

        private async Task<IEnumerable<PaymentListDTO>> ObterPayments(DateTime dataInicial, DateTime dataFinal)
        {
            _logger.LogInformation($"Obtendo pagamentos da Track Cash...");

            var pagamentos = new List<PaymentListDTO>();
            var paginaAtual = 1;
            RootDTO root = null;
            do
            {
                _logger.LogInformation($"Página {paginaAtual}...");
                root = await _paymentHttpClient.ConsultarPorDataAsync(dataInicial, dataFinal, paginaAtual);

                pagamentos.AddRange(root.data.SelectMany(p => p.List));

                paginaAtual++;
            } while (paginaAtual <= root.meta.last_page);

            return pagamentos.Where(p => p.order != null);
        }
    }
}
