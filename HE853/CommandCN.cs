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
    internal sealed class CommandCN : Command
    {
        private byte count = 0;

        public CommandCN()
        {
            StartBitHTime = 32;
            StartBitLTime = 480;
            EndBitHTime = 0;
            EndBitLTime = 0;
            DataBit0HTime = 32;
            DataBit0LTime = 96;
            DataBit1HTime = 96;
            DataBit1LTime = 32;
            DataBitCount = 28;
            FrameCount = 7;
        }

        protected override void BuildData(ref byte[,] binaryCommand, int deviceCode, string commandString)
        {
            int[] tbFx = new int[] { 0x609, 0x306, 0x803, 0xa08, 10, 0x200, 0xc02, 0x40c, 0xe04, 0x70e, 0x507, 0x105, 0xf01, 0xb0f, 0xd0b, 0x90d };
            binaryCommand[2, 0] = 3;
            binaryCommand[3, 0] = 4;
            this.count = (byte)(this.count + 1);
            int[] gbuf = new int[10];
            gbuf[0] = 1;
            gbuf[1] = (this.count << 2) & 15;
            if (commandString != Off)
            {
                gbuf[1] |= 2;
            }

            gbuf[2] = deviceCode & 15;
            gbuf[3] = (deviceCode >> 4) & 15;
            gbuf[4] = (deviceCode >> 8) & 15;
            gbuf[5] = (deviceCode >> 12) & 15;
            if ((commandString == On) || (commandString == Off))
            {
                gbuf[6] = 0;
            }
            else
            {
                int percent = byte.Parse(commandString) - 1;
                gbuf[6] = percent;
                gbuf[6] |= 8;
            }

            byte[] kbuf = new byte[7];
            int idx = gbuf[0];
            kbuf[0] = (byte)(tbFx[idx] >> 8);
            idx = gbuf[1] ^ kbuf[0];
            kbuf[1] = (byte)(tbFx[idx] >> 8);
            idx = gbuf[2] ^ kbuf[1];
            kbuf[2] = (byte)(tbFx[idx] >> 8);
            idx = gbuf[3] ^ kbuf[2];
            kbuf[3] = (byte)(tbFx[idx] >> 8);
            idx = gbuf[4] ^ kbuf[3];
            kbuf[4] = (byte)(tbFx[idx] >> 8);
            idx = gbuf[5] ^ kbuf[4];
            kbuf[5] = (byte)(tbFx[idx] >> 8);
            kbuf[6] = (byte)gbuf[6];
            byte[] cbuf = new byte[7];
            idx = kbuf[0];
            cbuf[0] = (byte)(tbFx[idx] & 0xff);
            idx = kbuf[1] ^ cbuf[0];
            cbuf[1] = (byte)(tbFx[idx] & 0xff);
            idx = kbuf[2] ^ cbuf[1];
            cbuf[2] = (byte)(tbFx[idx] & 0xff);
            idx = kbuf[3] ^ cbuf[2];
            cbuf[3] = (byte)(tbFx[idx] & 0xff);
            idx = kbuf[4] ^ cbuf[3];
            cbuf[4] = (byte)(tbFx[idx] & 0xff);
            idx = kbuf[5] ^ cbuf[4];
            cbuf[5] = (byte)(tbFx[idx] & 0xff);
            cbuf[6] = (byte)(kbuf[6] ^ 9);
            int temp = ((((((cbuf[6] << 0x18) | (cbuf[5] << 20)) | (cbuf[4] << 0x10)) | (cbuf[3] << 12)) | (cbuf[2] << 8)) | (cbuf[1] << 4)) | cbuf[0];
            temp = (temp >> 2) | ((temp & 3) << 0x1a);
            binaryCommand[2, 1] = (byte)(temp >> 20);
            binaryCommand[2, 2] = (byte)(temp >> 12);
            binaryCommand[2, 3] = (byte)(temp >> 4);
            binaryCommand[2, 4] = (byte)(temp << 4);
            binaryCommand[2, 5] = 0;
            binaryCommand[2, 6] = 0;
            binaryCommand[2, 7] = 0;
            binaryCommand[3, 1] = 0;
            binaryCommand[3, 2] = 0;
            binaryCommand[3, 3] = 0;
            binaryCommand[3, 4] = 0;
            binaryCommand[3, 5] = 0;
            binaryCommand[3, 6] = 0;
            binaryCommand[3, 7] = 0;
        }
    }
}