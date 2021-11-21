Imports AlgoritmosExamen2

Public Class Component
    Implements IComponent

    Public Overridable Function operacion() As String Implements IComponent.operacion
        Return "I am walking"
    End Function
End Class
