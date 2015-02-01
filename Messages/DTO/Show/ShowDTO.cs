using System;
using System.Collections.Generic;

namespace Messages.DTO
{
    public class ShowDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CurrentSeason { get; set; }
        public int NextEpisode { get; set; }
        public int EpisodeCount { get; set; }
        public DateTime? ReleaseNextEpisode { get; set; }
        public List<Season> Seasons { get; set; }
        public int NumberOfSeasons { get; set; }
    }
}