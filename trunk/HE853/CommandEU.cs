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
    internal sealed class CommandEU : Command
    {
        public CommandEU()
        {
            StartBitHTime = 260;
            StartBitLTime = 8600;
            EndBitHTime = 0;
            EndBitLTime = 0;
            DataBit0HTime = 260;
            DataBit0LTime = 260;
            DataBit1HTime = 260;
            DataBit1LTime = 1300;
            DataBitCount = 57;
            FrameCount = 7;
        }

        protected override void BuildData(ref byte[,] binaryCommand, int deviceCode, string commandString)
        {
            int i;
            int[] tbFx = new int[] { 7, 11, 13, 14, 0x13, 0x15, 0x16, 0x19, 0x1a, 0x1c, 3, 5, 6, 9, 10, 12 };
            binaryCommand[2, 0] = 3;
            binaryCommand[3, 0] = 4;
            byte[] buf = new byte[] { 0, (byte)((deviceCode >> 8) & 0xff), (byte)(deviceCode & 0xff), 1 };
            if (commandString == On)
            {
                buf[3] = (byte)(buf[3] | 0x10);
            }

            byte[] gbuf = new byte[] { (byte)(buf[0] >> 4), (byte)(buf[0] & 15), (byte)(buf[1] >> 4), (byte)(buf[1] & 15), (byte)(buf[2] >> 4), (byte)(buf[2] & 15), (byte)(buf[3] >> 4), (byte)(buf[3] & 15) };
            byte[] kbuf = new byte[8];
            for (i = 0; i < 8; i++)
            {
                kbuf[i] = (byte)tbFx[gbuf[i]];
            }

            for (i = 0; i < 8; i++)
            {
                kbuf[i] = (byte)(kbuf[i] | 0x40);
            }

            for (i = 0; i < 8; i++)
            {
                kbuf[i] = (byte)(kbuf[i] & 0x7f);
            }

            kbuf[0] = (byte)(kbuf[0] | 0x80);
            ulong t64 = 0L;
            t64 = kbuf[0];
            for (i = 1; i < 8; i++)
            {
                t64 = (t64 << 7) | kbuf[i];
            }

            t64 = t64 << 7;
            kbuf[0] = (byte)(t64 >> 0x38);
            kbuf[1] = (byte)(t64 >> 0x30);
            kbuf[2] = (byte)(t64 >> 40);
            kbuf[3] = (byte)(t64 >> 0x20);
            kbuf[4] = (byte)(t64 >> 0x18);
            kbuf[5] = (byte)(t64 >> 0x10);
            kbuf[6] = (byte)(t64 >> 8);
            kbuf[7] = (byte)t64;
            binaryCommand[2, 1] = kbuf[0];
            binaryCommand[2, 2] = kbuf[1];
            binaryCommand[2, 3] = kbuf[2];
            binaryCommand[2, 4] = kbuf[3];
            binaryCommand[2, 5] = kbuf[4];
            binaryCommand[2, 6] = kbuf[5];
            binaryCommand[2, 7] = kbuf[6];
            binaryCommand[3, 1] = kbuf[7];
            binaryCommand[3, 2] = 0;
            binaryCommand[3, 3] = 0;
            binaryCommand[3, 4] = 0;
            binaryCommand[3, 5] = 0;
            binaryCommand[3, 6] = 0;
            binaryCommand[3, 7] = 0;
        }
    }
}
