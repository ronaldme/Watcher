using System.Collections.Generic;
using System.Web.Mvc;
using EasyNetQ;
using Watcher.Messages.Movie;
using Watcher.Messages.Subscription;

namespace Watcher.Web.Controllers
{
    [Authorize]
    public class MoviesController : BaseController
    {
        private readonly IBus bus;

        public MoviesController(IBus bus)
        {
            this.bus = bus;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upcoming()
        {
            var response = bus.Request<MovieRequest, List<MovieDto>>(new MovieRequest());

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(string input)
        {
            var response = bus.Request<MovieSearch, List<MovieDto>>(new MovieSearch
            {
                Search = input
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Subscribe(int id)
        {
            var response = bus.Request<MovieSubscription, Subscription>(new MovieSubscription
            {
                TheMovieDbId = id,
                EmailUser = Email()
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}