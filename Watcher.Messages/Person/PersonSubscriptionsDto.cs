using System;

namespace Watcher.Messages.Person
{
    public class PersonSubscriptionsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductionName { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string PosterPath { get; set; }
    }
}