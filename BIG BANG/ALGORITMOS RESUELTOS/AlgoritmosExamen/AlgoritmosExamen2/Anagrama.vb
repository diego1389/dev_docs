Public Class Anagrama
    Public Function anagrama(cadena As String, cadena2 As String)
        If cadena.Length <> cadena2.Length Then Return False

        Dim vector As Char() = cadena.ToCharArray()
        Dim vector2 As Char() = cadena2.ToCharArray()

            Array.Sort(vector)
            Array.Sort(vector2)

            Dim string1 = String.Join("", vector)
            Dim string2 = String.Join("", vector2)

            Return string1 = string2

    End Function
End Class
