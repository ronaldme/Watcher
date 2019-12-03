using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Watcher.Service.Services.Notifiers
{
    public class NotifyScheduler : INotifyScheduler
    {
        private IScheduler _scheduler;

        public async Task Start()
        {
            var factory = new StdSchedulerFactory();
            _scheduler = await factory.GetScheduler();
            var job = JobBuilder.Create<NotifyJob>().Build();

            var startTime = GetStartTime();

            var trigger = BuildTrigger(startTime);
            await _scheduler.ScheduleJob(job, trigger);
        }

        public async Task Stop() => await _scheduler.Clear();

        private static ITrigger BuildTrigger(DateTime startTime)
        {
            return TriggerBuilder.Create()
                .StartAt(startTime)
                .WithSimpleSchedule(builder => builder
                    .WithIntervalInHours(1)
                    .RepeatForever())
                .Build();
        }

        /// <summary>
        /// Start at the next hour
        /// </summary>
        private static DateTime GetStartTime()
        {
            int minutesUntilNextHour = 60 - DateTime.Now.Minute;
            return DateTime.Now.AddMinutes(minutesUntilNextHour);
        }
    }
}