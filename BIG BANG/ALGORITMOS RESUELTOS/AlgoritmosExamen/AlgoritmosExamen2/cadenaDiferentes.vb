Public Class cadenaDiferentes
    Public Function validaCadena(cadena As String)
        For i = 0 To cadena.Length - 2
            If cadena(i) = cadena(i + 1) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Function eliminaRepetidos(letra As String, letraDos As String, cadena As String)
        Dim cadenaFinal As String = ""
        For i = 0 To cadena.Length - 1
            If cadena(i) = letra Or cadena(i) = letraDos Then
                cadenaFinal = cadenaFinal & cadena(i)
            End If
        Next
        Return cadenaFinal
    End Function

    Public Function mayorCadena(cadena As String)
        Dim diferentes As String = ""
        Dim cadenaTemp As String = ""
        Dim mayor As Integer = -1
        diferentes = String.Join("", cadena.Distinct.ToArray)
        For i = 0 To diferentes.Length - 2
            For j = i + 1 To diferentes.Length - 1
                cadenaTemp = eliminaRepetidos(diferentes(i), diferentes(j), cadena)
                If validaCadena(cadenaTemp) = True Then
                    If mayor = -1 Then
                        mayor = cadenaTemp.Length
                    Else
                        If mayor < cadenaTemp.Length Then
                            mayor = cadenaTemp.Length
                        End If
                    End If
                    End If
            Next
        Next
        Return mayor
    End Function

End Class
