using System;
using System.Collections.Generic;
using System.Linq;
using EasyNetQ;
using Messages.Request;
using Repository.Entities;
using Repository.Repositories.Interfaces;
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
            var user = usersRepository.All().FirstOrDefault(x => x.Email == request.OldEmail);

            if (user != null)
            {
                if (request.SetData)
                {
                    user.NotifyHoursPastMidnight = request.NotifyHour;
                    user.Email = request.Email;
                    usersRepository.Update();
                    
                    return new ManagementResponse
                    {
                        Success = true
                    };
                }
                return new ManagementResponse
                {
                    NotifyHour = user.NotifyHoursPastMidnight
                };
            }

            usersRepository.Insert(new User
            {
                Email = request.Email,
                NotifyHoursPastMidnight = request.NotifyHour
            });

            return new ManagementResponse();
        }
    }
}