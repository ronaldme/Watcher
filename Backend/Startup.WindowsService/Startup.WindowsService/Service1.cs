using System.Linq;
using System.ServiceProcess;
using Castle.Core.Internal;
using Castle.Windsor;
using Services.DependencyInjection;
using Services.Interfaces;

namespace Startup.WindowsService
{
    public partial class Service1 : ServiceBase
    {
        private WindsorContainer container;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            container = new WindsorContainer();
            container.Install(new BackendDependencyInstaller());

            container.ResolveAll<IMqResponder>().ForEach(x => x.Start());
            container.ResolveAll<Services.Interfaces.IStartable>().ForEach(x => x.Start());

        }

        protected override void OnStop()
        {
            container.ResolveAll<IMqResponder>().ToList().ForEach(x => x.Stop());
            container.ResolveAll<Services.Interfaces.IStartable>().ToList().ForEach(x => x.Stop());
        }
    }
}