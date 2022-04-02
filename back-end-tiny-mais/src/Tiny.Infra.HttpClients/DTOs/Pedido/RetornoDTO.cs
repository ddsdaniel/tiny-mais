namespace Tiny.Infra.HttpClients.DTOs.Pedido
{
    public class RetornoDTO
    {
        public string status_processamento { get; set; }
        public string status { get; set; }
        public PedidoDTO pedido { get; set; }
    }
}
