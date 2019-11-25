using System.Collections.Generic;
using Watcher.Messages.Infrastructure;

namespace Watcher.Messages.Person
{
    public class PersonSubscriptionListDto : PagedResponse
    {
        public List<PersonSubscriptionsDto> Subscriptions { get; set; }
    }
}