Public Class Pastelero
    Public Function pastelero(vector() As Integer)
        Dim contador As Integer = 0
        Dim numero As Integer
        Dim currentNum As Integer
        While True
            currentNum = 0
            contador = contador + 1
            For i = 0 To vector.Length - 1
                If vector(i) < contador Then
                    If contador = 1 Then
                        MsgBox("El numero que no se pudo formar fue:" & currentNum)
                        Return "El numero que no se pudo formar fue:" & currentNum
                    Else
                        For j = 1 To contador
                            numero = numero & currentNum
                        Next
                        MsgBox("El numero que no se pudo formar fue:" & numero)
                        Return "El numero que no se pudo formar fue:" & numero
                    End If
                End If
                currentNum = currentNum + 1
            Next
        End While
        Return "Nunca va llegar aqui."
    End Function
End Class
