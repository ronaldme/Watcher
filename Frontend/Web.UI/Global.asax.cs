using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Web.UI
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            /*var container = new WindsorContainer();

            container.Register(
                Classes.FromAssemblyContaining<Request>()
                    .Where(type => true)
                    .WithServiceAllInterfaces()
                    .LifestylePerWebRequest()
                );
           
            var factory = new WindsorControllerFactory(container.Kernel);

            ControllerBuilder.Current.SetControllerFactory(factory);*/
        }
    }
}