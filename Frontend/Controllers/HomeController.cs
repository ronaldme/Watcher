using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly string apiKey = ConfigurationManager.AppSettings.Get("apiKey");

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult AiringToday()
        {
            string response = GetResponse("http://api.themoviedb.org/3/tv/airing_today");

            return Json(new { response }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Search(string input)
        {
            string response = GetResponse("http://api.themoviedb.org/3/search/tv?query=" + GetInput(input) + "&sort_by=popularity.desc");

            return Json(new { response }, JsonRequestBehavior.AllowGet);
        }

        public string GetInput(string input)
        {
            return input.Replace(' ', '+');
        }

        public string GetResponse(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url + "&api_key=" + apiKey);
            request.KeepAlive = true;
            request.Method = "GET";
            request.Accept = "application/json";
            request.ContentLength = 0;

            string content;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    content = reader.ReadToEnd();
                }
            }

            return content;
        }
    }
}