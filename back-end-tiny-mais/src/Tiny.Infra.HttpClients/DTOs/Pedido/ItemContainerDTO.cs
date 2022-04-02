namespace Tiny.Infra.HttpClients.DTOs.Pedido
{
    public class ItemContainerDTO
    {
        public string id_produto { get; set; }
        public string codigo { get; set; }
        public string descricao { get; set; }
        public string unidade { get; set; }
        public string quantidade { get; set; }
        public string valor_unitario { get; set; }
    }
}
