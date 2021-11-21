Public Class Matriz90Grados
    Public Function matriz90Grados(matriz(,) As Integer, matrizTemp(,) As Integer)


        For i = 0 To matriz.GetUpperBound(0)
            For j = 0 To matriz.GetUpperBound(1)
                matrizTemp(j, (matriz.GetUpperBound(0) - i)) = matriz(i, j)
            Next
        Next
        imprimeMatriz90Grados(matrizTemp)
        Return True



    End Function
    Public Sub imprimeMatriz90Grados(matriz(,) As Integer)
        For i = 0 To matriz.GetUpperBound(0)
            For j = 0 To matriz.GetUpperBound(1)
                Console.WriteLine("Matriz en la posicion i , j:" & i & j & " valor :" & matriz(i, j))
            Next
        Next
    End Sub

End Class
