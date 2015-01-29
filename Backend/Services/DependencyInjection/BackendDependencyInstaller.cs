using System.Configuration;
using BLL;
using BLL.Notifier;
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

            var emailNotifier = new MailNotifier();
            container.Register(Component.For<INotifyUser>().Instance(emailNotifier));
            
            container.Register(Component.For<WatcherData>().LifeStyle.Singleton);
            
            container.Register(
                Classes.FromAssemblyContaining<TvShowService>()
                    .Where(type => true)
                    .WithServiceAllInterfaces()
                    .LifestyleSingleton()
                );

            container.Register(
                Classes.FromAssemblyContaining<IPersonRepository>()
                    .Where(type => true)
                    .WithServiceAllInterfaces()
                    .LifestyleTransient()
                );
        } 
    }
}