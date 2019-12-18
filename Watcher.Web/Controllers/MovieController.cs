using System.Collections.Generic;
using EasyNetQ;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Watcher.Messages.Movie;
using Watcher.Web.Models;

namespace Watcher.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : Controller
    {
        private readonly IBus _bus;
        private readonly UserManager<ApplicationUser> _userManager;

        public MovieController(IBus bus, UserManager<ApplicationUser> userManager)
        {
            _bus = bus;
            _userManager = userManager;
        }

        [HttpGet]
        public List<MovieDto> Get()
        {
            return _bus.Request<MovieRequest, List<MovieDto>>(new MovieRequest());
        }

        [Route("Search")]
        public List<MovieDto> Search(string search)
        {
            var response = _bus.Request<MovieSearchQuery, List<MovieDto>>(new MovieSearchQuery { Search = search});
            return response;
        }
    }
}