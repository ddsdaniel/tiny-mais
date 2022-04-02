namespace TrackCash.Infra.HttpClients.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToTrackCashDate(this DateTime source) => source.ToString("yyyy-MM-dd");
    }
}
