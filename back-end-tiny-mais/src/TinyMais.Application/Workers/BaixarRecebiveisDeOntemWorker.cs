using Microsoft.Extensions.DependencyInjection;
using TinyMais.Application.Abstractions.AppServices;
using TinyMais.Application.Abstractions.Workers;

namespace TinyMais.Application.Workers
{
    public class BaixarRecebiveisDeOntemWorker : Worker
    {
        public BaixarRecebiveisDeOntemWorker(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public override Task WorkAsync()
        {
            var ontem = DateTime.Today.AddDays(-1);
            using var scope = ServiceProvider.CreateScope();
            var baixarRecebiveisAppService = scope.ServiceProvider.GetRequiredService<IBaixarRecebiveisAppService>();
            return baixarRecebiveisAppService.BaixarAsync(ontem, ontem);
        }
    }
}
