Public Class RectanguloTraslapadoOeracion
    Public Function rectanguloTraslapa(supIzq1 As Punto, infDer1 As Punto,
                                       supIzq2 As Punto, infDer2 As Punto)

        Dim Ax1 As Integer = supIzq1.x
        Dim Ax2 As Integer = infDer1.x
        Dim Ay1 As Integer = infDer1.y
        Dim Ay2 As Integer = supIzq1.y

        Dim Bx1 As Integer = supIzq2.x
        Dim Bx2 As Integer = infDer2.x
        Dim By1 As Integer = infDer2.y
        Dim By2 As Integer = supIzq2.y

        If (Ax1 > Bx1 And Ax1 < Bx2) And (Ay2 < By2 And Ay2 > By1) Then
            Return True
        ElseIf (AX1 > BX1 And AX1 < BX2) And (Ay1 > By1 And Ay1 < By2) Then
            Return True
        ElseIf (Ax2 > Bx1 And Ax2 < Bx2) And (Ay1 > By1 And Ay1 < By2) Then
            Return True
        ElseIf (Ax2 > Bx1 And Ax2 < Bx2) And (Ay2 < By2 And Ay2 > By1) Then
            Return True
        End If
        Return False
    End Function
End Class
