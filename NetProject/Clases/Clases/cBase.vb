Imports Microsoft.VisualBasic
Imports System.Configuration

'Clase Base abstracta para todas las demás
Public Class cBase

#Region "Variables Protegidas"
    Protected daDATOS As New NpgsqlDataAdapter
    Protected Shared _DBConexion As New NpgsqlConnection
    Protected Shared _DBTransaccion As NpgsqlTransaction
#End Region

#Region "Propiedades de la Clase"
    Protected ReadOnly Property DBConexion() As NpgsqlConnection
        Get
            Return _DBConexion
        End Get
    End Property
    Protected ReadOnly Property DBTransaction() As NpgsqlTransaction
        Get
            Return _DBTransaccion
        End Get
    End Property
#End Region

#Region "Funciones Publicas de la clase"
    Public Function bPruebaConexion() As Boolean

        'Prueba la conexion con el servidor de Oracle
        Dim bConecta As Boolean = True

        'Obtiene una cadena de conexion con el servidor de Oracle
        Dim connections As ConnectionStringSettingsCollection = ConfigurationManager.ConnectionStrings

        Dim cnx As New NpgsqlConnection
        cnx.ConnectionString = connections("CustomPostgresConnection").ConnectionString

        Try
            cnx.Open()
        Catch exp As Exception
            bConecta = False
        Finally
            cnx.Close()
            cnx.Dispose()
        End Try

        Return bConecta
    End Function
    Public Function UpdateData(ByVal pdsDataTable As DataTable) As Boolean
        If Not pdsDataTable Is Nothing Then
            Try
                daDATOS.Update(pdsDataTable)
            Catch excp As Exception
                Return False
            End Try
        End If
        Return True
    End Function
#End Region

#Region "Funciones de Conexion a la Base de Datos"
    Public Function ObtenerConexion() As NpgsqlConnection

        'Obtiene una cadena de conexion con el servidor de Oracle
        Dim connections As ConnectionStringSettingsCollection = ConfigurationManager.ConnectionStrings

        Dim cnx As New NpgsqlConnection
        cnx.ConnectionString = connections("CustomPostgresConnection").ConnectionString

        Return cnx

    End Function
    Public Function bAbreConexion() As Boolean

        If _DBConexion.State = ConnectionState.Open Then
            Return True
        ElseIf _DBConexion.State = ConnectionState.Closed Then

            _DBConexion = Me.ObtenerConexion()

            Try
                _DBConexion.Open()
                _DBTransaccion = _DBConexion.BeginTransaction()
            Catch excp As NpgsqlException
                MsgBox("No ha sido posible abrir la conexión con la Base de Datos Oracle. Motivo : " + excp.Message)
                Return False
            End Try

            Return True
        Else
            Return False
        End If
    End Function
    Public Function bCierraConexion() As Boolean
        Try
            _DBConexion.Close()
        Catch excp As NpgsqlException
            MsgBox("No ha sido posible cerrar la conexión con la Base de Datos Oracle. Motivo : " + excp.Message)
            Return False
        End Try
        Return True
    End Function
    Public Sub AceptaOperacion()
        If Not _DBTransaccion.Connection Is Nothing Then
            If _DBTransaccion.Connection.State = ConnectionState.Open Then
                _DBTransaccion.Commit()
            End If
        End If
    End Sub
    Public Sub CancelaOperacion()
        If Not _DBTransaccion.Connection Is Nothing Then
            If _DBTransaccion.Connection.State = ConnectionState.Open Then
                _DBTransaccion.Rollback()
            End If
        End If
    End Sub
#End Region

End Class
