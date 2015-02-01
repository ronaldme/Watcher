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
            disposables.Add(bus.Respond<MovieRequest, List<MovieDTO>>(x => new List<MovieDTO>(theMovieDb.Upcoming())));
        }

        public void Search()
        {
            disposables.Add(bus.Respond<MovieSearch, List<MovieDTO>>(x => new List<MovieDTO>(theMovieDb.SearchMovie(x.Search))));
        }
    }
}
