using System.Collections.Generic;
using EasyNetQ;
using Microsoft.Extensions.Options;
using Watcher.Common;
using Watcher.Messages.Movie;
using Watcher.Messages.Person;
using Watcher.Messages.Show;
using Watcher.Service.API;

namespace Watcher.Service.Services
{
    class OverviewService : IMqService
    {
        private readonly IBus _bus;
        private readonly ITheMovieDb _theMovieDb;
        private readonly string _theMovieDbApi;

        public OverviewService(IOptions<AppSettings> config,
            IBus bus,
            ITheMovieDb theMovieDb)
        {
            _theMovieDbApi = config.Value.TheMovieDbApi;
            _bus = bus;
            _theMovieDb = theMovieDb;
        }

        public void HandleRequests()
        {
            _bus.Respond<TvShowRequest, List<ShowDto>>(shows => _theMovieDb.PopularShows());
            _bus.Respond<TvShowSearchQuery, List<ShowDto>>(request => _theMovieDb.SearchTvShows(request.Search));

            _bus.Respond<MovieRequest, List<MovieDto>>(movies => _theMovieDb.PopularMovies());
            _bus.Respond<MovieSearchQuery, List<MovieDto>>(movies => _theMovieDb.SearchMovies(movies.Search));

            _bus.Respond<PersonRequest, List<PersonDto>>(persons => _theMovieDb.PopularPersons());
            _bus.Respond<PersonSearchQuery, List<PersonDto>>(persons => _theMovieDb.SearchPersons(persons.Search));
        }
    }
}