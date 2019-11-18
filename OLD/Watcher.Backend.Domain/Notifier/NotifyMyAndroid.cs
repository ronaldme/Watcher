using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using log4net;
using Watcher.Shared.Common;

namespace Watcher.Backend.Domain.Notifier
{
   public static class NotifyMyAndroid
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool NotifyUser(List<string> items, string key)
        {
            try
            {
                string subject = items.Aggregate("", (current, item) => current + item.Replace(" ", "%20") + "%0A");

                var request = (HttpWebRequest) WebRequest.Create(Urls.GetNotifyMyAndroidUrl(key) + subject);
                request.KeepAlive = true;
                request.Method = "GET";
                request.Accept = "application/json";
                request.ContentLength = 0;

                string content;

                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        content = reader.ReadToEnd();
                    }
                }

                const string success = "success code=\"200\"";

                return content.Contains(success);
            }
            catch (ArgumentNullException e)
            {
                log.Error("Error during user notification for NotifyMyAndroid", e);
                return false;
            }
        }
    }
}           