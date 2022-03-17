using Microsoft.Extensions.DependencyInjection;
using TinyMais.Application.Abstractions.Workers;
using TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Abstractions;
using TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Enums;

namespace TinyMais.Application.Workers
{
    public class BaixarPedidoWorker : Worker
    {
        private readonly IPedidoHttpClient _pedidoHttpClient;

        public BaixarPedidoWorker(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            _pedidoHttpClient = scope.ServiceProvider.GetRequiredService<IPedidoHttpClient>();
        }

        public override Task WorkAsync()
        {
            var pedidos = _pedidoHttpClient.Consultar(DateTime.Today, DateTime.Today, StatusPedido.Processando);
            return Task.CompletedTask;
        }
    }
}
