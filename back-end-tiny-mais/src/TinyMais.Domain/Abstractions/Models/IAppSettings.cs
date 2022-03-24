using TinyMais.Domain.Models;

namespace TinyMais.Domain.Abstractions.Models
{
    public interface IAppSettings
    {
        public TrackCash TrackCash { get; set; }
        public Tiny Tiny { get; set; }
    }
}
