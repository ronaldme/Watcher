using System.Linq;
using System.Web.Mvc;
using DataTables.Mvc;
using EasyNetQ;
using Messages.DTO;
using Messages.Request;
using Messages.Response;

namespace Web.UI.Controllers
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

        public JsonResult GetTvSubscriptions([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            string email = GetEmail();

            var subscriptionsList = bus.Request<ShowSubscriptionRequest, ShowSubscriptionListDto>(new ShowSubscriptionRequest
            {
                Email = email
            });

            var data = subscriptionsList.Subscriptions.Select(x => new
            {
                x.Id,
                x.Name,
                ReleaseDate = x.ReleaseDate.Year != 1 ? x.ReleaseDate.ToString("dd-MM-yyyy") : "Unknown",
                CurrentSeason = x.CurrentSeason == 0 ? "" : x.CurrentSeason.ToString(),
                EpisodeNumber = x.EpisodeNumber == 0 ? "" : x.EpisodeNumber.ToString(),
                Remaining = x.RemainingEpisodes == 0 ? "" : x.RemainingEpisodes.ToString()
            }).ToList();

            return Json(new DataTablesResponse(requestModel.Draw, data, data.Count(), subscriptionsList.Filtered), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMovieSubscriptions([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            string email = GetEmail();

            var subscriptionsList = bus.Request<MovieSubscriptionRequest, MovieSubscriptionListDto>(new MovieSubscriptionRequest
            {
                Email = email
            });

            var data = subscriptionsList.Subscriptions.Select(x => new
            {
                x.Id,
                x.Name,
                ReleaseDate = x.ReleaseDate.Year != 1 ? x.ReleaseDate.ToString("dd-MM-yyyy") : "Unknown"
            }).ToList();

            return Json(new DataTablesResponse(requestModel.Draw, data, data.Count(), subscriptionsList.Filtered), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Unsubscribe(int id, string name)
        {
            string email = GetEmail();

            Unsubscription response = bus.Request<Unsubscribe, Unsubscription>(new Unsubscribe
            {
                Email = email,
                Id = id,
                Name = name
            });
              
            return Json(new { response.IsSuccess }, JsonRequestBehavior.AllowGet);
        }
    }
}