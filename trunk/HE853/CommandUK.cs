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
    using System.IO;

    internal sealed class CommandUK : Command
    {
        public CommandUK()
        {
            StartBitHTime = 32;
            StartBitLTime = 970;
            EndBitHTime = 0;
            EndBitLTime = 0;
            DataBit0HTime = 32;
            DataBit0LTime = 96;
            DataBit1HTime = 96;
            DataBit1LTime = 32;
            DataBitCount = 24;
            FrameCount = 18;
        }

        protected override void BuildData(ref MemoryStream stream, int deviceCode, string commandString)
        {
            stream.WriteByte(3);

            ushort address = this.EncodeDeviceCode(deviceCode);
            stream.WriteByte((byte)((address >> 8) & 0xFF));
            stream.WriteByte((byte)(address & 0xFF));
            
            byte command = 20;
            if (commandString == On)
            {
                command = (byte)(command | 1);
            }

            stream.WriteByte(command);
            this.WriteZero(ref stream, 4);

            stream.WriteByte(4);
            this.WriteZero(ref stream, 7);
        }

        private ushort EncodeDeviceCode(int deviceCode)
        {
            int[] encodingBuffer = new int[8];
            for (int i = 0; i < encodingBuffer.Length; ++i)
            {
                encodingBuffer[i] = deviceCode % 3;
                deviceCode /= 3;

                if (encodingBuffer[i] == 1)
                {
                    encodingBuffer[i] = 3;
                }
                else if (encodingBuffer[i] == 2)
                {
                    encodingBuffer[i] = 1;
                }
            }

            ushort encodedDeviceCode = 0;
            for (int i = (encodingBuffer.Length - 1); i >= 0; --i)
            {
                encodedDeviceCode = (ushort)(encodedDeviceCode << 2);
                encodedDeviceCode = (ushort)(encodedDeviceCode | ((ushort)encodingBuffer[i]));
            }

            return encodedDeviceCode;
        }
    }
}