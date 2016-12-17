using Watcher.Messages.Movie;
using Watcher.Messages.Person;
using Watcher.Messages.Show;

namespace Watcher.Messages.Subscription
{
    public class SubscriptionResponse
    {
        public MovieSubscriptionListDto MovieSubscriptions { get; set; }
        public ShowSubscriptionListDto ShowSubscriptions { get; set; }
        public PersonSubscriptionListDto PersonSubscriptions { get; set; }
    }
}