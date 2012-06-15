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
    using System.Runtime.InteropServices;

    /// <summary>
    /// Does specify how much information is send.
    /// </summary>
    [ComVisible(true), GuidAttribute("3DAA2F62-5469-4F71-A4EA-5D7077FE920A")]
    public enum CommandStyle
    {
        /// <summary>
        /// Short send sequence. Does not support all devices.
        /// </summary>
        Short,

        /// <summary>
        /// Long send sequence. Does support all devices but requires more time to send information.
        /// </summary>
        Comprehensive
    }
}
