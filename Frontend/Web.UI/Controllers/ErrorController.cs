using System;
using System.Web.Mvc;

namespace Web.UI.Controllers
{
        public class ErrorController : Controller
        {
            public ActionResult Index(int statusCode, Exception exception)
            {
                Response.StatusCode = statusCode;
                return View();
            }
        }
}