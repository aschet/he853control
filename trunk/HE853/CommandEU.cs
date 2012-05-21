﻿/*
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

    public sealed class CommandEU : Command
    {
        public CommandEU()
        {
            this.StartBitHTime = 26;
            this.StartBitLTime = 860;
            this.EndBitHTime = 0;
            this.EndBitLTime = 0;
            this.DataBit0HTime = 26;
            this.DataBit0LTime = 26;
            this.DataBit1HTime = 26;
            this.DataBit1LTime = 130;
            this.DataBitCount = 57;
            this.FrameCount = 7;
        }

        protected override void BuildData(ref MemoryStream stream, int deviceCode, string commandString)
        {
            int[] seed = new int[] { 0x7, 0xB, 0xD, 0xE, 0x13, 0x15, 0x16, 0x19, 0x1A, 0x1C, 0x3, 0x5, 0x6, 0x9, 0xA, 0xC };
            byte[] buf = new byte[] { 0, (byte)((deviceCode >> 8) & 0xFF), (byte)(deviceCode & 0xFF), 1 };
            if (commandString == Command.On)
            {
                buf[3] = (byte)(buf[3] | 0x10);
            }

            byte[] gbuf = new byte[] { (byte)(buf[0] >> 4), (byte)(buf[0] & 0xF), (byte)(buf[1] >> 4), (byte)(buf[1] & 0xF), (byte)(buf[2] >> 4), (byte)(buf[2] & 0xF), (byte)(buf[3] >> 4), (byte)(buf[3] & 0xF) };
            byte[] kbuf = new byte[8];
            for (int i = 0; i < kbuf.Length; ++i)
            {
                kbuf[i] = (byte)seed[gbuf[i]];
                kbuf[i] = (byte)(kbuf[i] | 0x40);
                kbuf[i] = (byte)(kbuf[i] & 0x7F);
            }

            kbuf[0] = (byte)(kbuf[0] | 0x80);
            ulong t64 = kbuf[0];
            for (int i = 1; i < kbuf.Length; ++i)
            {
                t64 = (t64 << 7) | kbuf[i];
            }

            t64 = t64 << 7;
            kbuf[0] = (byte)(t64 >> 56);
            kbuf[1] = (byte)(t64 >> 48);
            kbuf[2] = (byte)(t64 >> 40);
            kbuf[3] = (byte)(t64 >> 32);
            kbuf[4] = (byte)(t64 >> 24);
            kbuf[5] = (byte)(t64 >> 16);
            kbuf[6] = (byte)(t64 >> 8);
            kbuf[7] = (byte)t64;

            stream.WriteByte(3);
            stream.WriteByte(kbuf[0]);
            stream.WriteByte(kbuf[1]);
            stream.WriteByte(kbuf[2]);
            stream.WriteByte(kbuf[3]);
            stream.WriteByte(kbuf[4]);
            stream.WriteByte(kbuf[5]);
            stream.WriteByte(kbuf[6]);

            stream.WriteByte(4);
            stream.WriteByte(kbuf[7]);
            this.WriteZero(ref stream, 6);
        }
    }
}
