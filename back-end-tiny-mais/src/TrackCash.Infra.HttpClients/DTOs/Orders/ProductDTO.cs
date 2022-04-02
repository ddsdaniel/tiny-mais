namespace TrackCash.Infra.HttpClients.DTOs.Orders
{
    public class ProductDTO
    {
        public string sku { get; set; }
        public string quantity { get; set; }
        public string selling_price { get; set; }
        public string discount { get; set; }
    }
}
