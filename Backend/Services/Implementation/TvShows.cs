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
    public class TvShows : ITvShows, IMqResponder
    {
        private List<IDisposable> disposables;
        private readonly IBus bus;
        private readonly ITheMovieDb theMovieDb;

        public TvShows(IBus bus, ITheMovieDb theMovieDb)
        {
            this.bus = bus;
            this.theMovieDb = theMovieDb;
        }

        public void TopRated()
        {
            disposables.Add(bus.Respond<TvShow, TvShowListDTO>(request => new TvShowListDTO
            {
                TvShows = theMovieDb.GetTopRated()
            }));
        }

        public void AiringToday()
        {
        }

        public void New()
        {
        }

        public void Start()
        {
            disposables = new List<IDisposable>();

            // Run all methods implemented from the ITvShow interface
            typeof(ITvShows).GetMethods().ToList().ForEach(x => x.Invoke(this, null));
        }

        public void Stop()
        {
            disposables.ForEach(x => x.Dispose());
        }
    }
}
