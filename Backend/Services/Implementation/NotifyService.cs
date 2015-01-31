using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using BLL.Notifier;
using Repository;
using Repository.Entities;
using Repository.Repositories.Interfaces;
using Repository.UOW;
using Services.Interfaces;
using Timer = System.Timers.Timer;

namespace Services
{
    public class NotifyService : INotifyService, IStartable
    {
        private readonly IUsersRepository usersRepository;
        private readonly INotifyUser notifyService;
        private Thread Thread { get; set; }

        public NotifyService(IUsersRepository usersRepository, INotifyUser notifyService)
        {
            this.usersRepository = usersRepository;
            this.notifyService = notifyService;
        }

        public void Start()
        {
            Thread = new Thread(() =>
            {
                // Start notifying on the whole hour
                while (DateTime.UtcNow.Minute != 0)
                {
                    Thread.Sleep(500);
                }

                NotifyUsers();
            });

            Thread.Start();
        }

        public void Stop()
        {
            Thread.Abort();
        }

        public void NotifyUsers()
        {
            // after initial notify, execute every hour
            Notify(null, null);

            var notify = new Timer(3600000);
            notify.Elapsed += Notify;
            notify.Enabled = true;
        }

        private void Notify(object sender, ElapsedEventArgs args)
        {
            using (var watcherContext = new WatcherContext())
            {
                UnitOfWork.Current = new UnitOfWork(watcherContext);
                UnitOfWork.Current.BeginTransaction();
                try
                {
                    var users = usersRepository.All();

                    foreach (User user in users.Where(x => x.NotifyHoursPastMidnight == DateTime.UtcNow.Hour && 
                        (x.GetEmailNotifications || !string.IsNullOrEmpty(x.NotifyMyAndroidKey))))
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
                                    notifyService.NotifyUser(new UserNotification
                                    {
                                        Destination = user.Email,
                                        Message = GetSubject(notificationList),
                                        Subject = "New releases!"
                                    });
                                }
                            }
                            catch (Exception)
                            {
                            }

                            if (!string.IsNullOrEmpty(user.NotifyMyAndroidKey))
                            {
                                NotifyMyAndroid.NotifyUser(notificationList, user.NotifyMyAndroidKey);
                            }
                        }
                    }
                }
                finally
                {
                    UnitOfWork.Current.Commit();
                }
            }
        }

        private static string GetSubject(IEnumerable<string> names)
        {
            return names.Aggregate<string, string>(null, (current, name) => current + (name + "\n"));
        }

        private static void AddMovie(User user, List<string> notificationList, bool notifyDayLater)
        {
            notificationList.AddRange(
                from movie in user.Movies
                where movie.ReleaseDate.HasValue &&
                (notifyDayLater ? movie.ReleaseDate.Value.Date.AddDays(1) == DateTime.UtcNow.Date : movie.ReleaseDate.Value.Date == DateTime.UtcNow.Date)
                select movie.Name);
        }

        private static void AddShow(User user, List<string> notificationList, bool notifyDayLater)
        {
            notificationList.AddRange(
                from show in user.Shows
                where show.ReleaseNextEpisode.HasValue &&
                (notifyDayLater ? show.ReleaseNextEpisode.Value.Date.AddDays(1) == DateTime.UtcNow.Date : show.ReleaseNextEpisode.Value.Date == DateTime.UtcNow.Date)
                select string.Format("{0} season: {1} Episode nr: {2}", show.Name, show.CurrentSeason, show.NextEpisode));
        }

        private static void AddPerson(User user, List<string> notificationList, bool notifyDayLater)
        {
            notificationList.AddRange(
                from person in user.Persons
                where person.ReleaseDate.HasValue &&
                (notifyDayLater ? person.ReleaseDate.Value.Date.AddDays(1) == DateTime.UtcNow.Date : person.ReleaseDate.Value.Date == DateTime.UtcNow.Date)
                select string.Format("{0} {1}", person.Name, person.ProductionName));
        }
    }
}