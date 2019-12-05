using MailKit.Net.Smtp;
using MimeKit;

namespace Watcher.Service.Services.Notifiers
{
    // TODO: DI

    public class MailNotifier
    {
        private readonly SmtpSettings _settings;

        public MailNotifier(SmtpSettings settings)
        {
            _settings = settings;
            // TODO: Implement using MailKit
        }

        public void NotifyUser(UserNotification notification)
        {
            var message = BuildMessage(notification);

            using var client = new SmtpClient {ServerCertificateValidationCallback = (s, c, h, e) => true};
            client.Connect("smtp.friends.com", 587, false);
            client.Authenticate(_settings.UserName, _settings.Password);

            client.Send(message);
            client.Disconnect(true);
        }

        private MimeMessage BuildMessage(UserNotification notification)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.From));
            message.To.Add(new MailboxAddress(notification.Destination));

            message.Subject = notification.Subject;
            message.Body = new TextPart("plain") {Text = notification.Message};
            return message;
        }
    }
}