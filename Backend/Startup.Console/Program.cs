using Castle.Core.Internal;
using Castle.Windsor;
using log4net;
using Services.DependencyInjection;
using Services.Interfaces;

namespace Startup.Console
{
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            log.Info("Starting server");
            var container = new WindsorContainer();
            container.Install(new BackendDependencyInstaller());

            container.ResolveAll<IMqResponder>().ForEach(x => x.Start());
            container.ResolveAll<IStartable>().ForEach(x => x.Start());

            log.Info("Server started");
            System.Console.ReadLine();

            System.Console.WriteLine("Press again to stop.");
            System.Console.ReadLine();
            log.Info("Stopping server");

            container.ResolveAll<IMqResponder>().ForEach(x => x.Stop());
            container.ResolveAll<IStartable>().ForEach(x => x.Stop());
            log.Info("Server stopped");
        }
    }
}
