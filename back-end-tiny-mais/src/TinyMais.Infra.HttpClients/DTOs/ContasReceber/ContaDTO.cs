namespace TinyMais.Infra.HttpClients.DTOs.ContasReceber
{
    public class ContaDTO
    {
        public string id { get; set; }
        public string nome_cliente { get; set; }
        public string historico { get; set; }
        public object numero_banco { get; set; }
        public string numero_doc { get; set; }
        public string serie_doc { get; set; }
        public string data_vencimento { get; set; }
        public string situacao { get; set; }
        public string data_emissao { get; set; }
        public string valor { get; set; }
        public string saldo { get; set; }
    }
}
