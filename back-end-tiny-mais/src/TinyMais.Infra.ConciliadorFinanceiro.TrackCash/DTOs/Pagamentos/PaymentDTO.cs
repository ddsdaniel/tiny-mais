﻿namespace TinyMais.Infra.ConciliadorFinanceiro.TrackCash.DTOs.Pagamentos
{
    public class PaymentDTO
    {
        public string date { get; set; }
        public object name { get; set; }
        public string id_channel { get; set; }
        public string channel { get; set; }
        public string id_withdrawal { get; set; }
        public string mkp_order_id { get; set; }
        public string account { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string current_installment { get; set; }
        public string installments { get; set; }
        public string value { get; set; }
    }
}
