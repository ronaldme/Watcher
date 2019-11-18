using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Watcher.Web.Startup))]
namespace Watcher.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
