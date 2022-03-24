using Microsoft.Extensions.Logging;
using TinyMais.Application.Abstractions.AppServices;
using TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Abstractions;

namespace TinyMais.Application.AppServices
{
    public class BaixarRecebiveisAppService : AppService, IBaixarRecebiveisAppService
    {
        private readonly IPagamentoHttpClient _pagamentoHttpClient;
        private readonly ILogger<BaixarRecebiveisAppService> _logger;

        public BaixarRecebiveisAppService(
            IPagamentoHttpClient pagamentoHttpClient,
            ILogger<BaixarRecebiveisAppService> logger)
        {
            _pagamentoHttpClient = pagamentoHttpClient;
            _logger = logger;
        }

        public async Task BaixarAsync(DateTime dataInicial, DateTime dataFinal)
        {
            _logger.LogInformation($"Iniciando {nameof(BaixarRecebiveisAppService)}.BaixarAsync({dataInicial:dd/MM/yyyy}, {dataFinal:dd/MM/yyyy})");

            _logger.LogInformation($"Obtendo pagamentos...");
            var pagamentos = await _pagamentoHttpClient.ConsultarAsync(dataInicial, dataFinal);

            
        }
    }
}
