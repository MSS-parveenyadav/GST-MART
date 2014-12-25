using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace GST_MARTService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[] 
            //{ 
            //    new ImpoterService() 
            //};
            //ServiceBase.Run(ServicesToRun);

            var myService = new ImpoterService();
            if (Environment.UserInteractive)
            {
              //  Console.WriteLine("Starting service...");
                myService.Start();
               // Console.WriteLine("Service is running.");
               // Console.WriteLine("Press any key to stop...");
               Console.ReadKey(true);
              //  Console.WriteLine("Stopping service...");
              //  myService.Stop();
               // Console.WriteLine("Service stopped.");
            }
            else
            {
                var servicesToRun = new ServiceBase[] { myService };
                ServiceBase.Run(servicesToRun);
            }

        }
    }
}
