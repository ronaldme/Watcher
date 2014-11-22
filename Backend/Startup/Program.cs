using System.ServiceProcess;

namespace Startup
{
    static class Program
    {
        static void Main()
        {
            var servicesToRun = new ServiceBase[] 
            { 
                new Service1() 
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
