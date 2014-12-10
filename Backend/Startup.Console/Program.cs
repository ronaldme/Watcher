using Castle.Core.Internal;
using Castle.Windsor;
using Services.DependencyInjection;
using Services.Interfaces;

namespace Startup.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = new WindsorContainer();
            container.Install(new BackendDependencyInstaller());

            container.ResolveAll<IMqResponder>().ForEach(x => x.Start());
            container.ResolveAll<IStartable>().ForEach(x => x.Start());
            
            System.Console.WriteLine("Services started!");
            System.Console.ReadLine();

            System.Console.WriteLine("Press again to stop.");
            System.Console.ReadLine();

            container.ResolveAll<IMqResponder>().ForEach(x => x.Stop());
            container.ResolveAll<IStartable>().ForEach(x => x.Stop());
        }
    }
}
