using Tiny.Infra.HttpClients.DTOs.NotaFiscal;

namespace Tiny.Infra.HttpClients.Abstractions.HttpClients
{
    public interface INotaFiscalHttpClient
    {
        Task<NotaFiscalRootDTO?> ConsultarPorIdAsync(string id);
    }
}