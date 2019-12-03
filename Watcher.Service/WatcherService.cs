using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Watcher.Common;
using Watcher.Service.Services.Notifiers;

namespace Watcher.Service
{
    class WatcherService : IHostedService
    {
        private readonly IOptions<AppSettings> _config;
        private readonly INotifyScheduler _notifyScheduler;

        public WatcherService(
            IOptions<AppSettings> config,
            INotifyScheduler notifyScheduler)
        {
            _config = config;
            _notifyScheduler = notifyScheduler;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _notifyScheduler.Start();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _notifyScheduler.Stop();
        }
    }
}