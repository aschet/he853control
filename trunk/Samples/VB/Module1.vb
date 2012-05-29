Imports System
Imports System.IO

Module Module1

    Sub Main()
        Dim device As New HE853.Device()

        Try
            device.Open()
            device.SwitchOn(1001, False)
            device.SwitchOff(1001, False)
            device.Close()

        Catch exception As FileNotFoundException
            Console.WriteLine(exception.Message)

        Catch exception As IOException
            Console.WriteLine(exception.Message)
            device.Close()

        End Try

    End Sub

End Module
