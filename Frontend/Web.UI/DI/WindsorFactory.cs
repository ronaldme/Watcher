using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;

namespace Web.UI.DI
{
    public class WindsorFactory : DefaultControllerFactory
    {
        public IWindsorContainer Container { get; protected set; }

        public WindsorFactory(IWindsorContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            Container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return null;
            }

            return Container.Resolve(controllerType) as IController;
        }

      
    }
}