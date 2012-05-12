/*
Home Easy HE853 Control Library
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

    public sealed class Device
    {
        private int outputReportByteLength = 0;
        private IntPtr hidHandle = IntPtr.Zero;
        private IntPtr readHandle = IntPtr.Zero;
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

            string devicePath = string.Empty;

            PInvoke.GetHIDHandle(0x4d9, 0x1357, ref this.hidHandle, ref devicePath);
            if (this.hidHandle != IntPtr.Zero)
            {
                this.outputReportByteLength = PInvoke.GetHIDOutputReportByteLenght(this.hidHandle);

                this.readHandle = PInvoke.CreateFile(devicePath, PInvoke.GenericRead, PInvoke.FileFlagOverlapped);
                if (this.readHandle == IntPtr.Zero)
                {
                    this.CloseUnlocked();
                }
                else
                {
                    this.writeHandle = PInvoke.CreateFile(devicePath, PInvoke.GenericWrite);
                    PInvoke.HidD_FlushQueue(this.readHandle);

                    if (this.writeHandle == IntPtr.Zero)
                    {
                        this.CloseUnlocked();
                    }
                }
            }

            return this.hidHandle != IntPtr.Zero;
        }

        private void CloseUnlocked()
        {
            PInvoke.CloseHandle(ref this.readHandle);
            PInvoke.CloseHandle(ref this.writeHandle);
            PInvoke.CloseHandle(ref this.hidHandle);        
        }

        private bool SendTextCommand(int deviceCode, string commandString)
        {
            bool result = this.GetStatus();
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

        private bool SendCommand(byte[,] binaryCommand)
        {
            bool result = this.DownloadCommand(binaryCommand);
            if (result)
            {
                result = this.ExecuteCommand();
            }

            return result;
        }

        private bool DownloadCommand(byte[,] binaryCommand)
        {
            bool result = true;
            byte[] binaryCommandPart = new byte[8];

            for (int i = 0; i < 4 && result; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    binaryCommandPart[j] = binaryCommand[i, j];
                }

                result = this.SetOutputReport(binaryCommandPart);
            }

            return result;
        }

        private bool ExecuteCommand()
        {
            byte[] buf = new byte[8];
            buf[0] = 5;
            return this.SetOutputReport(buf);
        }

        private bool SetOutputReport(byte[] binaryCommandPart)
        {
            if (this.readHandle != IntPtr.Zero && this.writeHandle != IntPtr.Zero && this.outputReportByteLength > 0)
            {
                byte[] outputBuffer = new byte[this.outputReportByteLength];
                outputBuffer[0] = 0;
                for (int count = 0; count < 8; ++count)
                {
                    outputBuffer[count + 1] = binaryCommandPart[count];
                }

                if (PInvoke.HidD_SetOutputReport(this.writeHandle, ref outputBuffer[0], outputBuffer.Length))
                {
                    return true;
                }
            }

            return false;
        }

        private bool GetStatus()
        {
            byte[] buf = new byte[8];
            buf[0] = 6;
            buf[1] = 1;

            bool result = this.SetOutputReport(buf);
            if (!result)
            {
                result = this.OpenUnlocked();
            }

            return result;
        }
    }
}
