namespace Watcher.Service.Services.Notifiers
{
    public class SmtpSettings
    {
        public string From { get; set; }
        public string Host { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
    }
}