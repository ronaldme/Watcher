using System.Collections.Generic;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Watcher.Messages.Person;

namespace Watcher.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IBus _bus;

        public PersonController(IBus bus)
        {
            _bus = bus;
        }

        [HttpGet]
        public List<PersonDto> Get() => _bus.Request<PersonRequest, List<PersonDto>>(new PersonRequest());

        [Route("Search")]
        public List<PersonDto> Search(string search)
        {
            var response = _bus.Request<PersonSearchQuery, List<PersonDto>>(new PersonSearchQuery { Search = search });
            return response;
        }
    }
}