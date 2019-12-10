using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using Watcher.DAL;
using Watcher.DAL.Entities;
using Watcher.Messages;

namespace Watcher.Service.Services
{
    public class ManagementService : IMqService
    {
        private readonly IBus _bus;

        public ManagementService(IBus bus)
        {
            _bus = bus;
        }

        public void HandleRequests()
        {
            _bus.RespondAsync<ManagementRequest, ManagementResponse>(Manage);
        }

        private async Task<ManagementResponse> Manage(ManagementRequest request)
        {
            await using var context = new WatcherDbContext();
            var user = await context.Users.SingleOrDefaultAsync(x => x.Email == request.OldEmail);
                    
            if (user != null)
            {
                if (request.SetData)
                {
                    user.NotifyAtHoursPastMidnight = request.NotifyHour;
                    user.Email = request.Email;
                    user.NotifyDayLater = request.NotifyDayLater;
                    user.GetEmailNotifications = request.GetEmailNotifications;
                    await context.SaveChangesAsync();
                }

                return new ManagementResponse
                {
                    Success = true,
                    NotifyHour = user.NotifyAtHoursPastMidnight,
                    NotifyDayLater = user.NotifyDayLater,
                    GetEmailNotifications = user.GetEmailNotifications
                };
            }

            context.Users.Add(new User
            {
                Email = request.Email,
                NotifyAtHoursPastMidnight = request.NotifyHour,
                NotifyDayLater = request.NotifyDayLater,
                GetEmailNotifications = request.GetEmailNotifications
            });

            await context.SaveChangesAsync();

            return new ManagementResponse {Success = true};
        }
    }
}