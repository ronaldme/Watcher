using System.Web.Mvc;
using Services;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITvShows tvShows;
        private readonly ISearchTv searchTv;

        public HomeController()
        {
            // TODO: Dependecy injection here
            searchTv = new SearchTv();
            tvShows = new TvShows();
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult TopRated()
        {
            var result = tvShows.TopRated();

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Search(string input)
        {
            var result = searchTv.Search(input);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get(int id)
        {
            return null;
        }
    }
}