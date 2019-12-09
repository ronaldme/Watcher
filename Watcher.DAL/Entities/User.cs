using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Watcher.DAL.Infrastructure;

namespace Watcher.DAL.Entities
{
    public class User : Entity
    {
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        public bool GetEmailNotifications { get; set; }
        public int NotifyAtHoursPastMidnight { get; set; }
        public bool NotifyDayLater { get; set; }
        public virtual ICollection<UserMovie> UserMovies { get; set; }
        public virtual ICollection<UserShow> UserShows { get; set; }
        public virtual ICollection<UserPerson> UserPersons { get; set; }
    }
}
