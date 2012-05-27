Module Module1

    Sub Main()
        Dim device As New HE853.Device()
        If (device.Open()) Then
            device.SwitchOn(1001, false)
            device.SwitchOff(1001, false)
            device.Close()
        End If

    End Sub

End Module
