using System;
using System.Collections.ObjectModel;
using System.Linq;
using EasyNetQ;
using log4net;
using Watcher.Common;
using Watcher.DAL;
using Watcher.DAL.Entities;
using Watcher.Messages;
using Watcher.Messages.Movie;
using Watcher.Messages.Person;
using Watcher.Messages.Show;
using Watcher.Messages.Subscription;
using Watcher.Service.Infrastructure;
using Watcher.Service.TheMovieDb;

namespace Watcher.Service.Services
{
    public class SubscribeService : IService
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ITheMovieDb theMovieDb;
        private readonly IBus bus;

        public SubscribeService(IBus bus,
            ITheMovieDb theMovieDb)
        {
            this.bus = bus;
            this.theMovieDb = theMovieDb;
        }

        public void HandleRequests()
        {
            bus.Respond<TvSubscription, Subscription>(SubscribeToShow);
            bus.Respond<MovieSubscription, Subscription>(SubscribeToMovie);
            bus.Respond<PersonSubscription, Subscription>(SubscribeToPerson);
            bus.Respond<Unsubscribe, Unsubscription>(Unsubscribe);
        }

        private Subscription SubscribeToShow(TvSubscription tvSubscription)
        {
            using (var context = new WatcherDbContext())
            {
                try
                {
                    var user = context.Users.FirstOrDefault(x => x.Email == tvSubscription.EmailUser);
                    var show = context.Shows.FirstOrDefault(x => x.TheMovieDbId == tvSubscription.TheMovieDbId);
                    
                    ShowDto showInfo = theMovieDb.GetShowBy(tvSubscription.TheMovieDbId);
                    ShowDto dto = theMovieDb.GetLatestEpisode(showInfo.Id, showInfo.Seasons);

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

                    //user.Shows.Add(show);
                    //context.Users.AddOrUpdate(user);
                    context.SaveChanges();

                    return new Subscription {IsSuccess = true};
                }
                catch (Exception e)
                {
                    log.WarnFormat("Subscribing to show failed: {0}", e.Message);
                    
                    return new Subscription { IsSuccess = false, Message = e.InnerException.ToString() };
                }
            }
        }

        private Subscription SubscribeToMovie(MovieSubscription movieSubscription)
        {
            using (var context = new WatcherDbContext())
            {
                try
                {
                    User user = context.Users.FirstOrDefault(x => x.Email == movieSubscription.EmailUser);
                    Movie movie = context.Movies.FirstOrDefault(x => x.TheMovieDbId == movieSubscription.TheMovieDbId);
                    MovieDto movieInfo = theMovieDb.GetMovieBy(movieSubscription.TheMovieDbId);

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

                    //user.Movies.Add(movie);
                    //context.Users.AddOrUpdate(user);
                    context.SaveChanges();

                    return new Subscription {IsSuccess = true};
                }
                catch (Exception e)
                {
                    log.WarnFormat("Subscribing to movie failed: {0} ", e.Message);

                    return new Subscription { IsSuccess = false, Message = e.InnerException.ToString() };
                }
            }
        }

        private Subscription SubscribeToPerson(PersonSubscription personSubscription)
        {
            using (var context = new WatcherDbContext())
            {
                try
                {
                    User user = context.Users.FirstOrDefault(x => x.Email == personSubscription.EmailUser);
                    Person person = context.Persons.FirstOrDefault(x => x.TheMovieDbId == personSubscription.TheMovieDbId);
                    PersonDto personInfo = theMovieDb.GetPersonBy(personSubscription.TheMovieDbId);

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

                   // user.Persons.Add(person);
                    //context.Users.AddOrUpdate(user);
                    context.SaveChanges();

                    return new Subscription {IsSuccess = true};
                }
                catch (Exception e)
                {
                    log.WarnFormat("Subscribing to person failed: {0} ", e.Message);

                    return new Subscription { IsSuccess = false, Message = e.InnerException.ToString() };
                }
            }
        }

        private Unsubscription Unsubscribe(Unsubscribe unsubscribe)
        {
            using (var context = new WatcherDbContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Email == unsubscribe.Email);

                if (user != null)
                {
                    /*switch (unsubscribe.SubcriptionType)
                    {
                        case SubscriptionType.Movie:
                            var movie = context.Movies.Find(unsubscribe.Id);
                            user.Movies.Remove(movie);
                            break;
                        case SubscriptionType.TvShow:
                            var show = context.Shows.Find(unsubscribe.Id);
                            user.Shows.Remove(show);
                            break;
                        case SubscriptionType.Person:
                            var person = context.Persons.Find(unsubscribe.Id);
                            user.Persons.Remove(person);
                            break;
                    }*/

                    context.SaveChanges();
                    return new Unsubscription { IsSuccess = true };
                }

                return new Unsubscription
                {
                    IsSuccess = false,
                    Message = "No user found."
                };
            }
        }

        private Movie CreateMovie(MovieSubscription movieSubscription, MovieDto movieInfo)
        {
            return new Movie
            {
                TheMovieDbId = movieSubscription.TheMovieDbId,
                Name = movieInfo.Name,
                ReleaseDate = movieInfo.ReleaseDate,
                PosterPath = movieInfo.PosterPath
            };
        }

        private User CreateUser(PersonSubscription personSubscription, Person person)
        {
            return new User
            {
                Email = personSubscription.EmailUser,
                //Persons = new Collection<Person> { person }
            };
        }

        private User CreateUser(MovieSubscription movieSubscription, Movie movie)
        {
            return new User
            {
                Email = movieSubscription.EmailUser,
                //Movies = new Collection<Movie> { movie }
            };
        }

        private User CreateUser(TvSubscription tvSubscription, Show show)
        {
            return new User
            {
                Email = tvSubscription.EmailUser,
                //UserShows = new Collection<Show> { show }
            };
        }

        private Person CreatePerson(PersonSubscription personSubscription, PersonDto personInfo)
        {
            return new Person
            {
                Name = personInfo.Name,
                TheMovieDbId = personSubscription.TheMovieDbId,
                Birthday = !string.IsNullOrEmpty(personInfo.Birthday) ? DateTime.Parse(personInfo.Birthday) : (DateTime?)null,
                ProductionName = personInfo.ProductionName,
                ReleaseDate = personInfo.ReleaseDate,
                PosterPath = personInfo.ProfilePath
            };
        }

        private Show CreateShow(TvSubscription tvSubscription, ShowDto showInfo, ShowDto dto)
        {
            return new Show
            {
                TheMovieDbId = tvSubscription.TheMovieDbId,
                Name = showInfo.Name,
                CurrentSeason = dto.CurrentSeason,
                ReleaseNextEpisode = dto.ReleaseNextEpisode,
                EpisodeCount = dto.EpisodeCount,
                NextEpisode = dto.NextEpisode,
                PosterPath = showInfo.PosterPath
            };
        }
    }
}