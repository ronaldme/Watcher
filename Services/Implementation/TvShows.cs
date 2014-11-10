using System;
using System.Collections.Generic;
using Backend;
using Models;

namespace Services
{
    public class TvShows : ITvShows
    {
        private readonly TheMovieDb movieDb;

        public TvShows()
        {
            movieDb = new TheMovieDb();    
        }

        public List<Show> AiringToday()
        {
            throw new NotImplementedException();
        }

        public List<Show> TopRated()
        {
            return movieDb.GetTopRated();
        }

        public List<Show> New(int ageInWeeks)
        {
            throw new NotImplementedException();
        }
    }
}
