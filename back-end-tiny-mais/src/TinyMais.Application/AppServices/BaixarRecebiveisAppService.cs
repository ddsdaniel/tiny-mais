using Microsoft.Extensions.Logging;
using Tiny.Infra.HttpClients.Abstractions.HttpClients;
using Tiny.Infra.HttpClients.DTOs.ContasReceber;
using Tiny.Infra.HttpClients.DTOs.NotaFiscal;
using Tiny.Infra.HttpClients.DTOs.Pedido;
using Tiny.Infra.HttpClients.DTOs.Pedidos;
using TinyMais.Application.Abstractions.AppServices;
using TrackCash.Infra.HttpClients.Abstractions.Formatters;
using TrackCash.Infra.HttpClients.Abstractions.HttpClients;
using TrackCash.Infra.HttpClients.DTOs.Payments;

namespace TinyMais.Application.AppServices
{
    public class BaixarRecebiveisAppService : AppService, IBaixarRecebiveisAppService
    {
        private readonly IPaymentHttpClient _paymentHttpClient;
        private readonly ILogger<BaixarRecebiveisAppService> _logger;
        private readonly IMarketPlaceOrderIdFormatter _marketPlaceOrderIdFactory;
        private readonly IContaReceberHttpClient _contaReceberHttpClient;
        private readonly INotaFiscalHttpClient _notaFiscalHttpClient;
        private readonly IPedidoHttpClient _pedidoHttpClient;
        private readonly IPedidosHttpClient _pedidosHttpClient;

        public BaixarRecebiveisAppService(
            IPaymentHttpClient paymentHttpClient,
            ILogger<BaixarRecebiveisAppService> logger,
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

            var payments = await ObterPayments(dataInicial, dataFinal);
            foreach (var payment in payments)
            {
                foreach (var order in payment.order)
                {
                    var pedidoResumido = await ObterPedidoResumido(order);

                    if (pedidoResumido != null)
                    {
                        var pedidoCompleto = await ObterPedidoCompleto(pedidoResumido);
                        var notaFiscal = await ObterNotaFiscal(pedidoCompleto);
                        var contasReceber = await ObterContasReceber(notaFiscal);

                        foreach (var conta in contasReceber)
                        {
                            //if (conta.Conta.situacao ==)
                        }
                    }
                }
            }

        }

        private async Task<List<ContaContainerDTO>> ObterContasReceber(NotaFiscalDTO notaFiscal)
        {
            _logger.LogInformation($"Obtendo contas a receber...");

            var numeroNotaFiscal = $"{notaFiscal.numero}/01";//TODO: não sei se é sempre neste formato

            var contasReceber = (await _contaReceberHttpClient.ConsultarPorIdOrigemAsync(numeroNotaFiscal)).retorno.contas;
            
            return contasReceber;
        }

        private async Task<NotaFiscalDTO> ObterNotaFiscal(PedidoDTO pedido)
        {
            _logger.LogInformation($"Obtendo nota fiscal...");

            var idNotaFiscal = pedido.id_nota_fiscal;

            var notaFiscal = (await _notaFiscalHttpClient.ConsultarPorIdAsync(idNotaFiscal)).retorno.nota_fiscal;
            return notaFiscal;
        }

        private async Task<PedidoDTO> ObterPedidoCompleto(PedidosRootDTO pedidoResumido)
        {
            _logger.LogInformation($"Obtendo pedido completo...");

            string idPedido = pedidoResumido.retorno.pedidos.FirstOrDefault().pedido.id;

            var pedido = (await _pedidoHttpClient.ConsultarPorIdAsync(idPedido)).retorno.pedido;

            return pedido;
        }

        private async Task<PedidosRootDTO> ObterPedidoResumido(OrderDTO order)
        {
            _logger.LogInformation($"Obtendo pedidos (resumidos)...");

            var marketPlaceOrderId = _marketPlaceOrderIdFactory.Formatar(order.mkp_order_id, order.mkp_channel);

            var pedidos = await _pedidosHttpClient.ConsultarPorNumeroEcommerceAsync(marketPlaceOrderId);

            return pedidos;
        }

        private async Task<IEnumerable<PaymentListDTO>> ObterPayments(DateTime dataInicial, DateTime dataFinal)
        {
            _logger.LogInformation($"Obtendo pagamentos...");

            var payments = (await _paymentHttpClient.ConsultarPorDataAsync(dataInicial, dataFinal)).data.SelectMany(p => p.List);

            return payments.Where(p => p.order != null);
        }
    }
}
