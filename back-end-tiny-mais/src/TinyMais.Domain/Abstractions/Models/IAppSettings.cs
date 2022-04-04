using TinyMais.Domain.Models;

namespace TinyMais.Domain.Abstractions.Models
{
    public interface IAppSettings
    {
        public TrackCash TrackCash { get; set; }
        public Tiny Tiny { get; set; }
        public IEnumerable<CodigoMarketPlace> CodigosMarketPlace { get; set; }
    }
}
