namespace Watcher.Messages.Infrastructure
{
    public class PagedRequest
    {
        public string Email { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}