using System.Collections.Generic;
using System.Web.Mvc;
using EasyNetQ;
using Watcher.Messages.Show;
using Watcher.Messages.Subscription;

namespace Watcher.Web.Controllers
{
    [Authorize]
    public class TvShowsController : BaseController
    {
        private readonly IBus bus;

        public TvShowsController(IBus bus)
        {
            this.bus = bus;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult TopRated()
        {
            var tvShows = bus.Request<TvShowRequest, List<ShowDto>>(new TvShowRequest());
            return Json(tvShows, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Search(string input)
        {
            var response = bus.Request<TvShowSearch, List<ShowDto>>(new TvShowSearch
            {
                Search = input
            });
            
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Subscribe(int id)
        {
            var response = bus.Request<TvSubscription, Subscription>(new TvSubscription
            {
                TheMovieDbId = id,
                EmailUser = Email()
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}