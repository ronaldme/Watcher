using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Watcher.Common;
using Watcher.DAL;

namespace Watcher.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .UseWindowsService()
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<WatcherService>();
                    services.AddDbContext<WatcherDbContext>();
                    
                });
    }
}
