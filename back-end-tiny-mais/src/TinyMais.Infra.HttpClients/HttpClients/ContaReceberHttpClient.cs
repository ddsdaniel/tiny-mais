using Microsoft.Extensions.Logging;
using TinyMais.Domain.Abstractions.Models;
using TinyMais.Infra.HttpClients.Abstractions.HttpClients;
using TinyMais.Infra.HttpClients.DTOs.ContasReceber;

namespace TinyMais.Infra.HttpClients.HttpClients
{
    public class ContaReceberHttpClient : TinyMaisHttpClient, IContaReceberHttpClient
    {
        private const string URL_CONTAS_RECEBER = "contas.receber.pesquisa.php";
        private readonly IAppSettings _appSettings;

        public ContaReceberHttpClient(
            HttpClient httpClient,
            ILogger<ContaReceberHttpClient> logger,
            IAppSettings appSettings
            ) : base(httpClient, logger)
        {
            _appSettings = appSettings;
        }

        public Task<RootDTO?> ConsultarPorIdOrigemAsync(string idOrigem)
        {
            var filtros = "formato=json";
            filtros += $"&token={_appSettings.Tiny.ApiToken}";
            filtros += $"&id_origem={idOrigem}";

            var url = $"{URL_BASE}/{URL_CONTAS_RECEBER}?{filtros}";

            return GetAsync<RootDTO>(url);
        }
    }
}
