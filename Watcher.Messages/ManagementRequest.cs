﻿namespace Watcher.Messages
{
    public class ManagementRequest
    {
        public int NotifyHour { get; set; }
        public string Email { get; set; }
        public string OldEmail { get; set; }
        public bool SetData { get; set; }
        public bool NotifyDayLater { get; set; }
        public bool GetEmailNotifications { get; set; }
    }
}