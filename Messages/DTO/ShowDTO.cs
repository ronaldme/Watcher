using System;
using System.Collections.Generic;

namespace Messages.DTO
{
    public class ShowDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LastFinishedSeason { get; set; }
        public int NextEpisode { get; set; }
        public DateTime? ReleaseNextEpisode { get; set; }
        public List<Season> Seasons { get; set; }
        public int NumberOfSeasons { get; set; }
    }

    public class Season
    {
        public string Air_Date { get; set; }
        public int Id { get; set; }
        public string Poster_Path { get; set; }
        public int Season_Number { get; set; }
    }
}