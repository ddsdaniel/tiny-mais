namespace TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Enums
{
    public enum StatusPedido
    {
        NaoIdentificado = 1,
        AguardandoPagamento = 2,
        Processando = 3,
        Enviado = 4,
        Entregue = 5,
        Cancelado = 6,
        Troca = 7,
        Devolucao = 8,
        NaoRelacionado = 9,
        PedidoIncompleto = 10
    }
}
