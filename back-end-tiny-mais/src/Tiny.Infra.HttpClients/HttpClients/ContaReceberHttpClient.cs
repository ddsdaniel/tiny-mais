using Microsoft.Extensions.Logging;
using Tiny.Infra.HttpClients.Abstractions.HttpClients;
using Tiny.Infra.HttpClients.DTOs.ContasReceber;
using TinyMais.Domain.Abstractions.Models;

namespace Tiny.Infra.HttpClients.HttpClients
{
    public class ContaReceberHttpClient : TinyHttpClient, IContaReceberHttpClient
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

        public Task<ContasReceberRootDTO?> ConsultarPorIdOrigemAsync(string idOrigem)
        {
            var filtros = "formato=json";
            filtros += $"&token={_appSettings.Tiny.ApiToken}";
            filtros += $"&id_origem={idOrigem}";

            var url = $"{URL_BASE}/{URL_CONTAS_RECEBER}?{filtros}";

            return GetAsync<ContasReceberRootDTO>(url);
        }
    }
}
