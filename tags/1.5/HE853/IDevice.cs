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
    /// Interface to perform tasks with the HE853 device.
    /// </summary>
    [ComVisible(true), GuidAttribute("A968C162-5E23-4448-A92C-95588B21A0B7")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IDevice
    {
        /// <summary>
        /// Prepares the HE853 device for usage.
        /// </summary>
        void Open();

        /// <summary>
        /// Shuts down the HE853 device.
        /// </summary>
        void Close();

        /// <summary>
        /// Swiches receivers with specific device code on.
        /// </summary>
        /// <param name="deviceCode">Device code of receivers.</param>
        /// <param name="commandStyle">Does specify how much information is send.</param>
        void SwitchOn(int deviceCode, CommandStyle commandStyle);

        /// <summary>
        /// Swiches receivers with specific device code off.
        /// </summary>
        /// <param name="deviceCode">Device code of receivers.</param>
        /// <param name="commandStyle">Does specify how much information is send.</param>
        void SwitchOff(int deviceCode, CommandStyle commandStyle);
        
        /// <summary>
        /// Adjusts dim level on receivers with specific device code.
        /// </summary>
        /// <param name="deviceCode">Device code of receivers.</param>
        /// <param name="commandStyle">Does specify how much information is send.</param>
        /// <param name="amount">Amount of dim. A value between 1 an 8.</param>
        void AdjustDim(int deviceCode, CommandStyle commandStyle, int amount);
    }
}