namespace TinyMais.Infra.ConciliadorFinanceiro.TrackCash.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToTrackCashDate(this DateTime source) => source.ToString("yyyy-MM-dd");
    }
}
