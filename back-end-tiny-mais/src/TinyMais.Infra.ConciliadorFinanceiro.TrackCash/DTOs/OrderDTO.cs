namespace TinyMais.Infra.ConciliadorFinanceiro.TrackCash.DTOs
{
    public class OrderDTO
    {
        public string id_order { get; set; }
        public string invoice { get; set; }
        public string status { get; set; }
        public string date { get; set; }
        public string partial_total { get; set; }
        public string taxes { get; set; }
        public string discount { get; set; }
        public string type_factor { get; set; }
        public string type_factor_value { get; set; }
        public string logis_shipping_preview { get; set; }
        public string shipment { get; set; }
        public string shipment_value { get; set; }
        public string shipment_code { get; set; }
        public string shipment_date { get; set; }
        public string delivered { get; set; }
        public string paid { get; set; }
        public string refunded { get; set; }
        public string total { get; set; }
        public List<ProductDTO> products { get; set; }
        public string point_sale { get; set; }
        public string point_sale_code { get; set; }
    }
}
