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

    /// <summary>
    /// Encoder for command strings and device code for some EU specific
    /// receivers. Maybe for legacy support.
    /// </summary>
    public sealed class CommandEU : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandEU"/> class. Sets
        /// region specific RF specification.
        /// </summary>
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

        /// <summary>
        /// Writes encoded data part of command sequence.
        /// </summary>
        /// <param name="stream">Receiving output stream.</param>
        /// <param name="deviceCode">Device code of receivers to encode.</param>
        /// <param name="command">Text command to encode.</param>
        protected override void WriteData(Stream stream, int deviceCode, string command)
        {
            if (stream == null)
            {
                return;
            }

            byte[] lookup = new byte[] { 0x7, 0xB, 0xD, 0xE, 0x13, 0x15, 0x16, 0x19, 0x1A, 0x1C, 0x3, 0x5, 0x6, 0x9, 0xA, 0xC };

            int commandValue = 0x1;
            if (command == Command.On)
            {
                commandValue |= 0x10;
            }

            byte[] encodingBuffer = new byte[] { 0x0, 0x0, (byte)((deviceCode >> 12) & 0xF), (byte)((deviceCode >> 8) & 0xF), (byte)((deviceCode >> 4) & 0xF), (byte)(deviceCode & 0xF), (byte)(commandValue >> 4), (byte)(commandValue & 0xF) };
            for (int i = 0; i < encodingBuffer.Length; ++i)
            {
                encodingBuffer[i] = (byte)((lookup[encodingBuffer[i]] | 0x40) & 0x7F);
            }

            encodingBuffer[0] |= 0x80;
            ulong encodingValue = encodingBuffer[0];
            for (int i = 1; i < encodingBuffer.Length; ++i)
            {
                encodingValue = (encodingValue << 7) | encodingBuffer[i];
            }

            encodingValue = encodingValue << 7;
            int shift = 56;
            for (int i = 0; i < encodingBuffer.Length; ++i)
            {
                encodingBuffer[i] = (byte)(encodingValue >> shift);
                shift -= 8;
            }

            stream.Write(encodingBuffer, 0, encodingBuffer.Length);
            Command.WriteZero(stream, 6);
        }
    }
}
