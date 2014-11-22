using System.Configuration;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;

namespace BLL.Notifier
{
    public class Mailer
    {
        private readonly SmtpSection smtpSection;

        public Mailer()
        {
            smtpSection = (SmtpSection) ConfigurationManager.GetSection("system.net/mailSettings/smtp");
        }

        public void Send(string destination, string subject, string message)
        {
            var client = InitializeSmtpClient();

            var mailMessage = new MailMessage(destination, smtpSection.From)
            {
                Subject = subject,
                Body = message
            };
            
            client.Send(mailMessage);
        }

        public SmtpClient InitializeSmtpClient()
        {
            return new SmtpClient()
            {
                Host = smtpSection.Network.Host,
                Port = smtpSection.Network.Port,
                UseDefaultCredentials = smtpSection.Network.DefaultCredentials,
                Credentials = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password),
                EnableSsl = smtpSection.Network.EnableSsl,
                DeliveryMethod = smtpSection.DeliveryMethod
            };
        }
    }
}