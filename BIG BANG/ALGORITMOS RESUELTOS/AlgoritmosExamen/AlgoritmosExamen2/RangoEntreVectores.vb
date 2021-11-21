Public Class RangoEntreVectores


    Public Function rangoMenor(vector1() As Rango, vector2() As Rango, vector3() As Rango)
        Dim lista As New List(Of Rango)
        Dim tempRango As Integer = -1
        Dim rango As String =""
        lista = creaUnaUnicaLista(vector3, vector2)
        lista = quicksort(lista, 0, lista.Count - 1)

 

        For i = 0 To vector1.Length - 1
            For j = 0 To lista.Count - 1
                If j + 1 <= lista.Count - 1 Then
                    If (lista(j).vectorNombre <> lista(j + 1).vectorNombre) Then
                        If (vector1(i).valor <= lista(j).valor) Then
                            If tempRango = -1 Then
                                tempRango = lista(j + 1).valor - vector1(i).valor
                                rango = formaString(vector1(i).valor, vector1(i).vectorNombre, lista(j).valor,
                                                    lista(j).vectorNombre, lista(j + 1).valor,
                                                    lista(j + 1).vectorNombre)

                            ElseIf tempRango > lista(j + 1).valor - vector1(i).valor Then
                                tempRango = lista(j + 1).valor - vector1(i).valor

                                rango = formaString(vector1(i).valor, vector1(i).vectorNombre, lista(j).valor,
                                                    lista(j).vectorNombre, lista(j + 1).valor,
                                                    lista(j + 1).vectorNombre)

                            End If
                        ElseIf (vector1(i).valor >= lista(j).valor And vector1(i).valor <= lista(j + 1).valor) Then
                            If tempRango = -1 Then

                                tempRango = lista(j + 1).valor - lista(j).valor
                                rango = formaString(lista(j).valor, lista(j).vectorNombre,
                                                    vector1(i).valor, vector1(i).vectorNombre,
                                                    lista(j + 1).valor, lista(j + 1).vectorNombre)
                            ElseIf tempRango > lista(j + 1).valor - lista(j).valor Then

                                rango = formaString(lista(j).valor, lista(j).vectorNombre,
                                                    vector1(i).valor, vector1(i).vectorNombre,
                                                    lista(j + 1).valor, lista(j + 1).vectorNombre)
                            End If
                        ElseIf vector1(i).valor >= lista(j + 1).valor Then
                            If tempRango = -1 Then
                                tempRango = vector1(i).valor - lista(j).valor
                                rango = formaString(lista(j).valor, lista(j).vectorNombre,
                                                   lista(j + 1).valor, lista(j + 1).vectorNombre,
                                                   vector1(i).valor, lista(i).vectorNombre
                                                   )
                            ElseIf tempRango > vector1(i).valor - lista(j).valor Then
                                tempRango = vector1(i).valor - lista(j).valor
                                rango = formaString(lista(j).valor, lista(j).vectorNombre,
                                                   lista(j + 1).valor, lista(j + 1).vectorNombre,
                                                   vector1(i).valor, lista(i).vectorNombre
                                                   )
                            End If
                        End If
                    End If
                End If
            Next
        Next
        Return rango
    End Function

    Public Function formaString(menor As Integer, menorNombre As String, medio As Integer, medioNombre As String,
                               mayor As Integer, mayorNombre As String)
        Dim rango As String = ""
        rango = "El rango es [" & menor & "-" & mayor & " ]"
        rango = rango & " los numeros que lo conforman son :"
        rango = rango & menor & " contenido en el vector:" & menorNombre
        rango = rango & " y :" & medio & " contenido en el vector:" & medioNombre
        rango = rango & " y :" & mayor & " contenido en el vector:" & mayorNombre
        Return rango
    End Function

    Public Function busquedaBinaria(arreglo() As Rango, valor As Integer)
        Dim inicio, final, posicion As Integer
        inicio = 0
        final = arreglo.Length - 1
        While inicio <= final
            posicion = (inicio + final) / 2
            If arreglo(posicion).valor = valor Then
                Return True
            ElseIf arreglo(inicio).valor < valor Then
                inicio = posicion + 1
            Else
                final = posicion - 1
            End If
        End While
        Return False
    End Function

    Public Function quicksort(vector As List(Of Rango), inicio As Integer, final As Integer)
        Dim tempInicio, tempFinal, tempCambio, pivote As Integer
        tempInicio = inicio
        tempFinal = final
        pivote = vector((inicio + final) / 2).valor
        Dim tempCambioString As String = ""
        While tempInicio <= tempFinal
            While (vector(tempInicio).valor < pivote And tempInicio < final)
                tempInicio = tempInicio + 1
            End While

            While (vector(tempFinal).valor > pivote And tempFinal > inicio)
                tempFinal = tempFinal - 1
            End While

            If tempInicio <= tempFinal Then
                tempCambio = vector(tempInicio).valor
                tempCambioString = vector(tempInicio).vectorNombre

                vector(tempInicio).valor = vector(tempFinal).valor
                vector(tempInicio).vectorNombre = vector(tempFinal).vectorNombre

                vector(tempFinal).valor = tempCambio
                vector(tempFinal).vectorNombre = tempCambioString
                tempFinal = tempFinal - 1
                tempInicio = tempInicio + 1
            End If
            If inicio < tempFinal Then quicksort(vector, inicio, tempFinal)
            If tempInicio < final Then quicksort(vector, tempInicio, final)
        End While

        Return vector
    End Function


    Public Function quicksortMayorAMenor(vector As List(Of Rango), inicio As Integer, final As Integer)
        Dim tempInicio, tempFinal, tempCambio, pivote As Integer
        tempInicio = inicio
        tempFinal = final
        pivote = vector((inicio + final) / 2).valor
        Dim tempCambioString As String = ""
        While tempInicio <= tempFinal
            While (vector(tempInicio).valor > pivote And tempInicio < final)
                tempInicio = tempInicio + 1
            End While

            While (vector(tempFinal).valor < pivote And tempFinal > inicio)
                tempFinal = tempFinal - 1
            End While

            If tempInicio <= tempFinal Then
                tempCambio = vector(tempInicio).valor
                tempCambioString = vector(tempInicio).vectorNombre

                vector(tempInicio).valor = vector(tempFinal).valor
                vector(tempInicio).vectorNombre = vector(tempFinal).vectorNombre

                vector(tempFinal).valor = tempCambio
                vector(tempFinal).vectorNombre = tempCambioString
                tempFinal = tempFinal - 1
                tempInicio = tempInicio + 1
            End If
            If inicio < tempFinal Then quicksortMayorAMenor(vector, inicio, tempFinal)
            If tempInicio < final Then quicksortMayorAMenor(vector, tempInicio, final)
        End While

        Return vector
    End Function

    Public Sub validaVectorMayor(vector() As Rango, vector2() As Rango, vector3() As Rango)
        If vector.Length >= vector2.Length And vector.Length >= vector3.Length Then
            rangoMenor(vector, vector2, vector3)
        ElseIf vector2.Length >= vector.Length And vector2.Length >= vector3.Length Then
            rangoMenor(vector2, vector, vector3)
        Else
            rangoMenor(vector3, vector, vector2)

        End If
    End Sub

    Public Function creaUnaUnicaLista(vector() As Rango, vector2() As Rango)
        Dim lista As New List(Of Rango)
        For i = 0 To vector.Length - 1
            lista.Add(vector(i))
            If (vector2.Length - 1) >= i Then
                lista.Add(vector2(i))
            End If
        Next
        Return lista
    End Function

End Class
