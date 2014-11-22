using System;

namespace Repository.Entities
{
    public class Show : Entity
    {
        public string Name { get; set; }
        public int Seasons { get; set; }
        public DateTime NextEpisode { get; set; }
    }
}
