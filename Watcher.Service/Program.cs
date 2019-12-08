using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Watcher.Common;
using Watcher.DAL;
using Watcher.Service.Services;
using Watcher.Service.Services.Notifiers;

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

                    services.AddSingleton<INotifyScheduler, NotifyScheduler>();

                    var config = hostContext.Configuration;
                    services.Configure<AppSettings>(config.GetSection("AppSettings"));
                    services.Configure<ConnectionString>(config.GetSection("ConnectionStrings"));
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Debug);
                });
    }
}
