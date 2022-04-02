namespace TrackCash.Infra.HttpClients.DTOs.Payments
{
    public class ListDTO
    {
        public List<OrderDTO> order { get; set; }
        public List<PaymentDTO> payments { get; set; }
    }
}
