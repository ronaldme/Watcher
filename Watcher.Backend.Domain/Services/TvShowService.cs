using System.Collections.Generic;
using EasyNetQ;
using Watcher.Backend.Domain.Infrastructure;
using Watcher.Messages.Show;

namespace Watcher.Backend.Domain.Services
{
    public class TvShowService : IService
    {
        private readonly IBus bus;
        private readonly ITheMovieDb theMovieDb;

        public TvShowService(IBus bus,
            ITheMovieDb theMovieDb)
        {
            this.bus = bus;
            this.theMovieDb = theMovieDb;
        }

        public void HandleRequests()
        {
            bus.Respond<TvShowRequest, List<ShowDto>>(shows => theMovieDb.TopRated());
            bus.Respond<TvShowSearch, List<ShowDto>>(request => theMovieDb.SearchTvShows(request.Search));
        }
    }
}