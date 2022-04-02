using Infra.HttpClients.Abstractions;
using Microsoft.Extensions.Logging;

namespace Tiny.Infra.HttpClients.Abstractions.HttpClients
{
    public abstract class TinyMaisHttpClient : InfraHttpClient
    {
        protected const string URL_BASE = "https://api.tiny.com.br/api2";

        protected TinyMaisHttpClient(
            HttpClient httpClient,
            ILogger<TinyMaisHttpClient> logger
            ) : base(httpClient, logger)
        {
        }
    }
}
