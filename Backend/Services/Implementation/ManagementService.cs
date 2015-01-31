using System;
using System.Collections.Generic;
using System.Linq;
using EasyNetQ;
using Messages.Request;
using Repository;
using Repository.Entities;
using Repository.Repositories.Interfaces;
using Repository.UOW;
using Services.Interfaces;

namespace Services
{
    public class ManagementService : IManagementService, IMqResponder
    {
        private List<IDisposable> disposables;
        private readonly IBus bus;
        private readonly IUsersRepository usersRepository;

        public ManagementService(IBus bus, IUsersRepository usersRepository)
        {
            this.bus = bus;
            this.usersRepository = usersRepository;
        }

        public void Start()
        {
            disposables = new List<IDisposable>();

            // Run all methods implemented from the ISearchTV interface
            typeof(IManagementService).GetMethods().ToList().ForEach(x => x.Invoke(this, null));
        }

        public void Stop()
        {
            disposables.ForEach(x => x.Dispose());
        }

        public void ManageUser()
        {
            bus.Respond<ManagementRequest, ManagementResponse>(Manage);
        }

        private ManagementResponse Manage(ManagementRequest request)
        {
            using (var watcherContext = new WatcherContext())
            {
                UnitOfWork.Current = new UnitOfWork(watcherContext);
                UnitOfWork.Current.BeginTransaction();
                try
                {
                    var user = usersRepository.All().FirstOrDefault(x => x.Email == request.OldEmail);

                    if (user != null)
                    {
                        if (request.SetData)
                        {
                            user.NotifyHoursPastMidnight = request.NotifyHour;
                            user.Email = request.Email;
                            user.NotifyDayLater = request.NotifyDayLater;
                            user.NotifyMyAndroidKey = request.NotifyMyAndroidKey;
                            user.GetEmailNotifications = request.GetEmailNotifications;
                            usersRepository.Update(user);

                            return new ManagementResponse {Success = true};
                        }
                        return new ManagementResponse
                        {
                            NotifyHour = user.NotifyHoursPastMidnight,
                            NotifyDayLater = user.NotifyDayLater,
                            NotifyMyAndroidKey = user.NotifyMyAndroidKey,
                            GetEmailNotifications = user.GetEmailNotifications
                        };
                    }

                    usersRepository.Insert(new User
                    {
                        Email = request.Email,
                        NotifyHoursPastMidnight = request.NotifyHour,
                        NotifyMyAndroidKey = request.NotifyMyAndroidKey,
                        NotifyDayLater = request.NotifyDayLater,
                        GetEmailNotifications = request.GetEmailNotifications
                    });

                    return new ManagementResponse();
                }
                finally
                {
                    UnitOfWork.Current.Commit();
                }
            }
        }
    }
}