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
    /// Encoder for command strings and device code for some CN specific
    /// receivers. This seems to be the most common one and the only
    /// that supports dim.
    /// </summary>
    public sealed class CommandCN : Command
    {
        /// <summary>
        /// Per command incremented counter that will be embedded into the data part.
        /// </summary>
        private byte count = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandCN"/> class. Sets
        /// region specific RF specification.
        /// </summary>
        public CommandCN()
        {
            this.StartBitHTime = 32;
            this.StartBitLTime = 480;
            this.EndBitHTime = 0;
            this.EndBitLTime = 0;
            this.DataBit0HTime = 32;
            this.DataBit0LTime = 96;
            this.DataBit1HTime = 96;
            this.DataBit1LTime = 32;
            this.DataBitCount = 28;
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
            ushort[] lookup = new ushort[] { 0x609, 0x306, 0x803, 0xA08, 0xA, 0x200, 0xC02, 0x40C, 0xE04, 0x70E, 0x507, 0x105, 0xF01, 0xB0F, 0xD0B, 0x90D };
            this.count = (byte)(this.count + 1);
            byte[] encodingBuffer = new byte[7];
            encodingBuffer[0] = 0x1;
            encodingBuffer[1] = (byte)((this.count << 2) & 0xF);
            if (command != Command.Off)
            {
                encodingBuffer[1] |= 0x2;
            }

            encodingBuffer[2] = (byte)(deviceCode & 0xF);
            encodingBuffer[3] = (byte)((deviceCode >> 4) & 0xF);
            encodingBuffer[4] = (byte)((deviceCode >> 8) & 0xF);
            encodingBuffer[5] = (byte)((deviceCode >> 12) & 0xF);
            if ((command == Command.On) || (command == Command.Off))
            {
                encodingBuffer[6] = 0x0;
            }
            else
            {
                string firstDigitString = command.Substring(0, 1);
                encodingBuffer[6] = (byte)(byte.Parse(firstDigitString) - 1);
                encodingBuffer[6] |= 0x8;
            }

            int nextLookup = encodingBuffer[0];
            for (int i = 0; i < encodingBuffer.Length - 1; ++i)
            {
                encodingBuffer[i] = (byte)(lookup[nextLookup] >> 8);
                nextLookup = encodingBuffer[i + 1] ^ encodingBuffer[i];
            }
            
            nextLookup = encodingBuffer[0];
            for (int i = 0; i < encodingBuffer.Length - 1; ++i)
            {
                encodingBuffer[i] = (byte)(lookup[nextLookup] & 0xFF);
                nextLookup = encodingBuffer[i + 1] ^ encodingBuffer[i];
            }

            encodingBuffer[6] = (byte)(encodingBuffer[6] ^ 0x9);

            int temp = ((((((encodingBuffer[6] << 24) | (encodingBuffer[5] << 20)) | (encodingBuffer[4] << 16)) | (encodingBuffer[3] << 12)) | (encodingBuffer[2] << 8)) | (encodingBuffer[1] << 4)) | encodingBuffer[0];
            temp = (temp >> 2) | ((temp & 0x3) << 26);

            stream.WriteByte((byte)(temp >> 20));
            stream.WriteByte((byte)(temp >> 12));
            stream.WriteByte((byte)(temp >> 4));
            stream.WriteByte((byte)(temp << 4));
            this.WriteZero(stream, 10);
        }
    }
}