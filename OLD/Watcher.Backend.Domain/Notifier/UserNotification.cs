namespace Watcher.Backend.Domain.Notifier
{
    public class UserNotification
    {
        public string Destination { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}