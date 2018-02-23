Public Class cKPI_INDICATOR_WIDGETS
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT INDICATORID, IDWIDGET, FECHA, IDTIPO " & _
                                "FROM KPI_INDICATOR_WIDGETS " & _
                                "WHERE 1=1 "

    Private Const strInserta = "kpis.ADD_KPI_INDICATOR_WIDGETS"
    Private Const strElimina = "kpis.DEL_KPI_INDICATOR_WIDGETS"
#End Region

#Region "Variables Privadas"
    Private _INDICATORID As  Nullable(Of Integer)
    Private _IDWIDGET As  Nullable(Of Integer)
    Private _FECHA As Nullable(Of DateTime)
    Private _IDTIPO As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "KPI_INDICATOR_WIDGETS"
        End Get
    End Property
    Public Property indicatorid As  Nullable(Of Integer)
        Get
            Return _INDICATORID
        End Get
        Set(ByVal value As  Nullable(Of Integer))
            _INDICATORID = value
        End Set
    End Property
    Public Property idwidget As  Nullable(Of Integer)
        Get
            Return _IDWIDGET
        End Get
        Set(ByVal value As  Nullable(Of Integer))
            _IDWIDGET = value
        End Set
    End Property
    Public Property fecha As  Nullable(Of DateTime)
        Get
            Return _FECHA
        End Get
        Set(ByVal value As  Nullable(Of DateTime))
            _FECHA = value
        End Set
    End Property
    Public Property idtipo As String
        Get
            Return _IDTIPO
        End Get
        Set(ByVal value As String)
            _IDTIPO = value
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
        Me._INDICATORID = Nothing
        Me._IDWIDGET = Nothing
        Me._FECHA = Nothing
        Me._IDTIPO = String.Empty
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

                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Me._IDWIDGET.HasValue Then
                    .CommandText = .CommandText & " AND A.IDWIDGET = :P_IDWIDGET "
                    .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                End If
                If Not String.IsNullOrEmpty(Me._IDTIPO) Then
                    .CommandText = .CommandText & " AND A.IDTIPO = :P_IDTIPO "
                    .Parameters.Add("P_IDTIPO", NpgsqlDbType.Varchar, 10).Value = Me._IDTIPO
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
                If Not IsDBNull(.GetValue(.GetOrdinal("INDICATORID"))) Then
                    Me._INDICATORID = Convert.ToInt64(.GetValue(.GetOrdinal("INDICATORID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("IDWIDGET"))) Then
                    Me._IDWIDGET = Convert.ToInt64(.GetValue(.GetOrdinal("IDWIDGET")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("FECHA"))) Then
                    Me._FECHA = Convert.ToDateTime(.GetValue(.GetOrdinal("FECHA")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("IDTIPO"))) Then
                    Me._IDTIPO = .GetValue(.GetOrdinal("IDTIPO"))
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
    Public Function bInsertar() As Boolean
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
            Dim SqlInserta As New NpgsqlCommand

            With SqlInserta
                .Connection = dbconexion
                .CommandText = strInserta
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                .Parameters.Add("P_IDTIPO", NpgsqlDbType.Varchar, 10).Value = Me._IDTIPO
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
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
    Public Function bInsertar(ByVal dbConexion As NpgsqlConnection, ByVal dbTransaccion As NpgsqlTransaction) As Boolean
        Dim nResultado As Int64 = 0

        Try
            Dim SqlInserta As New NpgsqlCommand

            With SqlInserta
                .Connection = dbConexion
                .Transaction = dbTransaccion
                .CommandText = strInserta
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                .Parameters.Add("P_IDTIPO", NpgsqlDbType.Varchar, 10).Value = Me._IDTIPO
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            nResultado = -1
        End Try

        If nResultado = 0 Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Function bEliminar() As Boolean
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
            Dim SqlElimina As New NpgsqlCommand

            With SqlElimina
                .Connection = dbconexion
                .CommandText = strElimina
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                .Parameters.Add("P_IDTIPO", NpgsqlDbType.Varchar, 10).Value = Me._IDTIPO
            End With

            SqlElimina.ExecuteNonQuery()
            nResultado = SqlElimina.Parameters("RETURN_VALUE").Value
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Eliminación sobre la Base de Datos." & Chr(13) & _
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
    Public Function bEliminar(ByVal dbConexion As NpgsqlConnection, ByVal dbTransaccion As NpgsqlTransaction) As Boolean
        Dim nResultado As Int64 = 0

        Try
            Dim SqlElimina As New NpgsqlCommand

            With SqlElimina
                .Connection = dbconexion
                .Transaction = dbTransaccion
                .CommandText = strElimina
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                .Parameters.Add("P_IDTIPO", NpgsqlDbType.Varchar, 10).Value = Me._IDTIPO
            End With

            SqlElimina.ExecuteNonQuery()
            nResultado = SqlElimina.Parameters("RETURN_VALUE").Value
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Eliminación sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            nResultado = -1
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

                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Me._IDWIDGET.HasValue Then
                    .CommandText = .CommandText & " AND A.IDWIDGET = :P_IDWIDGET "
                    .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
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

                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Me._IDWIDGET.HasValue Then
                    .CommandText = .CommandText & " AND A.IDWIDGET = :P_IDWIDGET "
                    .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
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
