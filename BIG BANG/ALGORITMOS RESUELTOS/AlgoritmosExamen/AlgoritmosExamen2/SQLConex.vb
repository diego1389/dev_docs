Imports System.Data.SqlClient
Imports AlgoritmosExamen2

Public Class SQLConex
    Implements IConexion
    Private Property sqlConecta As SqlConnection
    Private Property cadena As String
    Sub New(c As String)
        Me.cadena = c
    End Sub

    Public Sub conecta() Implements IConexion.conecta
        Me.sqlConecta = New SqlConnection(Me.cadena)
        Me.sqlConecta.Open()
    End Sub

    Public Sub cierraConexion() Implements IConexion.cierraConexion
        Throw New NotImplementedException()
    End Sub

    Public Sub ejecutarSP(sp As String) Implements IConexion.ejecutarSP
        '
        '
        '
        'ejecutar
    End Sub
End Class
