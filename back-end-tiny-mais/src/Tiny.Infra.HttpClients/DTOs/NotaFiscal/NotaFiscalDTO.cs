namespace Tiny.Infra.HttpClients.DTOs.NotaFiscal
{
    public class NotaFiscalDTO
    {
        public string id { get; set; }
        public string tipo_nota { get; set; }
        public string natureza_operacao { get; set; }
        public string regime_tributario { get; set; }
        public string finalidade { get; set; }
        public string serie { get; set; }
        public string numero { get; set; }
        public string numero_ecommerce { get; set; }
        public string data_emissao { get; set; }
        public string data_saida { get; set; }
        public string hora_saida { get; set; }
        public ClienteDTO cliente { get; set; }
        public EnderecoEntregaDTO endereco_entrega { get; set; }
        public List<ItemContainerDTO> itens { get; set; }
        public string base_icms { get; set; }
        public string valor_icms { get; set; }
        public string base_icms_st { get; set; }
        public string valor_icms_st { get; set; }
        public string valor_servicos { get; set; }
        public string valor_produtos { get; set; }
        public string valor_frete { get; set; }
        public string valor_seguro { get; set; }
        public string valor_outras { get; set; }
        public string valor_ipi { get; set; }
        public string valor_issqn { get; set; }
        public string valor_nota { get; set; }
        public string valor_desconto { get; set; }
        public string valor_faturado { get; set; }
        public string frete_por_conta { get; set; }
        public TransportadorDTO transportador { get; set; }
        public string placa { get; set; }
        public string uf_placa { get; set; }
        public string quantidade_volumes { get; set; }
        public string especie_volumes { get; set; }
        public string marca_volumes { get; set; }
        public string numero_volumes { get; set; }
        public string peso_bruto { get; set; }
        public string peso_liquido { get; set; }
        public FormaEnvioDTO forma_envio { get; set; }
        public FormaFreteDTO forma_frete { get; set; }
        public string codigo_rastreamento { get; set; }
        public string url_rastreamento { get; set; }
        public string condicao_pagamento { get; set; }
        public string forma_pagamento { get; set; }
        public object meio_pagamento { get; set; }
        public List<ParcelaContainerDTO> parcelas { get; set; }
        public string id_venda { get; set; }
        public string id_vendedor { get; set; }
        public string nome_vendedor { get; set; }
        public string situacao { get; set; }
        public string descricao_situacao { get; set; }
        public string obs { get; set; }
        public string chave_acesso { get; set; }
        public IntermediadorDTO intermediador { get; set; }
    }


}
