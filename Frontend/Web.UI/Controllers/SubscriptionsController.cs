using System.Web;
using System.Web.Mvc;
using EasyNetQ;
using Messages.DTO;
using Messages.Request;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Web.UI.Controllers
{
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
            var response = bus.Request<TvList, TvShowListDTO>(new TvList
            {
                Email = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId()).Email
            });
            
            return Json(response.TvShows, JsonRequestBehavior.AllowGet);
        }
    }
}