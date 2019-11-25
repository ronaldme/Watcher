using Watcher.Common;

namespace Watcher.Messages
{
    public class Unsubscribe
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
    }
}
