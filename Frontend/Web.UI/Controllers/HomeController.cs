using System;
using System.IO;
using System.Web.Mvc;

namespace Web.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetImage()
        {
            var dir = Server.MapPath(@"\Content\images\");
            var path = Path.Combine(dir, + new Random().Next(1, 20) + ".jpg");
            
            var file = File(path, "image/jpeg");
            byte[] bytes = System.IO.File.ReadAllBytes(file.FileName);

            var image = Convert.ToBase64String(bytes);

            return Json(image, JsonRequestBehavior.AllowGet);
        }
    }
}