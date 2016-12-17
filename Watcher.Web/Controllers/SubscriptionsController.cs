using System.Web.Mvc;
using EasyNetQ;
using Watcher.Messages;
using Watcher.Messages.Movie;
using Watcher.Messages.Person;
using Watcher.Messages.Show;
using Watcher.Messages.Subscription;
using Watcher.Shared.Common;

namespace Watcher.Web.Controllers
{
    [Authorize]
    public class SubscriptionsController : BaseController
    {
        private readonly IBus bus;

        public SubscriptionsController(IBus bus)
        {
            this.bus = bus;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetMovieSubscriptions(int skip, int take, string search)
        {
            var subscriptionsList = bus.Request<MovieSubscriptionRequest, MovieSubscriptionListDto>(new MovieSubscriptionRequest
            {
                Email = Email(),
                Skip = skip,
                Take = take
            });

            return Json(subscriptionsList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTvShowSubscriptions(int skip, int take, string search)
        {
            var subscriptionsList = bus.Request<ShowSubscriptionRequest, ShowSubscriptionListDto>(new ShowSubscriptionRequest
            {
                Email = Email(),
                Skip = skip,
                Take = take
            });

            return Json(subscriptionsList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPersonSubscriptions(int skip, int take, string search)
        {
            var subscriptionsList = bus.Request<PersonSubscriptionRequest, PersonSubscriptionListDto>(new PersonSubscriptionRequest
            {
                Email = Email(),
                Skip = skip,
                Take = take
            });

            return Json(subscriptionsList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Unsubscribe(int id, SubscriptionType type)
        {
            bus.Request<Unsubscribe, Unsubscription>(new Unsubscribe
            {
                Email = Email(),
                Id = id,
                SubcriptionType = type
            });

            return RedirectToAction(nameof(Index));
        }
    }
}