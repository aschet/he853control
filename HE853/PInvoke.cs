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
    
    internal sealed class PInvoke
    {
        public const int FileFlagOverlapped = 0x40000000;
        public const uint GenericRead = 0x80000000;
        public const uint GenericWrite = 0x40000000;

        public static void CloseHandle(ref IntPtr handle)
        {
            if (handle != IntPtr.Zero)
            {
                CloseHandle(handle);
                handle = IntPtr.Zero;
            }
        }

        public static IntPtr CreateFile(string fileName, uint desiredAccess, uint flagsAndAttributes = 0)
        {
            SecurityAttributes security = new SecurityAttributes();
            security.Length = Marshal.SizeOf(security);
            security.SecurityDescriptor = IntPtr.Zero;
            security.InheritHandle = Convert.ToInt32(true);

            return CreateFile(fileName, desiredAccess, 0x3, ref security, 0x3, flagsAndAttributes, IntPtr.Zero);
        }

        public static bool GetHIDHandle(short vendorID, short productID, ref IntPtr hidHandle, ref string devicePath)
        {
            hidHandle = IntPtr.Zero;
            devicePath = string.Empty;

            string[] paths = GetHIDDevicePaths();
            foreach (string path in paths)
            {
                hidHandle = CreateFile(path, 0);

                if (hidHandle != IntPtr.Zero)
                {
                    if (IsHIDProduct(hidHandle, 0x4D9, 0x1357))
                    {
                        devicePath = path;
                        break;
                    }
                    else
                    {
                        CloseHandle(ref hidHandle);
                    }
                }
            }

            return hidHandle != IntPtr.Zero;
        }

        public static int GetHIDOutputReportByteLenght(IntPtr hidHandle)
        {
            IntPtr preparsedData = IntPtr.Zero;
            if (HidD_GetPreparsedData(hidHandle, ref preparsedData) == 0)
            {
                return 0;
            }

            HIDPCaps capabilities = new HIDPCaps();
            HidP_GetCaps(preparsedData, ref capabilities);

            HidD_FreePreparsedData(ref preparsedData);
            return capabilities.OutputReportByteLength;
        }

        [DllImport("hid.dll")]
        public static extern bool HidD_FlushQueue(IntPtr hidDeviceObject);

        [DllImport("hid.dll")]
        public static extern bool HidD_SetOutputReport(IntPtr hidDeviceObject, ref byte reportBuffer, int reportBufferLength);

        private static bool IsHIDProduct(IntPtr hidHandle, short vendorID, short productID)
        {
            HIDDAttributes deviceAttributes = new HIDDAttributes();
            deviceAttributes.Size = Marshal.SizeOf(deviceAttributes);
            if (HidD_GetAttributes(hidHandle, ref deviceAttributes) != 0)
            {
                if ((deviceAttributes.VendorID == vendorID) && (deviceAttributes.ProductID == productID))
                {
                    return true;
                }
            }

            return false;
        }

        private static string[] GetHIDDevicePaths()
        {
            string[] paths = new string[0];
            Guid guid = Guid.Empty;
            HidD_GetHidGuid(ref guid);

            bool done = false;
            int index = 0;
            IntPtr deviceInfoSet = SetupDiGetClassDevs(ref guid, null, IntPtr.Zero, 0x12);

            do
            {
                SPDeviceInterfaceData deviceInterfaceData = new SPDeviceInterfaceData();
                deviceInterfaceData.Size = Marshal.SizeOf(deviceInterfaceData);

                if (SetupDiEnumDeviceInterfaces(deviceInfoSet, IntPtr.Zero, ref guid, index, ref deviceInterfaceData) != 0)
                {
                    /* BUG: The SetupDiGetDeviceInterfaceDetail API is a bitch concerning PInvoke with
                       different platform configurations or Any CPU configuration.
                       This is a hack to make it at least work with x86 configuration. */
                    
                    int bufferSize = 0;
                    SPDeviceInterfaceDetailData deviceInterfaceDetailData = new SPDeviceInterfaceDetailData();
                    SPDeviceInterfaceDetailDataFake fake = new SPDeviceInterfaceDetailDataFake();
                    deviceInterfaceDetailData.Size = Marshal.SizeOf(fake);
                    SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref deviceInterfaceData, ref deviceInterfaceDetailData, 2048, ref bufferSize, IntPtr.Zero);

                    Array.Resize(ref paths, paths.Length + 1);
                    paths[paths.Length - 1] = deviceInterfaceDetailData.DevicePath;
                }
                else
                {
                    done = true;
                }

                ++index;
            }
            while (!done);

            SetupDiDestroyDeviceInfoList(deviceInfoSet);

            return paths;
        }

        [DllImport("kernel32.dll")]
        private static extern int CloseHandle(IntPtr handleObject);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr CreateFile(string fileName, uint desiredAccess, uint shareMode, ref SecurityAttributes securityAttributes, int creationDisposition, uint flagsAndAttributes, IntPtr handleTemplateFile);

        [DllImport("hid.dll")]
        private static extern void HidD_GetHidGuid(ref Guid hidGuid);

        [DllImport("hid.dll")]
        private static extern int HidD_GetAttributes(IntPtr hidDeviceObject, ref HIDDAttributes atributes);

        [DllImport("hid.dll")]
        private static extern int HidD_GetPreparsedData(IntPtr hidDeviceObject, ref IntPtr preparsedData);

        [DllImport("hid.dll")]
        private static extern int HidD_FreePreparsedData(ref IntPtr preparsedData);

        [DllImport("hid.dll")]
        private static extern int HidP_GetCaps(IntPtr preparsedData, ref HIDPCaps capabilities);

        [DllImport("setupapi.dll")]
        private static extern int SetupDiEnumDeviceInterfaces(IntPtr deviceInfoSet, IntPtr deviceInfoData, ref Guid interfaceClassGuid, int memberIndex, ref SPDeviceInterfaceData deviceInterfaceData);

        [DllImport("setupapi.dll")]
        private static extern int SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, string enumerator, IntPtr hwndParent, int flags);
 
        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        private static extern int SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref SPDeviceInterfaceData deviceInterfaceData, ref SPDeviceInterfaceDetailData deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, IntPtr deviceInfoData);

        [StructLayout(LayoutKind.Sequential)]
        private struct SecurityAttributes
        {
            public int Length;
            public IntPtr SecurityDescriptor;
            public int InheritHandle;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HIDPCaps
        {
            public short Usage;
            public short UsagePage;
            public short InputReportByteLength;
            public short OutputReportByteLength;
            public short FeatureReportByteLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            public short[] Reserved;
            public short NumberLinkCollectionNodes;
            public short NumberInputButtonCaps;
            public short NumberInputValueCaps;
            public short NumberInputDataIndices;
            public short NumberOutputButtonCaps;
            public short NumberOutputValueCaps;
            public short NumberOutputDataIndices;
            public short NumberFeatureButtonCaps;
            public short NumberFeatureValueCaps;
            public short NumberFeatureDataIndices;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HIDDAttributes
        {
            public int Size;
            public short VendorID;
            public short ProductID;
            public short VersionNumber;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SPDeviceInterfaceData
        {
            public int Size;
            public Guid InterfaceClassGuid;
            public int Flags;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)] 
        private struct SPDeviceInterfaceDetailData
        {
            public int Size;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)] 
            public string DevicePath;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)] 
        private struct SPDeviceInterfaceDetailDataFake
        {
            public int Size;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
            public string DevicePath;
        }
    }
}
