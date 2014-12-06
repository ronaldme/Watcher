using System;
using System.Collections.Generic;

namespace Messages.DTO
{
    public class SubscriptionListDTO
    {
        public List<SubscriptionsDTO> Subscriptions { get; set; }
        public int Filtered { get; set; }
    }

    public class SubscriptionsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LastFinishedSeason { get; set; }
        public int EpisodeNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}