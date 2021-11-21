Public Class Adn
    Public Sub cadenaAdn(cadena As String, cadena2 As String)
        Dim tempCadena As String = ""
        Dim cadenaMayor As String = ""
        For i = 0 To cadena2.Length - 1
            tempCadena = tempCadena & cadena2(i)
            If cadena.Contains(tempCadena) Then
                If cadenaMayor.Length < tempCadena.Length Then
                    cadenaMayor = tempCadena
                End If
            Else
                tempCadena = tempCadena.Substring(0, tempCadena.Length - 1)
                If cadenaMayor.Length < tempCadena.Length Then
                    cadenaMayor = tempCadena
                End If
                tempCadena = cadena2(i)
            End If
        Next
        MsgBox("La cadena de adn mas grande es: " & cadenaMayor)
    End Sub
End Class
