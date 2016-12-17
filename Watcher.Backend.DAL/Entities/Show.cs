using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Watcher.Backend.DAL.Infrastructure;

namespace Watcher.Backend.DAL.Entities
{
    public class Show : Entity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string PosterPath { get; set; }
        public int CurrentSeason { get; set; }
        public int NextEpisode { get; set; }
        public int EpisodeCount { get; set; }
        public DateTime? ReleaseNextEpisode { get; set; }
        public int TheMovieDbId { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
