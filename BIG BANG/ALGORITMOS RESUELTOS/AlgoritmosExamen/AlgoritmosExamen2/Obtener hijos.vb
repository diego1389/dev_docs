Public Class Obtener_hijos

    Public Function obtenerHojas(raiz As Nodo, ByRef cadena As String)
        If raiz IsNot Nothing Then
            If raiz.izquierda Is Nothing And raiz.derecha Is Nothing Then
                cadena = "  Hoja:" & raiz.valor & cadena
            End If
            obtenerHojas(raiz.izquierda, cadena)
            obtenerHojas(raiz.derecha, cadena)
        End If
        Return cadena
    End Function
End Class
