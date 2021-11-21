Public Class RutasDiarias
    Public Sub rutasAlerta(cadena1 As String, cadena2 As String)
        Dim cadenaMayor As String = ""
        Dim cadenaTemp As String = ""
        For i = 0 To cadena2.Length - 1
            cadenaTemp = cadenaTemp & cadena2(i)
            If cadena1.Contains(cadenaTemp) Then
                If cadenaMayor.Length < cadenaTemp.Length Then
                    cadenaMayor = cadenaTemp
                End If
            Else
                cadenaTemp = cadena2(i)
            End If
        Next
        MsgBox("La cadena mas larga es:" & cadenaMayor)
        cadena2 = cadena2.Replace(cadenaMayor, "")
        MsgBox("Las rutas de alerta son:" & cadena2)
    End Sub

End Class
