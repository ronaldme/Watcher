using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Watcher.Web.Controllers
{
    public class BaseController : Controller
    {
        public string Email()
        {
            return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(User.Identity.GetUserId()).Email;
        }

        public void SetEmailCookie(string mail = null)
        {
            var cookie = new HttpCookie("email")
            {
                Value = mail ?? Email(),
                Expires = DateTime.Now.AddDays(10)
            };
            HttpContext.Response.SetCookie(cookie);
        }
    }
}