using System.Collections.Generic;
using EasyNetQ;
using Watcher.Messages.Movie;
using Watcher.Messages.Person;
using Watcher.Messages.Show;
using Watcher.Service.API;

namespace Watcher.Service.Services
{
    public class OverviewService : IMqService
    {
        private IBus _bus;
        private readonly ITheMovieDb _theMovieDb;

        public OverviewService(
            ITheMovieDb theMovieDb)
        {
            _theMovieDb = theMovieDb;
        }

        public void HandleRequests()
        {
            _bus = RabbitHutch.CreateBus("host=localhost;username=guest;password=guest");
            _bus.Respond<TvShowRequest, List<ShowDto>>(shows => _theMovieDb.TopRated());
            _bus.Respond<TvShowSearch, List<ShowDto>>(request => _theMovieDb.SearchTvShows(request.Search));

            _bus.Respond<MovieRequest, List<MovieDto>>(movies => _theMovieDb.Upcoming());
            _bus.Respond<MovieSearch, List<MovieDto>>(movies => _theMovieDb.SearchMovie(movies.Search));

            _bus.Respond<PersonRequest, List<PersonDto>>(persons => _theMovieDb.Populair());
            _bus.Respond<PersonSearch, List<PersonDto>>(persons => _theMovieDb.SearchPerson(persons.Search));
        }
    }
}