using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EasyNetQ;
using Messages.DTO;
using Messages.Request;
using Messages.Response;

namespace Web.UI.Controllers
{
    [Authorize]
    public class PersonsController : BaseController
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
            var response = bus.Request<PersonRequest, PersonListDTO>(new PersonRequest());

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Search(string input)
        {
            var response = bus.Request<PersonSearch, PersonListDTO>(new PersonSearch { Search = input });

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
    }
}