namespace Watcher.Messages
{
    public class ManagementResponse
    {
        public int NotifyHour { get; set; }
        public bool NotifyDayLater { get; set; }
        public bool GetEmailNotifications { get; set; }
        public string NotifyMyAndroidKey { get; set; }
        public bool Success { get; set; }
    }
}