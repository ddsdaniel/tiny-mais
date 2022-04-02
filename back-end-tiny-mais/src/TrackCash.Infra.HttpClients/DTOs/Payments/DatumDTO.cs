namespace TrackCash.Infra.HttpClients.DTOs.Payments
{
    public class DatumDTO
    {
        public TotalDTO Total { get; set; }
        public List<ListDTO> List { get; set; }
    }
}
