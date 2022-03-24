namespace TinyMais.Infra.ConciliadorFinanceiro.TrackCash.DTOs.Pagamentos
{
    public class ListDTO
    {
        public object order { get; set; }
        public List<PaymentDTO> payments { get; set; }
    }
}
