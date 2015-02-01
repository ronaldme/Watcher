using System.Linq;
using System.ServiceProcess;
using Castle.Core.Internal;
using Castle.Windsor;
using log4net;
using Services.DependencyInjection;
using Services.Interfaces;

namespace Startup.WindowsService
{
    public partial class Service1 : ServiceBase
    {
        private WindsorContainer container;
        private static readonly ILog log = LogManager.GetLogger(typeof(Service1));

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            log.Info("Starting server");
            container = new WindsorContainer();
            container.Install(new BackendDependencyInstaller());

            container.ResolveAll<IMqResponder>().ForEach(x => x.Start());
            container.ResolveAll<IStartable>().ForEach(x => x.Start());
            log.Info("Server started");
        }

        protected override void OnStop()
        {
            log.Info("Stopping server");
            container.ResolveAll<IMqResponder>().ToList().ForEach(x => x.Stop());
            container.ResolveAll<IStartable>().ToList().ForEach(x => x.Stop());
            log.Info("Sever stopped");
        }
    }
}