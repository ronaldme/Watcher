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

        public JsonResult GetSubscriptions([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            string email = GetEmail();

            var subscriptionsList = bus.Request<SubscriptionRequest, SubscriptionListDTO>(new SubscriptionRequest
            {
                Email = email
            });

            var data = subscriptionsList.Subscriptions.Select(x => new
            {
                x.Id,
                x.Name,
                x.EpisodeNumber,
                x.LastFinishedSeason,
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