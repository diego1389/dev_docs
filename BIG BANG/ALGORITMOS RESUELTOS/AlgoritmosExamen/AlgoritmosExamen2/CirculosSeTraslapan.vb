Public Class CirculosSeTraslapan
    Public Function circulosTraslapados(ByVal x1 As Integer,
                                        ByVal y1 As Integer,
                                        ByVal x2 As Integer,
                                        ByVal y2 As Integer,
                                        ByVal r1 As Integer,
                                        ByVal r2 As Integer)

        If (r1 + r2 - calcularDistancia(x1, y1, x2, y2, r1, r2) > 0) Then
            Return True
        Else
            Return False
        End If
        Return True
    End Function

    Public Function calcularDistancia(ByVal x1 As Integer,
                                        ByVal y1 As Integer,
                                        ByVal x2 As Integer,
                                        ByVal y2 As Integer,
                                        ByVal r1 As Integer,
                                        ByVal r2 As Integer)
        Return Math.Sqrt((x2 - x1) ^ 2 + (y2 - y1) ^ 2)
    End Function
End Class
