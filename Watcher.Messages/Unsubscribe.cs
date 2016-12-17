using Watcher.Shared.Common;

namespace Watcher.Messages
{
    public class Unsubscribe
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public SubscriptionType SubcriptionType { get; set; }
    }
}
