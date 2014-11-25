using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entities
{
    [Table("Show")]
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

        public DateTime? NextEpisode { get; set; }

        public DateTime? NextEpisodeNr { get; set; }

        public int? TheMovieDbId { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
