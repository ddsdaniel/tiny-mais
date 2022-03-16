namespace TinyMais.Domain.Abstractions.Validacoes
{
    public abstract class Validavel : IValidavel
    {
        public bool Valido => !Criticas.Any();
        public bool Invalido => !Valido;

        public IEnumerable<string> Criticas { get; private set; }

        public Validavel()
        {
            Criticas = new List<string>();
        }

        protected void Criticar(string critica)
        {
            if (string.IsNullOrEmpty(critica)) return;

            var novaLista = Criticas.ToList();
            novaLista.Add(critica);
            Criticas = novaLista;
        }

        protected void ImportarCriticas(IValidavel validavel)
        {
            if (validavel == null || validavel.Criticas == null) return;

            foreach (var critica in validavel.Criticas)
                Criticar(critica);
        }
    }
}
