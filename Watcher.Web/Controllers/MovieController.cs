using System;
using System.Collections.Generic;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using Watcher.Messages.Movie;
using Watcher.Messages.Subscription;

namespace Watcher.Web.Controllers
{
    public class MoviesController
    {
        private readonly IBus bus;

        public MoviesController()
        {
            this.bus = RabbitHutch.CreateBus("");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upcoming()
        {
            var response = bus.Request<MovieRequest, List<MovieDto>>(new MovieRequest());

            return View();// Json(response, JsonRequestBehavior.AllowGet);
        }

        private ActionResult View()
        {
            throw new NotImplementedException();
        }
/*

        public ActionResult Search(string input)
        {
            var response = bus.Request<MovieSearch, List<MovieDto>>(new MovieSearch
            {
                Search = input
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Subscribe(int id)
        {
            var response = bus.Request<MovieSubscription, Subscription>(new MovieSubscription
            {
                TheMovieDbId = id,
                EmailUser = Email()
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }*/
    }
}