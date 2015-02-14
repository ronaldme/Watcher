using System.Collections.Generic;

namespace Messages.DTO
{
    public class MovieSubscriptionListDto
    {
        public List<MovieSubscriptionsDTO> Subscriptions { get; set; }
        public Filter Filter { get; set; }
        public string PrefixPath { get; set; }
    }
}