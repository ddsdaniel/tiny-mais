using System.Reactive.Linq;
using TinyMais.Application.Abstractions.Workers;
using TinyMais.Application.Workers;

namespace TinyMais.WebAPI.HostedService
{
    public class SchedulerBackgroundService : BackgroundService
    {
        private readonly ILogger<SchedulerBackgroundService> _logger;
        private readonly BaixarRecebiveisDeOntemWorker _baixarPedidoWorker;

        public SchedulerBackgroundService(
            ILogger<SchedulerBackgroundService> logger,
            BaixarRecebiveisDeOntemWorker baixarPedidoWorker
            )
        {
            _logger = logger;
            _baixarPedidoWorker = baixarPedidoWorker;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Iniciando {nameof(SchedulerBackgroundService)}...");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Parando {nameof(SchedulerBackgroundService)}...");
            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ExecutarDiariamente(0, 0, _baixarPedidoWorker, stoppingToken);
            return Task.CompletedTask;
        }

        private void ExecutarDiariamente(int horas, int minutos, Worker worker, CancellationToken stoppingToken)
        {
            var primeiraExecucao = DateTime
                .Today
                .AddHours(horas)
                .AddMinutes(minutos);

            if (primeiraExecucao < DateTime.Now)
                primeiraExecucao = primeiraExecucao.AddDays(1);

            primeiraExecucao = DateTime.Now.AddSeconds(2);

            var tempoEsperaPrimeiraExecucao = primeiraExecucao - DateTime.Now;

            Observable.Concat(
                Observable.Timer(tempoEsperaPrimeiraExecucao),
                Observable.Interval(TimeSpan.FromDays(1))
                ).Subscribe(_ =>
                {
                    try
                    {
                        _logger.LogInformation($"Executando {worker.GetType().Name}...");
                        worker.WorkAsync().Wait();
                    }
                    catch (Exception erro)
                    {
                        _logger.LogError($"{erro.Message} em {erro.StackTrace}");
                    }
                }, stoppingToken);
        }
    }
}
