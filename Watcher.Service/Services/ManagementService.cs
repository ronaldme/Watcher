using System.Linq;
using EasyNetQ;
using Watcher.DAL;
using Watcher.DAL.Entities;
using Watcher.Messages;
using Watcher.Service.Infrastructure;

namespace Watcher.Service.Services
{
    public class ManagementService : IService
    {
        private readonly IBus bus;

        public ManagementService(IBus bus)
        {
            this.bus = bus;
        }

        public void HandleRequests()
        {
            bus.Respond<ManagementRequest, ManagementResponse>(Manage);
        }

        private ManagementResponse Manage(ManagementRequest request)
        {
            using (var context = new WatcherDbContext())
            {
                var user = context.Users.SingleOrDefault(x => x.Email == request.OldEmail);
                    
                if (user != null)
                {
                    if (request.SetData)
                    {
                        user.NotifyAtHoursPastMidnight = request.NotifyHour;
                        user.Email = request.Email;
                        user.NotifyDayLater = request.NotifyDayLater;
                        user.NotifyMyAndroidKey = request.NotifyMyAndroidKey;
                        user.GetEmailNotifications = request.GetEmailNotifications;
                        context.SaveChanges();
                    }

                    return new ManagementResponse
                    {
                        Success = true,
                        NotifyHour = user.NotifyAtHoursPastMidnight,
                        NotifyDayLater = user.NotifyDayLater,
                        NotifyMyAndroidKey = user.NotifyMyAndroidKey,
                        GetEmailNotifications = user.GetEmailNotifications
                    };
                }

                context.Users.Add(new User
                {
                    Email = request.Email,
                    NotifyAtHoursPastMidnight = request.NotifyHour,
                    NotifyMyAndroidKey = request.NotifyMyAndroidKey,
                    NotifyDayLater = request.NotifyDayLater,
                    GetEmailNotifications = request.GetEmailNotifications
                });

                context.SaveChanges();

                return new ManagementResponse {Success = true};
            }
        }
    }
}