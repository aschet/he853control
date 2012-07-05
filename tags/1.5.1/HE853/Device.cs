/*
Home Easy HE853 Control
Copyright (C) 2012 Thomas Ascher

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/

namespace HE853
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Implementation for communication with the HE853 device.
    /// </summary>
    [ComVisible(true), GuidAttribute("D4A99D66-CAB0-40A9-A288-AED3BDBF6092")]
    [ProgId("HE853.Device")]
    [ClassInterface(ClassInterfaceType.None)]
    public sealed class Device : MarshalByRefObject, IDevice
    {
        /// <summary>
        /// Handle to the device.
        /// </summary>
        private IntPtr writeHandle = IntPtr.Zero;
        
        /// <summary>
        /// Command sequence generator for CN specific receivers.
        /// </summary>
        private CommandCN commandCN = new CommandCN();
        
        /// <summary>
        /// Command sequence generator for EU specific receivers.
        /// </summary>
        private CommandEU commandEU = new CommandEU();
        
        /// <summary>
        /// Command sequence generator for UK specific receivers.
        /// </summary>
        private CommandUK commandUK = new CommandUK();

        /// <summary>
        /// Lock object for command synchronisation.
        /// </summary>
        private object locker = new object();

        /// <summary>
        /// Prepares the HE853 device for usage. Uses locking.
        /// </summary>
        public void Open()
        {
            lock (this.locker)
            {
                this.OpenUnlocked();
            }
        }

        /// <summary>
        /// Shuts down the HE853 device. Uses locking.
        /// </summary>
        public void Close()
        {
            lock (this.locker)
            {
                this.CloseUnlocked();
            }
        }

        /// <summary>
        /// Swiches receivers with specific device code on.
        /// </summary>
        /// <param name="deviceCode">Device code of receivers.</param>
        /// <param name="commandStyle">Does specify how much information is send.</param>
        public void SwitchOn(int deviceCode, CommandStyle commandStyle)
        {
            lock (this.locker)
            {
                this.SendCommand(deviceCode, Command.On, commandStyle);
            }
        }

        /// <summary>
        /// Swiches receivers with specific device code off.
        /// </summary>
        /// <param name="deviceCode">Device code of receivers.</param>
        /// <param name="commandStyle">Does specify how much information is send.</param>
        public void SwitchOff(int deviceCode, CommandStyle commandStyle)
        {
            lock (this.locker)
            {
                this.SendCommand(deviceCode, Command.Off, commandStyle);
            }
        }

        /// <summary>
        /// Adjusts dim level on receivers with specific device code.
        /// </summary>
        /// <param name="deviceCode">Device code of receivers.</param>
        /// <param name="commandStyle">Does specify how much information is send.</param>
        /// <param name="amount">Amount of dim. A value between 1 an 8.</param>
        public void AdjustDim(int deviceCode, CommandStyle commandStyle, int amount)
        {
            if (!Command.IsValidDim(amount))
            {
                throw new ArgumentOutOfRangeException("amount", "Must be value between " + Command.MinDim + " and " + Command.MaxDim + ".");
            }

            lock (this.locker)
            {
                this.SendCommand(deviceCode, Convert.ToString(amount, CultureInfo.InvariantCulture), commandStyle);
            }
        }

        /// <summary>
        /// Prepares the HE853 device for usage.
        /// </summary>
        private void OpenUnlocked()
        {
            this.CloseUnlocked();

            // "\\\\?\\hid#vid_04d9&pid_1357#6&37b1e0bf&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}\0"
            string devicePath = NativeMethodsHelper.GetHE853DevicePath();
            if (devicePath.Length != 0)
            {
                NativeMethodsHelper.FlushHIDQueue(devicePath);
                this.writeHandle = NativeMethodsHelper.CreateFileForWrite(devicePath);
            }

            if (this.writeHandle == IntPtr.Zero)
            {
                throw new FileNotFoundException("HE853 device is not connected or in use.");
            }
        }

        /// <summary>
        /// Shuts down the HE853 device.
        /// </summary>
        private void CloseUnlocked()
        {
            NativeMethodsHelper.CloseHandle(ref this.writeHandle);     
        }

        /// <summary>
        /// Encodes and sends a command in text form to the HE853 device.
        /// </summary>
        /// <param name="deviceCode">Device code of receivers.</param>
        /// <param name="command">Text command to send.</param>
        /// <param name="commandStyle">Does specify how much information is send.</param>
        private void SendCommand(int deviceCode, string command, CommandStyle commandStyle)
        {
            if (!Command.IsValidDeviceCode(deviceCode))
            {
                throw new ArgumentOutOfRangeException("deviceCode", "Must be value between " + Command.MinDeviceCode + " and " + Command.MaxDeviceCode + ".");
            }
            
            this.TestStatus();

            this.SendCommand(this.commandCN.Build(deviceCode, command));
            if (commandStyle == CommandStyle.Comprehensive && (command == Command.On || command == Command.Off))
            {
                this.SendCommand(this.commandUK.Build(deviceCode, command));
                this.SendCommand(this.commandEU.Build(deviceCode, command));
            }
        }

        /// <summary>
        /// Sends a command sequence as series of 9 byte long HID reports to the HE853 device.
        /// </summary>
        /// <param name="command">Command sequence to send.</param>
        private void SendCommand(byte[] command)
        {            
            bool result = true;
            const int ChunkLength = 8;
            byte[] chunk = new byte[ChunkLength + 1];
            chunk[0] = 0;

            for (int i = 0; i < (command.Length / ChunkLength) && result; ++i)
            {
                for (int j = 0; j < ChunkLength; ++j)
                {
                    chunk[j + 1] = command[(i * ChunkLength) + j];
                }

                result = result && NativeMethodsHelper.SetHIDOutputReport(this.writeHandle, chunk);
            }

            if (!result)
            {
                throw new IOException("Command could not be send to HE853 device.");
            }

            System.Threading.Thread.Sleep(10);
        }

        /// <summary>
        /// Tests if the HE853 device is still available and if not tries to reopen it.
        /// </summary>
        private void TestStatus()
        {
            if (this.writeHandle == IntPtr.Zero)
            {
                this.OpenUnlocked();
            }
            else
            {
                try
                {
                    this.SendCommand(Command.BuildStatus());
                }
                catch (IOException)
                {
                    this.OpenUnlocked();
                }
            }
        }
    }
}