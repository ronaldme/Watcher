using System.Collections.Generic;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Watcher.Messages.Show;

namespace Watcher.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TvShowController : ControllerBase
    {
        private readonly IBus _bus;

        public TvShowController(IBus bus)
        {
            _bus = bus;
        }

        [HttpGet]
        public List<ShowDto> Get() => _bus.Request<TvShowRequest, List<ShowDto>>(new TvShowRequest());

        [Route("Search")]
        public List<ShowDto> Search(string search)
        {
            var response = _bus.Request<TvShowSearchQuery, List<ShowDto>>(new TvShowSearchQuery { Search = search });
            return response;
        }
    }
}
