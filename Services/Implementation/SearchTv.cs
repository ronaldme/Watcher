using System.Collections.Generic;
using Backend;
using Models;

namespace Services
{
    public class SearchTv : ISearchTv
    {
        private readonly TheMovieDb movieDb;

        public SearchTv()
        {
            movieDb = new TheMovieDb();    
        }

        public List<Show> Search(string input)
        {
            return movieDb.SearchTv(input);
        }

        public List<Show> SearchByActor(string actorName)
        {
            throw new System.NotImplementedException();
        }

        public Show SearchById(int id)
        {
            return movieDb.GetBy(id);
        }
    }
}