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

        public override async Task WorkAsync()
        {
            var ontem = DateTime.Today.AddDays(-1);
            using var scope = ServiceProvider.CreateScope();
            var baixarRecebiveisAppService = scope.ServiceProvider.GetRequiredService<IBaixarRecebiveisAppService>();
            await baixarRecebiveisAppService.BaixarAsync(ontem, ontem);
        }
    }
}
