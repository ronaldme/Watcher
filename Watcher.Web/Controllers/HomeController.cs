using System;
using System.IO;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Watcher.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly int amountOfImages = int.Parse(WebConfigurationManager.AppSettings["amountOfImages"]);

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetImage()
        {
            var random = new Random();

            var dir = Server.MapPath(@"\Content\images\");
            var path = Path.Combine(dir, + random.Next(1, amountOfImages) + ".jpg");

            var file = File(path, "image/jpeg");
            byte[] bytes = System.IO.File.ReadAllBytes(file.FileName);

            var image = Convert.ToBase64String(bytes);
            return Json(image, JsonRequestBehavior.AllowGet);
        }
    }
}
