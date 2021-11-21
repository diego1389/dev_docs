Public Class PalindromoPrimo
    Public Function primo(num As Integer)
        Dim raiz As Integer = Math.Sqrt(num)
        If num > 2 Then
            For i = 2 To raiz
                If num Mod i = 0 Then
                    Return False
                End If
            Next
        ElseIf num < 2 Then
            Return False
        End If
        Return True
    End Function

    Public Function palindromo(num As Integer)

        If num.ToString = (StrReverse(num.ToString)) Then
            Return True
        End If
        Return False
    End Function

    Public Function menorPalindromoPrimo(num)
        While True
            num = num + 1
            If primo(num) = True Then
                If palindromo(num) = True Then
                    MsgBox("El numero es" & num)
                    Return num
                End If
            End If

        End While
        Return 0
    End Function

End Class
