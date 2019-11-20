namespace Watcher.DAL.Entities
{
    public class UserShow
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int ShowId { get; set; }
        public Show Show { get; set; }
    }
}