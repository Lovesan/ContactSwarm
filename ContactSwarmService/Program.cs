using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace ContactSwarmService
{
    static class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main()
        {
            if (Environment.UserInteractive)
            {
                Logger.Trace("Starting interactive service");
                using (var service = new ServiceHost(typeof (Service.DataService)))
                {
                    service.Open();
                    Logger.Trace("Press any key to exit...");
                    Console.ReadLine();
                }
            }
            else
            {
                using (var service = new ContactSwarmWindowsService())
                {
                    ServiceBase.Run(service);
                }
            }
        }
    }
}
