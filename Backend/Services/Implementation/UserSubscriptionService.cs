using System;
using System.Collections.Generic;
using System.Linq;
using Messages.DTO;
using Messages.Request;
using Repository;
using Repository.Repositories.Interfaces;
using Repository.UOW;
using Services.Interfaces;
using EasyNetQ;

namespace Services
{
    public class UserSubscriptionService : IUserSubscriptionService, IMqResponder
    {
        private List<IDisposable> disposables;
        private readonly IBus bus;
        private readonly IUsersRepository usersRepository;

        public UserSubscriptionService(IBus bus, IUsersRepository usersRepository)
        {
            this.bus = bus;
            this.usersRepository = usersRepository;
        }

        public void Start()
        {
            disposables = new List<IDisposable>();

            // Run all methods implemented from the ISearchTV interface
            typeof(IUserSubscriptionService).GetMethods().ToList().ForEach(x => x.Invoke(this, null));

            disposables.Add(bus.Respond<ShowSubscriptionRequest, ShowSubscriptionListDto>(GetShowSubscriptions));
            disposables.Add(bus.Respond<MovieSubscriptionRequest, MovieSubscriptionListDto>(GetMovieSubscriptions));
            disposables.Add(bus.Respond<PersonSubscriptionRequest, PersonSubscriptionListDto>(GetPersonSubscriptions));
        }
        
        public void Stop()
        {
            disposables.ForEach(x => x.Dispose());
        }

        public void GetShows()
        {
            disposables.Add(bus.Respond<TvList, TvShowListDTO>(GetUsersShows));
        }

        public void GetMovies()
        {
            disposables.Add(bus.Respond<MovieList, MovieListDTO>(GetUsersMovies));
        }

        public void GetPersons()
        {
            disposables.Add(bus.Respond<PersonList, PersonListDTO>(GetUsersPersons));
        }
        
        private TvShowListDTO GetUsersShows(TvList tvList)
        {
            using (var watcherContext = new WatcherContext())
            {
                UnitOfWork.Current = new UnitOfWork(watcherContext);
                UnitOfWork.Current.BeginTransaction();
                try
                {
                    var user = usersRepository.All().FirstOrDefault(x => x.Email == tvList.Email);

                    if (user != null)
                    {
                        return new TvShowListDTO
                        {
                            TvShows = user.Shows.Select(x => new ShowDTO
                            {
                                Id = x.Id,
                                Name = x.Name,
                                ReleaseNextEpisode = x.ReleaseNextEpisode,
                                CurrentSeason = x.CurrentSeason,
                                NextEpisode = x.NextEpisode,
                                EpisodeCount = x.EpisodeCount
                            }).ToList()
                        };
                    }
                }
                finally
                {
                    UnitOfWork.Current.Commit();
                }
            }
            return new TvShowListDTO();
        }

        private MovieListDTO GetUsersMovies(MovieList movieList)
        {
            using (var watcherContext = new WatcherContext())
            {
                UnitOfWork.Current = new UnitOfWork(watcherContext);
                UnitOfWork.Current.BeginTransaction();
                try
                {
                    var user = usersRepository.All().FirstOrDefault(x => x.Email == movieList.Email);

                    if (user != null)
                    {
                        return new MovieListDTO
                        {
                            Movies = user.Movies.Select(x => new MovieDTO
                            {
                                Id = x.Id,
                                Name = x.Name,
                                ReleaseDate = x.ReleaseDate.Value
                            }).ToList()
                        };
                    }
                }
                finally
                {
                    UnitOfWork.Current.Commit();
                }
            }
            return new MovieListDTO();
        }

        private PersonListDTO GetUsersPersons(PersonList personList)
        {
            using (var watcherContext = new WatcherContext())
            {
                UnitOfWork.Current = new UnitOfWork(watcherContext);
                UnitOfWork.Current.BeginTransaction();
                try
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
                }
                finally
                {
                    UnitOfWork.Current.Commit();
                }
            }
            return new PersonListDTO();
        }

        private PersonListDTO GetPersonMovies(string email)
        {
            using (var watcherContext = new WatcherContext())
            {
                UnitOfWork.Current = new UnitOfWork(watcherContext);
                UnitOfWork.Current.BeginTransaction();
                try
                {
                    var user = usersRepository.All().FirstOrDefault(x => x.Email == email);

                    if (user != null)
                    {
                        return new PersonListDTO
                        {
                            Persons = user.Persons.Select(x => new PersonDTO
                            {
                                Id = x.Id,
                                Name = x.Name,
                                ProductionName = x.ProductionName,
                                ReleaseDate = x.ReleaseDate
                            }).ToList()
                        };
                    }
                }
                finally
                {
                    UnitOfWork.Current.Commit();
                }

                return new PersonListDTO();
            }
        }

        private ShowSubscriptionListDto GetShowSubscriptions(ShowSubscriptionRequest subscriptionRequest)
        {
            var shows = GetUsersShows(new TvList {Email = subscriptionRequest.Email}).TvShows;
            var resultSet = new List<ShowSubscriptionsDTO>();

            if (shows != null)
            {
                resultSet.AddRange(shows.Select(show => new ShowSubscriptionsDTO
                {
                    Id = show.Id,
                    Name = show.Name,
                    ReleaseDate = show.ReleaseNextEpisode.HasValue ? show.ReleaseNextEpisode.Value : new DateTime(1),
                    EpisodeNumber = show.NextEpisode,
                    CurrentSeason = show.CurrentSeason,
                    RemainingEpisodes = show.EpisodeCount - show.NextEpisode + 1 // also count next episode
                }).ToList());
            }

            var data = resultSet.Select(x => new ShowSubscriptionsDTO
            {
                Id = x.Id,
                Name = x.Name,
                ReleaseDate = x.ReleaseDate,
                EpisodeNumber = x.EpisodeNumber,
                CurrentSeason = x.CurrentSeason,
                RemainingEpisodes = x.RemainingEpisodes
            }).OrderByDescending(x => x.ReleaseDate)
            .ThenBy(x => x.CurrentSeason)
            .ThenBy(x => x.EpisodeNumber)
            .ToList();

            return new ShowSubscriptionListDto
            {
                Subscriptions = data,
                Filtered = resultSet.Count
            };
        }

        private MovieSubscriptionListDto GetMovieSubscriptions(MovieSubscriptionRequest subscriptionRequest)
        {
            var movies = GetUsersMovies(new MovieList { Email = subscriptionRequest.Email }).Movies;
            var resultSet = new List<MovieSubscriptionsDTO>();

            if (movies != null)
            {
                resultSet.AddRange(movies.Select(movie => new MovieSubscriptionsDTO
                {
                    Id = movie.Id,
                    Name = movie.Name,
                    ReleaseDate = movie.ReleaseDate
                }).ToList());
            }

            var data = resultSet.Select(x => new MovieSubscriptionsDTO
            {
                Id = x.Id,
                Name = x.Name,
                ReleaseDate = x.ReleaseDate
            })
            .OrderByDescending(x => x.ReleaseDate)
            .ToList();

            return new MovieSubscriptionListDto
            {
                Subscriptions = data,
                Filtered = resultSet.Count
            };
        }

        private PersonSubscriptionListDto GetPersonSubscriptions(PersonSubscriptionRequest request)
        {
            var persons = GetPersonMovies(request.Email).Persons;
            var data = new List<PersonSubscriptionsDTO>();

            if (persons != null)
            {
                data.AddRange(persons.Select(person => new PersonSubscriptionsDTO
                {
                    Id = person.Id,
                    Name = person.Name,
                    ReleaseDate = person.ReleaseDate,
                    ProductionName = person.ProductionName
                }).ToList().OrderByDescending(x => x.ReleaseDate));
            }

            return new PersonSubscriptionListDto
            {
                Subscriptions = data,
                Filtered = data.Count
            };
        }
    }
}