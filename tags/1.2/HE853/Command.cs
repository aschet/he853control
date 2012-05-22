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

    public abstract class Command
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

        public byte[] BuildStatus()
        {
            MemoryStream stream = new MemoryStream();

            stream.WriteByte(6);
            stream.WriteByte(1);
            this.WriteZero(ref stream, 6);

            return stream.ToArray();
        }

        public byte[] Build(int deviceCode, string commandString)
        {
            MemoryStream stream = new MemoryStream();

            this.BuildSpec(ref stream);
            this.BuildData(ref stream, deviceCode, commandString);
            this.BuildExec(ref stream);

            return stream.ToArray();
        }

        protected abstract void BuildData(ref MemoryStream stream, int deviceCode, string commandString);

        protected void WriteZero(ref MemoryStream stream, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                stream.WriteByte(0);
            }
        }

        private void BuildSpec(ref MemoryStream stream)
        {
            stream.WriteByte(1);
            stream.WriteByte((byte)((this.StartBitHTime >> 8) & 0xff));
            stream.WriteByte((byte)(this.StartBitHTime & 0xff));
            stream.WriteByte((byte)((this.StartBitLTime >> 8) & 0xff));
            stream.WriteByte((byte)(this.StartBitLTime & 0xff));
            stream.WriteByte((byte)((this.EndBitHTime >> 8) & 0xff));
            stream.WriteByte((byte)(this.EndBitHTime & 0xff));
            stream.WriteByte((byte)((this.EndBitLTime >> 8) & 0xff));

            stream.WriteByte(2);            
            stream.WriteByte((byte)(this.EndBitLTime & 0xff));
            stream.WriteByte(this.DataBit0HTime);
            stream.WriteByte(this.DataBit0LTime);
            stream.WriteByte(this.DataBit1HTime);
            stream.WriteByte(this.DataBit1LTime);
            stream.WriteByte(this.DataBitCount);
            stream.WriteByte(this.FrameCount);
        }

        private void BuildExec(ref System.IO.MemoryStream stream)
        {
            stream.WriteByte(5);
            this.WriteZero(ref stream, 7);
        }
    }
}