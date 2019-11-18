using System.Collections.Generic;
using System.Linq;
using Watcher.Backend.Domain.Infrastructure;
using Watcher.Backend.Domain.Services;

namespace Watcher.Backend.Startup
{
    public class Watcher
    {
        private readonly INotifyService notifyService;
        private readonly IUpdateService updateService;
        private readonly List<IService> services;

        public Watcher(IEnumerable<IService> services,
            INotifyService notifyService,
            IUpdateService updateService)
        {
            this.notifyService = notifyService;
            this.updateService = updateService;
            this.services = services.ToList();
        }

        public void Start()
        {
            services.ForEach(s => s.HandleRequests());
            notifyService.Start();
            updateService.Start();
        }

        public void Stop()
        {
            notifyService.Stop();
            updateService.Stop();
        }
    }
}
