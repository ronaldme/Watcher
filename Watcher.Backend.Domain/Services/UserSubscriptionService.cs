using System;
using System.Linq;
using EasyNetQ;
using Watcher.Backend.DAL;
using Watcher.Backend.Domain.Infrastructure;
using Watcher.Messages.Movie;
using Watcher.Messages.Person;
using Watcher.Messages.Show;
using Watcher.Messages.Subscription;

namespace Watcher.Backend.Domain.Services
{
    public class UserSubscriptionService : IService
    {
        private readonly IBus bus;
        
        public UserSubscriptionService(IBus bus)
        {
            this.bus = bus;
        }

        public void HandleRequests()
        {
            bus.Respond<ShowSubscriptionRequest, ShowSubscriptionListDto>(GetShowSubscriptions);
            bus.Respond<MovieSubscriptionRequest, MovieSubscriptionListDto>(GetMovieSubscriptions);
            bus.Respond<PersonSubscriptionRequest, PersonSubscriptionListDto>(GetPersonSubscriptions);
        }

        private PersonSubscriptionListDto GetPersonSubscriptions(PersonSubscriptionRequest request)
        {
            using (var context = new WatcherContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Email == request.Email);

                if (user != null)
                {
                   var subscriptions = user.Persons.Select(x => new PersonSubscriptionsDto
                   {
                        Id = x.Id,
                        Name = x.Name,
                        ProductionName = x.ProductionName,
                        ReleaseDate = x.ReleaseDate,
                        PosterPath = x.PosterPath
                    })
                    .OrderByDescending(x => x.ReleaseDate)
                    .Skip(request.Skip)
                    .Take(request.Take)
                    .ToList();

                    return new PersonSubscriptionListDto
                    {
                        Subscriptions = subscriptions,
                        Total = user.Persons.Count
                    };
                }

                return new PersonSubscriptionListDto();
            }
        }

        private ShowSubscriptionListDto GetShowSubscriptions(ShowSubscriptionRequest request)
        {
            using (var context = new WatcherContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Email == request.Email);
                    
                if (user != null)
                {
                    var shows = user.Shows
                    .Skip(request.Skip)
                    .Take(request.Take)
                    .Select(x => new ShowSubscriptionsDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ReleaseDate = x.ReleaseNextEpisode ?? new DateTime(1),
                        EpisodeNumber = x.NextEpisode,
                        CurrentSeason = x.CurrentSeason,
                        RemainingEpisodes = x.EpisodeCount - x.NextEpisode + 1, // also count next episode
                        PosterPath = x.PosterPath
                    })
                    .ToList();

                    return new ShowSubscriptionListDto
                    {
                        Subscriptions = shows,
                        Total = user.Shows.Count
                    };
                }

                return new ShowSubscriptionListDto();
            }
        }

        private MovieSubscriptionListDto GetMovieSubscriptions(MovieSubscriptionRequest request)
        {
            using (var context = new WatcherContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Email == request.Email);
                   
                if (user != null)
                {
                    var movies = user.Movies.Select(x => new MovieSubscriptionsDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ReleaseDate = x.ReleaseDate ?? DateTime.MinValue,
                        PosterPath = x.PosterPath
                    })
                    .Skip(request.Skip)
                    .Take(request.Take)
                    .ToList();

                    return new MovieSubscriptionListDto
                    {
                        Subscriptions = movies,
                        Total = user.Movies.Count
                    };
                }

                return new MovieSubscriptionListDto();
            }
        }
    }
}