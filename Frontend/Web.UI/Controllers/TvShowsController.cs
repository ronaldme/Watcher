using System;
using System.Web.Mvc;
using EasyNetQ;
using Messages.DTO;
using Messages.Request;
using Messages.Response;
using Messages.Types;

namespace Web.UI.Controllers
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
            var response = bus.Request<TvShow, TvShowListDTO>(new TvShow
            {
                ShowRequest = ShowRequest.TopRated
            });
            
            return Json(response.TvShows, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Search(string input)
        {
            var response = bus.Request<TvShowSearch, TvShowListDTO>(new TvShowSearch
            {
                Search = input
            });
            
            return Json(response.TvShows, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Subscribe(int id, string name)
        {
            string email = GetEmail();

            var response = bus.Request<TvSubscription, Subscription>(new TvSubscription
            {
                TheMovieDbId = Convert.ToInt32(id),
                EmailUser = email
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}