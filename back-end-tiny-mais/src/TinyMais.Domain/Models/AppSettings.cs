using TinyMais.Domain.Abstractions.Models;

namespace TinyMais.Domain.Models
{
    public class AppSettings : IAppSettings
    {
        public TrackCash TrackCash { get; set; }
        public Tiny Tiny { get; set; }
    }
}
