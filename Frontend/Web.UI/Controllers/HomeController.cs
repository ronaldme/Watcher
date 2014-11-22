using System;
using System.Web.Mvc;
using EasyNetQ;
using Messages;
using Messages.DTO;
using Messages.Request;

namespace Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBus bus;

        public HomeController(IBus bus)
        {
            this.bus = bus;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult TopRated()
        {
            var response = bus.Request<TvShow, TvShowListDTO>(new TvShow
            {
                ShowRequest = ShowRequest.TopRated
            });
            
            return Json(response.TvShows, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Search(string input)
        {
            var response = bus.Request<TvShowSearch, TvShowListDTO>(new TvShowSearch
            {
                Search = input
            });
            
            return Json(response.TvShows, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get(int id)
        {
            var response = bus.Request<TvShowSearchById, TvShowDTO>(new TvShowSearchById
            {
                Id = Convert.ToInt32(id)
            });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Subscribe(int id)
        {
            return null;
        }
    }
}