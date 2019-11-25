using Watcher.Messages.Show;

namespace Watcher.Service.TheMovieDb
{
    public class CurrentNextSeason
    {
        public Season Current { get; set; }
        public Season Next { get; set; }
    }
}
