Public Class AcomodarNegativosPositivos

    Public Sub acomodarVector(vector() As Integer)
        Dim colaPositivo As New Queue(Of Integer)
        Dim colaNegativo As New Queue(Of Integer)

        For i = 0 To vector.Length - 1
            If vector(i) > 0 Then
                colaPositivo.Enqueue(vector(i))
            Else
                colaNegativo.Enqueue(vector(i))
            End If
        Next

        Dim vectorFinal(vector.Length - 1) As Integer

        For i = 0 To vector.Length - 1
            If colaPositivo.Count > 0 And colaNegativo.Count > 0 Then
                If i Mod 2 = 0 Then

                    vectorFinal(i) = colaPositivo.Dequeue
                    MsgBox(vectorFinal(i))
                Else
                    vectorFinal(i) = colaNegativo.Dequeue
                    MsgBox(vectorFinal(i))
                End If
            ElseIf colaPositivo.Count > 0 Then
                vectorFinal(i) = colaPositivo.Dequeue

            Else
                vectorFinal(i) = colaNegativo.Dequeue
            End If

        Next

        For i = 0 To vectorFinal.Length - 1
            Console.WriteLine(vectorFinal(i))
        Next
    End Sub
End Class
