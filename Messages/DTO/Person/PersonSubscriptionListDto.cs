using System.Collections.Generic;

namespace Messages.DTO
{
    public class PersonSubscriptionListDto
    {
        public List<PersonSubscriptionsDTO> Subscriptions { get; set; }
        public int Filtered { get; set; }
    }
}