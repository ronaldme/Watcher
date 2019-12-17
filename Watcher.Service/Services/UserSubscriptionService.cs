using System;
using System.Linq;
using EasyNetQ;
using Watcher.DAL;
using Watcher.Messages.Movie;
using Watcher.Messages.Person;
using Watcher.Messages.Show;
using Watcher.Messages.Subscription;

namespace Watcher.Service.Services
{
    class UserSubscriptionService : IMqService
    {
        private readonly IBus _bus;
        
        public UserSubscriptionService(IBus bus)
        {
            _bus = bus;
        }

        public void HandleRequests()
        {
            _bus.Respond<ShowSubscriptionRequest, ShowSubscriptionListDto>(GetShowSubscriptions);
            _bus.Respond<MovieSubscriptionRequest, MovieSubscriptionListDto>(GetMovieSubscriptions);
            _bus.Respond<PersonSubscriptionRequest, PersonSubscriptionListDto>(GetPersonSubscriptions);
        }

        private PersonSubscriptionListDto GetPersonSubscriptions(PersonSubscriptionRequest request)
        {
            using var context = new WatcherDbContext();
            var user = context.Users.FirstOrDefault(x => x.Email == request.Email);

            if (user != null)
            {
                var subscriptions = user.UserPersons.Select(x => new PersonSubscriptionsDto
                    {
                        Id = x.Person.Id,
                        Name = x.Person.Name,
                        ProductionName = x.Person.ProductionName,
                        ReleaseDate = x.Person.ReleaseDate,
                        PosterPath = x.Person.PosterPath
                    })
                    .OrderByDescending(x => x.ReleaseDate)
                    .Skip(request.Skip)
                    .Take(request.Take)
                    .ToList();

                return new PersonSubscriptionListDto
                {
                    Subscriptions = subscriptions,
                    Total = user.UserPersons.Count
                };
            }

            return new PersonSubscriptionListDto();
        }

        private ShowSubscriptionListDto GetShowSubscriptions(ShowSubscriptionRequest request)
        {
            using var context = new WatcherDbContext();
            var user = context.Users.FirstOrDefault(x => x.Email == request.Email);
                    
            if (user != null)
            {
                var shows = user.UserShows
                    .Skip(request.Skip)
                    .Take(request.Take)
                    .Select(x => new ShowSubscriptionsDto
                    {
                        Id = x.Show.Id,
                        Name = x.Show.Name,
                        ReleaseDate = x.Show.ReleaseNextEpisode ?? new DateTime(1),
                        EpisodeNumber = x.Show.NextEpisode,
                        CurrentSeason = x.Show.CurrentSeason,
                        RemainingEpisodes = x.Show.EpisodeCount - x.Show.NextEpisode + 1, // also count next episode
                        PosterPath = x.Show.PosterPath
                    })
                    .ToList();

                return new ShowSubscriptionListDto
                {
                    Subscriptions = shows,
                    Total = user.UserShows.Count
                };
            }

            return new ShowSubscriptionListDto();
        }

        private MovieSubscriptionListDto GetMovieSubscriptions(MovieSubscriptionRequest request)
        {
            using var context = new WatcherDbContext();
            var user = context.Users.FirstOrDefault(x => x.Email == request.Email);
                   
            if (user != null)
            {
                var movies = user.UserMovies.Select(x => new MovieSubscriptionsDto
                    {
                        Id = x.Movie.Id,
                        Name = x.Movie.Name,
                        ReleaseDate = x.Movie.ReleaseDate ?? DateTime.MinValue,
                        PosterPath = x.Movie.PosterPath
                    })
                    .Skip(request.Skip)
                    .Take(request.Take)
                    .ToList();

                return new MovieSubscriptionListDto
                {
                    Subscriptions = movies,
                    Total = user.UserMovies.Count
                };
            }

            return new MovieSubscriptionListDto();
        }
    }
}