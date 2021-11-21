Imports AlgoritmosExamen2

Public MustInherit Class Decorador
    Implements IComponent
    Protected Property component As IComponent

    Sub New(_componente As IComponent)
        component = _componente
    End Sub

    Public Overridable Function operacion() As String Implements IComponent.operacion
        Return component.operacion()
    End Function
End Class
