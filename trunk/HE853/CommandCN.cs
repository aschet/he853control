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
            int[] gbuf = new int[10];
            gbuf[0] = 0x1;
            gbuf[1] = (this.count << 2) & 0xF;
            if (commandString != Command.Off)
            {
                gbuf[1] |= 0x2;
            }

            gbuf[2] = deviceCode & 0xF;
            gbuf[3] = (deviceCode >> 4) & 0xF;
            gbuf[4] = (deviceCode >> 8) & 0xF;
            gbuf[5] = (deviceCode >> 12) & 0xF;
            if ((commandString == Command.On) || (commandString == Command.Off))
            {
                gbuf[6] = 0x0;
            }
            else
            {
                string firstDigitString = commandString.Substring(0, 1);
                gbuf[6] = byte.Parse(firstDigitString) - 1;
                gbuf[6] |= 0x8;
            }

            byte[] kbuf = new byte[7];
            int idx = gbuf[0];
            kbuf[0] = (byte)(seed[idx] >> 8);
            idx = gbuf[1] ^ kbuf[0];
            kbuf[1] = (byte)(seed[idx] >> 8);
            idx = gbuf[2] ^ kbuf[1];
            kbuf[2] = (byte)(seed[idx] >> 8);
            idx = gbuf[3] ^ kbuf[2];
            kbuf[3] = (byte)(seed[idx] >> 8);
            idx = gbuf[4] ^ kbuf[3];
            kbuf[4] = (byte)(seed[idx] >> 8);
            idx = gbuf[5] ^ kbuf[4];
            kbuf[5] = (byte)(seed[idx] >> 8);
            kbuf[6] = (byte)gbuf[6];

            byte[] cbuf = new byte[7];
            idx = kbuf[0];
            cbuf[0] = (byte)(seed[idx] & 0xFF);
            idx = kbuf[1] ^ cbuf[0];
            cbuf[1] = (byte)(seed[idx] & 0xFF);
            idx = kbuf[2] ^ cbuf[1];
            cbuf[2] = (byte)(seed[idx] & 0xFF);
            idx = kbuf[3] ^ cbuf[2];
            cbuf[3] = (byte)(seed[idx] & 0xFF);
            idx = kbuf[4] ^ cbuf[3];
            cbuf[4] = (byte)(seed[idx] & 0xFF);
            idx = kbuf[5] ^ cbuf[4];
            cbuf[5] = (byte)(seed[idx] & 0xFF);
            cbuf[6] = (byte)(kbuf[6] ^ 0x9);

            int temp = ((((((cbuf[6] << 24) | (cbuf[5] << 20)) | (cbuf[4] << 16)) | (cbuf[3] << 12)) | (cbuf[2] << 8)) | (cbuf[1] << 4)) | cbuf[0];
            temp = (temp >> 2) | ((temp & 0x3) << 26);

            stream.WriteByte((byte)(temp >> 20));
            stream.WriteByte((byte)(temp >> 12));
            stream.WriteByte((byte)(temp >> 4));
            stream.WriteByte((byte)(temp << 4));
            this.WriteZero(ref stream, 10);
        }
    }
}