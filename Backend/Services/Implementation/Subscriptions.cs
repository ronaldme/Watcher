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

        public void Start()
        {
            disposables = new List<IDisposable>();

            // Run all methods implemented from the ISearchTV interface
            typeof(ISubscriptions).GetMethods().ToList().ForEach(x => x.Invoke(this, null));

            disposables.Add(bus.Respond<SubscriptionRequest, SubscriptionListDTO>(GetSubscriptions));
        }

        public void Stop()
        {
            disposables.ForEach(x => x.Dispose());
        }

        public void GetShows()
        {
            disposables.Add(bus.Respond<TvList, TvShowListDTO>(GetUsersShows));
        }

        private TvShowListDTO GetUsersShows(TvList tvList)
        {
            var user = usersRepository.All().FirstOrDefault(x => x.Email == tvList.Email);

            if (user != null)
            {
                return new TvShowListDTO
                {
                    TvShows = user.Shows.Select(x => new TvShowDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ReleaseNextEpisode = x.ReleaseDate
                    }).ToList()
                };
            }
            return new TvShowListDTO();
        }

        public void GetMovies()
        {
            disposables.Add(bus.Respond<MovieList, MovieListDTO>(GetUsersMovies));
        }

        private MovieListDTO GetUsersMovies(MovieList movieList)
        {
            var user = usersRepository.All().FirstOrDefault(x => x.Email == movieList.Email);

            if (user != null)
            {
                return new MovieListDTO
                {
                    Movies =
                    user.Movies.Select(x => new MovieDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ReleaseDate = x.ReleaseDate.Value

                    }).ToList()
                };    
            }
            return new MovieListDTO();
        }

        public void GetPersons()
        {
            disposables.Add(bus.Respond<PersonList, PersonListDTO>(GetUsersPersons));
        }

        private PersonListDTO GetUsersPersons(PersonList personList)
        {
            var user = usersRepository.All().FirstOrDefault(x => x.Email == personList.Email);

            if (user != null)
            {
                return new PersonListDTO
                {
                    Persons = user.Persons.Select(x => new PersonDTO
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList()
                };
            }
            return new PersonListDTO();
        }

        private SubscriptionListDTO GetSubscriptions(SubscriptionRequest subscriptionRequest)
        {
            var movies = GetUsersMovies(new MovieList {Email = subscriptionRequest.Email});
            var shows = GetUsersShows(new TvList {Email = subscriptionRequest.Email});

            var entities = movies.Movies.Select(movie => new SubscriptionsDTO
            {
                Id = movie.Id,
                Name = movie.Name,
                ReleaseDate = movie.ReleaseDate
            }).ToList();

            entities.AddRange(shows.TvShows.Select(show => new SubscriptionsDTO
            {
                Id = show.Id, Name = show.Name, ReleaseDate = show.ReleaseNextEpisode.Value, EpisodeNumber = show.NextEpisodeNr, LastFinishedSeason = show.LastFinishedSeasonNr
            }));

            var data = entities.OrderByDescending(x => x.ReleaseDate).Select(x => new SubscriptionsDTO
            {
                Id = x.Id,
                Name = x.Name,
                ReleaseDate = x.ReleaseDate,
                EpisodeNumber = x.EpisodeNumber,
                LastFinishedSeason = x.LastFinishedSeason
            }).ToList();

            return new SubscriptionListDTO
            {
                Subscriptions = data,
                Filtered = entities.Count
            };
        }
    }
}
