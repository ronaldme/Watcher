using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Quartz;
using Watcher.Backend.DAL;
using Watcher.Backend.DAL.Entities;

namespace Watcher.Backend.Domain.Notifier
{
    public class NotifyJob : IJob
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(IJobExecutionContext jobExecutionContext)
        {
            using (var context = new WatcherContext())
            {
                var users = context.Users
                    .Where(user => user.GetEmailNotifications || !string.IsNullOrEmpty(user.NotifyMyAndroidKey) &&
                                   (user.NotifyAtHoursPastMidnight == DateTime.UtcNow.Hour))
                    .ToList();

                var mailNotifier = new MailNotifier();

                foreach (User user in users)
                {
                    var notificationList = new List<string>();
                    bool notifyDayLater = user.NotifyDayLater;

                    if (user.Movies != null)
                    {
                        AddMovie(user, notificationList, notifyDayLater);
                    }
                    if (user.Shows != null)
                    {
                        AddShow(user, notificationList, notifyDayLater);
                    }
                    if (user.Persons != null)
                    {
                        AddPerson(user, notificationList, notifyDayLater);
                    }

                    if (notificationList.Count > 0)
                    {
                        try
                        {
                            if (user.GetEmailNotifications)
                            {
                                mailNotifier.NotifyUser(new UserNotification
                                {
                                    Destination = user.Email,
                                    Message = GetSubject(notificationList),
                                    Subject = "New releases!"
                                });
                            }
                        }
                        catch (Exception e)
                        {
                            log.Warn("Sending email notification failed", e);
                        }

                        if (!string.IsNullOrEmpty(user.NotifyMyAndroidKey))
                        {
                            try
                            {
                                NotifyMyAndroid.NotifyUser(notificationList, user.NotifyMyAndroidKey);
                            }
                            catch (Exception e)
                            {
                                log.Error("Sending NotifyMyAndroid notification failed", e);
                            }
                        }
                    }
                }
            }
        }

        private static string GetSubject(IEnumerable<string> names)
        {
            return names.Aggregate<string, string>(null, (current, name) => current + name + "\n");
        }

        private static void AddMovie(User user, List<string> notificationList, bool notifyDayLater)
        {
            notificationList.AddRange(
                from movie in user.Movies
                where movie.ReleaseDate.HasValue &&
                      (notifyDayLater
                          ? movie.ReleaseDate.Value.Date.AddDays(1) == DateTime.UtcNow.Date
                          : movie.ReleaseDate.Value.Date == DateTime.UtcNow.Date)
                select movie.Name);
        }

        private static void AddShow(User user, List<string> notificationList, bool notifyDayLater)
        {
            notificationList.AddRange(
                from show in user.Shows
                where show.ReleaseNextEpisode.HasValue &&
                      (notifyDayLater
                          ? show.ReleaseNextEpisode.Value.Date.AddDays(1) == DateTime.UtcNow.Date
                          : show.ReleaseNextEpisode.Value.Date == DateTime.UtcNow.Date)
                select $"{show.Name} season: {show.CurrentSeason} Episode nr: {show.NextEpisode}");
        }

        private static void AddPerson(User user, List<string> notificationList, bool notifyDayLater)
        {
            notificationList.AddRange(
                from person in user.Persons
                where person.ReleaseDate.HasValue &&
                      (notifyDayLater
                          ? person.ReleaseDate.Value.Date.AddDays(1) == DateTime.UtcNow.Date
                          : person.ReleaseDate.Value.Date == DateTime.UtcNow.Date)
                select $"{person.Name} {person.ProductionName}");
        }
    }
}