namespace HE853Service
{
    using System.ComponentModel;
    using System.ServiceProcess;
    
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        public Installer()
        {
            this.InitializeComponent();

            ServiceProcessInstaller process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;

            ServiceInstaller serviceAdmin = new ServiceInstaller();

            serviceAdmin.StartType = ServiceStartMode.Manual;
            serviceAdmin.ServiceName = "HE853";
            serviceAdmin.DisplayName = "HE853";

            Installers.Add(process);
            Installers.Add(serviceAdmin);
        }
    }
}
