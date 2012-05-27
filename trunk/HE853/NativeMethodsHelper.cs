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
    using System.Runtime.InteropServices;

    /// <summary>
    /// Collection of Win32 API helper methods. 
    /// </summary>
    internal static class NativeMethodsHelper
    {
        /// <summary>
        /// Convenience helper for CloseHandle.
        /// </summary>
        /// <param name="handle">Handle to file to close.</param>
        public static void CloseHandle(ref IntPtr handle)
        {
            if (handle != IntPtr.Zero)
            {
                NativeMethods.CloseHandle(handle);
                handle = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Convenience helper for CreateFile.
        /// </summary>
        /// <param name="fileName">Name of the file to open.</param>
        /// <returns>Handle to file.</returns>
        public static IntPtr CreateFileForWrite(string fileName)
        {
            return CreateFile(fileName, NativeMethods.GenericWrite);
        }

        /// <summary>
        /// Get HE853 HID device path.
        /// </summary>
        /// <returns>HE853 HID device path or empty string if device is not available.</returns>
        public static string GetHE853DevicePath()
        {
            IntPtr hidHandle = IntPtr.Zero;
            string devicePath = string.Empty;

            string[] paths = GetHIDDevicePaths();
            foreach (string path in paths)
            {
                hidHandle = CreateFile(path, 0);

                if (hidHandle != IntPtr.Zero)
                {
                    bool isHIDProduct = IsHE853Device(hidHandle);
                    CloseHandle(ref hidHandle);

                    if (isHIDProduct)
                    {
                        devicePath = path;
                        break;
                    }
                }
            }

            return devicePath;
        }

        /// <summary>
        /// Convenience helper for HidD_SetOutputReport.
        /// </summary>
        /// <param name="hidDeviceObject">Valid handle to HID device.</param>
        /// <param name="reportBuffer">Data to send.</param>
        /// <returns>True if the data could be send.</returns>
        public static bool SetHIDOutputReport(IntPtr hidDeviceObject, byte[] reportBuffer)
        {
            return NativeMethods.HidD_SetOutputReport(hidDeviceObject, reportBuffer, reportBuffer.Length);
        }

        /// <summary>
        /// Convenience helper for CreateFile.
        /// </summary>
        /// <param name="fileName">Name of the file to open.</param>
        /// <param name="desiredAccess">Flags for access.</param>
        /// <returns>Handle to file.</returns>
        private static IntPtr CreateFile(string fileName, uint desiredAccess)
        {
            NativeMethods.SecurityAttributes security = new NativeMethods.SecurityAttributes();
            security.Length = Marshal.SizeOf(security);
            security.SecurityDescriptor = IntPtr.Zero;
            security.InheritHandle = Convert.ToInt32(true);

            return NativeMethods.CreateFile(fileName, desiredAccess, 0x3, ref security, 0x3, 0x0, IntPtr.Zero);
        }

        /// <summary>
        /// Tests if device is HE853 based on vendor and product ID.
        /// </summary>
        /// <param name="hidHandle">Valid handle to HID device.</param>
        /// <returns>True if it an HE853 device.</returns>
        private static bool IsHE853Device(IntPtr hidHandle)
        {
            const short VendorID = 0x4D9;
            const short ProductID = 0x1357;
            
            NativeMethods.HIDDAttributes deviceAttributes = new NativeMethods.HIDDAttributes();
            deviceAttributes.Size = Marshal.SizeOf(deviceAttributes);
            if (NativeMethods.HidD_GetAttributes(hidHandle, ref deviceAttributes) != 0)
            {
                if ((deviceAttributes.VendorID == VendorID) && (deviceAttributes.ProductID == ProductID))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a list of available HID device paths.
        /// </summary>
        /// <returns>List of available HID device paths.</returns>
        private static string[] GetHIDDevicePaths()
        {
            string[] paths = new string[0];
            Guid guid = Guid.Empty;
            NativeMethods.HidD_GetHidGuid(ref guid);

            int index = 0;
            IntPtr deviceInfoSet = NativeMethods.SetupDiGetClassDevs(ref guid, null, IntPtr.Zero, 0x12);

            if (deviceInfoSet == IntPtr.Zero)
            {
                return paths;
            }

            NativeMethods.SPDeviceInterfaceData deviceInterfaceData = new NativeMethods.SPDeviceInterfaceData();
            deviceInterfaceData.Size = Marshal.SizeOf(deviceInterfaceData);

            while (NativeMethods.SetupDiEnumDeviceInterfaces(deviceInfoSet, IntPtr.Zero, ref guid, index, ref deviceInterfaceData) != 0)
            {
                int bufferSize = 0;
                NativeMethods.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref deviceInterfaceData, IntPtr.Zero, 0, ref bufferSize, IntPtr.Zero);
                IntPtr deviceInterfaceDetailData = Marshal.AllocHGlobal(bufferSize);

                int detailDataSize = 6;
                if (IntPtr.Size == 8)
                {
                    detailDataSize = 8;
                }

                Marshal.WriteInt32(deviceInterfaceDetailData, detailDataSize);
                NativeMethods.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref deviceInterfaceData, deviceInterfaceDetailData, bufferSize, ref bufferSize, IntPtr.Zero);

                byte[] deviceInterfaceDetailDataManaged = new byte[bufferSize];
                Marshal.Copy(deviceInterfaceDetailData, deviceInterfaceDetailDataManaged, 0, deviceInterfaceDetailDataManaged.Length);
                Marshal.FreeHGlobal(deviceInterfaceDetailData);

                byte[] deviceName = new byte[deviceInterfaceDetailDataManaged.Length - 4];
                for (int i = 0; i < deviceName.Length; ++i)
                {
                    deviceName[i] = deviceInterfaceDetailDataManaged[i + 4];
                }

                Array.Resize(ref paths, paths.Length + 1);
                paths[paths.Length - 1] = System.Text.Encoding.Unicode.GetString(deviceName);
                ++index;
            }

            NativeMethods.SetupDiDestroyDeviceInfoList(deviceInfoSet);

            return paths;
        }
    }
}
