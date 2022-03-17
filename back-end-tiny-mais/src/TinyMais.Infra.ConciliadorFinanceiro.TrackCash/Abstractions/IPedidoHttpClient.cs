using TinyMais.Infra.ConciliadorFinanceiro.TrackCash.DTOs;
using TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Enums;

namespace TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Abstractions
{
    public interface IPedidoHttpClient
    {
        Task<OrdersDTO?> Consultar(DateTime dataInicial, DateTime dataFinal, StatusPedido status);
    }
}