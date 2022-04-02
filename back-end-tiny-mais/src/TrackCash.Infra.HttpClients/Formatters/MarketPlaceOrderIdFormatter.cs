using TrackCash.Infra.HttpClients.Abstractions.Formatters;

namespace TrackCash.Infra.HttpClients.Formatters
{
    public class MarketPlaceOrderIdFormatter : IMarketPlaceOrderIdFormatter
    {
        public string Formatar(string orderId, string channel)
        {
            switch (channel)
            {
                case "Carrefour": return $"{orderId}-A";
                default: return orderId;
            }
        }
    }
}
