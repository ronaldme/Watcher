using System.Web.Mvc;

namespace Web.UI.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}