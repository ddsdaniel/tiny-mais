namespace Tiny.Infra.HttpClients.DTOs.Pedidos
{
    public class PedidoResumidoDTO
    {
        public string id { get; set; }
        public string numero { get; set; }
        public string numero_ecommerce { get; set; }
        public string data_pedido { get; set; }
        public string data_prevista { get; set; }
        public string nome { get; set; }
        public double valor { get; set; }
        public string id_vendedor { get; set; }
        public string nome_vendedor { get; set; }
        public string situacao { get; set; }
        public string codigo_rastreamento { get; set; }
        public string url_rastreamento { get; set; }
    }


}
