using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BLL;
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
                User user = usersRepository.All().FirstOrDefault(x => x.Email == tvSubscription.EmailUser);
                Show show = showRepository.All().FirstOrDefault(x => x.TheMovieDbId == tvSubscription.TheMovieDbId);
                ShowDTO showInfo = theMovieDb.GetShowBy(tvSubscription.TheMovieDbId);

                ShowDTO dto = theMovieDb.GetLatestEpisode(showInfo.Id, showInfo.Seasons);

                if (user == null)
                {
                    if (show == null)
                    {
                        show = showRepository.Insert(new Show
                        {
                            TheMovieDbId = tvSubscription.TheMovieDbId,
                            Name = showInfo.Name,
                            LastFinishedSeason = dto.LastFinishedSeason,
                            ReleaseNextEpisode = dto.ReleaseNextEpisode,
                            NextEpisode = dto.NextEpisode
                        });

                        usersRepository.Insert(new User
                        {
                            Email = tvSubscription.EmailUser,
                            Shows = new Collection<Show> {show},
                            NotifyHoursPastMidnight = 9
                        });
                    }
                    else
                    {
                        usersRepository.Insert(new User
                        {
                            Email = tvSubscription.EmailUser,
                            Shows = new Collection<Show> {show},
                            NotifyHoursPastMidnight = 9
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
                            LastFinishedSeason = dto.LastFinishedSeason,
                            ReleaseNextEpisode = dto.ReleaseNextEpisode,
                            NextEpisode = dto.NextEpisode
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
                User user = usersRepository.All().FirstOrDefault(x => x.Email == movieSubscription.EmailUser);
                Movie movie = movieRepository.All().FirstOrDefault(x => x.TheMovieDbId == movieSubscription.TheMovieDbId);
                MovieDTO movieInfo = theMovieDb.GetMovieBy(movieSubscription.TheMovieDbId);

                if (user == null)
                {
                    if (movie == null)
                    {
                        movie = movieRepository.Insert(new Movie
                        {
                            TheMovieDbId = movieSubscription.TheMovieDbId,
                            Name = movieInfo.Name,
                            ReleaseDate = movieInfo.ReleaseDate
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
                            ReleaseDate = movieInfo.ReleaseDate
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
                User user = usersRepository.All().FirstOrDefault(x => x.Email == personSubscription.EmailUser);
                Person person = personRepository.All().FirstOrDefault(x => x.TheMovieDbId == personSubscription.TheMovieDbId);
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
        #endregion

        public void Unsubscribe()
        {
            disposables.Add(bus.Respond<Unsubscribe, Unsubscription>(UnsubscribeThis));
        }

        private Unsubscription UnsubscribeThis(Unsubscribe unsubscribe)
        {
            User user = usersRepository.All().FirstOrDefault(x => x.Email == unsubscribe.Email);
            
            if (user != null)
            {
                Person person = personRepository.All().FirstOrDefault(x => x.Id == unsubscribe.Id && x.Name == unsubscribe.Name);
                if (person != null)
                {
                    user.Persons.Remove(person);
                    personRepository.Delete(person);
                    usersRepository.Update();

                    return CreateUnsubscription(true);
                }

                Movie movie = movieRepository.All().FirstOrDefault(x => x.Id == unsubscribe.Id && x.Name == unsubscribe.Name);
                if (movie != null)
                {
                    user.Movies.Remove(movie);
                    movieRepository.Delete(movie);
                    usersRepository.Update();
                }
                
                Show show = showRepository.All().FirstOrDefault(x => x.Id == unsubscribe.Id && x.Name == unsubscribe.Name);
                if (show != null)
                {
                    user.Shows.Remove(show);
                    showRepository.Delete(show);
                    usersRepository.Update();
                }
                return CreateUnsubscription(false, "No subscription found.");
            }
            return CreateUnsubscription(false, "User not found.");
        }

        private Unsubscription CreateUnsubscription(bool succes, string message = null)
        {
            return new Unsubscription { IsSuccess = succes, Message = message};
        }
    }
}