Imports System.Text.RegularExpressions

Public Class CuentaVocalesConsonantes
    Public Sub cuentaVocalesConsonan(cadena As String)
        Dim contadorVocales, contadorConsonantes As Integer
        cadena = cadena.ToLower
        cadena = Regex.Replace(cadena, "[^a-z]", "")
        Dim cadena2 As String = ""
        For i = 0 To cadena.Length - 1
            If cadena(i) = "a" Or cadena(i) = "e" Or cadena(i) = "i" Or cadena(i) = "o" Or cadena(i) = "u" Then
                contadorVocales = contadorVocales + 1
            Else
                contadorConsonantes = contadorConsonantes + 1
            End If
        Next
        MsgBox("La cantidad de vocales y consonantes es " & contadorVocales & " :" & contadorConsonantes)
    End Sub
End Class
