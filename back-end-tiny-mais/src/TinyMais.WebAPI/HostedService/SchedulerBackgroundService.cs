using System.Reactive.Linq;
using TinyMais.Application.Abstractions.Workers;
using TinyMais.Application.Workers;

namespace TinyMais.WebAPI.HostedService
{
    public class SchedulerBackgroundService : BackgroundService
    {
        private readonly ILogger<SchedulerBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly BaixarPedidoWorker _baixarPedidoWorker;

        public SchedulerBackgroundService(
            ILogger<SchedulerBackgroundService> logger,
            IServiceProvider serviceProvider,
            BaixarPedidoWorker baixarPedidoWorker
            )
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
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
            _baixarPedidoWorker.WorkAsync().Wait();

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

            var tempoEsperaPrimeiraExecucao = primeiraExecucao - DateTime.Now;

            Observable.Concat(
                Observable.Timer(tempoEsperaPrimeiraExecucao),
                Observable.Interval(TimeSpan.FromDays(1))
                ).Subscribe(_ =>
                {
                    _logger.LogInformation($"Executando {worker.GetType().Name}...");
                    worker.WorkAsync().Wait();
                }, stoppingToken);
        }
    }
}
