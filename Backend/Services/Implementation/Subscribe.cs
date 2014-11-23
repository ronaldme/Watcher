using System;
using System.Collections.Generic;
using System.Linq;
using EasyNetQ;
using Messages.Request;
using Messages.Response;
using Repository.Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class Subscribe : ISubscribe, IMqResponder
    {
        private List<IDisposable> disposables;
        private readonly IBus bus;
        private readonly IUsersRepository usersRepository;
        private readonly IMovieRepository movieRepository;

        public Subscribe(IBus bus, IUsersRepository usersRepository, IMovieRepository movieRepository)
        {
            this.bus = bus;
            this.usersRepository = usersRepository;
            this.movieRepository = movieRepository;
        }

        public void SubscribeTv()
        {
            disposables.Add(bus.Respond<TvSubscription, Subscription>(SubscribeTo));
        }

        private Subscription SubscribeTo(TvSubscription tvSubscription)
        {
            var users = usersRepository.GetAll().ToList();

            foreach (var user in users)
            {
                if (user.Email == tvSubscription.EmailUser)
                {
                    user.Email = user.Email + "this is a test";
                    usersRepository.Update();
                }
            }

            return new Subscription
            {
                IsSuccess = true,
                Message = "hoera"
            };
        }

        public void SubscribeMovie()
        {
        }

        public void SubscribeActor()
        {
            
        }

        public void Start()
        {
            disposables = new List<IDisposable>();

            // Run all methods implemented from the ISearchTV interface
            typeof(ISubscribe).GetMethods().ToList().ForEach(x => x.Invoke(this, null));
        }

        public void Stop()
        {
        }
    }
}
