using System.ServiceProcess;

namespace Startup
{
    static class Program
    {
        static void Main()
        {
            var servicesToRun = new ServiceBase[] 
            { 
                new Service() 
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
