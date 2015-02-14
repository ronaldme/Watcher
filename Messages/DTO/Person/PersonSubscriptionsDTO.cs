using System;

namespace Messages.DTO
{
    public class PersonSubscriptionsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductionName { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string PosterPath { get; set; }
    }
}