using TrackCash.Infra.HttpClients.DTOs.Payments;

namespace TrackCash.Infra.HttpClients.Abstractions.HttpClients
{
    public interface IPaymentHttpClient
    {
        Task<RootDTO?> ConsultarPorDataAsync(DateTime dataInicial, DateTime dataFinal);
    }
}