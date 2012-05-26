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

    /// <summary>
    /// Abstract encoder for command strings and device code. Translates text
    /// into binary sequence that can be send as HID report.
    /// A command is a series of 8 byte long sequences where the first byte
    /// contains the sequence number.
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Constant for the On command.
        /// </summary>
        public const string On = "ON";
        
        /// <summary>
        /// Constant for the Off command.
        /// </summary>
        public const string Off = "OFF";

        /// <summary>
        /// Gets or sets part of RF specification. Extracted from MDB.
        /// </summary>
        protected ushort StartBitHTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets part of RF specification. Extracted from MDB.
        /// </summary>
        protected ushort StartBitLTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets part of RF specification. Extracted from MDB.
        /// </summary>
        protected ushort EndBitHTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets part of RF specification. Extracted from MDB.
        /// </summary>
        protected ushort EndBitLTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets part of RF specification. Extracted from MDB.
        /// </summary>
        protected byte DataBit0HTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets part of RF specification. Extracted from MDB.
        /// </summary>
        protected byte DataBit0LTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets part of RF specification. Extracted from MDB.
        /// </summary>
        protected byte DataBit1HTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets part of RF specification. Extracted from MDB.
        /// </summary>
        protected byte DataBit1LTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets part of RF specification. Extracted from MDB.
        /// </summary>
        protected byte DataBitCount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets part of RF specification. Extracted from MDB.
        /// </summary>
        protected byte FrameCount
        {
            get;
            set;
        }

        /// <summary>
        /// Builds command sequence required to query status.
        /// </summary>
        /// <returns>Command sequence required to query status.</returns>
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

        /// <summary>
        /// Builds command sequence to perform given command.
        /// </summary>
        /// <param name="deviceCode">Device code of receivers.</param>
        /// <param name="command">Text command to send.</param>
        /// <returns>Command sequence to perform given command.</returns>
        public byte[] Build(int deviceCode, string command)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                this.WriteRFSpec(stream);
                this.WriteData(stream, deviceCode, command);
                this.WriteExec(stream);
                return this.PackSevenWithSequenceNumber(stream.ToArray());
            }
        }

        /// <summary>
        /// Writes encoded data part of command sequence.
        /// </summary>
        /// <param name="stream">Receiving output stream.</param>
        /// <param name="deviceCode">Device code of receivers to encode.</param>
        /// <param name="command">Text command to encode.</param>
        protected abstract void WriteData(Stream stream, int deviceCode, string command);

        /// <summary>
        /// Writes a given amount of zeros.
        /// </summary>
        /// <param name="stream">Receiving output stream.</param>
        /// <param name="count">Amount of zeros to write.</param>
        protected void WriteZero(Stream stream, int count)
        {
            byte[] bytes = new byte[count];
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes a ushort value.
        /// </summary>
        /// <param name="stream">Receiving output stream.</param>
        /// <param name="value">Value to write.</param>
        protected void WriteUShort(Stream stream, ushort value)
        {
            if (stream.CanWrite)
            {
                stream.WriteByte((byte)((value >> 8) & 0xFF));
                stream.WriteByte((byte)(value & 0xFF));
            }
        }

        /// <summary>
        /// Writes the RF specification part from properties.
        /// </summary>
        /// <param name="stream">Receiving output stream.</param>
        private void WriteRFSpec(Stream stream)
        {
            if (stream.CanWrite)
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
        }

        /// <summary>
        /// Writes the command execution sequence part.
        /// </summary>
        /// <param name="stream">Receiving output stream.</param>
        private void WriteExec(Stream stream)
        {
            if (stream.CanWrite)
            {
                this.WriteZero(stream, 7);
            }
        }

        /// <summary>
        /// Splits a command sequence into a series of 7 bytes and prepends a incrementing
        /// sequence number.
        /// </summary>
        /// <param name="command">Command sequence to split.</param>
        /// <returns>Command sequence with sequence numbers.</returns>
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