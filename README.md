# HE853 Control Project

The HE853 Control project is dedicated to create a Windows SDK for the [HE853 USB dongle](http://service.smartwares.eu/en-us/product/10.036.05/he853-he-comp-usb-netwerk-dongle.aspx) of the Home Easy home automation products since the vendor does not provide one. The SDK consists of GPL licensed tools and a LGPL licensed library that can be used from other applications.

Our target audience are power users and software developers who wish to extend the usage of the HE853 USB dongle beyond the limits of the vendor software.

![Screenshot](https://cloud.githubusercontent.com/assets/13846346/26529320/2db8733e-43be-11e7-8960-a2a4b4a9793b.png)

## Command Line Utility - HE853.Util.exe

```
Usage: HE853.Util <command> <device_code> [/service] [/short]

<command> := ON | OFF | 1..8
<device_code> := 1..6000

/service: use service instead of device
/short: use short command sequence, less compatible

Example: HE853.Util ON 1001
```

## Using the Library - HE853.dll

The library can be used by any .NET language by referencing the HE853.dll assembly from the installation directory or via COM. Please note that the assembly is compiled as Any CPU and works with x86 and x64 platform configurations.

C# Sample Code:

```csharp
IDevice device = new Device();
device.Open();
device.SwitchOn(1001, CommandStyle.Comprehensive);
device.SwitchOff(1001, CommandStyle.Comprehensive);
device.Close();
```

© 2012 Thomas Ascher
