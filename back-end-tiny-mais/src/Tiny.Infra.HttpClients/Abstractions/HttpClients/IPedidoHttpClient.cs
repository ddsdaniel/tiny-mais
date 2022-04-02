using Tiny.Infra.HttpClients.DTOs.Pedido;

namespace Tiny.Infra.HttpClients.Abstractions.HttpClients
{
    public interface IPedidoHttpClient
    {
        Task<PedidoRootDTO?> ConsultarPorIdAsync(string id);
    }
}