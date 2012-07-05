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
    /// Collecion of Win32 API imports that are required for device communication.s
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// Import for CloseHandle function.
        /// </summary>
        /// <param name="handleObject">A valid handle to an open object.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(IntPtr handleObject);

        /// <summary>
        /// Import for CreateFile function.
        /// </summary>
        /// <param name="fileName">The name of the file or device to be created or opened.</param>
        /// <param name="desiredAccess">The requested access to the file or device.</param>
        /// <param name="shareMode">The requested sharing mode of the file or device.</param>
        /// <param name="securityAttributes">Security specifier for access.</param>
        /// <param name="creationDisposition">An action to take on a file or device that exists or does not exist.</param>
        /// <param name="flagsAndAttributes">The file or device attributes and flags.</param>
        /// <param name="handleTemplateFile">A valid handle to a template file with the GENERIC_READ access right.</param>
        /// <returns>If the function succeeds, the return value is an open handle to the specified file, device, named pipe, or mail slot.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateFile(string fileName, uint desiredAccess, uint shareMode, ref SecurityAttributes securityAttributes, int creationDisposition, uint flagsAndAttributes, IntPtr handleTemplateFile);

        /// <summary>
        /// Import for HidD_GetHidGuid function.
        /// </summary>
        /// <param name="hidGuid">Pointer to a caller-allocated GUID buffer that the routine uses to return the device interface GUID for HIDClass devices.</param>
        [DllImport("hid.dll")]
        public static extern void HidD_GetHidGuid(ref Guid hidGuid);

        /// <summary>
        /// Import for HidD_FlushQueue function.
        /// </summary>
        /// <param name="hidDeviceObject">Specifies an open handle to a top-level collection.</param>
        /// <returns>True if flush operation succeeded.</returns>
        [DllImport("hid.dll")]
        public static extern int HidD_FlushQueue(IntPtr hidDeviceObject); 

        /// <summary>
        /// Import for HidD_GetAttributes function.
        /// </summary>
        /// <param name="hidDeviceObject">Specifies an open handle to a top-level collection.</param>
        /// <param name="atributes">Pointer to a caller-allocated structure.</param>
        /// <returns>Returns true if succeeds; otherwise, it returns false.</returns>
        [DllImport("hid.dll")]
        public static extern int HidD_GetAttributes(IntPtr hidDeviceObject, ref HIDDAttributes atributes);

        /// <summary>
        /// Import for HidD_SetOutputReport function.
        /// </summary>
        /// <param name="hidDeviceObject">Specifies an open handle to a top-level collection.</param>
        /// <param name="reportBuffer">Pointer to a caller-allocated output report buffer that the caller uses to specify a report ID.</param>
        /// <param name="reportBufferLength">The size in bytes.</param>
        /// <returns>If HidD_SetOutputReport succeeds, it returns true; otherwise, it returns false.</returns>
        [DllImport("hid.dll")]
        public static extern int HidD_SetOutputReport(IntPtr hidDeviceObject, byte[] reportBuffer, int reportBufferLength);

        /// <summary>
        /// Import for SetupDiEnumDeviceInterfaces function.
        /// </summary>
        /// <param name="deviceInfoSet">A pointer to a device information set that contains the device interfaces for which to return information.</param>
        /// <param name="deviceInfoData">A pointer to structure that specifies a device information element in deviceInfoSet.</param>
        /// <param name="interfaceClassGuid">A pointer to a GUID that specifies the device interface class for the requested interface.</param>
        /// <param name="memberIndex">A zero-based index into the list of interfaces in the device information set.</param>
        /// <param name="deviceInterfaceData">A pointer to a caller-allocated buffer that contains, on successful return, a structure that identifies an interface that meets the search parameters.</param>
        /// <returns>If succeeds, it returns true; otherwise, it returns false.</returns>
        [DllImport("setupapi.dll")]
        public static extern int SetupDiEnumDeviceInterfaces(IntPtr deviceInfoSet, IntPtr deviceInfoData, ref Guid interfaceClassGuid, int memberIndex, ref SPDeviceInterfaceData deviceInterfaceData);

        /// <summary>
        /// Import for SetupDiDestroyDeviceInfoList function.
        /// </summary>
        /// <param name="deviceInfoSet">A handle to the device information set to delete.</param>
        /// <returns>The function returns true if it is successful.</returns>
        [DllImport("setupapi.dll")]
        public static extern int SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        /// <summary>
        /// Import for SetupDiGetClassDevs function.
        /// </summary>
        /// <param name="classGuid">A pointer to the GUID for a device setup class or a device interface class.</param>
        /// <param name="enumerator">An identifier (ID) of a Plug and Play (PnP) enumerator.</param>
        /// <param name="hwndParent">A handle to the top-level window to be used for a user interface that is associated with installing a device instance in the device information set.</param>
        /// <param name="flags">A variable of type DWORD that specifies control options that filter the device information elements that are added to the device information set.</param>
        /// <returns>If the operation succeeds, returns a handle to a device information set that contains all installed devices that matched the supplied parameters.</returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, string enumerator, IntPtr hwndParent, int flags);

        /// <summary>
        /// Import for SetupDiGetDeviceInterfaceDetail function.
        /// </summary>
        /// <param name="deviceInfoSet">A pointer to the device information set that contains the interface for which to retrieve details.</param>
        /// <param name="deviceInterfaceData">A pointer to structure that specifies the interface in DeviceInfoSet for which to retrieve details.</param>
        /// <param name="deviceInterfaceDetailData">A pointer to structure to receive information about the specified interface.</param>
        /// <param name="deviceInterfaceDetailDataSize">The size of the deviceInterfaceDetailData buffer.</param>
        /// <param name="requiredSize">A pointer to a variable of that receives the required size of the deviceInterfaceDetailData buffer.</param>
        /// <param name="deviceInfoData">A pointer to a buffer that receives information about the device that supports the requested interface.</param>
        /// <returns>Returns TRUE if the function completed without error.</returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Unicode)]
        public static extern int SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref SPDeviceInterfaceData deviceInterfaceData, IntPtr deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, IntPtr deviceInfoData);

        /// <summary>
        /// Define for SECURITY_ATTRIBUTES structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SecurityAttributes
        {
            /// <summary>
            /// The size in bytes.
            /// </summary>
            public int Length;
            
            /// <summary>
            /// A pointer to a SECURITY_DESCRIPTOR structure that controls access to the object.
            /// </summary>
            public IntPtr SecurityDescriptor;
            
            /// <summary>
            /// A Boolean value that specifies whether the returned handle is inherited when a new process is created.
            /// </summary>
            public int InheritHandle;
        }

        /// <summary>
        /// Define for HIDD_ATTRIBUTES structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct HIDDAttributes
        {
            /// <summary>
            /// The size in bytes.
            /// </summary>
            public int Size;
            
            /// <summary>
            /// Specifies a HID device's vendor ID.
            /// </summary>
            public short VendorID;
            
            /// <summary>
            /// Specifies a HID device's product ID.
            /// </summary>
            public short ProductID;
            
            /// <summary>
            /// Specifies the manufacturer's revision number for a HIDClass device.
            /// </summary>
            public short VersionNumber;
        }

        /// <summary>
        /// Define for SP_DEVICE_INTERFACE_DATA structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SPDeviceInterfaceData
        {
            /// <summary>
            /// The size in bytes.
            /// </summary>
            public int Size;
            
            /// <summary>
            /// The GUID for the class to which the device interface belongs.
            /// </summary>
            public Guid InterfaceClassGuid;
            
            /// <summary>
            /// See MSDN.
            /// </summary>
            public int Flags;
            
            /// <summary>
            /// Reserved. Do not use.
            /// </summary>
            public IntPtr Reserved;
        }
    }
}