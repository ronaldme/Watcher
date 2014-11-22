using System.Web.Mvc;
using EasyNetQ;

namespace Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBus bus;

        public HomeController(IBus bus)
        {
            this.bus = bus;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult TopRated()
        {
            //var result = tvShows.TopRated();
            return null;
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Search(string input)
        {
            //var result = searchTv.Search(input);
            return null;
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get(int id)
        {
            //var result = searchTv.SearchById(id);
            return null;
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Subscribe(int id)
        {
            return null;
            //var subscribeResult = subscribe.SubscribeTv(id);

            //return Json(subscribeResult, JsonRequestBehavior.AllowGet);
        }
    }
}