Imports AlgoritmosExamen2

Public Class DecoradorConcreto
    Inherits Decorador

    Public Sub New(_componente As IComponent)
        MyBase.New(_componente)
    End Sub

    Public Overrides Function operacion() As String
        Return MyBase.operacion() + " y escuchando perreo"
    End Function
End Class
