namespace Tiny.Infra.HttpClients.DTOs.ContasReceberBaixa.Response
{
    public class RetornoDTO
    {
        //public string status_processamento { get; set; }
        public string status { get; set; }
        //public string codigo_erro { get; set; }
        public List<ErroDTO> erros { get; set; }
        public List<RegistroContainerDTO> registros { get; set; }
    }
}
