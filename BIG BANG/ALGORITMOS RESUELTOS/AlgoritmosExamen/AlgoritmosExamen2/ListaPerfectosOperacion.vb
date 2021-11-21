Public Class ListaPerfectosOperacion
    Public Sub listaperfecto(numero As Integer)
        Dim lista As New List(Of Integer)
        Dim sumaPerfecto, contador As Integer
        contador = 1
        Dim resultado As Integer = 0
        For i = 3 To numero
            sumaPerfecto = i
            While contador < sumaPerfecto
                If sumaPerfecto Mod contador = 0 Then
                    resultado = contador + resultado
                End If
                contador = contador + 1
            End While
            If resultado = sumaPerfecto Then
                lista.Add(sumaPerfecto)
            End If
            contador = 1
            resultado = 0
        Next
        imprimeLista(lista)
    End Sub
    Public Sub imprimeLista(lista As List(Of Integer))
        For i = 0 To lista.Count - 1
            Console.WriteLine("El numero :" & lista(i) & " es perfecto")
        Next
    End Sub
End Class
