using BLL;
using EasyNetQ;
using Messages.DTO;
using Messages.Request;

namespace Services
{
    public class SearchTv : ISearchTv
    {
        private readonly TheMovieDb movieDb;
        private readonly IBus bus;

        public SearchTv()
        {
            movieDb = new TheMovieDb();
            bus = RabbitHutch.CreateBus("host=localhost;username=watcher;password=watcher");
        }

        public void Search()
        {
            bus.Respond<TvShowSearch, TvShowListDTO>(request => new TvShowListDTO
            {
                TvShows = movieDb.SearchTv(request.Search)
            });
        }

        public void SearchByActor()
        {
            throw new System.NotImplementedException();
        }

        public void SearchById()
        {
            bus.Respond<TvShowSearchById, TvShowDTO>(request => movieDb.GetBy(request.Id));
        }
    }
}