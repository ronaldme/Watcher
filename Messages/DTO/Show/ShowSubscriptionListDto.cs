using System.Collections.Generic;

namespace Messages.DTO
{
    public class ShowSubscriptionListDto
    {
        public List<ShowSubscriptionsDTO> Subscriptions { get; set; }
        public Filter Filter { get; set; }
        public string PrefixPath { get; set; }
    }
}