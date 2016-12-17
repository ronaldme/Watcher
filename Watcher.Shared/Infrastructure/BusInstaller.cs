using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EasyNetQ;

namespace Watcher.Shared.Infrastructure
{
    public class BusInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IBus>().UsingFactoryMethod(BusBuilder.CreateMessageBus).LifestyleSingleton());
        }
    }
}