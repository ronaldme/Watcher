using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Watcher.DAL;
using Watcher.Service.Services;
using Watcher.Service.Services.Notifiers;
using Watcher.Service.TheMovieDb;

namespace Watcher.Service.Infrastructure
{
    public class DomainInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ITheMovieDb>()
                .ImplementedBy<TheMovieDb.TheMovieDb>().LifestyleTransient());

            container.Register(Component.For<INotifyScheduler>()
                .ImplementedBy<NotifyScheduler>());

            container.Register(Component.For<IUpdateService>()
                .ImplementedBy<UpdateService>());

            container.Register(Component.For<WatcherDbContext>()
                .LifestyleTransient());

            container.Register(Classes.FromAssemblyContaining<IService>()
                .BasedOn<IService>().WithServiceAllInterfaces());
        }
    }
}