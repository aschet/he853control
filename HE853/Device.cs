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

    [ComVisible(true), GuidAttribute("D4A99D66-CAB0-40A9-A288-AED3BDBF6092")]
    [ProgId("HE853.Device")]
    [ClassInterface(ClassInterfaceType.None)]
    public sealed class Device : MarshalByRefObject, IDevice
    {
        private IntPtr writeHandle = IntPtr.Zero;

        private CommandCN commandCN = new CommandCN();
        private CommandEU commandEU = new CommandEU();
        private CommandUK commandUK = new CommandUK();

        public bool Open()
        {
            bool result = false;
            lock (this)
            {
                result = this.OpenUnlocked();
            }

            return result;
        }

        public void Close()
        {
            lock (this)
            {
                this.CloseUnlocked();
            }
        }

        public bool On(int deviceCode)
        {
            bool result = false;
            lock (this)
            {
                result = this.SendTextCommand(deviceCode, Command.On);
            }

            return result;
        }

        public bool Off(int deviceCode)
        {
            bool result = false;
            lock (this)
            {
                result = this.SendTextCommand(deviceCode, Command.Off);
            }

            return result;
        }

        public bool Dim(int deviceCode, int percent)
        {
            bool result = false;
            lock (this)
            {
                result = this.SendTextCommand(deviceCode, Convert.ToString(percent));
            }

            return result;
        }

        private bool OpenUnlocked()
        {
            this.CloseUnlocked();

            string devicePath = PInvoke.GetHIDDevicePath(0x4d9, 0x1357);
            if (devicePath.Length != 0)
            {
                this.writeHandle = PInvoke.CreateFile(devicePath, PInvoke.GenericWrite);
            }

            return this.writeHandle != IntPtr.Zero;
        }

        private void CloseUnlocked()
        {
            PInvoke.CloseHandle(ref this.writeHandle);     
        }

        private bool SendTextCommand(int deviceCode, string commandString)
        {
            bool result = this.TestStatus();
            if (result)
            {
                result = this.SendCommand(this.commandCN.Build(deviceCode, commandString));
                if ((commandString == Command.On) || (commandString == Command.Off))
                {
                    if (result)
                    {
                        result = this.SendCommand(this.commandUK.Build(deviceCode, commandString));
                    }

                    if (result)
                    {
                        result = this.SendCommand(this.commandEU.Build(deviceCode, commandString));
                    }
                }
            }

            return result;
        }

        private bool SendCommand(byte[] binaryCommand)
        {
            if (this.writeHandle == IntPtr.Zero)
                return false;
            
            bool result = true;
            const int ChunkLength = 8;
            byte[] chunk = new byte[ChunkLength + 1];
            chunk[0] = 0;

            for (int i = 0; i < (binaryCommand.Length / ChunkLength) && result; ++i)
            {
                for (int j = 0; j < ChunkLength; ++j)
                {
                    chunk[j + 1] = binaryCommand[(i * ChunkLength) + j];
                }

                result = result && PInvoke.SetHIDOutputReport(this.writeHandle, chunk);
            }

            return result;
        }

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