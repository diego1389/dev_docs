Public Class cocienteResiduo
    Public Sub cocienteResiduo(num1 As Integer, num2 As Integer)
        Dim cociente As Integer = 0
        Dim residuo As Integer = 0
        While num1 >= num2
            num1 = num1 - num2
            cociente = cociente + 1
        End While
        MsgBox("El cociente es:" & cociente & " el residuo es : " & num1)
    End Sub
End Class
