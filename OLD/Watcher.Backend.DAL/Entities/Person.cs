using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Watcher.Backend.DAL.Infrastructure;

namespace Watcher.Backend.DAL.Entities
{
    public class Person : Entity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        public string ProductionName { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int TheMovieDbId { get; set; }
        public string PosterPath { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
