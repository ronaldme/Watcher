using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Topshelf;
using Watcher.Backend.Domain.Infrastructure;
using Watcher.Shared.Infrastructure;

namespace Watcher.Backend.Startup
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            container.Install(
                new DomainInstaller(),
                new BusInstaller(),
                new BackendInstaller());

            HostFactory.Run(x =>
            {
                x.Service<Watcher>(s =>
                {
                    var watcher = container.Resolve<Watcher>();
                    s.ConstructUsing(name => watcher);

                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.DependsOnMsSql();
                x.RunAsLocalSystem();

                x.SetDescription("Keep track of your favorite shows/movies");
                x.SetDisplayName("Watcher");
                x.SetServiceName("Watcher");
            });
        }
    }
}
