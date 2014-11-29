using System.Web;
using System.Web.Mvc;
using EasyNetQ;
using Messages.DTO;
using Messages.Request;
using Messages.Response;
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

            var personList = bus.Request<PersonList, PersonListDTO>(new PersonList
            {
                Email = email
            });

            return Json(new { tvList.TvShows, movieList.Movies, personList.Persons }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Unsubscribe(int id, string unsubscribeType)
        {
            string email = GetEmail();
            Unsubscription response;

            switch (unsubscribeType)
            {
                case "show":
                    response = bus.Request<UnsubscribeTv, Unsubscription>(new UnsubscribeTv
                    {
                        Email = email,
                        Id = id
                    });
                    break;

                case "movie":
                    response = bus.Request<UnsubscribeMovie, Unsubscription>(new UnsubscribeMovie
                    {
                        Email = email,
                        Id = id
                    });
                    break;

                case "person":
                    response = bus.Request<UnsubscribePerson, Unsubscription>(new UnsubscribePerson
                    {
                        Email = email,
                        Id = id
                    });
                    break;

                default:
                    response = new Unsubscription{IsSuccess = false};
                break;
            }

            return Json(new { response.IsSuccess }, JsonRequestBehavior.AllowGet);
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