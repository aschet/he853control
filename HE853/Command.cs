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

        protected byte DataBit0HTime
        {
            get;
            set;
        }

        protected byte DataBit0LTime
        {
            get;
            set;
        }

        protected byte DataBit1HTime
        {
            get;
            set;
        }

        protected byte DataBit1LTime
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
            binaryCommand[0, 1] = (byte)((this.StartBitHTime >> 8) & 0xff);
            binaryCommand[0, 2] = (byte)(this.StartBitHTime & 0xff);
            binaryCommand[0, 3] = (byte)((this.StartBitLTime >> 8) & 0xff);
            binaryCommand[0, 4] = (byte)(this.StartBitLTime & 0xff);
            binaryCommand[0, 5] = (byte)((this.EndBitHTime >> 8) & 0xff);
            binaryCommand[0, 6] = (byte)(this.EndBitHTime & 0xff);
            binaryCommand[0, 7] = (byte)((this.EndBitLTime >> 8) & 0xff);
            binaryCommand[1, 1] = (byte)(this.EndBitLTime & 0xff);
            binaryCommand[1, 2] = this.DataBit0HTime;
            binaryCommand[1, 3] = this.DataBit0LTime;
            binaryCommand[1, 4] = this.DataBit1HTime;
            binaryCommand[1, 5] = this.DataBit1LTime;
            binaryCommand[1, 6] = this.DataBitCount;
            binaryCommand[1, 7] = this.FrameCount;
        }
    }
}