using System.Collections.Generic;
using Watcher.Messages.Infrastructure;

namespace Watcher.Messages.Movie
{
    public class MovieSubscriptionListDto : PagedResponse
    {
        public List<MovieSubscriptionsDto> Subscriptions { get; set; }
    }
}