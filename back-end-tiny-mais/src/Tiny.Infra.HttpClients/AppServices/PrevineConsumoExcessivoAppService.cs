using Microsoft.Extensions.Logging;
using Tiny.Infra.HttpClients.Abstractions.AppServices;
using TinyMais.Domain.Abstractions.Models;

namespace Tiny.Infra.HttpClients.AppServices
{
    public class PrevineConsumoExcessivoAppService : IPrevineConsumoExcessivoAppService
    {
        private readonly ILogger<PrevineConsumoExcessivoAppService> _logger;
        private readonly IAppSettings _appSettings;
        private int _requisicoes = 0;

        public PrevineConsumoExcessivoAppService(
           ILogger<PrevineConsumoExcessivoAppService> logger,
           IAppSettings appSettings
           ) 
        {
            _logger = logger;
            _appSettings = appSettings;
        }

        public void Previnir()
        {
            _requisicoes++;
            if (_requisicoes == _appSettings.Tiny.RequisicoesPorMinuto)
            {
                _logger.LogInformation($"Aguardando 1 minuto para previnir consumo excessivo");
                Thread.Sleep(TimeSpan.FromMinutes(1));
                _requisicoes = 0;
            }
        }
    }
}
