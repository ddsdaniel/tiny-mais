using TinyMais.Infra.HttpClients.DTOs.ContasReceber;

namespace TinyMais.Infra.HttpClients.Abstractions.HttpClients
{
    public interface IContaReceberHttpClient
    {
        Task<RootDTO?> ConsultarPorIdOrigemAsync(string idOrigem);
    }
}