using System.Linq;
using System.Reflection;
using System.Web.Configuration;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EasyNetQ;
using Web.UI.Controllers;

namespace Web.UI.DI
{
    public class WebInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var contollers = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.BaseType == typeof(BaseController)).ToList();
            foreach (var controller in contollers)
            {
                container.Register(Component.For(controller).LifestylePerWebRequest());
            }

            var connectionString = WebConfigurationManager.AppSettings.Get("rabbitMq");

            IBus bus = RabbitHutch.CreateBus(connectionString);

            container.Register(
                Component.For<IBus>().Instance(bus)
            );


        }
    }
}