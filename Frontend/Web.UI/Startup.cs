using Microsoft.Owin;
using Owin;
using Web.UI;

[assembly: OwinStartup(typeof(Startup))]
namespace Web.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}