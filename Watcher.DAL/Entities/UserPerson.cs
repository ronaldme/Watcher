namespace Watcher.DAL.Entities
{
    public class UserPerson
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}