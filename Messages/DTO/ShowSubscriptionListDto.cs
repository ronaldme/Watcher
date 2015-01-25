using System;
using System.Collections.Generic;

namespace Messages.DTO
{
    public class ShowSubscriptionListDto
    {
        public List<ShowSubscriptionsDTO> Subscriptions { get; set; }
        public int Filtered { get; set; }
    }

    public class ShowSubscriptionsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrentSeason { get; set; }
        public int RemainingEpisodes { get; set; }
        public int EpisodeNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

    public class MovieSubscriptionListDto
    {
        public List<MovieSubscriptionsDTO> Subscriptions { get; set; }
        public int Filtered { get; set; }
    }

    public class MovieSubscriptionsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}