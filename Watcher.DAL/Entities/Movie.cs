using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Watcher.DAL.Infrastructure;

namespace Watcher.DAL.Entities
{
    public class Movie : Entity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int TheMovieDbId { get; set; }
        public string PosterPath { get; set; }
        public virtual ICollection<UserMovie> UserMovies { get; set; }
    }
}
