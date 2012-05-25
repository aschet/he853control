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
    using System;
    using System.IO;

    public abstract class Command
    {
        public const string On = "ON";
        public const string Off = "OFF";
        
        protected ushort StartBitHTime
        {
            get;
            set;
        }

        protected ushort StartBitLTime
        {
            get;
            set;
        }

        protected ushort EndBitHTime
        {
            get;
            set;
        }

        protected ushort EndBitLTime
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
            using (MemoryStream stream = new MemoryStream())
            {
                stream.WriteByte(6);
                stream.WriteByte(1);
                this.WriteZero(stream, 6);

                return stream.ToArray();
            }
        }

        public byte[] Build(int deviceCode, string commandString)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                this.BuildSpec(stream);
                this.BuildData(stream, deviceCode, commandString);
                this.BuildExec(stream);
                return this.PackSevenWithSequenceNumber(stream.ToArray());
            }
        }

        protected abstract void BuildData(MemoryStream stream, int deviceCode, string commandString);

        protected void WriteZero(MemoryStream stream, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                stream.WriteByte(0);
            }
        }

        protected void WriteUShort(MemoryStream stream, ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            stream.Write(bytes, 0, bytes.Length);
        }

        private void BuildSpec(MemoryStream stream)
        {
            this.WriteUShort(stream, this.StartBitHTime);
            this.WriteUShort(stream, this.StartBitLTime);
            this.WriteUShort(stream, this.EndBitHTime);
            this.WriteUShort(stream, this.EndBitLTime);

            stream.WriteByte(this.DataBit0HTime);
            stream.WriteByte(this.DataBit0LTime);
            stream.WriteByte(this.DataBit1HTime);
            stream.WriteByte(this.DataBit1LTime);
            stream.WriteByte(this.DataBitCount);
            stream.WriteByte(this.FrameCount);
        }

        private void BuildExec(System.IO.MemoryStream stream)
        {
            this.WriteZero(stream, 7);
        }

        private byte[] PackSevenWithSequenceNumber(byte[] command)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                const int ChunkSize = 7;
                int chunkCount = 1;
                int chunkIndex = ChunkSize;

                foreach (byte value in command)
                {
                    if (chunkIndex == ChunkSize)
                    {
                        stream.WriteByte((byte)chunkCount);
                        ++chunkCount;
                        chunkIndex = 0;
                    }

                    ++chunkIndex;
                    stream.WriteByte(value);
                }

                return stream.ToArray();
            }
        }
    }
}