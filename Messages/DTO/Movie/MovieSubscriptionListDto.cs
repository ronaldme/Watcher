using System.Collections.Generic;

namespace Messages.DTO
{
    public class MovieSubscriptionListDto
    {
        public List<MovieSubscriptionsDTO> Subscriptions { get; set; }
        public int Filtered { get; set; }
    }
}