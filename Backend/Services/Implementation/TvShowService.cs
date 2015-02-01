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
    public class TvShowService : ITvShowService, IMqResponder
    {
        private List<IDisposable> disposables;
        private readonly IBus bus;
        private readonly ITheMovieDb theMovieDb;

        public TvShowService(IBus bus, ITheMovieDb theMovieDb)
        {
            this.bus = bus;
            this.theMovieDb = theMovieDb;
        }

        public void Start()
        {
            disposables = new List<IDisposable>();
            typeof(ITvShowService).GetMethods().ToList().ForEach(x => x.Invoke(this, null));
        }

        public void Stop()
        {
            disposables.ForEach(x => x.Dispose());
        }

        public void TopRated()
        {
            disposables.Add(bus.Respond<TvShow, TvShowListDTO>(request => new TvShowListDTO { TvShows = theMovieDb.TopRated()}));
        }

        public void Search()
        {
            disposables.Add(bus.Respond<TvShowSearch, TvShowListDTO>(request => new TvShowListDTO { TvShows = theMovieDb.SearchTv(request.Search) }));
        }
    }
}