namespace Tiny.Infra.HttpClients.DTOs.ContasReceberBaixa.Request
{
    public class ContaBaixaDTO
    {
        public string id { get; set; }
        public string contaDestino { get; set; }
        public string data { get; set; }
        public string categoria { get; set; }
        public string historico { get; set; }
        public double valorTaxas { get; set; }
        public double valorJuros { get; set; }
        public double valorDesconto { get; set; }
        public double valorAcrescimo { get; set; }
        public double valorPago { get; set; }
    }
}
