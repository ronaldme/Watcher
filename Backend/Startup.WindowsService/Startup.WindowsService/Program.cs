﻿using System.ServiceProcess;

namespace Startup.WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
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
