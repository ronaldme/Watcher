using System;
using System.Collections.Generic;
using BLL;
using EasyNetQ;
using Messages.DTO;
using Messages.Request;

namespace Services
{
    public class TvShows : ITvShows
    {
        private readonly TheMovieDb movieDb;
        private readonly IBus bus;

        public TvShows()
        {
            movieDb = new TheMovieDb();
            bus = RabbitHutch.CreateBus("host=localhost;username=watcher;password=watcher");
        }

        public List<TvShowDTO> AiringToday()
        {
            throw new NotImplementedException();
        }

        public void TopRated()
        {
            bus.Respond<TvShow, TvShowListDTO>(request => new TvShowListDTO
            {
                TvShows = movieDb.GetTopRated()
            });
        }

        public List<TvShowDTO> New(int ageInWeeks)
        {
            throw new NotImplementedException();
        }
    }
}
