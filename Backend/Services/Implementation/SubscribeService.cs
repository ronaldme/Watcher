using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BLL;
using BLL.Json.Shows;
using EasyNetQ;
using Messages.DTO;
using Messages.Request;
using Messages.Response;
using Repository.Entities;
using Repository.Repositories.Interfaces;
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

            // Run all methods implemented from the ISearchTV interface
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
            try
            {
                User user = usersRepository.GetAll().FirstOrDefault(x => x.Email == tvSubscription.EmailUser);
                Show show = showRepository.GetAll().FirstOrDefault(x => x.TheMovieDbId == tvSubscription.TheMovieDbId);
                Testing showInfo = theMovieDb.GetShowBy(tvSubscription.TheMovieDbId);

                TvShowDTO dto = theMovieDb.GetLatestEpisode(showInfo.Id, showInfo.Seasons);

                if (user == null)
                {
                    if (show == null)
                    {
                        show = showRepository.Insert(new Show
                        {
                            TheMovieDbId = tvSubscription.TheMovieDbId,
                            Name = showInfo.Name,
                            ReleaseDate = showInfo.AirDate,
                            LastFinishedSeason = dto.LastFinishedSeasonNr,
                            ReleaseNextEpisode = dto.ReleaseNextEpisode
                        });

                        usersRepository.Insert(new User
                        {
                            Email = tvSubscription.EmailUser,
                            Shows = new Collection<Show> {show}
                        });
                    }
                    else
                    {
                        usersRepository.Insert(new User
                        {
                            Email = tvSubscription.EmailUser,
                            Shows = new Collection<Show> {show}
                        });
                    }
                }
                else
                {
                    if (show == null)
                    {
                        show = showRepository.Insert(new Show
                        {
                            TheMovieDbId = tvSubscription.TheMovieDbId,
                            Name = showInfo.Name,
                            ReleaseDate = showInfo.AirDate,
                            LastFinishedSeason = dto.LastFinishedSeasonNr,
                            ReleaseNextEpisode = dto.ReleaseNextEpisode
                        });
                        user.Shows.Add(show);
                    }
                    else
                    {
                        user.Shows.Add(show);
                    }
                    usersRepository.Update();
                }

                return new Subscription { IsSuccess = true };
            }
            catch (Exception e)
            {
                return new Subscription
                {
                    IsSuccess = false,
                    Message = e.InnerException.ToString()
                };
            }
        }

        public void UnsubscribeTv()
        {
            disposables.Add(bus.Respond<UnsubscribeTv, Unsubscription>(UnsubscribeToTv));
        }

        private Unsubscription UnsubscribeToTv(UnsubscribeTv unsubscribeTv)
        {
            User user = usersRepository.GetAll().FirstOrDefault(x => x.Email == unsubscribeTv.Email);
            Show show = showRepository.GetAll().FirstOrDefault(x => x.Id == unsubscribeTv.Id);

            if (user != null)
            {
                user.Shows.Remove(show);
                showRepository.Delete(show);
                usersRepository.Update();
            }

            return new Unsubscription { IsSuccess = true };
        }
        #endregion

        #region Movies
        public void SubscribeMovie()
        {
            disposables.Add(bus.Respond<MovieSubscription, Subscription>(SubscribeToMovie));
        }

        private Subscription SubscribeToMovie(MovieSubscription movieSubscription)
        {
            try
            {
                User user = usersRepository.GetAll().FirstOrDefault(x => x.Email == movieSubscription.EmailUser);
                Movie movie = movieRepository.GetAll().FirstOrDefault(x => x.TheMovieDbId == movieSubscription.TheMovieDbId);
                MovieDTO movieInfo = theMovieDb.GetMovieBy(movieSubscription.TheMovieDbId);

                if (user == null)
                {
                    if (movie == null)
                    {
                        movie = movieRepository.Insert(new Movie
                        {
                            TheMovieDbId = movieSubscription.TheMovieDbId,
                            Name = movieInfo.Name,
                            ReleaseDate = DateTime.Parse(movieInfo.ReleaseDate)
                        });

                        usersRepository.Insert(new User
                        {
                            Email = movieSubscription.EmailUser,
                            Movies = new Collection<Movie> { movie }
                        });
                    }
                    else
                    {
                        usersRepository.Insert(new User
                        {
                            Email = movieSubscription.EmailUser,
                            Movies = new Collection<Movie> { movie }
                        });
                    }
                }
                else
                {
                    if (movie == null)
                    {
                        movie = movieRepository.Insert(new Movie
                        {
                            TheMovieDbId = movieSubscription.TheMovieDbId,
                            Name = movieInfo.Name,
                            ReleaseDate = DateTime.Parse(movieInfo.ReleaseDate)
                        });
                        user.Movies.Add(movie);
                    }
                    else
                    {
                        user.Movies.Add(movie);
                    }
                    usersRepository.Update();
                }

                return new Subscription { IsSuccess = true };
            }
            catch (Exception e)
            {
                return new Subscription
                {
                    IsSuccess = false,
                    Message = e.InnerException.ToString()
                };
            }
        }

        public void UnsubscribeMovie()
        {
            disposables.Add(bus.Respond<UnsubscribeMovie, Unsubscription>(UnsubscribeToMovie));
        }

        private Unsubscription UnsubscribeToMovie(UnsubscribeMovie unsubscribeMovie)
        {
            User user = usersRepository.GetAll().FirstOrDefault(x => x.Email == unsubscribeMovie.Email);
            Movie movie = movieRepository.GetAll().FirstOrDefault(x => x.Id == unsubscribeMovie.Id);

            if (user != null)
            {
                user.Movies.Remove(movie);
                movieRepository.Delete(movie);
                usersRepository.Update();
            }

            return new Unsubscription { IsSuccess = true };
        }
        #endregion

        #region Persons
        public void SubscribePerson()
        {
            disposables.Add(bus.Respond<PersonSubscription, Subscription>(SubscribeToPerson));
        }

        private Subscription SubscribeToPerson(PersonSubscription personSubscription)
        {
            try
            {
                User user = usersRepository.GetAll().FirstOrDefault(x => x.Email == personSubscription.EmailUser);
                Person person = personRepository.GetAll().FirstOrDefault(x => x.TheMovieDbId == personSubscription.TheMovieDbId);
                PersonDTO personInfo = theMovieDb.GetPersonBy(personSubscription.TheMovieDbId);

                if (user == null)
                {
                    if (person == null)
                    {
                        person = personRepository.Insert(new Person
                        {
                            Name = personSubscription.Name,
                            TheMovieDbId = personSubscription.TheMovieDbId,
                            Birthday = DateTime.Parse(personInfo.Birthday)
                        });

                        usersRepository.Insert(new User
                        {
                            Email = personSubscription.EmailUser,
                            Persons = new Collection<Person> { person }
                        });
                    }
                    else
                    {
                        usersRepository.Insert(new User
                        {
                            Email = personSubscription.EmailUser,
                            Persons = new Collection<Person> { person }
                        });
                    }
                }
                else
                {
                    if (person == null)
                    {
                        person = personRepository.Insert(new Person
                        {
                            Name = personSubscription.Name,
                            TheMovieDbId = personSubscription.TheMovieDbId,
                            Birthday = DateTime.Parse(personInfo.Birthday)
                        });
                        user.Persons.Add(person);
                    }
                    else
                    {
                        user.Persons.Add(person);
                    }
                    usersRepository.Update();
                }

                return new Subscription {IsSuccess = true};
            }
            catch (Exception e)
            {
                return new Subscription
                {
                    IsSuccess = false,
                    Message = e.InnerException.ToString()
                };
            }
        }

        public void UnsubscribePerson()
        {
            disposables.Add(bus.Respond<UnsubscribePerson, Unsubscription>(UnsubscribeToPerson));
        }

        private Unsubscription UnsubscribeToPerson(UnsubscribePerson unsubscribePerson)
        {
            User user = usersRepository.GetAll().FirstOrDefault(x => x.Email == unsubscribePerson.Email);
            Person person = personRepository.GetAll().FirstOrDefault(x => x.Id == unsubscribePerson.Id);

            if (user != null)
            {
                user.Persons.Remove(person);
                personRepository.Delete(person);
                usersRepository.Update();
            }

            return new Unsubscription {IsSuccess = true};
        }
        #endregion
    }
}