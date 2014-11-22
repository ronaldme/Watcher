using System.Configuration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EasyNetQ;

namespace Services.DependencyInjection
{
    public class BackendDependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            IBus bus = RabbitHutch.CreateBus(ConfigurationManager.AppSettings["rabbitMq"]);
            container.Register(Component.For<IBus>().Instance(bus));

            container.Register(
                Classes.FromAssemblyContaining<TvShows>()
                    .Where(type => true)
                    .WithServiceAllInterfaces()
                    .LifestyleSingleton()
                );
        } 
    }
}