using Microsoft.Extensions.Logging;
using TinyMais.Application.Abstractions.AppServices;
using TinyMais.Infra.HttpClients.Abstractions.HttpClients;
using TrackCash.Infra.HttpClients.Abstractions.Formatters;
using TrackCash.Infra.HttpClients.Abstractions.HttpClients;

namespace TinyMais.Application.AppServices
{
    public class BaixarRecebiveisAppService : AppService, IBaixarRecebiveisAppService
    {
        private readonly IPaymentHttpClient _pagamentoHttpClient;
        private readonly ILogger<BaixarRecebiveisAppService> _logger;
        private readonly IMarketPlaceOrderIdFormatter _marketPlaceOrderIdFactory;
        private readonly IContaReceberHttpClient _contaReceberHttpClient;

        public BaixarRecebiveisAppService(
            IPaymentHttpClient pagamentoHttpClient,
            ILogger<BaixarRecebiveisAppService> logger,
            IMarketPlaceOrderIdFormatter marketPlaceOrderIdFactory,
            IContaReceberHttpClient contaReceberHttpClient
            )
        {
            _pagamentoHttpClient = pagamentoHttpClient;
            _logger = logger;
            _marketPlaceOrderIdFactory = marketPlaceOrderIdFactory;
            _contaReceberHttpClient = contaReceberHttpClient;
        }

        public async Task BaixarAsync(DateTime dataInicial, DateTime dataFinal)
        {
            //TODO: remover este teste
            dataInicial = Convert.ToDateTime("11/03/2022");
            dataFinal = Convert.ToDateTime("26/03/2022");

            _logger.LogInformation($"Iniciando {nameof(BaixarRecebiveisAppService)}.BaixarAsync({dataInicial:dd/MM/yyyy}, {dataFinal:dd/MM/yyyy})");

            _logger.LogInformation($"Obtendo pagamentos...");
            var pagamentos = await _pagamentoHttpClient.ConsultarPorDataAsync(dataInicial, dataFinal);
            if (pagamentos != null)
            {
                foreach (var dados in pagamentos.data)
                {
                    foreach (var item in dados.List)
                    {
                        if (item.order != null)
                        {
                            foreach (var order in item.order)
                            {
                                var marketPlaceOrderId = _marketPlaceOrderIdFactory.Formatar(order.mkp_order_id, order.mkp_channel);
                                var contasResult = await _contaReceberHttpClient.ConsultarPorIdOrigemAsync(marketPlaceOrderId);
                                if (contasResult != null)
                                {

                                }
                            }
                        }
                    }
                }
            }
            
        }
    }
}
