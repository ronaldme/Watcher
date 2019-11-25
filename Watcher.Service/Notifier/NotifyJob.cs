/*using System;
using System.Collections.Generic;
using System.Linq;
using Watcher.DAL.Entities;

namespace Watcher.Service.Notifier
{
    public class NotifyJob : IJob
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(IJobExecutionContext jobExecutionContext)
        {
            using (var context = new WatcherDbContext())
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

                    if (user.UserMovies != null)
                    {
                        AddMovie(user, notificationList, notifyDayLater);
                    }
                    if (user.UserShows != null)
                    {
                        AddShow(user, notificationList, notifyDayLater);
                    }
                    if (user.UserPersons != null)
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
                from userMovie in user.UserMovies
                where userMovie.Movie.ReleaseDate.HasValue &&
                      (notifyDayLater
                          ? userMovie.Movie.ReleaseDate.Value.Date.AddDays(1) == DateTime.UtcNow.Date
                          : userMovie.Movie.ReleaseDate.Value.Date == DateTime.UtcNow.Date)
                select userMovie.Movie.Name);
        }

        private static void AddShow(User user, List<string> notificationList, bool notifyDayLater)
        {
            notificationList.AddRange(
                from userShow in user.UserShows
                where userShow.Show.ReleaseNextEpisode.HasValue &&
                      (notifyDayLater
                          ? userShow.Show.ReleaseNextEpisode.Value.Date.AddDays(1) == DateTime.UtcNow.Date
                          : userShow.Show.ReleaseNextEpisode.Value.Date == DateTime.UtcNow.Date)
                select $"{userShow.Show.Name} season: {userShow.Show.CurrentSeason} Episode nr: {userShow.Show.NextEpisode}");
        }

        private static void AddPerson(User user, List<string> notificationList, bool notifyDayLater)
        {
            notificationList.AddRange(
                from person in user.UserPersons
                where person.Person.ReleaseDate.HasValue &&
                      (notifyDayLater
                          ? person.Person.ReleaseDate.Value.Date.AddDays(1) == DateTime.UtcNow.Date
                          : person.Person.ReleaseDate.Value.Date == DateTime.UtcNow.Date)
                select $"{person.Person.Name} {person.Person.ProductionName}");
        }
    }
}*/