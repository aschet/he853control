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
    using System.IO;
    using System.Reflection;
    using System.Runtime.Remoting;

    /// <summary>
    /// Main program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// List of possible exit codes.
        /// </summary>
        private enum ExitCode : int
        {
            /// <summary>
            /// Everything was ok.
            /// </summary>
            Success = 0,
            
            /// <summary>
            /// The device is not attached or available.
            /// </summary>
            DeviceNotFound = -1,
            
            /// <summary>
            /// Sending of command failed.
            /// </summary>
            CommandFailed = -2
        }

        /// <summary>
        /// Main program.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>0 if no error occured.</returns>
        public static int Main(string[] args)
        {
            PrintInformation();

            Arguments arguments = new Arguments();

            try
            {
                arguments.Parse(args);
            }
            catch (Exception)
            {
                PrintUsage();
                return (int)ExitCode.Success;
            }

            if (arguments.Service)
            {
                Rpc.RegisterClient();
            }

            Device device = new Device();

            try
            {
                device.Open();

                if (arguments.CommandText == Command.On)
                {
                    device.SwitchOn(arguments.DeviceCode, arguments.Style);
                }
                else if (arguments.CommandText == Command.Off)
                {
                    device.SwitchOff(arguments.DeviceCode, arguments.Style);
                }
                else
                {
                    device.AdjustDim(arguments.DeviceCode, arguments.Style, arguments.Dim);
                }
            }
            catch (FileNotFoundException exception)
            {
                Console.WriteLine(exception.Message);
                return (int)ExitCode.DeviceNotFound;
            }
            catch (RemotingException)
            {
                Console.WriteLine("The service does not respond.");
                return (int)ExitCode.DeviceNotFound;
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.Message);
                device.Close();
                return (int)ExitCode.CommandFailed;
            }

            return (int)ExitCode.Success;
        }

        /// <summary>
        /// Print program information.
        /// </summary>
        private static void PrintInformation()
        {
            Console.WriteLine("Home Easy HE853 Control Utility v" + Assembly.GetExecutingAssembly().GetName().Version + " by Thomas Ascher");
            Console.WriteLine("This program is licensed under the terms of the GNU GPL.");
            Console.WriteLine("http://he853control.sourceforge.net");
            Console.WriteLine();
        }

        /// <summary>
        /// Print user documentation.
        /// </summary>
        private static void PrintUsage()
        {
            string name = Assembly.GetExecutingAssembly().GetName().Name;

            Console.WriteLine("Usage: " + name + " <command> <device_code> [" + Rpc.ServiceArg + "] [" + Arguments.Short + "]");
            Console.WriteLine();
            Console.WriteLine("<command> := " + Command.On + " | " + Command.Off + " | " + Command.MinDim + ".." + Command.MaxDim);
            Console.WriteLine("<device_code> := " + Command.MinDeviceCode + ".." + Command.MaxDeviceCode);
            Console.WriteLine();
            Console.WriteLine(Rpc.ServiceArg + ": use service instead of device");
            Console.WriteLine(Arguments.Short + ": use short command sequence, less compatible");
            Console.WriteLine();
            Console.WriteLine("The device code has to programed to a receiver first.");
            Console.WriteLine("To program the code hold the learn button on the receiver for");
            Console.WriteLine("about 1 second and send an " + Command.On + " command with the specific code.");
            Console.WriteLine();
            Console.WriteLine("Example: " + name + " " + Command.On + " 1001");
        }
    }
}