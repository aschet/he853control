Module Module1

    Sub Main()
        Dim device As New HE853.Device()
        If (device.Open()) Then
            device.On(1001)
            device.Off(1001)
            device.Close()
        End If

    End Sub

End Module
