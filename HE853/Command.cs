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
    internal abstract class Command
    {
        public const string On = "ON";
        public const string Off = "OFF";
        
        protected int StartBitHTime
        {
            get;
            set;
        }

        protected int StartBitLTime
        {
            get;
            set;
        }

        protected int EndBitHTime
        {
            get;
            set;
        }

        protected int EndBitLTime
        {
            get;
            set;
        }

        protected int DataBit0HTime
        {
            get;
            set;
        }

        protected int DataBit0LTime
        {
            get;
            set;
        }

        protected int DataBit1HTime
        {
            get;
            set;
        }

        protected int DataBit1LTime
        {
            get;
            set;
        }

        protected byte DataBitCount
        {
            get;
            set;
        }

        protected byte FrameCount
        {
            get;
            set;
        }

        public byte[,] Build(int deviceCode, string commandString)
        {
            byte[,] binaryCommand = new byte[4, 8];
            this.BuildSpec(ref binaryCommand);
            this.BuildData(ref binaryCommand, deviceCode, commandString);
            return binaryCommand;
        }

        protected abstract void BuildData(ref byte[,] binaryCommand, int deviceCode, string commandString);

        private void BuildSpec(ref byte[,] binaryCommand)
        {
            binaryCommand[0, 0] = 1;
            binaryCommand[1, 0] = 2;
            int temp = this.StartBitHTime / 10;
            binaryCommand[0, 1] = (byte)((temp >> 8) & 0xff);
            binaryCommand[0, 2] = (byte)(temp & 0xff);
            temp = this.StartBitLTime / 10;
            binaryCommand[0, 3] = (byte)((temp >> 8) & 0xff);
            binaryCommand[0, 4] = (byte)(temp & 0xff);
            temp = this.EndBitHTime / 10;
            binaryCommand[0, 5] = (byte)((temp >> 8) & 0xff);
            binaryCommand[0, 6] = (byte)(temp & 0xff);
            temp = this.EndBitLTime / 10;
            binaryCommand[0, 7] = (byte)((temp >> 8) & 0xff);
            binaryCommand[1, 1] = (byte)(temp & 0xff);
            temp = this.DataBit0HTime / 10;
            binaryCommand[1, 2] = (byte)(temp & 0xff);
            temp = this.DataBit0LTime / 10;
            binaryCommand[1, 3] = (byte)(temp & 0xff);
            temp = this.DataBit1HTime / 10;
            binaryCommand[1, 4] = (byte)(temp & 0xff);
            temp = this.DataBit1LTime / 10;
            binaryCommand[1, 5] = (byte)(temp & 0xff);
            binaryCommand[1, 6] = this.DataBitCount;
            binaryCommand[1, 7] = this.FrameCount;
        }
    }
}