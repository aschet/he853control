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
        /// <param name="commandString">Text command to encode.</param>
        protected override void WriteData(Stream stream, int deviceCode, string commandString)
        {
            byte[] lookup = new byte[] { 0x7, 0xB, 0xD, 0xE, 0x13, 0x15, 0x16, 0x19, 0x1A, 0x1C, 0x3, 0x5, 0x6, 0x9, 0xA, 0xC };

            int command = 0x1;
            if (commandString == Command.On)
            {
                command |= 0x10;
            }

            byte[] encodingBuffer = new byte[] { 0x0, 0x0, (byte)((deviceCode >> 12) & 0xF), (byte)((deviceCode >> 8) & 0xF), (byte)((deviceCode >> 4) & 0xF), (byte)(deviceCode & 0xF), (byte)(command >> 4), (byte)(command & 0xF) };
            for (int i = 0; i < encodingBuffer.Length; ++i)
            {
                encodingBuffer[i] = (byte)((lookup[encodingBuffer[i]] | 0x40) & 0x7F);
            }

            encodingBuffer[0] |= 0x80;
            ulong t64 = encodingBuffer[0];
            for (int i = 1; i < encodingBuffer.Length; ++i)
            {
                t64 = (t64 << 7) | encodingBuffer[i];
            }

            t64 = t64 << 7;
            encodingBuffer[0] = (byte)(t64 >> 56);
            encodingBuffer[1] = (byte)(t64 >> 48);
            encodingBuffer[2] = (byte)(t64 >> 40);
            encodingBuffer[3] = (byte)(t64 >> 32);
            encodingBuffer[4] = (byte)(t64 >> 24);
            encodingBuffer[5] = (byte)(t64 >> 16);
            encodingBuffer[6] = (byte)(t64 >> 8);
            encodingBuffer[7] = (byte)t64;

            stream.Write(encodingBuffer, 0, encodingBuffer.Length);

            this.WriteZero(stream, 6);
        }
    }
}
