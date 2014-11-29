using System;
using System.Collections.Generic;
using System.Linq;
using Messages.DTO;
using Messages.Request;
using Repository.Repositories.Interfaces;
using Services.Interfaces;
using EasyNetQ;

namespace Services
{
    public class Subscriptions : ISubscriptions, IMqResponder
    {
        private List<IDisposable> disposables;
        private readonly IBus bus;
        private readonly IUsersRepository usersRepository;

        public Subscriptions(IBus bus, IUsersRepository usersRepository)
        {
            this.bus = bus;
            this.usersRepository = usersRepository;
        }

        public void GetShows()
        {
            disposables.Add(bus.Respond<TvList, TvShowListDTO>(GetUsersShows));
        }

        private TvShowListDTO GetUsersShows(TvList tvList)
        {
            var user = usersRepository.GetAll().FirstOrDefault(x => x.Email == tvList.Email);

            var tvShows = user.Shows.Select(x => new TvShowDTO
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return new TvShowListDTO
            {
                TvShows = tvShows
            };
        }

        public void GetMovies()
        {
            disposables.Add(bus.Respond<MovieList, MovieListDTO>(GetUsersMovies));
        }

        private MovieListDTO GetUsersMovies(MovieList movieList)
        {
            var user = usersRepository.GetAll().FirstOrDefault(x => x.Email == movieList.Email);

            return new MovieListDTO
            {
                Movies = user.Movies.Select(x => new MovieDTO
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList()
            };
        }

        public void GetActors()
        {
            
        }

        public void Start()
        {
            disposables = new List<IDisposable>();

            // Run all methods implemented from the ISearchTV interface
            typeof(ISubscriptions).GetMethods().ToList().ForEach(x => x.Invoke(this, null));
        }

        public void Stop()
        {
            disposables.ForEach(x => x.Dispose());
        }
    }
}
