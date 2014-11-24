using System;
using System.Web;
using System.Web.Mvc;
using EasyNetQ;
using Messages;
using Messages.DTO;
using Messages.Request;
using Messages.Response;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Web.UI.Controllers
{
    [Authorize]
    public class TvShowsController : Controller
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
            var user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());

            var response = bus.Request<TvSubscription, Subscription>(new TvSubscription
            {
                Id = Convert.ToInt32(id),
                Name = name,
                EmailUser = user.Email
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}