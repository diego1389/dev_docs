

Public Class SecuenciaCollage

    Public Sub secuenciaN(tempIteracion As Decimal)
        Dim cantIteraciones As Decimal = 0
        Dim tempContador As Decimal = 0
        While (tempIteracion > 1)
            If tempIteracion Mod 2 = 0 Then
                tempIteracion = tempIteracion \ 2
                tempContador = tempContador + 1
            Else
                tempIteracion = tempIteracion * 3 + 1
                tempContador = tempContador + 1
            End If
        End While

        MsgBox(tempContador + 1)
    End Sub
    Public Sub secuencia()
        Dim tempContador As Integer = 0
        Dim bandera As Boolean = False
        Dim tempIteracion As Decimal = 0
        Dim iteracion As Integer = 999999
        Dim numero As Integer = 0

        Dim cantIteraciones As Integer = 0
        For i = iteracion To 0 Step -1
            tempIteracion = i
            While (tempIteracion > 1)
                If tempIteracion Mod 2 = 0 Then
                    tempIteracion = tempIteracion \ 2
                    tempContador = tempContador + 1
                Else
                    tempIteracion = tempIteracion * 3 + 1
                    tempContador = tempContador + 1
                End If
            End While
            If bandera = False Then
                cantIteraciones = tempContador + 1
                bandera = True
                numero = i
            Else
                If (tempContador + 1 > cantIteraciones) Then
                    cantIteraciones = tempContador
                    numero = i
                End If
            End If
            tempContador = 0
        Next


        MsgBox("El numero es :" & numero & " la cantidad de iteraciones son: " & cantIteraciones)
    End Sub

End Class
