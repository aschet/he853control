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

    public static class Program
    {
        public enum ExitCode : int
        {
            Success = 0,
            DeviceNotFound = -1,
            CommandFailed = -2
        }

        public static int Main(string[] args)
        {
            Console.WriteLine("Home Easy HE853 Control Utility by Thomas Ascher");
            Console.WriteLine("This program is licensed under the terms of the GNU GPL.");
            Console.WriteLine("http://he853control.sourceforge.net");
            Console.WriteLine();
            
            string command;
            int dim;
            int deviceCode;
            bool useService;
            bool shortCommand;

            if (!ParseArgs(args, out command, out dim, out deviceCode, out useService, out shortCommand))
            {
                return (int)ExitCode.Success;
            }

            if (useService)
            {
                Rpc.RegisterClient();
            }

            Device device = new Device();

            try
            {
                if (!device.Open())
                {
                    Console.WriteLine("The device is not attached or in use!");
                    return (int)ExitCode.DeviceNotFound;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("The service does not respond!");
                return (int)ExitCode.DeviceNotFound;
            }

            bool result = false;
            if (command == Command.On)
            {
                result = device.On(deviceCode, shortCommand);
            }
            else if (command == Command.Off)
            {
                result = device.Off(deviceCode, shortCommand);
            }
            else
            {
                result = device.Dim(deviceCode, dim);
            }

            device.Close();

            if (!result)
            {
                Console.WriteLine("Error during command send!");
                return (int)ExitCode.CommandFailed;
            }

            return (int)ExitCode.Success;
        }

        private static bool ParseArgs(string[] args, out string command, out int dim, out int deviceCode, out bool useService, out bool shortCommnd)
        {
            command = string.Empty;
            dim = 0;
            deviceCode = 0;
            useService = Rpc.HasServiceArg(args);
            shortCommnd = false;
            
            if (args.Length < 2)
            {
                PrintUsage();
                return false;
            }

            command = args[0];
            if (!(command == Command.On || command == Command.Off || int.TryParse(command, out dim)))
            {
                PrintUsage();
                return false;
            }

            if (!int.TryParse(args[1], out deviceCode))
            {
                PrintUsage();
                return false;
            }

            foreach (string arg in args)
            {
                if (arg == "/short")
                {
                    shortCommnd = true;
                    break;
                }
            }

            return true;
        }

        private static void PrintUsage()
        {
            string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            Console.WriteLine("Usage: " + name + " <command> <device_code> [" + Rpc.ServiceArg + "] [/short]");
            Console.WriteLine();
            Console.WriteLine("<command> := " + Command.On + " | " + Command.Off + " | " + Command.MinDim + ".." + Command.MaxDim);
            Console.WriteLine("<device_code> := " + Command.MinDeviceCode + ".." + Command.MaxDeviceCode);
            Console.WriteLine();
            Console.WriteLine("/service use service instead of device");
            Console.WriteLine("/short use short command sequence, less compatible");
            Console.WriteLine();
            Console.WriteLine("The device code has to programmed to a receiver first.");
            Console.WriteLine("To program the code hold the learn button on the receiver for");
            Console.WriteLine("about 2 seconds and send an ON command with the specific code.");
            Console.WriteLine();
            Console.WriteLine("Example: " + name + " " + Command.On + " 1001");
        }
    }
}