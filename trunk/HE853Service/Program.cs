﻿namespace HE853Service
{
    using System.ServiceProcess;

    static class Program
    {

        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new Service() 
			};

            ServiceBase.Run(ServicesToRun);
        }
    }
}
