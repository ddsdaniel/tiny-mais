using TrackCash.Infra.HttpClients.DTOs.Orders;
using TrackCash.Infra.HttpClients.Enums;

namespace TrackCash.Infra.HttpClients.Abstractions.HttpClients
{
    public interface IOrderHttpClient
    {
        Task<OrdersDTO?> ConsultarAsync(DateTime dataInicial, DateTime dataFinal, StatusPedido status);
    }
}