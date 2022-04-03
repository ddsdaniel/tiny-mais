namespace Tiny.Infra.HttpClients.DTOs.ContasReceberBaixa.Response
{
    public class RetornoDTO
    {
        public int status_processamento { get; set; }
        public string status { get; set; }
        public int codigo_erro { get; set; }
        public List<ErroDTO> erros { get; set; }
        public List<RegistroContainerDTO> registros { get; set; }
    }
}
