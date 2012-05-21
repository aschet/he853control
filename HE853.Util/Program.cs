/*
Home Easy HE853 Control
Copyright (C) 2012 Thomas Ascher

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/

namespace HE853.Util
{
    using System;

    public class Program
    {        
        public static void Main(string[] args)
        {
            Console.WriteLine("Home Easy HE853 Control Utility by Thomas Ascher");
            Console.WriteLine("This program is licensed under the terms of the GNU GPL.");
            Console.WriteLine("http://he853control.sourceforge.net");
            Console.WriteLine();
            
            string command;
            int dim;
            int deviceCode;
            bool useService;

            if (!ParseArgs(args, out command, out dim, out deviceCode, out useService))
            {
                return;
            }

            if (useService)
            {
                RPC.RegisterClient();
            }

            Device device = new Device();

            try
            {
                if (!device.Open())
                {
                    Console.WriteLine("The device is not attached or in use!");
                    return;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("The service does not respond!");
                return;
            }

            bool result = false;
            if (command == "ON")
            {
                result = device.On(deviceCode);
            }
            else if (command == "OFF")
            {
                result = device.Off(deviceCode);
            }
            else
            {
                result = device.Dim(deviceCode, dim);
            }

            device.Close();

            if (!result)
            {
                Console.WriteLine("Error during command send!");
            }
        }

        private static bool ParseArgs(string[] args, out string command, out int dim, out int deviceCode, out bool useService)
        {
            command = string.Empty;
            dim = 0;
            deviceCode = 0;
            useService = RPC.HasServiceArg(args);
            
            if (args.Length < 2)
            {
                PrintUsage();
                return false;
            }

            command = args[0];
            if (!(command == "ON" || command == "OFF" || int.TryParse(command, out dim)))
            {
                PrintUsage();
                return false;
            }

            if (!int.TryParse(args[1], out deviceCode))
            {
                PrintUsage();
                return false;
            }

            return true;
        }

        private static void PrintUsage()
        {
            string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            Console.WriteLine("Usage: " + name + " <command> <device_code> [/service]");
            Console.WriteLine();
            Console.WriteLine("<command> := ON | OFF | 10..80");
            Console.WriteLine("<device_code> := 1..6000");
            Console.WriteLine();
            Console.WriteLine("The device code has to programmed to a receiver first.");
            Console.WriteLine("To program the code hold the learn button on the receiver for");
            Console.WriteLine("about 2 seconds and send an ON command with the specific code.");
            Console.WriteLine();
            Console.WriteLine("Example: " + name + " ON 1001");
        }
    }
}