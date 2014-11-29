using System.Web;
using System.Web.Mvc;
using EasyNetQ;
using Messages.DTO;
using Messages.Request;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Web.UI.Controllers
{
    [Authorize]
    public class SubscriptionsController : Controller
    {
        private readonly IBus bus;

        public SubscriptionsController(IBus bus)
        {
            this.bus = bus;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetSubscriptions()
        {
            string email = GetEmail();
            
            var tvList = bus.Request<TvList, TvShowListDTO>(new TvList
            {
                Email = email
            });

            var movieList = bus.Request<MovieList, MovieListDTO>(new MovieList
            {
                Email = email
            });

            return Json(new { tvList.TvShows, movieList.Movies }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Unsubscribe(int id, bool isMovie)
        {
            string email = GetEmail();
            
            Unsubscription response;

            if (isMovie)
            {
                response = bus.Request<UnsubscribeMovie, Unsubscription>(new UnsubscribeMovie
                {
                    Email = email,
                    Id = id
                });
            }
            else
            {
                response = bus.Request<UnsubscribeTv, Unsubscription>(new UnsubscribeTv
                {
                    Email = email,
                    Id = id
                });
            }

            return Json(new { response .IsSuccess }, JsonRequestBehavior.AllowGet);
        }

        public string GetEmail()
        {
            var httpCookie = HttpContext.Request.Cookies.Get("email");

            if (httpCookie != null)
            {
                return httpCookie.Value;
            }
            
            return HttpContext.GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindById(User.Identity.GetUserId())
                .Email;
        }
    }
}