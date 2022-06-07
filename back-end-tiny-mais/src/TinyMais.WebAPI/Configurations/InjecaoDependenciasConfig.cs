using Microsoft.Extensions.Options;
using Tiny.Infra.HttpClients.Abstractions.AppServices;
using Tiny.Infra.HttpClients.Abstractions.HttpClients;
using Tiny.Infra.HttpClients.AppServices;
using Tiny.Infra.HttpClients.HttpClients;
using TinyMais.Application.Abstractions.AppServices;
using TinyMais.Application.AppServices;
using TinyMais.Application.Workers;
using TinyMais.Domain.Abstractions.Models;
using TinyMais.Domain.Models;
using TinyMais.WebAPI.HostedService;
using TrackCash.Infra.HttpClients.Abstractions.Factories;
using TrackCash.Infra.HttpClients.Abstractions.Formatters;
using TrackCash.Infra.HttpClients.Abstractions.HttpClients;
using TrackCash.Infra.HttpClients.Factories;
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
            services.AddScoped<IMarketPlaceConfigFactory, MarketPlaceConfigFactory>();

            //Infra Tiny
            services.AddScoped<IContaReceberHttpClient, ContaReceberHttpClient>();
            services.AddScoped<INotaFiscalHttpClient, NotaFiscalHttpClient>();
            services.AddScoped<IPedidoHttpClient, PedidoHttpClient>();
            services.AddScoped<IPedidosHttpClient, PedidosHttpClient>();
            services.AddScoped<IPrevineConsumoExcessivoAppService, PrevineConsumoExcessivoAppService>();

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
