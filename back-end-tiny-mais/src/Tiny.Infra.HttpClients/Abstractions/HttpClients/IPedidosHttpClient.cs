using Tiny.Infra.HttpClients.DTOs.Pedidos;

namespace Tiny.Infra.HttpClients.Abstractions.HttpClients
{
    public interface IPedidosHttpClient
    {
        Task<PedidosRootDTO?> ConsultarPorNumeroEcommerceAsync(string numeroEcommerce);
    }
}