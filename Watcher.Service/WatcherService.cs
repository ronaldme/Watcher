using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Watcher.Common;
using Watcher.DAL;
using Watcher.Service.Services;
using Watcher.Service.Services.Notifiers;
using Timer = System.Timers.Timer;

namespace Watcher.Service
{
    class WatcherService : IHostedService
    {
        private readonly AppSettings _config;
        private readonly INotifyScheduler _notifyScheduler;
        private readonly IUpdateService _updateService;
        private readonly IMqService _overviewService;
        private Timer _movieInterval;
        private Timer _showInterval;
        private Timer _personInterval;

        public WatcherService(
            IOptions<AppSettings> config,
            INotifyScheduler notifyScheduler,
            IUpdateService updateService,
            IMqService overviewService)
        {
            _config = config.Value;
            _notifyScheduler = notifyScheduler;
            _updateService = updateService;
            _overviewService = overviewService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Migrate();

            await _notifyScheduler.Start();
            InitAndStartUpdateIntervals();
            _overviewService.HandleRequests();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _notifyScheduler.Stop();
            StopIntervals();
        }

        private async Task Migrate()
        {
            await using var db = new WatcherDbContext();
            await db.Database.MigrateAsync();
        }

        #region Timers (TODO: Refactor this)
        private void InitAndStartUpdateIntervals()
        {
            _movieInterval = CreateTimer(_config.MovieIntervalInSeconds);
            _showInterval = CreateTimer(_config.ShowIntervalInSeconds);
            _personInterval = CreateTimer(_config.PersonIntervalInSeconds);

            EnableTimer(_movieInterval, MovieIntervalElapsed);
            EnableTimer(_showInterval, ShowIntervalElapsed);
            EnableTimer(_personInterval, PersonIntervalElapsed);
        }

        private void EnableTimer(Timer timer, ElapsedEventHandler handler)
        {
            timer.Elapsed += handler;
            timer.Enabled = true;
        }

        private async void MovieIntervalElapsed(object sender, ElapsedEventArgs e) => await _updateService.UpdateMovies();
        private async void ShowIntervalElapsed(object sender, ElapsedEventArgs e) => await _updateService.UpdateShows();
        private async void PersonIntervalElapsed(object sender, ElapsedEventArgs e) => await _updateService.UpdatePersons();

        private Timer CreateTimer(int seconds) => new Timer(TimeSpan.FromSeconds(seconds).TotalMilliseconds);

        private void StopIntervals()
        {
            _movieInterval.Stop();
            _showInterval.Stop();
            _personInterval.Stop();
        }
        #endregion
    }
}