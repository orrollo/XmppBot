using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace jrobbot
{
    [RunInstaller(true)]
    public class JRobotServiceInstaller : Installer
    {
        public JRobotServiceInstaller()
        {
            var processInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;

            serviceInstaller.DisplayName = "jrobbot";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            serviceInstaller.ServiceName = "jrobbot";
            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);
        }
    }
}
