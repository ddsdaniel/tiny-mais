namespace Infra.HttpClients.Extensions
{
    public static class StringExtensions
    {
        public static double LerMoedaJson(this string source)
        {
            return Convert.ToDouble(source.Replace(".", ","));
        }
    }
}
