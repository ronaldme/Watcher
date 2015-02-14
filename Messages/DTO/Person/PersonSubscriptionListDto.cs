using System.Collections.Generic;

namespace Messages.DTO
{
    public class PersonSubscriptionListDto
    {
        public List<PersonSubscriptionsDTO> Subscriptions { get; set; }
        public Filter Filter { get; set; }
        public string PrefixPath { get; set; }
    }
}