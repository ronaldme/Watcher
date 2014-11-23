using System.Configuration;
using System.Data.Entity;
using BLL;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EasyNetQ;
using Repository;
using Repository.Repositories.Interfaces;

namespace Services.DependencyInjection
{
    public class BackendDependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            IBus bus = RabbitHutch.CreateBus(ConfigurationManager.AppSettings["rabbitMq"]);
            container.Register(Component.For<IBus>().Instance(bus));

            var theMovieDb = new TheMovieDb();
            container.Register(Component.For<ITheMovieDb>().Instance(theMovieDb));

            container.Register(Component.For<WatcherData>().LifeStyle.Singleton);

            container.Register(
                Classes.FromAssemblyContaining<TvShows>()
                    .Where(type => true)
                    .WithServiceAllInterfaces()
                    .LifestyleSingleton()
                );

            container.Register(
                Classes.FromAssemblyContaining<IActorRepository>()
                    .Where(type => true)
                    .WithServiceAllInterfaces()
                    .LifestyleTransient()
                );
        } 
    }
}