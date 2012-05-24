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

    public sealed class CommandCN : Command
    {
        private byte count = 0;

        public CommandCN()
        {
            this.StartBitHTime = 32;
            this.StartBitLTime = 480;
            this.EndBitHTime = 0;
            this.EndBitLTime = 0;
            this.DataBit0HTime = 32;
            this.DataBit0LTime = 96;
            this.DataBit1HTime = 96;
            this.DataBit1LTime = 32;
            this.DataBitCount = 28;
            this.FrameCount = 7;
        }

        protected override void BuildData(ref MemoryStream stream, int deviceCode, string commandString)
        {
            int[] seed = new int[] { 0x609, 0x306, 0x803, 0xA08, 0xA, 0x200, 0xC02, 0x40C, 0xE04, 0x70E, 0x507, 0x105, 0xF01, 0xB0F, 0xD0B, 0x90D };
            this.count = (byte)(this.count + 1);
            byte[] encodingBuffer1 = new byte[10];
            encodingBuffer1[0] = 0x1;
            encodingBuffer1[1] = (byte)((this.count << 2) & 0xF);
            if (commandString != Command.Off)
            {
                encodingBuffer1[1] |= 0x2;
            }

            encodingBuffer1[2] = (byte)(deviceCode & 0xF);
            encodingBuffer1[3] = (byte)((deviceCode >> 4) & 0xF);
            encodingBuffer1[4] = (byte)((deviceCode >> 8) & 0xF);
            encodingBuffer1[5] = (byte)((deviceCode >> 12) & 0xF);
            if ((commandString == Command.On) || (commandString == Command.Off))
            {
                encodingBuffer1[6] = 0x0;
            }
            else
            {
                string firstDigitString = commandString.Substring(0, 1);
                encodingBuffer1[6] = (byte)(byte.Parse(firstDigitString) - 1);
                encodingBuffer1[6] |= 0x8;
            }

            byte[] encodingBuffer2 = new byte[7];
            int idx = encodingBuffer1[0];
            encodingBuffer2[0] = (byte)(seed[idx] >> 8);
            idx = encodingBuffer1[1] ^ encodingBuffer2[0];
            encodingBuffer2[1] = (byte)(seed[idx] >> 8);
            idx = encodingBuffer1[2] ^ encodingBuffer2[1];
            encodingBuffer2[2] = (byte)(seed[idx] >> 8);
            idx = encodingBuffer1[3] ^ encodingBuffer2[2];
            encodingBuffer2[3] = (byte)(seed[idx] >> 8);
            idx = encodingBuffer1[4] ^ encodingBuffer2[3];
            encodingBuffer2[4] = (byte)(seed[idx] >> 8);
            idx = encodingBuffer1[5] ^ encodingBuffer2[4];
            encodingBuffer2[5] = (byte)(seed[idx] >> 8);
            encodingBuffer2[6] = (byte)encodingBuffer1[6];

            byte[] encodingBuffer3 = new byte[7];
            idx = encodingBuffer2[0];
            encodingBuffer3[0] = (byte)(seed[idx] & 0xFF);
            idx = encodingBuffer2[1] ^ encodingBuffer3[0];
            encodingBuffer3[1] = (byte)(seed[idx] & 0xFF);
            idx = encodingBuffer2[2] ^ encodingBuffer3[1];
            encodingBuffer3[2] = (byte)(seed[idx] & 0xFF);
            idx = encodingBuffer2[3] ^ encodingBuffer3[2];
            encodingBuffer3[3] = (byte)(seed[idx] & 0xFF);
            idx = encodingBuffer2[4] ^ encodingBuffer3[3];
            encodingBuffer3[4] = (byte)(seed[idx] & 0xFF);
            idx = encodingBuffer2[5] ^ encodingBuffer3[4];
            encodingBuffer3[5] = (byte)(seed[idx] & 0xFF);
            encodingBuffer3[6] = (byte)(encodingBuffer2[6] ^ 0x9);

            int temp = ((((((encodingBuffer3[6] << 24) | (encodingBuffer3[5] << 20)) | (encodingBuffer3[4] << 16)) | (encodingBuffer3[3] << 12)) | (encodingBuffer3[2] << 8)) | (encodingBuffer3[1] << 4)) | encodingBuffer3[0];
            temp = (temp >> 2) | ((temp & 0x3) << 26);

            stream.WriteByte((byte)(temp >> 20));
            stream.WriteByte((byte)(temp >> 12));
            stream.WriteByte((byte)(temp >> 4));
            stream.WriteByte((byte)(temp << 4));
            this.WriteZero(ref stream, 10);
        }
    }
}