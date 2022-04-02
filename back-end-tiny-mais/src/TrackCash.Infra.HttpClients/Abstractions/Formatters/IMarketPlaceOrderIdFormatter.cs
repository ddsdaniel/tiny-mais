namespace TrackCash.Infra.HttpClients.Abstractions.Formatters
{
    public interface IMarketPlaceOrderIdFormatter
    {
        string Formatar(string orderId, string channel);
    }
}