namespace TinyMais.Infra.ConciliadorFinanceiro.TrackCash.DTOs.Pagamentos
{
    public class TotalDTO
    {
        public string total { get; set; }
        public string current_balance { get; set; }
        public string bank_statement { get; set; }
        public string shipping { get; set; }
        public string shipping_received { get; set; }
        public string mkt_delivery_service { get; set; }
        public string shipping_total { get; set; }
        public string withdrawals { get; set; }
        public string withdrawals_total { get; set; }
        public string automatic_transfers { get; set; }
        public string anticipations { get; set; }
        public string cost_anticipations { get; set; }
        public string total_anticipations { get; set; }
        public string comissions { get; set; }
        public string disputes { get; set; }
        public string refundeds { get; set; }
        public string comissions_refundeds { get; set; }
        public string refundeds_comissions { get; set; }
        public string income_tax { get; set; }
        public string court_lawsuits { get; set; }
        public string others { get; set; }
        public string cost_withdrawals { get; set; }
        public string others_total { get; set; }
        public string cashback { get; set; }
        public string promotions_shipping { get; set; }
        public string rebate { get; set; }
        public string promotions_total { get; set; }
        public object filter_order { get; set; }
        public object filter_status { get; set; }
        public object filter_channel { get; set; }
    }
}
