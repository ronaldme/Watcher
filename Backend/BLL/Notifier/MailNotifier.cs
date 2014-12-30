﻿using System.Configuration;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;

namespace BLL.Notifier
{
    public class MailNotifier : INotifyUser
    {
        private readonly SmtpSection smtpSection;

        public MailNotifier()
        {
            smtpSection = (SmtpSection) ConfigurationManager.GetSection("system.net/mailSettings/smtp");
        }

        public void NotifyUser(UserNotification notification)
        {
            var client = InitializeSmtpClient();

            var mailMessage = new MailMessage(smtpSection.From, notification.Destination)
            {
                Subject = notification.Subject,
                Body = notification.Message
            };

            client.SendAsync(mailMessage, null);
        }

        private SmtpClient InitializeSmtpClient()
        {
            return new SmtpClient()
            {
                Host = smtpSection.Network.Host,
                Port = smtpSection.Network.Port,
                UseDefaultCredentials = smtpSection.Network.DefaultCredentials,
                EnableSsl = smtpSection.Network.EnableSsl,
                DeliveryMethod = smtpSection.DeliveryMethod,
                Timeout = 10000,
                Credentials = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password),
            };
        }  
    }
}