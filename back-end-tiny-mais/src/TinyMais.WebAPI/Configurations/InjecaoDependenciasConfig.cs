using Microsoft.Extensions.Options;
using TinyMais.Application.Abstractions.AppServices;
using TinyMais.Application.AppServices;
using TinyMais.Application.Workers;
using TinyMais.Domain.Abstractions.Models;
using TinyMais.Domain.Models;
using TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Abstractions;
using TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Services;
using TinyMais.WebAPI.HostedService;

namespace TinyMais.WebAPI.Configurations
{
    public static class InjecaoDependenciasConfig
    {
        public static IServiceCollection AddInjecaoDependenciasConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(appSetting =>
            {
                configuration.GetSection(nameof(AppSettings)).Bind(appSetting);
            });

            services.AddSingleton<IAppSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value);

            services.AddHostedService<SchedulerBackgroundService>();
            services.AddSingleton<BaixarRecebiveisDeOntemWorker>();

            services.AddHttpClient();
            services.AddScoped<IPedidoHttpClient, PedidoHttpClient>();
            services.AddScoped<IPagamentoHttpClient, PagamentoHttpClient>();

            services.AddScoped<IBaixarRecebiveisAppService, BaixarRecebiveisAppService>();

            return services;
        }
    }
}
