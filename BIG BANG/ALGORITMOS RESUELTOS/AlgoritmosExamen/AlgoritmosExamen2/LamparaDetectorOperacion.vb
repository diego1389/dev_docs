Public Class LamparaDetectorOperacion
    Function distanciaLampara(lampara As lampara)
        Return lampara.x * lampara.y
    End Function
    Public Function distanciaDetector(detector As Detector)
        Return detector.x * detector.y
    End Function
    Function anguloLampara(lampara As lampara)
        Return lampara.x * lampara.y
    End Function
    Public Function anguloDetector(detector As Detector)
        Return detector.x * detector.y
    End Function

    Public Function intensidadRef(fotones As Double, dLampara As Integer, dDetector As Integer)
        Return fotones \ ((dLampara + dDetector) ^ 2)
    End Function
    Public Sub detectores(listaLamp As List(Of lampara), listaDec As List(Of Detector))
        Dim distanciaLamp As Integer
        Dim distanciaDet As Integer
        Dim intensidadReflejada As Double
        Dim listaFinal As New List(Of Detector)
        For Each lampara In listaLamp
            For Each detector In listaDec
                If anguloDetector(detector) = anguloLampara(lampara) Then
                    distanciaLamp = distanciaLampara(lampara)
                    distanciaDet = distanciaDetector(detector)
                    intensidadReflejada = intensidadRef(lampara.fotones, distanciaLamp, distanciaDet)
                    If detector.intensidad = intensidadReflejada Then
                        listaFinal.Add(detector)
                    End If
                End If

            Next
        Next
    End Sub


End Class
