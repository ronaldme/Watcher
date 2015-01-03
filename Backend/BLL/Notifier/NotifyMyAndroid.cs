using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace BLL.Notifier
{
   public static class NotifyMyAndroid
    {
        public static bool NotifyUser(List<string> items)
        {
            string subject = items.Aggregate("", (current, item) => current + (item.Replace(" ", "%20") + "%0A"));

            var request = (HttpWebRequest)WebRequest.Create(Urls.NotifyMyAndroid + subject);
            request.KeepAlive = true;
            request.Method = "GET";
            request.Accept = "application/json";
            request.ContentLength = 0;

            string content;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    content = reader.ReadToEnd();
                }
            }

            const string success = "success code=\"200\"";

            return content.Contains(success);
        }
    }
}           