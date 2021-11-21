Public Class NumerosRepetidos
    Public Function repetidosNumeros(cadena As String)
        Dim vector() As String = cadena.Split("-")
        For i = 0 To vector.Length - 2
            If vector(i) = vector(i + 1) Then
                MsgBox("Los números repetidos son:" & vector(i) & " y :" & vector(i + 1))
                Return True
            End If
        Next
        MsgBox("No hay números repetidos")
        Return False
    End Function
End Class
