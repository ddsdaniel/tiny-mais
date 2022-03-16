using TinyMais.Domain.Models;

namespace TinyMais.Domain.Abstractions.Models
{
    public interface IAppSettings
    {
        public Credencial Credencial { get; set; }
    }
}
