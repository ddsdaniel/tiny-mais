namespace TrackCash.Infra.HttpClients.DTOs.Payments
{
    public class DatumDTO
    {
        public TotalDTO Total { get; set; }
        public List<PaymentListDTO> List { get; set; }
    }
}
