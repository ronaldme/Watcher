using System;
using Quartz;
using Quartz.Impl;
using Watcher.Service.Notifier;

namespace Watcher.Service.Services
{
    public class NotifyService : INotifyService
    {
        private IScheduler scheduler;

        public void Start()
        {
            /*scheduler = new StdSchedulerFactory().GetScheduler();
            scheduler.Start();

            var job = JobBuilder.Create<NotifyJob>().Build();

            var minute = DateTime.Now.Minute;
            int nextHour = 60 - minute;
            var startTime = DateTime.Now.AddTicks(TimeSpan.FromMinutes(nextHour).Ticks);

            // start around the first whole hour
            ITrigger trigger = TriggerBuilder.Create()
                .StartNow()
                //.StartAt(startTime)
                .WithSimpleSchedule(builder => builder
                    .WithIntervalInHours(1)
                    .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);*/
        }

        public void Stop()
        {
            scheduler.Clear();
        }
    }
}