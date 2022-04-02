using Infra.HttpClients.Abstractions;
using Microsoft.Extensions.Logging;

namespace TinyMais.Infra.HttpClients.Abstractions.HttpClients
{
    public abstract class TinyMaisHttpClient : InfraHttpClient
    {
        protected const string URL_BASE = "https://sistema.trackcash.com.br/api";

        protected TinyMaisHttpClient(
            HttpClient httpClient,
            ILogger<TinyMaisHttpClient> logger
            ) : base(httpClient, logger)
        {
        }
    }
}
