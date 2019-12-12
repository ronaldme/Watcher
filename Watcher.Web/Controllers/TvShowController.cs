using System;
using System.Collections.Generic;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Watcher.Messages.Show;

namespace Watcher.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TvShowController : ControllerBase
    {
        // TODO: DI
        private IBus bus = RabbitHutch.CreateBus("host=localhost;username=guest;password=guest");
        private readonly ILogger<TvShowController> _logger;

        public TvShowController(ILogger<TvShowController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<ShowDto> Get() => bus.Request<TvShowRequest, List<ShowDto>>(new TvShowRequest());
    }
}
