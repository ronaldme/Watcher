using System.ComponentModel.DataAnnotations;

namespace Watcher.Backend.DAL.Infrastructure
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}