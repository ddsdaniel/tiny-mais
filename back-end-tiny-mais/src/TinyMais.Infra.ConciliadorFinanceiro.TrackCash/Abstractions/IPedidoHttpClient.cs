using TinyMais.Infra.ConciliadorFinanceiro.TrackCash.DTOs.Pedidos;
using TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Enums;

namespace TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Abstractions
{
    public interface IPedidoHttpClient
    {
        Task<OrdersDTO?> ConsultarAsync(DateTime dataInicial, DateTime dataFinal, StatusPedido status);
    }
}