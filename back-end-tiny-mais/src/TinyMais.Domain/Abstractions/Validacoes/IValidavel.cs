namespace TinyMais.Domain.Abstractions.Validacoes
{
    public interface IValidavel
    {
        bool Valido { get; }
        bool Invalido { get; }
        IEnumerable<string> Criticas { get; }
    }
}
