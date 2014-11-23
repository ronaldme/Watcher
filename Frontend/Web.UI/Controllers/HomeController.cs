using System;
using System.IO;
using System.Web.Mvc;

namespace Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly Random random;

        public HomeController()
        {
            random = new Random();
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetImage()
        {
            var dir = Server.MapPath(@"\Content\images\");
            var path = Path.Combine(dir, + random.Next(1, 21) + ".jpg");
            
            var file = File(path, "image/jpeg");
            byte[] bytes = System.IO.File.ReadAllBytes(file.FileName);

            var image = Convert.ToBase64String(bytes);

            return Json(image, JsonRequestBehavior.AllowGet);
        }
    }
}