namespace Tiny.Infra.HttpClients.DTOs.Pedido
{
    public class ClienteDTO
    {
        public string nome { get; set; }
        public string codigo { get; set; }
        public object nome_fantasia { get; set; }
        public string tipo_pessoa { get; set; }
        public string cpf_cnpj { get; set; }
        public string ie { get; set; }
        public string rg { get; set; }
        public string endereco { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string fone { get; set; }
        public string email { get; set; }
        public string cep { get; set; }
    }
}
