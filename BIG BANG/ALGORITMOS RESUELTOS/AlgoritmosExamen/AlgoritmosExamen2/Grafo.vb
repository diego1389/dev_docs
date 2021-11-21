Public Class Grafo
    Public Function recorridoProfunidad(nodo As NodoGrafo, ByRef listaVisitados As List(Of Integer))
        For Each hijo In nodo.listaNodos
            If Not listaVisitados.Contains(hijo.valor) Then
                listaVisitados.Add(nodo.valor)
                recorridoProfunidad(hijo, listaVisitados)
            End If
        Next
        Console.WriteLine(nodo.valor)
        Return listaVisitados
    End Function

    Public Sub recorridoAnchura(nodo As NodoGrafo, ByRef visitados As List(Of Integer))
        If nodo Is Nothing Then
            Return
        End If
        Dim cola As New Queue(Of NodoGrafo)
        cola.Enqueue(nodo)
        While cola.Count > 0
            Dim nodoTemp As NodoGrafo = cola.Dequeue
            Console.WriteLine(nodoTemp.valor)
            For Each currentNodo In nodo.listaNodos
                If Not visitados.Contains(currentNodo.valor) Then
                    cola.Enqueue(currentNodo)
                    visitados.Add(currentNodo.valor)
                End If
            Next

        End While

    End Sub
End Class
