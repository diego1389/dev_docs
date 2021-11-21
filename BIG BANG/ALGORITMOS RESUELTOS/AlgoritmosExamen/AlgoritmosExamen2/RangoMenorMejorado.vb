Public Class RangoMenorMejorado

    Public Function rangoMenorMejorado(vector1() As Rango, vector2() As Rango, vector3() As Rango)
        Dim lista As New List(Of Rango)
        lista = generaArreglo(vector2, vector3)
        lista = lista.OrderBy(Function(x) x.valor).ToList

        Dim rangoResultado As String = ""
        Dim rangoMenor As Integer = -1

        Dim rangoTempResultado As String = ""
        Dim rangoTempMenor As Integer = 0

        Dim bandera As Boolean = False
        For i = 0 To vector1.Length - 1
            For j = 0 To lista.Count - 2
                If lista(j).vectorNombre <> lista(j + 1).vectorNombre Then
                    If vector1(i).valor >= lista(j + 1).valor Then
                        rangoTempMenor = vector1(i).valor - lista(j).valor
                        rangoTempResultado = formaRango(vector1(i).valor,
                                                          vector1(i).vectorNombre,
                                                          lista(j + 1).valor,
                                                          lista(j + 1).vectorNombre,
                                                          lista(j).valor,
                                                          lista(j).vectorNombre)

                    ElseIf vector1(i).valor >= lista(j).valor Then
                        rangoTempMenor = lista(j + 1).valor - lista(j).valor
                        rangoTempResultado = formaRango(lista(j + 1).valor,
                                                                  lista(j + 1).vectorNombre,
                                                                  vector1(i).valor,
                                                                  vector1(i).vectorNombre,
                                                                  lista(j).valor,
                                                                  lista(j).vectorNombre)
                    Else
                        rangoTempMenor = lista(j + 1).valor - vector1(i).valor
                        rangoTempResultado = formaRango(lista(j + 1).valor,
                                                              lista(j + 1).vectorNombre,
                                                              lista(j).valor,
                                                              lista(j).vectorNombre,
                                                              vector1(i).valor,
                                                              vector1(i).vectorNombre)
                    End If
                    If rangoMenor = -1 And bandera = False Then
                        rangoMenor = rangoTempMenor
                        rangoResultado = rangoTempResultado
                        bandera = True
                    ElseIf rangoMenor <> -1 And bandera = True Then
                        If rangoMenor > rangoTempMenor Then
                            rangoMenor = rangoTempMenor
                            rangoResultado = rangoTempResultado
                        End If

                    End If

                End If
            Next
        Next
        MsgBox(rangoResultado)
        Return rangoResultado

    End Function
    Public Sub vectorMasGrande(vector1() As Rango, vector2() As Rango,
                               vector3() As Rango)
        If vector1.Length >= vector2.Length And vector1.Length >= vector3.Length Then
            If vector2.Length >= vector3.Length Then
                rangoMenorMejorado(vector1, vector2, vector3)
            Else
                rangoMenorMejorado(vector1, vector3, vector2)
            End If
        ElseIf vector2.Length >= vector1.Length And vector2.Length >= vector3.Length Then
            If vector1.Length >= vector3.Length Then
                rangoMenorMejorado(vector2, vector1, vector3)
            Else
                rangoMenorMejorado(vector2, vector3, vector1)
            End If
        ElseIf vector3.Length >= vector1.Length And vector3.Length >= vector2.Length Then
            If vector1.Length >= vector2.Length Then
                rangoMenorMejorado(vector3, vector1, vector2)
            Else
                rangoMenorMejorado(vector3, vector2, vector1)
            End If
        End If
    End Sub
    Public Function generaArreglo(vector1() As Rango, vector2() As Rango)
        Dim lista As New List(Of Rango)
        For i = 0 To vector1.Length - 1
            lista.Add(vector1(i))
            If (vector2.Length - 1) >= i Then
                lista.Add(vector2(i))
            End If
        Next
        Return lista
    End Function
    Public Function formaRango(mayor As Integer, mayorNombre As String, medio As Integer, medioNombre As String, menor As Integer, menorNombre As String)
        Return "El rango esta formado por [" & mayor & "-" & menor & " ]" & " el numero " & mayor & " esta en el vector:" & mayorNombre & " el numero:" & medio & " esta en el vector:" & medioNombre & " el numero:" & menor & " esta en el vector :" & menorNombre

    End Function
End Class
