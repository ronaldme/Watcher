using System;
using System.Collections.Generic;
using System.Linq;
using BLL;
using EasyNetQ;
using Messages.DTO;
using Messages.Request;
using Repository.Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class MovieService : IMovieService, IMqResponder
    {
        private List<IDisposable> disposables;
        private readonly IBus bus;
        private readonly IMovieRepository movieRepository;
        private readonly ITheMovieDb theMovieDb;

        public MovieService(IBus bus, IMovieRepository movieRepository, ITheMovieDb theMovieDb)
        {
            this.bus = bus;
            this.movieRepository = movieRepository;
            this.theMovieDb = theMovieDb;
        }

        public void Upcoming()
        {
            disposables.Add(bus.Respond<MovieRequest, List<MovieDTO>>(x => new List<MovieDTO>(theMovieDb.Upcoming())));
        }

        public void Search()
        {
            disposables.Add(bus.Respond<MovieSearch, List<MovieDTO>>(x => new List<MovieDTO>(theMovieDb.SearchMovie(x.Search))));

        }

        public void SearchByActor()
        {
            
        }

        public void SearchById()
        {
           
        }

        public List<MovieDTO> Test()
        {
            return null;
        }

        public void Start()
        {
            disposables = new List<IDisposable>();

            // Run all methods implemented from the ISearchTV interface
            typeof(IMovieService).GetMethods().ToList().ForEach(x => x.Invoke(this, null));
        }

        public void Stop()
        {
            disposables.ForEach(x => x.Dispose());
        }
    }
}
