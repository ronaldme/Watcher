using System;

namespace Watcher.Messages.Movie
{
    public class MovieSubscriptionsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string PosterPath { get; set; }
    }
}