using System.Collections.Generic;
using System.Web.Mvc;
using EasyNetQ;
using Watcher.Messages.Person;
using Watcher.Messages.Subscription;

namespace Watcher.Web.Controllers
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

        public ActionResult Popular()
        {
            var persons = bus.Request<PersonRequest, List<PersonDto>>(new PersonRequest());
            return Json(persons, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Search(string input)
        {
            var results = bus.Request<PersonSearch, List<PersonDto>>(new PersonSearch { Search = input });
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Subscribe(int id)
        {
            Subscription response = bus.Request<PersonSubscription, Subscription>(new PersonSubscription
            {
                TheMovieDbId = id,
                EmailUser = Email()
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}