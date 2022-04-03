namespace TrackCash.Infra.HttpClients.Extensions
{
    public static class StringExtensions
    {
        public static double LerMoedaTrackCash(this string source)
        {
            return Convert.ToDouble(source.Replace(".", ","));
        }
    }
}
