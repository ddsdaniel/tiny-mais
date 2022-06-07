using TinyMais.Domain.Models;

namespace TrackCash.Infra.HttpClients.Abstractions.Factories
{
    public interface IMarketPlaceConfigFactory
    {
        CodigoMarketPlace Obter(string channel, string channelId);
    }
}
