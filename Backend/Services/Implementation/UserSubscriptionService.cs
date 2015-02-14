using System;
using System.Collections.Generic;
using System.Linq;
using BLL;
using Messages;
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
            typeof(IUserSubscriptionService).GetMethods().ToList().ForEach(x => x.Invoke(this, null));
        }
        
        public void Stop()
        {
            disposables.ForEach(x => x.Dispose());
        }

        public void GetShows()
        {
            disposables.Add(bus.Respond<ShowSubscriptionRequest, ShowSubscriptionListDto>(GetShowSubscriptions));
        }

        public void GetMovies()
        {
            disposables.Add(bus.Respond<MovieSubscriptionRequest, MovieSubscriptionListDto>(GetMovieSubscriptions));
        }

        public void GetPersons()
        {
            disposables.Add(bus.Respond<PersonSubscriptionRequest, PersonSubscriptionListDto>(GetPersonSubscriptions));
        }

        #region Persons
        private PersonSubscriptionListDto GetPersonSubscriptions(PersonSubscriptionRequest request)
        {
            var personListDto = new PersonListDTO();
            int personCount = 0;

            using (var watcherContext = new WatcherContext())
            {
                UnitOfWork.Current = new UnitOfWork(watcherContext);
                UnitOfWork.Current.BeginTransaction();
                try
                {
                    var user = usersRepository.All().FirstOrDefault(x => x.Email == request.Email);

                    if (user != null)
                    {
                        personCount = user.Persons.Count;

                        personListDto.Persons = user.Persons.Select(x => new PersonDTO
                        {
                            Id = x.Id,
                            Name = x.Name,
                            ProductionName = x.ProductionName,
                            ReleaseDate = x.ReleaseDate,
                            ProfilePath = x.PosterPath
                        }).Skip(request.Start).Take(request.Length).ToList();
                    }
                }
                finally
                {
                    UnitOfWork.Current.Commit();
                }

                var personList = new List<PersonSubscriptionsDTO>();

                if (personListDto.Persons != null)
                {
                    personList.AddRange(personListDto.Persons.Select(person => new PersonSubscriptionsDTO
                    {
                        Id = person.Id,
                        Name = person.Name,
                        ReleaseDate = person.ReleaseDate,
                        ProductionName = person.ProductionName,
                        PosterPath = person.ProfilePath
                    }).ToList().OrderByDescending(x => x.ReleaseDate));
                }

                return new PersonSubscriptionListDto
                {
                    Subscriptions = personList,
                    PrefixPath = Urls.PrefixImages,
                    Filter = new Filter
                    {
                        Filtered = personCount,
                        Total = personList.Count
                    }
                };
            }
        }
        #endregion

        #region Shows
        private ShowSubscriptionListDto GetShowSubscriptions(ShowSubscriptionRequest request)
        {
            var showListDto = new TvShowListDTO();
            int showCount = 0;

            using (var watcherContext = new WatcherContext())
            {
                UnitOfWork.Current = new UnitOfWork(watcherContext);
                UnitOfWork.Current.BeginTransaction();
                try
                {
                    var user = usersRepository.All().FirstOrDefault(x => x.Email == request.Email);
                    
                    if (user != null)
                    {
                        showCount = user.Shows.Count();
                        showListDto.TvShows = user.Shows.Select(x => new ShowDTO
                        {
                            Id = x.Id,
                            Name = x.Name,
                            ReleaseNextEpisode = x.ReleaseNextEpisode,
                            CurrentSeason = x.CurrentSeason,
                            NextEpisode = x.NextEpisode,
                            EpisodeCount = x.EpisodeCount,
                            PosterPath = x.PosterPath
                        }).Skip(request.Start).Take(request.Length).ToList();
                    }
                }
                finally
                {
                    UnitOfWork.Current.Commit();
                }
            }

            var data = new List<ShowSubscriptionsDTO>();

            if (showListDto.TvShows != null)
            {
                data.AddRange(showListDto.TvShows.Select(show => new ShowSubscriptionsDTO
                {
                    Id = show.Id,
                    Name = show.Name,
                    ReleaseDate = show.ReleaseNextEpisode.HasValue ? show.ReleaseNextEpisode.Value : new DateTime(1),
                    EpisodeNumber = show.NextEpisode,
                    CurrentSeason = show.CurrentSeason,
                    RemainingEpisodes = show.EpisodeCount - show.NextEpisode + 1, // also count next episode
                    PosterPath = show.PosterPath
                })
                .OrderByDescending(x => x.ReleaseDate)
                .ThenBy(x => x.CurrentSeason).ThenBy(x => x.EpisodeNumber)
                .ToList());
            }

            return new ShowSubscriptionListDto
            {
                Subscriptions = data,
                PrefixPath = Urls.PrefixImages,
                Filter = new Filter
                {
                    Filtered = showCount,
                    Total = data.Count
                }
            };
        }
        #endregion

        #region Movies
        private MovieSubscriptionListDto GetMovieSubscriptions(MovieSubscriptionRequest request)
        {
            var movieListDto = new MovieListDTO();
            int movieCount = 0;

            using (var watcherContext = new WatcherContext())
            {
                UnitOfWork.Current = new UnitOfWork(watcherContext);
                UnitOfWork.Current.BeginTransaction();
                try
                {
                    var user = usersRepository.All().FirstOrDefault(x => x.Email == request.Email);
                   
                    if (user != null)
                    {
                        movieCount = user.Movies.Count();
                        movieListDto.Movies = user.Movies.Select(x => new MovieDTO
                        {
                            Id = x.Id,
                            Name = x.Name,
                            ReleaseDate = x.ReleaseDate.HasValue ? x.ReleaseDate.Value : DateTime.MinValue,
                            PosterPath = x.PosterPath
                        }).Skip(request.Start).Take(request.Length).ToList();
                    }
                }
                finally
                {
                    UnitOfWork.Current.Commit();
                }
            }

            var data = new List<MovieSubscriptionsDTO>();

            if (movieListDto.Movies != null)
            {
                data.AddRange(movieListDto.Movies.Select(movie => new MovieSubscriptionsDTO
                {
                    Id = movie.Id,
                    Name = movie.Name,
                    ReleaseDate = movie.ReleaseDate,
                    PosterPath = movie.PosterPath
                }) 
                .OrderByDescending(x => x.ReleaseDate).ToList());
            }

            return new MovieSubscriptionListDto
            {
                Subscriptions = data,
                PrefixPath = Urls.PrefixImages,
                Filter = new Filter
                {
                    Filtered = movieCount,
                    Total = data.Count
                }
            };
        }
        #endregion
    }
}