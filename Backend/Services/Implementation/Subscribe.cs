using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EasyNetQ;
using Messages.Request;
using Messages.Response;
using Repository.Entities;
using Repository.Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class Subscribe : ISubscriptionService, IMqResponder
    {
        private List<IDisposable> disposables;
        private readonly IBus bus;
        private readonly IUsersRepository usersRepository;
        private readonly IMovieRepository movieRepository;
        private readonly IShowRepository showRepository;

        public Subscribe(IBus bus, IUsersRepository usersRepository, IMovieRepository movieRepository, IShowRepository showRepository)
        {
            this.bus = bus;
            this.usersRepository = usersRepository;
            this.movieRepository = movieRepository;
            this.showRepository = showRepository;
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

        public void SubscribeTv()
        {
            disposables.Add(bus.Respond<TvSubscription, Subscription>(SubscribeToShow));
        }

        private Subscription SubscribeToShow(TvSubscription tvSubscription)
        {
            try
            {
                User user = usersRepository.GetAll().FirstOrDefault(x => x.Email == tvSubscription.EmailUser);
                Show show = showRepository.GetAll().FirstOrDefault(x => x.TheMovieDbId == tvSubscription.Id);

                if (user == null)
                {
                    if (show == null)
                    {
                        show = showRepository.Insert(new Show
                        {
                            Name = tvSubscription.Name,
                            TheMovieDbId = tvSubscription.Id
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
                            Name = tvSubscription.Name,
                            TheMovieDbId = tvSubscription.Id
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

        public void SubscribeMovie()
        {
            disposables.Add(bus.Respond<MovieSubscription, Subscription>(SubscribeToMovie));
        }

        private Subscription SubscribeToMovie(MovieSubscription movieSubscription)
        {
            try
            {
                User user = usersRepository.GetAll().FirstOrDefault(x => x.Email == movieSubscription.EmailUser);
                Movie movie = movieRepository.GetAll().FirstOrDefault(x => x.TheMovieDbId == movieSubscription.Id);

                if (user == null)
                {
                    if (movie == null)
                    {
                        movie = movieRepository.Insert(new Movie
                        {
                            Name = movieSubscription.Name,
                            TheMovieDbId = movieSubscription.Id
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
                            Name = movieSubscription.Name,
                            TheMovieDbId = movieSubscription.Id
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

        public void SubscribeActor()
        {
            
        }

        public void UnsubscribeTv()
        {
            
        }

        public void UnsubscribeMovie()
        {
            
        }

        public void UnsubscribeActor()
        {
            
        }
    }
}
