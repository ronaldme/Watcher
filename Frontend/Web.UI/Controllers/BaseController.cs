using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Web.UI.Controllers
{
    public class BaseController : Controller
    {
        public string GetEmail()
        {
            var httpCookie = HttpContext.Request.Cookies.Get("email");

            if (httpCookie != null)
            {
                return httpCookie.Value;
            }

            return Email();
        }

        public void SetEmailCookie()
        {
            var cookie = new HttpCookie("email")
            {
                Value = Email(),
                Expires = DateTime.Now.AddDays(10)
            };
            HttpContext.Response.SetCookie(cookie);
        }

        private string Email()
        {
            return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId()).Email;
        }
    }
}