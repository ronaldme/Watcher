using System;

namespace Messages.DTO
{
    public class TvShowDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? AirDate { get; set; }
        public int LastFinishedSeasonNr { get; set; }
        public int NextEpisodeNr { get; set; }
        public DateTime? ReleaseNextEpisode { get; set; }
    }
}