using System.Web.Mvc;

namespace Web.UI.Controllers
{
    public class MoviesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}