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
    public class SearchTv : ISearchTv, IMqResponder
    {
        private List<IDisposable> disposables;
        private readonly TheMovieDb movieDb;
        private readonly IBus bus;

        public SearchTv(IBus bus)
        {
            this.bus = bus;
            movieDb = new TheMovieDb(); // todo: get rid of this
        }

        public void Start()
        {
            disposables = new List<IDisposable>();

            // Run all methods implemented from the ISearchTV interface
            typeof (ISearchTv).GetMethods().ToList().ForEach(x => x.Invoke(this, null));
        }

        public void Stop()
        {
            disposables.ForEach(x => x.Dispose());
        }

        public void Search()
        {
            disposables.Add(bus.Respond<TvShowSearch, TvShowListDTO>(request => new TvShowListDTO {TvShows = movieDb.SearchTv(request.Search)}));
        }

        public void SearchByActor()
        {
            disposables.Add(bus.Respond<TvShowSearchByActor, TvShowListDTO>(request => new TvShowListDTO {TvShows = movieDb.SearchTv(request.Actor)}));
        }

        public void SearchById()
        {
            disposables.Add(bus.Respond<TvShowSearchById, TvShowDTO>(request => movieDb.GetBy(request.Id)));
        }
    }
}