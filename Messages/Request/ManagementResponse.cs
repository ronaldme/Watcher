﻿namespace Messages.Request
{
    public class ManagementResponse
    {
        public int NotifyHour { get; set; }
        public bool Success { get; set; }
        public bool NotifyDayLater { get; set; }
        public string NotifyMyAndroidKey { get; set; }
    }
}