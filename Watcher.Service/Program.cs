using Topshelf;

namespace Watcher.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(hf =>
            {
                hf.Service<WatcherService>(s =>
                {
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                hf.DependsOnMsSql();
                hf.RunAsLocalSystem();

                hf.SetDescription("Keep track of your favorite shows & movies");
                hf.SetDisplayName("Watcher");
                hf.SetServiceName("Watcher");
            });
        }
    }
}
