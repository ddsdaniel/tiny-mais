namespace TrackCash.Infra.HttpClients.DTOs.Payments
{
    public class RootDTO
    {
        public List<DatumDTO> data { get; set; }
        public LinksDTO links { get; set; }
        public MetaDTO meta { get; set; }
    }
}
