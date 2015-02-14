using System;

namespace Messages.DTO
{
    public class ShowSubscriptionsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrentSeason { get; set; }
        public int RemainingEpisodes { get; set; }
        public int EpisodeNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string PosterPath { get; set; }
    }
}