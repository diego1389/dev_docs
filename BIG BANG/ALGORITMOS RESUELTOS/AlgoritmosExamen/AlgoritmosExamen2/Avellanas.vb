Imports System.Text.RegularExpressions

Public Class Avellanas
    Public Function cuentaUnos(cadena As String)
        cadena = Regex.Replace(cadena, "[^1]", "")
        Return cadena.Length
    End Function
    Public Function toBinario(num As Integer)
        Dim cadena As String = ""
        Dim residuo As Integer = 0
        While num > 1
            residuo = num Mod 2
            cadena = residuo & cadena
            num = num \ 2
        End While
        cadena = num & cadena
        Return cadena
    End Function


    Public Function chicka(num As Integer)
        num = num - 1
        Dim mayor As Integer = -1
        Dim mitad As Integer = num \ 2
        Dim tempCadena As String = ""
        Dim tempMayor As Integer = 0
        For i = 1 To mitad
            tempCadena = toBinario(i)
            tempMayor = cuentaUnos(tempCadena)
            tempCadena = toBinario(num)
            tempMayor = cuentaUnos(tempCadena) + tempMayor
            If mayor = -1 Then
                mayor = tempMayor
            ElseIf mayor < tempMayor Then
                mayor = tempMayor
            End If
            num = num - 1
        Next
        If mayor < tempMayor Then
            mayor = tempMayor
        End If
        Return mayor
    End Function
End Class
