using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Watcher.Common;
using Watcher.DAL;
using Watcher.Service.API;
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
                    services.AddSingleton<ITheMovieDb, TheMovieDb>();
                    
                    // MQ services
                    AddMqServices(services);

                    services.AddTransient<IUpdateService, UpdateService>();

                    var config = hostContext.Configuration;
                    services.Configure<AppSettings>(config.GetSection("AppSettings"));
                    services.Configure<ConnectionString>(config.GetSection("ConnectionStrings"));

                    services.AddSingleton(RabbitHutch.CreateBus(config.GetConnectionString("easynetq")));
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Debug);
                });

        private static void AddMqServices(IServiceCollection services)
        {
            services.AddSingleton<IMqService, OverviewService>();
            services.AddSingleton<IMqService, ManagementService>();
            services.AddSingleton<IMqService, SubscribeService>();
            services.AddSingleton<IMqService, UserSubscriptionService>();
        }
    }
}
