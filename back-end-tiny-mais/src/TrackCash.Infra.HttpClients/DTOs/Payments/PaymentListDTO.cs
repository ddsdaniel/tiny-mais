namespace TrackCash.Infra.HttpClients.DTOs.Payments
{
    public class PaymentListDTO
    {
        public List<OrderDTO> order { get; set; }
        public List<PaymentDTO> payments { get; set; }
    }
}
