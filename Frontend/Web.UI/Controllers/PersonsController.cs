using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using EasyNetQ;
using Messages.DTO;
using Messages.Request;
using Messages.Response;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Web.UI.Controllers
{
    [Authorize]
    public class PersonsController : Controller
    {
        private readonly IBus bus;

        public PersonsController(IBus bus)
        {
            this.bus = bus;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Popular()
        {
            var response = bus.Request<PersonRequest, List<PersonDTO>>(new PersonRequest());

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Search(string input)
        {
            var response = bus.Request<PersonSearch, List<PersonDTO>>(new PersonSearch{Search = input});

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Subscribe(int id, string name)
        {
            var email = GetEmail();

            var response = bus.Request<PersonSubscription, Subscription>(new PersonSubscription
            {
                TheMovieDbId = Convert.ToInt32(id),
                Name = name,
                EmailUser = email
            });

            return Json(response.IsSuccess, JsonRequestBehavior.AllowGet);
        }

        public string GetEmail()
        {
            var httpCookie = HttpContext.Request.Cookies.Get("email");

            if (httpCookie != null)
            {
                return httpCookie.Value;
            }

            return HttpContext.GetOwinContext()
                .GetUserManager<ApplicationUserManager>()
                .FindById(User.Identity.GetUserId())
                .Email;
        }
    }
}