using TinyMais.Domain.Abstractions.Models;
using TrackCash.Infra.HttpClients.Abstractions.Formatters;

namespace TrackCash.Infra.HttpClients.Formatters
{
    public class MarketPlaceOrderIdFormatter : IMarketPlaceOrderIdFormatter
    {
        private readonly IAppSettings _appSettings;

        public MarketPlaceOrderIdFormatter(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public string Formatar(string orderId, string channel)
        {
            var codigoMarketPlace = _appSettings.CodigosMarketPlace
                .FirstOrDefault(cm => cm.MarketPlace.ToUpper() == channel.ToUpper());

            return codigoMarketPlace == null
                ? orderId
                : codigoMarketPlace.Codigo.Replace("{id}", orderId);
        }
    }
}
