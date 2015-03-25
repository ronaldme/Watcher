using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BLL;
using EasyNetQ;
using log4net;
using Messages.DTO;
using Messages.Request;
using Messages.Response;
using Repository;
using Repository.Entities;
using Repository.Repositories.Interfaces;
using Repository.UOW;
using Services.Interfaces;

namespace Services
{
    public class SubscribeService : ISubscriptionService, IMqResponder
    {
        private List<IDisposable> disposables;
        private readonly IBus bus;
        private readonly IUsersRepository usersRepository;
        private readonly IMovieRepository movieRepository;
        private readonly IShowRepository showRepository;
        private readonly IPersonRepository personRepository;
        private readonly ITheMovieDb theMovieDb;
        private static readonly ILog log = LogManager.GetLogger(typeof(SubscribeService));

        public SubscribeService(IBus bus, IUsersRepository usersRepository, IMovieRepository movieRepository, IShowRepository showRepository, IPersonRepository personRepository, ITheMovieDb theMovieDb)
        {
            this.bus = bus;
            this.usersRepository = usersRepository;
            this.movieRepository = movieRepository;
            this.showRepository = showRepository;
            this.personRepository = personRepository;
            this.theMovieDb = theMovieDb;
        }

        public void Start()
        {
            disposables = new List<IDisposable>();
            typeof(ISubscriptionService).GetMethods().ToList().ForEach(x => x.Invoke(this, null));
        }

        public void Stop()
        {
            disposables.ForEach(x => x.Dispose());
        }

        #region Tv shows
        public void SubscribeTv()
        {
            disposables.Add(bus.Respond<TvSubscription, Subscription>(SubscribeToShow));
        }

        private Subscription SubscribeToShow(TvSubscription tvSubscription)
        {
            using (var watcherContext = new WatcherContext())
            {
                UnitOfWork.Current = new UnitOfWork(watcherContext);
                UnitOfWork.Current.BeginTransaction();

                try
                {
                    User user = usersRepository.All().FirstOrDefault(x => x.Email == tvSubscription.EmailUser);
                    Show show = showRepository.All().FirstOrDefault(x => x.TheMovieDbId == tvSubscription.TheMovieDbId);
                    
                    ShowDTO showInfo = theMovieDb.GetShowBy(tvSubscription.TheMovieDbId);
                    ShowDTO dto = theMovieDb.GetLatestEpisode(showInfo.Id, showInfo.Seasons);

                    if (user == null)
                    {
                        user = CreateUser(tvSubscription, show);
                    }
                    if (show == null)
                    {
                        show = CreateShow(tvSubscription, showInfo, dto);
                    }
                    else
                    {
                        // update show info
                        show.ReleaseNextEpisode = dto.ReleaseNextEpisode;
                        show.NextEpisode = dto.NextEpisode;
                        show.PosterPath = dto.PosterPath;
                    }

                    user.Shows.Add(show);

                    return new Subscription {IsSuccess = true};
                }
                catch (Exception e)
                {
                    log.WarnFormat("Subscribing to show failed: {0}", e.Message);
                    
                    return new Subscription { IsSuccess = false, Message = e.InnerException.ToString() };
                }
                finally
                {
                    UnitOfWork.Current.Commit();
                }
            }
        }

        private Show CreateShow(TvSubscription tvSubscription, ShowDTO showInfo, ShowDTO dto)
        {
            return showRepository.Insert(new Show
            {
                TheMovieDbId = tvSubscription.TheMovieDbId,
                Name = showInfo.Name,
                CurrentSeason = dto.CurrentSeason,
                ReleaseNextEpisode = dto.ReleaseNextEpisode,
                EpisodeCount = dto.EpisodeCount,
                NextEpisode = dto.NextEpisode,
                PosterPath = showInfo.PosterPath
            });
        }
        #endregion

        #region Movies
        public void SubscribeMovie()
        {
            disposables.Add(bus.Respond<MovieSubscription, Subscription>(SubscribeToMovie));
        }

        private Subscription SubscribeToMovie(MovieSubscription movieSubscription)
        {
            using (var watcherContext = new WatcherContext())
            {
                UnitOfWork.Current = new UnitOfWork(watcherContext);
                UnitOfWork.Current.BeginTransaction();
                try
                {
                    User user = usersRepository.All().FirstOrDefault(x => x.Email == movieSubscription.EmailUser);
                    Movie movie = movieRepository.All().FirstOrDefault(x => x.TheMovieDbId == movieSubscription.TheMovieDbId);
                    MovieDTO movieInfo = theMovieDb.GetMovieBy(movieSubscription.TheMovieDbId);

                    user = user ?? CreateUser(movieSubscription, movie);

                    if (movie == null)
                    {
                        movie = CreateMovie(movieSubscription, movieInfo);
                    }
                    else
                    {
                        // update movie info
                        movie.ReleaseDate = movieInfo.ReleaseDate;
                        movie.PosterPath = movieInfo.PosterPath;
                    }

                    user.Movies.Add(movie);

                    return new Subscription {IsSuccess = true};
                }
                catch (Exception e)
                {
                    log.WarnFormat("Subscribing to movie failed: {0} ", e.Message);

                    return new Subscription { IsSuccess = false, Message = e.InnerException.ToString() };
                }
                finally
                {
                    UnitOfWork.Current.Commit();
                }
            }
        }

        private Movie CreateMovie(MovieSubscription movieSubscription, MovieDTO movieInfo)
        {
            return movieRepository.Insert(new Movie
            {
                TheMovieDbId = movieSubscription.TheMovieDbId,
                Name = movieInfo.Name,
                ReleaseDate = movieInfo.ReleaseDate,
                PosterPath = movieInfo.PosterPath
            });
        }

        #endregion

        #region Persons
        public void SubscribePerson()
        {
            disposables.Add(bus.Respond<PersonSubscription, Subscription>(SubscribeToPerson));
        }

        private Subscription SubscribeToPerson(PersonSubscription personSubscription)
        {
            using (var watcherContext = new WatcherContext())
            {
                UnitOfWork.Current = new UnitOfWork(watcherContext);
                UnitOfWork.Current.BeginTransaction();
                try
                {
                    User user = usersRepository.All().FirstOrDefault(x => x.Email == personSubscription.EmailUser);
                    Person person = personRepository.All().FirstOrDefault(x => x.TheMovieDbId == personSubscription.TheMovieDbId);
                    PersonDTO personInfo = theMovieDb.GetPersonBy(personSubscription.TheMovieDbId);

                    user = user ?? CreateUser(personSubscription, person);

                    if (person == null)
                    {
                        person = CreatePerson(personSubscription, personInfo);
                    }
                    else
                    {
                        // update person info
                        person.Birthday = !string.IsNullOrEmpty(personInfo.Birthday)
                            ? DateTime.Parse(personInfo.Birthday) : (DateTime?) null;
                        person.PosterPath = personInfo.ProfilePath;
                        person.ReleaseDate = personInfo.ReleaseDate;
                    }

                    user.Persons.Add(person);

                    return new Subscription {IsSuccess = true};
                }
                catch (Exception e)
                {
                    log.WarnFormat("Subscribing to person failed: {0} ", e.Message);

                    return new Subscription { IsSuccess = false, Message = e.InnerException.ToString() };
                }
                finally
                {
                    UnitOfWork.Current.Commit();
                }
            }
        }

        private Person CreatePerson(PersonSubscription personSubscription, PersonDTO personInfo)
        {
            return personRepository.Insert(new Person
            {
                Name = personSubscription.Name,
                TheMovieDbId = personSubscription.TheMovieDbId,
                Birthday = !string.IsNullOrEmpty(personInfo.Birthday) ? DateTime.Parse(personInfo.Birthday) : (DateTime?)null,
                ProductionName = personInfo.ProductionName,
                ReleaseDate = personInfo.ReleaseDate,
                PosterPath = personInfo.ProfilePath
            });
        }
        #endregion

        public void Unsubscribe()
        {
            disposables.Add(bus.Respond<Unsubscribe, Unsubscription>(UnsubscribeThis));
        }

        private Unsubscription UnsubscribeThis(Unsubscribe unsubscribe)
        {
            using (var watcherContext = new WatcherContext())
            {
                UnitOfWork.Current = new UnitOfWork(watcherContext);
                UnitOfWork.Current.BeginTransaction();

                User user = usersRepository.All().FirstOrDefault(x => x.Email == unsubscribe.Email);

                try
                {
                    if (user != null)
                    {
                        Person person = personRepository.All().FirstOrDefault(x => x.Id == unsubscribe.Id && x.Name == unsubscribe.Name);

                        if (person != null)
                        {
                            user.Persons.Remove(person);
                            return new Unsubscription { IsSuccess = true };
                        }

                        Movie movie = movieRepository.All().FirstOrDefault(x => x.Id == unsubscribe.Id && x.Name == unsubscribe.Name);
                        
                        if (movie != null)
                        {
                            user.Movies.Remove(movie);
                            return new Unsubscription { IsSuccess = true };
                        }

                        Show show = showRepository.All().FirstOrDefault(x => x.Id == unsubscribe.Id && x.Name == unsubscribe.Name);
                        if (show != null)
                        {
                            user.Shows.Remove(show);
                            return new Unsubscription { IsSuccess = true };
                        }

                        return new Unsubscription { IsSuccess = false, Message = "No subscription found." };
                    }
                    return new Unsubscription { IsSuccess = false, Message = "No user found."};
                }
                finally
                {
                    UnitOfWork.Current.Commit();
                }
            }
        }

        private User CreateUser(PersonSubscription personSubscription, Person person)
        {
            return usersRepository.Insert(new User
            {
                Email = personSubscription.EmailUser,
                Persons = new Collection<Person> { person }
            });
        }

        private User CreateUser(MovieSubscription movieSubscription, Movie movie)
        {
            return usersRepository.Insert(new User
            {
                Email = movieSubscription.EmailUser,
                Movies = new Collection<Movie> { movie }
            });
        }

        private User CreateUser(TvSubscription tvSubscription, Show show)
        {
            return usersRepository.Insert(new User
            {
                Email = tvSubscription.EmailUser,
                Shows = new Collection<Show> { show }
            });
        }
    }
}