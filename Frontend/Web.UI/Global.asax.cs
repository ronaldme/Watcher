using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using Web.UI.DI;

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

            var container = new WindsorContainer();
            container.Install(new WebInstaller());
 
            var castleControllerFactory = new WindsorFactory(container);
 
            ControllerBuilder.Current.SetControllerFactory(castleControllerFactory);
        }
    }
}