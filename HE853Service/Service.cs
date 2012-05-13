namespace HE853Service
{
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Channels.Ipc;
    using System.ServiceProcess;
    
    public partial class Service : ServiceBase
    {
        private HE853.Device device = new HE853.Device();
        
        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            device.Open();

            ChannelServices.RegisterChannel(new IpcChannel("HE853"), false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(HE853.Device), "Device", WellKnownObjectMode.Singleton);
        }

        protected override void OnStop()
        {
            device.Close();
        }
    }
}
