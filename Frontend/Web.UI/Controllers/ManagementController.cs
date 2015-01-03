using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyNetQ;
using Messages.Request;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Web.UI.ViewModels;

namespace Web.UI.Controllers
{
    [Authorize]
    public class ManagementController : BaseController
    {
        private readonly IBus bus;
        private readonly List<int> hours = Enumerable.Range(0, 24).ToList();

        public ManagementController(IBus bus)
        {
            this.bus = bus;
        }

        public ActionResult Index()
        {
            string email = GetEmail();
            var response = bus.Request<ManagementRequest, ManagementResponse>(new ManagementRequest
            {
                Email = email, OldEmail = email
            });

            return View(new ManagementViewModel
            {
                SelectedNotifyHour = response.NotifyHour,
                Hours = new SelectList(hours),
                Email = email,
                OldEmail = email
            });
        }

        public ActionResult Save(ManagementViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var response = bus.Request<ManagementRequest, ManagementResponse>(new ManagementRequest
                {
                    NotifyHour = viewModel.SelectedNotifyHour,
                    OldEmail = viewModel.OldEmail,
                    Email = viewModel.Email,
                    SetData = true
                });

                if (response.Success)
                {
                    var applicationManger = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                    var user = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());
                    user.UserName = viewModel.Email;
                    user.Email = viewModel.Email;

                    applicationManger.Update(user);
                    SetEmailCookie();
                }

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", viewModel);
        }
    }
}