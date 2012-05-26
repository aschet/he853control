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
        /// <returns>True if the device is available.</returns>
        public bool Open()
        {
            bool result = false;
            lock (this.locker)
            {
                result = this.OpenUnlocked();
            }

            return result;
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
        /// <returns>True if command could be send.</returns>
        public bool On(int deviceCode)
        {
            return this.On(deviceCode, false);
        }

        /// <summary>
        /// Swiches receivers with specific device code on.
        /// </summary>
        /// <param name="deviceCode">Device code of receivers.</param>
        /// <param name="shortCommand">Sends shorter less compatible command sequence.</param>
        /// <returns>True if command could be send.</returns>
        public bool On(int deviceCode, bool shortCommand)
        {
            bool result = false;
            lock (this.locker)
            {
                result = this.SendCommand(deviceCode, Command.On, shortCommand);
            }

            return result;
        }

        /// <summary>
        /// Swiches receivers with specific device code off.
        /// </summary>
        /// <param name="deviceCode">Device code of receivers.</param>
        /// <returns>True if command could be send.</returns>
        public bool Off(int deviceCode)
        {
            return this.Off(deviceCode, false);
        }

        /// <summary>
        /// Swiches receivers with specific device code off.
        /// </summary>
        /// <param name="deviceCode">Device code of receivers.</param>
        /// <param name="shortCommand">Sends shorter less compatible command sequence.</param>
        /// <returns>True if command could be send.</returns>
        public bool Off(int deviceCode, bool shortCommand)
        {
            bool result = false;
            lock (this.locker)
            {
                result = this.SendCommand(deviceCode, Command.Off, shortCommand);
            }

            return result;
        }

        /// <summary>
        /// Adjusts dim level on receivers with specific device code.
        /// </summary>
        /// <param name="deviceCode">Device code of receivers.</param>
        /// <param name="percent">Amount of dim. A value between 10 an 80.</param>
        /// <returns>True if command could be send.</returns>
        public bool Dim(int deviceCode, int percent)
        {
            bool result = false;
            lock (this.locker)
            {
                result = this.SendCommand(deviceCode, Convert.ToString(percent), false);
            }

            return result;
        }

        /// <summary>
        /// Prepares the HE853 device for usage.
        /// </summary>
        /// <returns>True if the device is available.</returns>
        private bool OpenUnlocked()
        {
            this.CloseUnlocked();

            string devicePath = PInvoke.GetHIDDevicePath(0x4D9, 0x1357);
            if (devicePath.Length != 0)
            {
                this.writeHandle = PInvoke.CreateFile(devicePath, PInvoke.GenericWrite);
            }

            return this.writeHandle != IntPtr.Zero;
        }

        /// <summary>
        /// Shuts down the HE853 device.
        /// </summary>
        private void CloseUnlocked()
        {
            PInvoke.CloseHandle(ref this.writeHandle);     
        }

        /// <summary>
        /// Encodes and sends a command in text form to the HE853 device.
        /// </summary>
        /// <param name="deviceCode">Device code of receivers.</param>
        /// <param name="command">Text command to send.</param>
        /// <param name="shortCommand">Sends shorter less compatible command sequence.</param> 
        /// <returns>True if command could be send.</returns>
        private bool SendCommand(int deviceCode, string command, bool shortCommand)
        {
            bool result = this.TestStatus();
            if (result)
            {
                result = this.SendCommand(this.commandCN.Build(deviceCode, command));
                if (!shortCommand && (command == Command.On || command == Command.Off))
                {
                    if (result)
                    {
                        result = this.SendCommand(this.commandUK.Build(deviceCode, command));
                    }

                    if (result)
                    {
                        result = this.SendCommand(this.commandEU.Build(deviceCode, command));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Sends a command sequence as series of 9 byte long HID reports to the HE853 device.
        /// </summary>
        /// <param name="command">Command sequence to send.</param>
        /// <returns>True if command could be send.</returns>
        private bool SendCommand(byte[] command)
        {
            if (this.writeHandle == IntPtr.Zero)
            {
                return false;
            }
            
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

                result = result && PInvoke.SetHIDOutputReport(this.writeHandle, chunk);
            }

            System.Threading.Thread.Sleep(10);

            return result;
        }

        /// <summary>
        /// Tests if the HE853 device is still available and if not tries to reopen it.
        /// </summary>
        /// <returns>True if the device is available.</returns>
        private bool TestStatus()
        {
            bool result = this.SendCommand(this.commandCN.BuildStatus());
            if (!result)
            {
                result = this.OpenUnlocked();
            }

            return result;
        }
    }
}