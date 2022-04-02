namespace Tiny.Infra.HttpClients.DTOs.NotaFiscal
{
    public class RetornoDTO
    {
        public string status_processamento { get; set; }
        public string status { get; set; }
        public NotaFiscalDTO nota_fiscal { get; set; }
    }


}
