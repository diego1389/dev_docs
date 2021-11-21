Imports AlgoritmosExamen2

Public Class DecoradorConcretoDibujoRojo
    Inherits DecoradorDibujo

    Public Sub New(_decoradorDibujo As IDibujar)
        MyBase.New(_decoradorDibujo)
    End Sub

    Public Overrides Function dibujar() As Object
        Return MyBase.dibujar() + " rojo"
    End Function

End Class
