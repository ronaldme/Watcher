using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;

namespace Frontend.DI
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel kernel;

        public WindsorControllerFactory(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public override void ReleaseController(IController controller)
        {
            kernel.ReleaseComponent(controller);
        }

        protected override IController GetControllerInstance(RequestContext context, Type controllerType)
        {
            if (controllerType == null)
            {
                return base.GetControllerInstance(context, controllerType);
            }
            try
            {
                return (IController)kernel.Resolve(controllerType);
            }
            catch
            {
                return base.GetControllerInstance(context, controllerType);
            }
        }
    }
}