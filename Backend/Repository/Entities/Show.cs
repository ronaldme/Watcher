using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entities
{
    public class Show
    {
        public Show()
        {
            Users = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public DateTime? ReleaseDate { get; set; }
        public int LastFinishedSeason { get; set; }
        public DateTime? ReleaseLastEpisode { get; set; }
        public DateTime? ReleaseNextEpisode { get; set; }

        public int? TheMovieDbId { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
