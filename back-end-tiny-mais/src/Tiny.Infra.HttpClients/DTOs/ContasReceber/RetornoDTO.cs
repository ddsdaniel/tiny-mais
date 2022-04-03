namespace Tiny.Infra.HttpClients.DTOs.ContasReceber
{
    public class RetornoDTO
    {
        public string status_processamento { get; set; }
        public string status { get; set; }//TODO: checar status em todas as requisicoes
        public int pagina { get; set; }
        public int numero_paginas { get; set; }
        public List<ContaContainerDTO> contas { get; set; }
    }
}
