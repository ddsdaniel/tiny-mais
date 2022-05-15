namespace TrackCash.Infra.HttpClients.DTOs.Payments
{
    public class OrderDTO
    {
        public string order_id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string invoice { get; set; }
        public string mkp_order_id { get; set; }
        public string mkp_channel { get; set; }
        public string mkp_account { get; set; }
        public string mkp_status { get; set; }
        public string mkp_partial_total { get; set; }
        public string mkp_discount { get; set; }
        public string mkp_fee { get; set; }
        public string mkp_shipping_customer { get; set; }
        public string mkp_shipping_shopkeeper_payments { get; set; }
        public string logis_service_value_fulfilled { get; set; }
        public string mkp_total { get; set; }
        public string mkp_refunded { get; set; }
        public string conciliation_payments { get; set; }
        public string mkp_id_channel { get; set; }
        //public string Product: []
    }
}
