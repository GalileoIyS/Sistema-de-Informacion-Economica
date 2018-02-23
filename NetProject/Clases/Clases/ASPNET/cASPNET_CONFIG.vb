Public Class cASPNET_CONFIG
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT CAMPO, VALOR_CADENA, VALOR_NUMERICO, DESCRIPCION " & _
                                "FROM ASPNET_CONFIG " & _
                                "WHERE 1=1 "

    Private Const strModifica = "aspnet.update_aspnet_config"
#End Region

#Region "Variables Privadas"
    Private _CAMPO As String
    Private _VALOR_CADENA As String
    Private _VALOR_NUMERICO As Nullable(Of Decimal)
    Private _DESCRIPCION As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "PARAMETROS"
        End Get
    End Property
    Public Property campo As String
        Get
            Return _CAMPO
        End Get
        Set(ByVal value As String)
            _CAMPO = value
        End Set
    End Property
    Public Property valor_cadena As String
        Get
            Return _VALOR_CADENA
        End Get
        Set(ByVal value As String)
            _VALOR_CADENA = value
        End Set
    End Property
    Public Property valor_numerico As Nullable(Of Decimal)
        Get
            Return _VALOR_NUMERICO
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _VALOR_NUMERICO = value
        End Set
    End Property
    Public Property descripcion As String
        Get
            Return _DESCRIPCION
        End Get
        Set(ByVal value As String)
            _DESCRIPCION = value
        End Set
    End Property
#End Region

#Region "Funciones Publicas de la Clase"
    Public Sub New()
        'Realiza la generación del ejemplar estándar.
        MyBase.New()
        'Inicializa las variables privadas de la clase.
        Inicializar()
    End Sub
    Public Sub Inicializar()
        Me._CAMPO = String.Empty
        Me._VALOR_CADENA = String.Empty
        Me._VALOR_NUMERICO = Nothing
        Me._DESCRIPCION = String.Empty
    End Sub
    Public Function bConsultar() As Boolean
        Dim bOk As Boolean = True
        Dim SqlConsulta As New NpgsqlCommand
        Dim drDATOS As NpgsqlDataReader
        Dim dbconexion As New NpgsqlConnection

        dbconexion = Me.ObtenerConexion()

        'Intentamos abrir la conexión
        Try
            dbconexion.Open()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido conectar con la Base de Datos." & Chr(13) & _
                   "Motivo: " & excp.Message)
            Return False
        End Try

        Try
            With SqlConsulta
                .Connection = dbconexion
                .CommandText = strConsulta
                .CommandType = CommandType.Text

                If Not String.IsNullOrEmpty(Me._CAMPO) Then
                    .CommandText = .CommandText & " AND CAMPO = :P_CAMPO "
                    .Parameters.Add("P_CAMPO", NpgsqlDbType.Varchar, 100).Value = Me._CAMPO
                End If
            End With
            drDATOS = SqlConsulta.ExecuteReader()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido obtener los resultados de la consulta de la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            dbconexion.Close()
            dbconexion.Dispose()
            Return False
        End Try

        With drDATOS
            Me.Inicializar()
            If .Read Then
                If Not IsDBNull(.GetValue(.GetOrdinal("CAMPO"))) Then
                    Me._CAMPO = .GetValue(.GetOrdinal("CAMPO"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("VALOR_CADENA"))) Then
                    Me._VALOR_CADENA = .GetValue(.GetOrdinal("VALOR_CADENA"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("VALOR_NUMERICO"))) Then
                    Me._VALOR_NUMERICO = Convert.ToInt64(.GetValue(.GetOrdinal("VALOR_NUMERICO")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("DESCRIPCION"))) Then
                    Me._DESCRIPCION = .GetValue(.GetOrdinal("DESCRIPCION"))
                End If
            Else
                bOk = False
            End If
        End With

        'Cerramos las Conexiones
        drDATOS.Close()
        dbconexion.Close()

        Return bOk

    End Function
    Public Function bModificar() As Boolean
        Dim dbconexion As New NpgsqlConnection
        Dim nResultado As Int64 = 0

        dbconexion = Me.ObtenerConexion()

        'Intentamos abrir la conexión
        Try
            dbconexion.Open()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido conectar con la Base de Datos." & Chr(13) & _
                   "Motivo: " & excp.Message)
            Return False
        End Try

        Try
            Dim SqlModifica As New NpgsqlCommand

            With SqlModifica
                .Connection = dbconexion
                .CommandText = strModifica
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                .Parameters.Add("P_CAMPO", NpgsqlDbType.Varchar, 100).Value = Me._CAMPO
                .Parameters.Add("P_VALOR_CADENA", NpgsqlDbType.Varchar, 255).Value = Me._VALOR_CADENA
                If Me._VALOR_NUMERICO.HasValue Then
                    .Parameters.Add("P_VALOR_NUMERICO", NpgsqlDbType.Integer).Value = Me._VALOR_NUMERICO.Value
                Else
                    .Parameters.Add("P_VALOR_NUMERICO", NpgsqlDbType.Integer).Value = DBNull.Value
                End If
                .Parameters.Add("P_DESCRIPCION", NpgsqlDbType.Varchar, 500).Value = Me._DESCRIPCION
            End With

            SqlModifica.ExecuteNonQuery()
            nResultado = SqlModifica.Parameters("RETURN_VALUE").Value
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Modificación sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            nResultado = -1
        Finally
            dbconexion.Close()
        End Try

        If nResultado = 0 Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Function ObtenerDatos() As DataTable
        Dim dbconexion As New NpgsqlConnection
        Dim _dtData As New DataTable

        'Limpiamos el contenido de la Tabla de Datos
        _dtData.Clear()

        dbconexion = Me.ObtenerConexion()

        'Intentamos abrir la conexión
        Try
            dbconexion.Open()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido conectar con la Base de Datos." & Chr(13) & _
                   "Motivo: " & excp.Message)
            Return Nothing
        End Try

        Try
            Dim SqlConsulta As New NpgsqlCommand
            Dim daData As New NpgsqlDataAdapter

            With SqlConsulta
                .Connection = dbconexion
                .CommandText = strConsulta
                .CommandType = CommandType.Text

                If Not String.IsNullOrEmpty(Me.campo) Then
                    .CommandText = .CommandText & " AND CAMPO = :P_CAMPO "
                    .Parameters.Add("P_CAMPO", NpgsqlDbType.Varchar, 100).Value = Me._CAMPO
                End If
            End With

            daData.SelectCommand = SqlConsulta
            daData.Fill(_dtData)

        Catch excp As NpgsqlException
            MsgBox("No se ha podido obtener los resultados de la consulta de la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return _dtData

    End Function
    Public Function ObtenerDatos(ByVal psFiltro As String) As DataTable
        Dim dbconexion As New NpgsqlConnection
        Dim _dtData As New DataTable

        'Limpiamos el contenido de la Tabla de Datos
        _dtData.Clear()

        dbconexion = Me.ObtenerConexion()

        'Intentamos abrir la conexión
        Try
            dbconexion.Open()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido conectar con la Base de Datos." & Chr(13) & _
                   "Motivo: " & excp.Message)
            Return Nothing
        End Try

        Try
            Dim SqlConsulta As New NpgsqlCommand
            Dim daData As New NpgsqlDataAdapter

            With SqlConsulta
                .Connection = dbconexion
                .CommandType = CommandType.Text
                .CommandText = strConsulta & psFiltro

                If Not String.IsNullOrEmpty(Me.campo) Then
                    .CommandText = .CommandText & " AND CAMPO = :P_CAMPO "
                    .Parameters.Add("P_CAMPO", NpgsqlDbType.Varchar, 100).Value = Me._CAMPO
                End If
            End With

            daData.SelectCommand = SqlConsulta
            daData.Fill(_dtData)

        Catch excp As NpgsqlException
            MsgBox("No se ha podido obtener los resultados de la consulta de la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return _dtData

    End Function
#End Region

#Region "Soporte a la Interfaz IDisposable "
    Private disposedValue As Boolean = False        ' Para detectar llamadas redundantes 

    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: Liberar recursos administrados cuando se llamen explícitamente
            End If

            ' TODO: Liberar recursos no administrados compartidos
        End If
        Me.disposedValue = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' No cambie este código. Coloque el código de limpieza en Dispose (ByVal que se dispone como Boolean).
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
