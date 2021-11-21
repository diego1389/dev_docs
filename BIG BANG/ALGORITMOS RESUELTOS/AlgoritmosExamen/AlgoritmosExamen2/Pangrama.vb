Imports System.Text.RegularExpressions

Public Class Pangrama
    Public Function eliminaRepetidos(cadena As String)
        cadena = String.Join("", cadena.Distinct.ToArray)
        Return cadena
    End Function
    Public Function validaCaracteres(cadena As String)
        cadena = cadena.ToLower
        cadena = Regex.Replace(cadena, "[^a-z]", "")
        cadena = Regex.Replace(cadena, "ñ", "")
        Return cadena
    End Function

    Public Sub pangrama(cadena As String)
        cadena = eliminaRepetidos(cadena)
        cadena = validaCaracteres(cadena)
        If cadena.Length >= 26 Then
            MsgBox("La palabra " & cadena & " es pangrama" & " tiene:" & cadena.Length & " caracteres")
        End If

    End Sub

End Class
