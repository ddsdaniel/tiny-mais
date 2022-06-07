using TrackCash.Infra.HttpClients.Abstractions.Factories;
using TrackCash.Infra.HttpClients.Abstractions.Formatters;

namespace TrackCash.Infra.HttpClients.Formatters
{
    public class MarketPlaceOrderIdFormatter : IMarketPlaceOrderIdFormatter
    {
        private readonly IMarketPlaceConfigFactory _marketPlaceConfigFactory;

        public MarketPlaceOrderIdFormatter(IMarketPlaceConfigFactory marketPlaceConfigFactory)
        {
            _marketPlaceConfigFactory = marketPlaceConfigFactory;
        }

        public string Formatar(string orderId, string channel, string channelId)
        {
            var codigoMarketPlace = _marketPlaceConfigFactory.Obter(channel, channelId);

            return codigoMarketPlace == null
                ? orderId
                : codigoMarketPlace.Codigo.Replace("{id}", orderId);
        }
    }
}
