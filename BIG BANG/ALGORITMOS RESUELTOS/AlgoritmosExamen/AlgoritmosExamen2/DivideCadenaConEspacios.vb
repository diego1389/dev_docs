Public Class DivideCadenaConEspacios
    Public Sub dividirCadena(cadena As String, diccionario As List(Of String))
        Dim cadenaFinal As String = ""
        Dim cadenaTemp As String = ""
        For i = 0 To cadena.Length - 1
            cadenaTemp = cadenaTemp & cadena(i)
            If diccionario.Contains(cadenaTemp) Then
                cadenaFinal = cadenaFinal & " " & cadenaTemp
                cadenaTemp = ""
            End If
        Next
        MsgBox(cadenaFinal)
    End Sub
End Class
