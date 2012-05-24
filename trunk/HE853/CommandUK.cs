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

    public sealed class CommandUK : Command
    {
        public CommandUK()
        {
            this.StartBitHTime = 32;
            this.StartBitLTime = 970;
            this.EndBitHTime = 0;
            this.EndBitLTime = 0;
            this.DataBit0HTime = 32;
            this.DataBit0LTime = 96;
            this.DataBit1HTime = 96;
            this.DataBit1LTime = 32;
            this.DataBitCount = 24;
            this.FrameCount = 18;
        }

        protected override void BuildData(ref MemoryStream stream, int deviceCode, string commandString)
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
            for (int i = encodingBuffer.Length - 1; i >= 0; --i)
            {
                encodedDeviceCode = (ushort)((encodedDeviceCode << 2) | encodingBuffer[i]);
            }

            this.WriteShort(ref stream, encodedDeviceCode);
            
            byte command = 0x14;
            if (commandString == Command.On)
            {
                command = (byte)(command | 0x1);
            }

            stream.WriteByte(command);
            this.WriteZero(ref stream, 11);
        }
    }
}