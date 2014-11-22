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
        private readonly TheMovieDb movieDb;

        public TvShows(IBus bus)
        {
            movieDb = new TheMovieDb();
            this.bus = bus;
        }

        public void AiringToday()
        {
        }

        public void TopRated()
        {
            bus.Respond<TvShow, TvShowListDTO>(request => new TvShowListDTO
            {
                TvShows = movieDb.GetTopRated()
            });
        }

        public void New()
        {
        }

        public void Start()
        {
            disposables = new List<IDisposable>();

            // Run all methods implemented from the ISearchTV interface
            typeof(ITvShows).GetMethods().ToList().ForEach(x => x.Invoke(this, null));
        }

        public void Stop()
        {
            disposables.ForEach(x => x.Dispose());
        }
    }
}
