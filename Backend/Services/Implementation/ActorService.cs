using System;
using System.Collections.Generic;
using System.Linq;
using BLL;
using EasyNetQ;
using Services.Interfaces;

namespace Services
{
    public class ActorService : IActorService, IMqResponder
    {
        private List<IDisposable> disposables;
        private readonly IBus bus;
        private readonly ITheMovieDb theMovieDb;

        public ActorService(IBus bus, ITheMovieDb theMovieDb)
        {
            this.bus = bus;
            this.theMovieDb = theMovieDb;
        }

        public void Start()
        {
            disposables = new List<IDisposable>();

            // Run all methods implemented from the ISearchTV interface
            typeof(IActorService).GetMethods().ToList().ForEach(x => x.Invoke(this, null));
        }

        public void Stop()
        {
            disposables.ForEach(x => x.Dispose());
        }

        public void Search()
        {
            
        }

        public void SearchById()
        {
            
        }
    }
}
