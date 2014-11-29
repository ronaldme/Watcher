using System;
using System.Collections.Generic;
using System.Linq;
using BLL;
using EasyNetQ;
using Messages.DTO;
using Messages.Request;
using Services.Interfaces;

namespace Services
{
    public class PersonService : IPersonService, IMqResponder
    {
        private List<IDisposable> disposables;
        private readonly IBus bus;
        private readonly ITheMovieDb theMovieDb;

        public PersonService(IBus bus, ITheMovieDb theMovieDb)
        {
            this.bus = bus;
            this.theMovieDb = theMovieDb;
        }

        public void Start()
        {
            disposables = new List<IDisposable>();

            // Run all methods implemented from the ISearchTV interface
            typeof(IPersonService).GetMethods().ToList().ForEach(x => x.Invoke(this, null));
        }

        public void Stop()
        {
            disposables.ForEach(x => x.Dispose());
        }

        public void Popular()
        {
            disposables.Add(bus.Respond<PersonRequest, List<PersonDTO>>(x => new List<PersonDTO>(theMovieDb.Populair())));
        }

        public void Search()
        {
            disposables.Add(bus.Respond<PersonSearch, List<PersonDTO>>(x => new List<PersonDTO>(theMovieDb.SearchPerson(x.Search))));
        }

        public void SearchById()
        {
            
        }
    }
}
