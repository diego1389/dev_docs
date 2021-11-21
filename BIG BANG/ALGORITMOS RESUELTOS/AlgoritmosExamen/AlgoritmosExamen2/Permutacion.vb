Public Class Permutacion

    Public Function permuta(ByRef a As String, ByRef b As String, ByRef c As String, ByRef cadena As String,
                            ByRef contador As Integer)

        cadena = cadena & " " & a & b & c
        contador = contador + 1
        If contador = 1 Then
            permuta(a, c, b, cadena, contador)
        ElseIf contador = 2 Then
            permuta(b, a, c, cadena, contador)
        ElseIf contador = 3 Then
            permuta(b, c, a, cadena, contador)
        ElseIf contador = 4 Then
            permuta(c, a, b, cadena, contador)
        ElseIf contador = 5 Then
            permuta(c, b, a, cadena, contador)
        End If
        Return cadena
    End Function

    Public Sub Permurtaciones(vectorPalabras() As String, ByVal j As Integer,
                              limite As Integer, ByRef lista As List(Of String))
        Dim cadenaCombinaciones As String = ""
        Dim letraTemp As String
        If j = 1 Then
            For i = 1 To limite
                cadenaCombinaciones = cadenaCombinaciones & vectorPalabras(i)
            Next
            lista.Add(cadenaCombinaciones)
        Else
            For i = 1 To j

                letraTemp = vectorPalabras(i)
                vectorPalabras(i) = vectorPalabras(j)
                vectorPalabras(j) = letraTemp
                Permurtaciones(vectorPalabras, j - 1, limite, lista)
                letraTemp = vectorPalabras(j)
                vectorPalabras(j) = vectorPalabras(i)
                vectorPalabras(i) = letraTemp
            Next i
        End If
    End Sub

End Class
