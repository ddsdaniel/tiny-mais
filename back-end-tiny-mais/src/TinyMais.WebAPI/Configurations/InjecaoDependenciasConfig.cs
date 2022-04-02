using Microsoft.Extensions.Options;
using TinyMais.Application.Abstractions.AppServices;
using TinyMais.Application.AppServices;
using TinyMais.Application.Workers;
using TinyMais.Domain.Abstractions.Models;
using TinyMais.Domain.Models;
using TinyMais.Infra.HttpClients.Abstractions.HttpClients;
using TinyMais.Infra.HttpClients.HttpClients;
using TinyMais.WebAPI.HostedService;
using TrackCash.Infra.HttpClients.Abstractions.Formatters;
using TrackCash.Infra.HttpClients.Abstractions.HttpClients;
using TrackCash.Infra.HttpClients.Formatters;
using TrackCash.Infra.HttpClients.HttpClients;

namespace TinyMais.WebAPI.Configurations
{
    public static class InjecaoDependenciasConfig
    {
        public static IServiceCollection AddInjecaoDependenciasConfig(this IServiceCollection services, IConfiguration configuration)
        {
            //AppSettings
            services.Configure<AppSettings>(appSetting =>
            {
                configuration.GetSection(nameof(AppSettings)).Bind(appSetting);
            });

            services.AddSingleton<IAppSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value);

            //Infra
            services.AddHttpClient();

            //Infra Track Cash
            services.AddScoped<IOrderHttpClient, OrderHttpClient>();
            services.AddScoped<IPaymentHttpClient, PaymentHttpClient>();
            services.AddScoped<IMarketPlaceOrderIdFormatter, MarketPlaceOrderIdFormatter>();

            //Infra Tiny
            services.AddScoped<IContaReceberHttpClient, ContaReceberHttpClient>();

            //Background Services
            services.AddHostedService<SchedulerBackgroundService>();

            //Workers
            services.AddSingleton<BaixarRecebiveisDeOntemWorker>();

            //Application
            services.AddScoped<IBaixarRecebiveisAppService, BaixarRecebiveisAppService>();

            return services;
        }
    }
}
