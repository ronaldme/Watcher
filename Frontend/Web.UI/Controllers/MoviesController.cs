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
    public class MoviesController : BaseController
    {
        private readonly IBus bus;

        public MoviesController(IBus bus)
        {
            this.bus = bus;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Upcoming()
        {
            var response = bus.Request<MovieRequest, List<MovieDTO>>(new MovieRequest());

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Search(string input)
        {
            var response = bus.Request<MovieSearch, List<MovieDTO>>(new MovieSearch
            {
                Search = input
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Subscribe(int id, string name)
        {
            var email = GetEmail();

            var response = bus.Request<MovieSubscription, Subscription>(new MovieSubscription
            {
                TheMovieDbId = Convert.ToInt32(id),
                EmailUser = email
            });

            return Json(response.IsSuccess, JsonRequestBehavior.AllowGet);
        }
    }
}