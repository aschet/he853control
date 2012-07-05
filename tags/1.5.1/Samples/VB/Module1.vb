Imports System
Imports System.IO
Imports HE853

Module Module1

    Sub Main()
        Dim device As New Device()

        Try
            device.Open()
            device.SwitchOn(1001, CommandStyle.Comprehensive)
            device.SwitchOff(1001, CommandStyle.Comprehensive)
            device.Close()

        Catch exception As FileNotFoundException
            Console.WriteLine(exception.Message)

        Catch exception As IOException
            Console.WriteLine(exception.Message)
            device.Close()

        End Try

    End Sub

End Module
