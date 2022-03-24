using TinyMais.Infra.ConciliadorFinanceiro.TrackCash.DTOs.Pagamentos;

namespace TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Abstractions
{
    public interface IPagamentoHttpClient
    {
        Task<PaymentResultDTO?> ConsultarAsync(DateTime dataInicial, DateTime dataFinal);
    }
}