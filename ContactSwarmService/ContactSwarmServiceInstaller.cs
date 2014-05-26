using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ContactSwarmService
{
    [RunInstaller(true)]
    public class ContactSwarmServiceInstaller : Installer
    {
        public ContactSwarmServiceInstaller()
        {
            var serviceProcessInstaller = new ServiceProcessInstaller
                {
                    Account = ServiceAccount.LocalSystem
                };


            var serviceInstaller = new ServiceInstaller
                {
                    DisplayName = "Contact Swarm Service",
                    Description = "Contact Swarm REST API Service",
                    StartType = ServiceStartMode.Automatic,
                    ServiceName = "ContactSwarmService"
                };

            Installers.Add(serviceInstaller);
            Installers.Add(serviceProcessInstaller);
        }
    }
}
