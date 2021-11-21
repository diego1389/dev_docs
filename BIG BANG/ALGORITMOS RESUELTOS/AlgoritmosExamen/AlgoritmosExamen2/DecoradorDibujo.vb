Imports AlgoritmosExamen2

Public MustInherit Class DecoradorDibujo
    Implements IDibujar
    Protected Property decoradorDibujo As IDibujar
    Sub New(_decoradorDibujo As IDibujar)
        Me.decoradorDibujo = _decoradorDibujo
    End Sub
    Public Overridable Function dibujar() As Object Implements IDibujar.dibujar
        Return decoradorDibujo.dibujar
    End Function
End Class
