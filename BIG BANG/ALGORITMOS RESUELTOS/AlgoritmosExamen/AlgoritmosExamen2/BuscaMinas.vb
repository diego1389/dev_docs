Public Class BuscaMinas
    Public Sub buscaMinas(matriz(,) As String)
        Dim contador As Integer = 0
        For i = 0 To matriz.GetUpperBound(0)
            For j = 0 To matriz.GetUpperBound(0)
                If matriz(i, j) <> "*" Then
                    If (i - 1) >= 0 Then
                        If matriz(i - 1, j) = "*" Then
                            contador = contador + 1
                        End If
                    End If

                    If (i + 1) <= matriz.GetUpperBound(0) Then
                        If matriz(i + 1, j) = "*" Then
                            contador = contador + 1
                        End If
                    End If

                    If (j - 1) >= 0 Then
                        If matriz(i, j - 1) = "*" Then
                            contador = contador + 1
                        End If
                    End If

                    If (j + 1) <= matriz.GetUpperBound(1) Then
                        If matriz(i, j + 1) = "*" Then
                            contador = contador + 1
                        End If
                    End If

                    If (j - 1) >= 0 And (i - 1) >= 0 Then
                        If matriz(i - 1, j) = "*" Then
                            contador = contador + 1
                        End If
                    End If

                    If (j + 1) <= matriz.GetUpperBound(1) And (i - 1) >= 0 Then
                        If matriz(i - 1, j + 1) = "*" Then
                            contador = contador + 1
                        End If
                    End If

                    If (i + 1) <= matriz.GetUpperBound(0) And (j - 1) >= 0 Then
                        If matriz(i + 1, j - 1) = "*" Then
                            contador = contador + 1
                        End If
                    End If

                    If (i + 1) <= matriz.GetUpperBound(0) And (j + 1) <= matriz.GetUpperBound(1) Then
                        If matriz(i + 1, j + 1) = "*" Then
                            contador = contador + 1
                        End If
                    End If

                    If contador > 0 Then
                        matriz(i, j) = contador
                    End If
                End If

                contador = 0
            Next

        Next
        imprimeMatriz(matriz)
    End Sub

    Public Sub imprimeMatriz(matriz(,))
        For i = 0 To matriz.GetUpperBound(0)
            For j = 0 To matriz.GetUpperBound(1)
                If matriz(i, j) <> "*" Then
                    Console.WriteLine("Matriz en la posicion:" & i & " j :" & j & " tiene:" & matriz(i, j) & " minas")
                End If

            Next
        Next
    End Sub
End Class
