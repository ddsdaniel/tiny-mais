using Tiny.Infra.HttpClients.DTOs.ContasReceber;
using Tiny.Infra.HttpClients.DTOs.ContasReceberBaixa.Request;
using Tiny.Infra.HttpClients.DTOs.ContasReceberBaixa.Response;

namespace Tiny.Infra.HttpClients.Abstractions.HttpClients
{
    public interface IContaReceberHttpClient
    {
        Task<ContasReceberRootDTO?> ConsultarPorIdOrigemAsync(string idOrigem);
        Task<ContasReceberBaixaResponseDTO?> BaixarAsync(ContaBaixaDTO contaDTO);
    }
}