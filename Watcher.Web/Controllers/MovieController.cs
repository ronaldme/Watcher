using System.Collections.Generic;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Watcher.Messages.Movie;

namespace Watcher.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IBus _bus;

        public MovieController(IBus bus)
        {
            _bus = bus;
        }

        [HttpGet]
        public List<MovieDto> Get() => _bus.Request<MovieRequest, List<MovieDto>>(new MovieRequest());
    }
}