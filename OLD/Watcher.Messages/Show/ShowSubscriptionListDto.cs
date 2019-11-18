using System.Collections.Generic;
using Watcher.Messages.Infrastructure;

namespace Watcher.Messages.Show
{
    public class ShowSubscriptionListDto : PagedResponse
    {
        public List<ShowSubscriptionsDto> Subscriptions { get; set; }
    }
}