Public Class EncontrarPatron
    Public Function encuentraPatron(cadena As String, patron As String)
        cadena = cadena.ToLower
        patron = patron.ToLower
        Dim contadorPatron As Integer = 0
        For i = 0 To cadena.Length
            If cadena(i) = patron(contadorPatron) Then
                contadorPatron = contadorPatron + 1
            Else
                contadorPatron = 0
                If cadena(i) = patron(contadorPatron) Then
                    contadorPatron = contadorPatron + 1
                Else
                    contadorPatron = 0
                End If
            End If
            If (patron.Length = contadorPatron) Then
                MsgBox("La posicion inicio es:" & (i - (patron.Length - 1)) & " la final es:" & i)
                Return "La posicion inicio es:" & (i - (patron.Length - 1)) & " la final es:" & i
            End If
        Next
        MsgBox("No se encontró el patrón")
        Return "No se encontró el patrón"
    End Function
End Class
