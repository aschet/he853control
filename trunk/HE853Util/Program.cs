/*
Home Easy HE853 Control Utility
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

namespace HE853Util
{
    using System;
    using HE853;

    public class Program
    {        
        public static void Main(string[] args)
        {
            Console.WriteLine("Home Easy HE853 Control Utility by Thomas Ascher");
            Console.WriteLine();
            
            if (args.Length < 2)
            {
                PrintUsage();
                return;
            }

            string command = args[0];
            int dim = 0;
            if (!(command == "ON" || command == "OFF" || int.TryParse(command, out dim)))
            {
                PrintUsage();
                return;
            }

            int deviceCode = 0;
            if (!int.TryParse(args[1], out deviceCode))
            {
                PrintUsage();
                return;
            }

            Device device = new Device();
            if (!device.Open())
            {
                Console.WriteLine("The device is not attached or in use!");
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

            if (!result)
                Console.WriteLine("Error during command send!");
        }

        private static void PrintUsage()
        {
            string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            
            Console.WriteLine("Usage: " + name + " <COMMAND> <DEVICE_CODE>");
            Console.WriteLine();
            Console.WriteLine("<COMMAND> := ON | OFF | 1..100");
            Console.WriteLine("<DEVICE_CODE> := 1..6000");
            Console.WriteLine();
            Console.WriteLine("The device code has to programmed to a receiver first.");
            Console.WriteLine("To program the code hold the learn button on the receiver for");
            Console.WriteLine("about 2 seconds and send an ON command with the specific code.");
            Console.WriteLine();
            Console.WriteLine("Example: " + name + " ON 1001");
        }
    }
}
