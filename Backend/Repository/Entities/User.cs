using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.Entities
{
    public class User
    {
        public User()
        {
            Movies = new HashSet<Movie>();
            Shows = new HashSet<Show>();
        }

         [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
        public virtual ICollection<Show> Shows { get; set; }
    }
}
