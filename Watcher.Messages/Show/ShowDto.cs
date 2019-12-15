using System;
using System.Collections.Generic;

namespace Watcher.Messages.Show
{
    public class ShowDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PosterPath { get; set; }
        public int CurrentSeason { get; set; }
        public int NextEpisode { get; set; }
        public int EpisodeCount { get; set; }
        public DateTime? ReleaseNextEpisode { get; set; }
        public List<Season> Seasons { get; set; }
        public int NumberOfSeasons { get; set; }
        public string Description { get; set; }
        public double VoteAverage { get; set; }
    }
}