using System;
using System.Configuration;
using EasyNetQ;

namespace Watcher.Shared.Infrastructure
{
    public class BusBuilder
    {
        public static IBus CreateMessageBus()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["easynetq"];
            if (connectionString == null || connectionString.ConnectionString == string.Empty)
            {
                throw new Exception("easynetq connection string is missing or empty");
            }

            return RabbitHutch.CreateBus(connectionString.ConnectionString);
        }
    }
}