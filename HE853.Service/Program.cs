namespace HE853.Service
{
    using System.ServiceProcess;

    public static class Program
    {
        public static void Main()
        {
            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[] { new Service() };
            ServiceBase.Run(servicesToRun);
        }
    }
}