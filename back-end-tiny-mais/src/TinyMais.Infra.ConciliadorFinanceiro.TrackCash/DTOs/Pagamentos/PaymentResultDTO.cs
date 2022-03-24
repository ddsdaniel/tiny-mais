namespace TinyMais.Infra.ConciliadorFinanceiro.TrackCash.DTOs.Pagamentos
{
    public class PaymentResultDTO
    {
        public List<DatumDTO> data { get; set; }
        public LinksDTO links { get; set; }
        public MetaDTO meta { get; set; }
    }
}
