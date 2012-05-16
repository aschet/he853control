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
    internal sealed class CommandUK : Command
    {
        public CommandUK()
        {
            StartBitHTime = 320;
            StartBitLTime = 9700;
            EndBitHTime = 0;
            EndBitLTime = 0;
            DataBit0HTime = 320;
            DataBit0LTime = 960;
            DataBit1HTime = 960;
            DataBit1LTime = 320;
            DataBitCount = 24;
            FrameCount = 18;
        }

        protected override void BuildData(ref byte[,] binaryCommand, int deviceCode, string commandString)
        {
            binaryCommand[2, 0] = 3;
            binaryCommand[3, 0] = 4;
            ushort address = this.UKGetAddress(deviceCode);
            byte buf = 20;
            if (commandString == On)
            {
                buf = (byte)(buf | 1);
            }

            binaryCommand[2, 1] = (byte)((address >> 8) & 0xff);
            binaryCommand[2, 2] = (byte)(address & 0xff);
            binaryCommand[2, 3] = buf;
            binaryCommand[2, 4] = 0;
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

        private ushort UKGetAddress(int deviceCode)
        {
            int[] buf = new int[8];
            for (int i = 0; i < 8; ++i)
            {
                buf[i] = deviceCode % 3;
                deviceCode /= 3;
            }

            for (int i = 0; i < 8; ++i)
            {
                switch (buf[i])
                {
                    case 0:
                        buf[i] = 0;
                        break;

                    case 1:
                        buf[i] = 3;
                        break;

                    case 2:
                        buf[i] = 1;
                        break;
                }
            }

            ushort temp = 0;
            for (int i = 7; i >= 0; --i)
            {
                temp = (ushort)(temp << 2);
                temp = (ushort)(temp | ((ushort)buf[i]));
            }

            return temp;
        }
    }
}