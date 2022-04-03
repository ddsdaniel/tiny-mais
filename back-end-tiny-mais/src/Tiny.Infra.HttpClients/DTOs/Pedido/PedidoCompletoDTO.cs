namespace Tiny.Infra.HttpClients.DTOs.Pedido
{
    public class PedidoCompletoDTO
    {
        public string id { get; set; }
        public string numero { get; set; }
        public string numero_ecommerce { get; set; }
        public string data_pedido { get; set; }
        public string data_prevista { get; set; }
        public string data_faturamento { get; set; }
        public string data_envio { get; set; }
        public string data_entrega { get; set; }
        public object id_lista_preco { get; set; }
        public string descricao_lista_preco { get; set; }
        public ClienteDTO cliente { get; set; }
        public List<ItemDTO> itens { get; set; }
        public List<ParcelaContainerDTO> parcelas { get; set; }
        public List<MarcadorDTO> marcadores { get; set; }
        public string condicao_pagamento { get; set; }
        public string forma_pagamento { get; set; }
        public object meio_pagamento { get; set; }
        public string nome_transportador { get; set; }
        public string frete_por_conta { get; set; }
        public string valor_frete { get; set; }
        public string valor_desconto { get; set; }
        public string outras_despesas { get; set; }
        public string total_produtos { get; set; }
        public string total_pedido { get; set; }
        public string numero_ordem_compra { get; set; }
        public string deposito { get; set; }
        public EcommerceDTO ecommerce { get; set; }
        public string forma_envio { get; set; }
        public string situacao { get; set; }
        public string obs { get; set; }
        public string obs_interna { get; set; }
        public string id_vendedor { get; set; }
        public string codigo_rastreamento { get; set; }
        public string url_rastreamento { get; set; }
        public string id_nota_fiscal { get; set; }
        public IntermediadorDTO intermediador { get; set; }
    }
}
