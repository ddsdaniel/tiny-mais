using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TinyMais.Domain.Abstractions.Models;
using TinyMais.Domain.Models;

namespace TinyMais.Infra.CrossCutting.IoC
{
    public static class ConfiguracoesIoC
    {
        public static IServiceCollection AddConfiguracoesIoC(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(appSetting =>
            {
                configuration.GetSection(nameof(AppSettings)).Bind(appSetting);
            });

            services.AddSingleton<IAppSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value);

            return services;
        }
    }
}
