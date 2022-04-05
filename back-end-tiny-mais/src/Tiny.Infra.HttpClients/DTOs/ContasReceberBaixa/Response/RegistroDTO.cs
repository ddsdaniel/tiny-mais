namespace Tiny.Infra.HttpClients.DTOs.ContasReceberBaixa.Response
{
    public class RegistroDTO
    {
        //public string sequencia { get; set; }
        public string status { get; set; }
        //public string codigo_erro { get; set; }
        public string id { get; set; }
        public List<ErroDTO> erros { get; set; }
    }
}
