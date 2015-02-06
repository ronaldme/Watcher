using System;
using System.Collections.Generic;
using System.Linq;
using BLL;
using EasyNetQ;
using Messages.DTO;
using Messages.Request;
using Services.Interfaces;

namespace Services
{
    public class MovieService : IMovieService, IMqResponder
    {
        private List<IDisposable> disposables;
        private readonly IBus bus;
        private readonly ITheMovieDb theMovieDb;

        public MovieService(IBus bus, ITheMovieDb theMovieDb)
        {
            this.bus = bus;
            this.theMovieDb = theMovieDb;
        }

        public void Start()
        {
            disposables = new List<IDisposable>();
            typeof(IMovieService).GetMethods().ToList().ForEach(x => x.Invoke(this, null));
        }

        public void Stop()
        {
            disposables.ForEach(x => x.Dispose());
        }

        public void Upcoming()
        {
            disposables.Add(bus.Respond<MovieRequest, MovieListDTO>(x => new MovieListDTO
            {
                Movies = theMovieDb.Upcoming(),
                PrefixPath = Urls.PrefixImages
            }));
        }

        public void Search()
        {
            disposables.Add(bus.Respond<MovieSearch, MovieListDTO>(x => new MovieListDTO
            {
                Movies = theMovieDb.SearchMovie(x.Search),
                PrefixPath = Urls.PrefixImages
            }));
        }
    }
}
