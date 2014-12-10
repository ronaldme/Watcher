using System.ServiceProcess;
using Castle.Core.Internal;
using Castle.Windsor;
using Services.DependencyInjection;
using Services.Interfaces;

namespace Startup
{
    public partial class Service : ServiceBase
    {
        private IWindsorContainer container;

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            container = new WindsorContainer();
            container.Install(new BackendDependencyInstaller());

            container.ResolveAll<IMqResponder>().ForEach(x => x.Start());
            container.ResolveAll<IStartable>().ForEach(x => x.Start());
        }

        protected override void OnStop()
        {
            container.ResolveAll<IMqResponder>().ForEach(x => x.Stop());
            container.ResolveAll<IStartable>().ForEach(x => x.Stop());
        }
    }
}
