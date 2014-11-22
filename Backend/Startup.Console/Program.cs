using Castle.Core.Internal;
using Castle.Windsor;
using Services.Interfaces;
using Startup.Console.DependencyInjection;

namespace Startup.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = new WindsorContainer();
            container.Install(new BackendDependencyInstaller());

            container.ResolveAll<IMqResponder>().ForEach(x => x.Start());
            
            System.Console.WriteLine("Services started!");
            System.Console.ReadLine();
        }
    }
}
