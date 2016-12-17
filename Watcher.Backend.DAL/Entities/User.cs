using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Watcher.Backend.DAL.Infrastructure;

namespace Watcher.Backend.DAL.Entities
{
    public class User : Entity
    {
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        public bool GetEmailNotifications { get; set; }
        public int NotifyAtHoursPastMidnight { get; set; }
        public string NotifyMyAndroidKey { get; set; }
        public bool NotifyDayLater { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
        public virtual ICollection<Show> Shows { get; set; }
        public virtual ICollection<Person> Persons { get; set; }
    }
}
