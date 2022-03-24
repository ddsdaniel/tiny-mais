namespace TinyMais.Application.Abstractions.AppServices
{
    public interface IBaixarRecebiveisAppService : IAppService
    {
        Task BaixarAsync(DateTime dataInicial, DateTime dataFinal);
    }
}
