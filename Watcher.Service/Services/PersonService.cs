using System.Collections.Generic;
using EasyNetQ;
using Watcher.Messages.Person;
using Watcher.Service.Infrastructure;
using Watcher.Service.TheMovieDb;

namespace Watcher.Service.Services
{
    public class PersonService : IService
    {
        private readonly ITheMovieDb theMovieDb;
        private readonly IBus bus;

        public PersonService(ITheMovieDb theMovieDb,
            IBus bus)
        {
            this.theMovieDb = theMovieDb;
            this.bus = bus;
        }

        public void HandleRequests()
        {
            Popular();
            Search();
        }

        public void Popular()
        {
            bus.Respond<PersonRequest, List<PersonDto>>(persons => theMovieDb.Populair());
        }

        public void Search()
        {
            bus.Respond<PersonSearch, List<PersonDto>>(persons => theMovieDb.SearchPerson(persons.Search));
        }
    }
}
