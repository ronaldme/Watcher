using System.Web.Mvc;

namespace Web.UI.Controllers
{
    [Authorize]
    public class ActorsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}