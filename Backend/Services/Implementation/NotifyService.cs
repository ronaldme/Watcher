using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using BLL.Notifier;
using Repository.Entities;
using Repository.Repositories.Interfaces;
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
            var users = usersRepository.All();

            foreach (User user in users.Where(x => x.NotifyHoursPastMidnight == DateTime.UtcNow.Hour))
            {
                var notificationList = new List<string>();

                if (user.Movies != null)
                {
                    AddMovie(user, notificationList);
                }
                if (user.Shows != null)
                {
                    AddShow(user, notificationList);
                }
                if (user.Persons != null)
                {
                    // implement later
                }

                if (notificationList.Count > 1)
                {
                    try
                    {
                        notifyService.NotifyUser(new UserNotification
                        {
                            Destination = user.Email,
                            Message = GetSubject(notificationList),
                            Subject = "New releases today!"
                        });
                    }
                    catch (Exception e)
                    {
                        // sending mail time-out
                    }

                    if (!string.IsNullOrEmpty(user.NotifyMyAndroidKey))
                    {
                        NotifyMyAndroid.NotifyUser(notificationList, user.NotifyMyAndroidKey);
                    }
                }
            }
        }

        private static string GetSubject(IEnumerable<string> names)
        {
            return names.Aggregate<string, string>(null, (current, name) => current + (name + "\n"));
        }

        private static void AddMovie(User user, List<string> notificationList)
        {
            notificationList.AddRange(
                from movie in user.Movies 
                where movie.ReleaseDate.HasValue && movie.ReleaseDate.Value.Day == DateTime.UtcNow.Day 
                select movie.Name);
        }


        private static void AddShow(User user, List<string> notificationList)
        {
            notificationList.AddRange(
                from show in user.Shows
                where show.ReleaseNextEpisode.HasValue && show.ReleaseNextEpisode.Value.Day == DateTime.UtcNow.Day
                select show.Name);
        }
    }
}