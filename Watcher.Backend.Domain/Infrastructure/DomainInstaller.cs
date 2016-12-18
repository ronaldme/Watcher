using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Watcher.Backend.DAL;
using Watcher.Backend.Domain.Notifier;
using Watcher.Backend.Domain.Services;

namespace Watcher.Backend.Domain.Infrastructure
{
    public class DomainInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ITheMovieDb>()
                .ImplementedBy<TheMovieDb>().LifestyleTransient());

            container.Register(Component.For<INotifyService>()
                .ImplementedBy<NotifyService>());

            container.Register(Component.For<IUpdateService>()
                .ImplementedBy<UpdateService>());

            container.Register(Component.For<WatcherContext>()
                .LifestyleTransient());

            container.Register(Classes.FromThisAssembly()
                .BasedOn<IService>().WithServiceAllInterfaces());
        }
    }
}