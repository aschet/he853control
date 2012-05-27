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
    /// 
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// 
        /// </summary>
        public const uint GenericWrite = 0x40000000;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handleObject"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(IntPtr handleObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="desiredAccess"></param>
        /// <param name="shareMode"></param>
        /// <param name="securityAttributes"></param>
        /// <param name="creationDisposition"></param>
        /// <param name="flagsAndAttributes"></param>
        /// <param name="handleTemplateFile"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateFile(string fileName, uint desiredAccess, uint shareMode, ref SecurityAttributes securityAttributes, int creationDisposition, uint flagsAndAttributes, IntPtr handleTemplateFile);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hidGuid"></param>
        [DllImport("hid.dll")]
        public static extern void HidD_GetHidGuid(ref Guid hidGuid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hidDeviceObject"></param>
        /// <param name="atributes"></param>
        /// <returns></returns>
        [DllImport("hid.dll")]
        public static extern int HidD_GetAttributes(IntPtr hidDeviceObject, ref HIDDAttributes atributes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hidDeviceObject"></param>
        /// <param name="reportBuffer"></param>
        /// <param name="reportBufferLength"></param>
        /// <returns></returns>
        [DllImport("hid.dll")]
        public static extern bool HidD_SetOutputReport(IntPtr hidDeviceObject, byte[] reportBuffer, int reportBufferLength);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfoSet"></param>
        /// <param name="deviceInfoData"></param>
        /// <param name="interfaceClassGuid"></param>
        /// <param name="memberIndex"></param>
        /// <param name="deviceInterfaceData"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll")]
        public static extern int SetupDiEnumDeviceInterfaces(IntPtr deviceInfoSet, IntPtr deviceInfoData, ref Guid interfaceClassGuid, int memberIndex, ref SPDeviceInterfaceData deviceInterfaceData);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfoSet"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll")]
        public static extern int SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classGuid"></param>
        /// <param name="enumerator"></param>
        /// <param name="hwndParent"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, string enumerator, IntPtr hwndParent, int flags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfoSet"></param>
        /// <param name="deviceInterfaceData"></param>
        /// <param name="deviceInterfaceDetailData"></param>
        /// <param name="deviceInterfaceDetailDataSize"></param>
        /// <param name="requiredSize"></param>
        /// <param name="deviceInfoData"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Unicode)]
        public static extern int SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref SPDeviceInterfaceData deviceInterfaceData, IntPtr deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, IntPtr deviceInfoData);

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SecurityAttributes
        {
            public int Length;
            public IntPtr SecurityDescriptor;
            public int InheritHandle;
        }

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct HIDDAttributes
        {
            public int Size;
            public short VendorID;
            public short ProductID;
            public short VersionNumber;
        }

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SPDeviceInterfaceData
        {
            public int Size;
            public Guid InterfaceClassGuid;
            public int Flags;
            public IntPtr Reserved;
        }
    }
}