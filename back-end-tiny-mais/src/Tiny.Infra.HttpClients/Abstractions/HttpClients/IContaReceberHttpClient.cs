using Tiny.Infra.HttpClients.DTOs.ContasReceber;

namespace Tiny.Infra.HttpClients.Abstractions.HttpClients
{
    public interface IContaReceberHttpClient
    {
        Task<ContasReceberRootDTO?> ConsultarPorIdOrigemAsync(string idOrigem);
    }
}