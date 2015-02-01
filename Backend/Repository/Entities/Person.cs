using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
    
namespace Repository.Entities
{
    public class Person
    {
        public Person()
        {
            Users = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public DateTime? Birthday { get; set; }
        public string ProductionName { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public int TheMovieDbId { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
