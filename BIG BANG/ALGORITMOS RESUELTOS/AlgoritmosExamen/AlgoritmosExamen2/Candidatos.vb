Public Class Candidatos
    Public Function candidatos(lista As List(Of Candidato))
        Dim combatiente As Integer = 0
        Dim combatiente2 As Integer = 0
        For i = 0 To lista.Count - 2
            For j = (i + 1) To lista.Count - 1
                While combatiente = combatiente2
                    combatiente = Rnd() * 13
                    combatiente2 = Rnd() * 13
                    If combatiente > combatiente2 Then
                        lista(i).puntaje = lista(i).puntaje + 1
                    ElseIf combatiente2 > combatiente Then
                        lista(j).puntaje = lista(j).puntaje + 1
                    End If
                End While
                combatiente = 0
                combatiente2 = 0
            Next
        Next
        Return lista
    End Function
End Class
