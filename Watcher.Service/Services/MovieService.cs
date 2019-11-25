using System.Collections.Generic;
using EasyNetQ;
using Watcher.Messages.Movie;
using Watcher.Service.Infrastructure;
using Watcher.Service.TheMovieDb;

namespace Watcher.Service.Services
{
    public class MovieService : IService
    {
        private readonly IBus bus;
        private readonly ITheMovieDb theMovieDb;

        public MovieService(IBus bus,
            ITheMovieDb theMovieDb)
        {
            this.bus = bus;
            this.theMovieDb = theMovieDb;
        }

        public void HandleRequests()
        {
            bus.Respond<MovieRequest, List<MovieDto>>(movies => theMovieDb.Upcoming());
            bus.Respond<MovieSearch, List<MovieDto>>(movies => theMovieDb.SearchMovie(movies.Search));
        }
    }
}
