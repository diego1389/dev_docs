Imports System.Text.RegularExpressions

Public Class Puente
    Public Function validaBase(cadena As String)
        If cadena(0) <> "*" Or cadena(cadena.Length - 1) <> "*" Then
            Return False
        End If
        For i = 1 To cadena.Length - 2
            If cadena(i) = "*" Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Function simetrico(cadena As String)
        If cadena <> StrReverse(cadena) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Function validaCadena(cadena As String)
        cadena = cadena.Replace("*", "")
        Dim vector() As String = cadena.Split("+")
        Dim mitad = (vector.Length - 1) \ 2
        For i = 0 To vector.Length - 1
            If i = mitad Then
                Dim temp = vector(i)
                MsgBox(temp)
                If temp.Length > 3 Then
                    Return False
                End If
            Else
                Dim temp = vector(i)
                MsgBox(temp)
                If temp.Length > 2 Then
                    Return False
                End If
            End If
        Next
        Return True
    End Function
    Public Function cadenaColgante(cadena As String)
        If cadena.Length >= 2 Then
            If validaBase(cadena) = True Then
                If simetrico(cadena) = True Then
                    If validaCadena(cadena) = True Then
                        MsgBox("etoy llegando")
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function


End Class
