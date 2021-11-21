Imports AlgoritmosExamen2

Public Class DataAcess
    Private objetoConexion As IConexion
    Sub New(_objetoConexion As IConexion)
        Me.objetoConexion = _objetoConexion
    End Sub


    Public Sub EjecutarSP(procedimiento As String)
        Me.objetoConexion.conecta()
        '
        '
        '
        '

        Dim proc = procedimiento
        '
        '
        '
        '
        Me.objetoConexion.ejecutarSP(proc)
    End Sub



End Class
