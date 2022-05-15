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

        public string Formatar(string orderId, string channel, string channelId)
        {
            if (String.IsNullOrWhiteSpace(channel))
                channel = ObterChannel(channelId);

            var codigoMarketPlace = _appSettings.CodigosMarketPlace
                .FirstOrDefault(cm => cm.MarketPlace.ToUpper() == channel.ToUpper());

            return codigoMarketPlace == null
                ? orderId
                : codigoMarketPlace.Codigo.Replace("{id}", orderId);
        }

        private string ObterChannel(string channelId)
        {
            switch (channelId)
            {
                case "1": return "Amazon";
                case "6": return "Magazine Luiza";
                case "12": return "Mercado Livre";
                case "13": return "Cnova";
                case "14": return "Carrefour";
                case "15": return "Netshoes";
                case "100": return "B2w";
                case "101": return "Leroy Merlin";
                case "113": return "Madeira Madeira";
                case "114": return "Dafiti";
                case "141": return "Pagseguro";
                case "142": return "Wirecard";
                case "143": return "Pagar.me";
                case "144": return "Zoom";
                case "159": return "GetNet";
                case "160": return "Redecard";
                case "165": return "Cielo";
                case "169": return "Ticket";
                case "172": return "Sodexo";
                case "179": return "VR Benefícios";
                case "196": return "Stone";
                case "202": return "SafraPay";
                case "431": return "Alpe";
                case "443": return "Ame";
                case "444": return "Shopee";
                default:
                    throw new Exception("Market place não informado pela Track Cash.");
            }
        }
    }
}
