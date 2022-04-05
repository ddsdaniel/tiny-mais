using Microsoft.Extensions.Logging;
using System.Text.Json;
using Tiny.Infra.HttpClients.Abstractions.HttpClients;
using Tiny.Infra.HttpClients.DTOs.ContasReceber;
using Tiny.Infra.HttpClients.DTOs.ContasReceberBaixa.Request;
using Tiny.Infra.HttpClients.DTOs.ContasReceberBaixa.Response;
using TinyMais.Domain.Abstractions.Models;

namespace Tiny.Infra.HttpClients.HttpClients
{
    public class ContaReceberHttpClient : TinyHttpClient, IContaReceberHttpClient
    {
        private const string URL_PESQUISA = "contas.receber.pesquisa.php";
        private const string URL_BAIXA = "conta.receber.baixar.php";

        private readonly IAppSettings _appSettings;

        public ContaReceberHttpClient(
            HttpClient httpClient,
            ILogger<ContaReceberHttpClient> logger,
            IAppSettings appSettings
            ) : base(httpClient, logger)
        {
            _appSettings = appSettings;
        }

        public Task<ContasReceberRootDTO?> ConsultarPorNumeroDocAsync(string numeroDoc)
        {
            var filtros = "formato=json";
            filtros += $"&token={_appSettings.Tiny.ApiToken}";
            filtros += $"&numero_doc={numeroDoc}";

            var url = $"{URL_BASE}/{URL_PESQUISA}?{filtros}";

            return GetAsync<ContasReceberRootDTO>(url);
        }

        public Task<ContasReceberBaixaResponseDTO?> BaixarAsync(ContaBaixaDTO contaDTO)
        {
            var contasReceberBaixaRequestRootDTO = new ContasReceberBaixaRequestDTO
            {
                conta = contaDTO
            };

            var parametros = "formato=json";
            parametros += $"&token={_appSettings.Tiny.ApiToken}";
            parametros += $"&conta={JsonSerializer.Serialize(contasReceberBaixaRequestRootDTO)}";

            var url = $"{URL_BASE}/{URL_BAIXA}?{parametros}";

            return PostAsync<ContasReceberBaixaRequestDTO, ContasReceberBaixaResponseDTO>(url, contasReceberBaixaRequestRootDTO);
        }
    }
}
