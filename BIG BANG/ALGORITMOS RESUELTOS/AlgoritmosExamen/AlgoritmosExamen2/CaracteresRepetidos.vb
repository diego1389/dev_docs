Public Class CaracteresRepetidos
    Public Function eliminaReptidos(cadena As String)
        Dim cadenaFinal As String = ""
        Dim contador As Integer = 0
        cadenaFinal = cadena(0)
        For i = 1 To cadena.Length - 1
            If cadena(i) <> cadenaFinal(contador) Then
                cadenaFinal = cadenaFinal & cadena(i)
                contador = contador + 1
            End If
        Next
        MsgBox("La cadena final es:" & cadenaFinal)
        Return cadenaFinal
    End Function
End Class
