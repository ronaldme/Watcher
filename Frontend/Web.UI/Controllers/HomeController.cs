using System.Web.Mvc;

namespace Web.UI.Controllers
{
    public class HomeController : Controller
    {
      
        public HomeController()
        {
            // TODO: Dependecy injection here
            searchTv = new SearchTv();
            tvShows = new TvShows();
            subscribe = new Subscribe();
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult TopRated()
        {
            var result = tvShows.TopRated();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Search(string input)
        {
            var result = searchTv.Search(input);
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get(int id)
        {
            var result = searchTv.SearchById(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Subscribe(int id)
        {
            var subscribeResult = subscribe.SubscribeTv(id);

            return Json(subscribeResult, JsonRequestBehavior.AllowGet);
        }
    }
}