Public Class cKPI_LIBRE
    Inherits cBase
    Implements IDisposable, ITemporal

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT DIA, SEMANA, QUINCENA " & _
                                "FROM KPI_DIAS " & _
                                "WHERE 1=1 "
#End Region

#Region "Variables Privadas"
    Private _DIA As Nullable(Of Integer)
    Private _SEMANA As Nullable(Of Integer)
    Private _QUINCENA As Nullable(Of Integer)

    Private _USERID As Nullable(Of Integer)
    Private _DATASETID As Nullable(Of Integer)
    Private _INDICATORID As Nullable(Of Integer)
#End Region

#Region "Propiedades Publicas"
    Public Property dia As Nullable(Of Integer)
        Get
            Return _DIA
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _DIA = value
        End Set
    End Property
    Public Property semana As Nullable(Of Integer)
        Get
            Return _SEMANA
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _SEMANA = value
        End Set
    End Property
    Public Property quincena As Nullable(Of Integer)
        Get
            Return _QUINCENA
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _QUINCENA = value
        End Set
    End Property

    Public Property userid As Nullable(Of Integer) Implements ITemporal.userid
        Get
            Return _USERID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _USERID = value
        End Set
    End Property
    Public Property datasetid As Nullable(Of Integer) Implements ITemporal.datasetid
        Get
            Return _DATASETID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _DATASETID = value
        End Set
    End Property
    Public Property indicatorid As Nullable(Of Integer) Implements ITemporal.indicatorid
        Get
            Return _INDICATORID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _INDICATORID = value
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
    Public Sub Inicializar() Implements ITemporal.Inicializar
        Me._DIA = Nothing
        Me._SEMANA = Nothing
        Me._QUINCENA = Nothing
        Me._USERID = Nothing
        Me._DATASETID = Nothing
        Me._INDICATORID = Nothing
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

                If Me._DATASETID.HasValue Then
                    .CommandText = .CommandText & " AND DATASETID = :P_DATASETID "
                    .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                End If
                If Me._DIA.HasValue Then
                    .CommandText = .CommandText & " AND DIA = :P_DIA "
                    .Parameters.Add("P_DIA", NpgsqlDbType.Bigint).Value = Me._DIA.Value
                End If
                If Me._SEMANA.HasValue Then
                    .CommandText = .CommandText & " AND SEMANA = :P_SEMANA "
                    .Parameters.Add("P_SEMANA", NpgsqlDbType.Bigint).Value = Me._SEMANA.Value
                End If
                If Me._QUINCENA.HasValue Then
                    .CommandText = .CommandText & " AND QUINCENA = :P_QUINCENA "
                    .Parameters.Add("P_QUINCENA", NpgsqlDbType.Bigint).Value = Me._QUINCENA.Value
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
                If Not IsDBNull(.GetValue(.GetOrdinal("DIA"))) Then
                    Me._DIA = Convert.ToInt64(.GetValue(.GetOrdinal("DIA")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("SEMANA"))) Then
                    Me._SEMANA = Convert.ToInt64(.GetValue(.GetOrdinal("SEMANA")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("QUINCENA"))) Then
                    Me._QUINCENA = Convert.ToInt64(.GetValue(.GetOrdinal("QUINCENA")))
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

                If Me._DIA.HasValue Then
                    .CommandText = .CommandText & " AND DIA = :P_DIA "
                    .Parameters.Add("P_DIA", NpgsqlDbType.Bigint).Value = Me._DIA.Value
                End If
                If Me._SEMANA.HasValue Then
                    .CommandText = .CommandText & " AND SEMANA = :P_SEMANA "
                    .Parameters.Add("P_SEMANA", NpgsqlDbType.Bigint).Value = Me._SEMANA.Value
                End If
                If Me._QUINCENA.HasValue Then
                    .CommandText = .CommandText & " AND QUINCENA = :P_QUINCENA "
                    .Parameters.Add("P_QUINCENA", NpgsqlDbType.Bigint).Value = Me._QUINCENA.Value
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

                If Me._DIA.HasValue Then
                    .CommandText = .CommandText & " AND DIA = :P_DIA "
                    .Parameters.Add("P_DIA", NpgsqlDbType.Bigint).Value = Me._DIA.Value
                End If
                If Me._SEMANA.HasValue Then
                    .CommandText = .CommandText & " AND SEMANA = :P_SEMANA "
                    .Parameters.Add("P_SEMANA", NpgsqlDbType.Bigint).Value = Me._SEMANA.Value
                End If
                If Me._QUINCENA.HasValue Then
                    .CommandText = .CommandText & " AND QUINCENA = :P_QUINCENA "
                    .Parameters.Add("P_QUINCENA", NpgsqlDbType.Bigint).Value = Me._QUINCENA.Value
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

#Region "Funciones temporales"
    Public Function ObtenerValores(ByVal pnDesde As Integer, ByVal pnHasta As Integer) As DataTable Implements ITemporal.ObtenerValores
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
                .CommandText = "SELECT TO_CHAR(Z.FECHA_INI, 'DD/MM/YYYY') FECHA, Z.VALOR FROM (" & _
                               "SELECT ROW_NUMBER() OVER() ORDEN, X.FECHA_INI, X.VALOR FROM " & _
                               "(SELECT FECHA_INI, VALOR " & _
                               "FROM KPI_DATASET_VALUES " & _
                               "WHERE DATASETID = :P_DATASETID " & _
                               "ORDER BY FECHA_INI DESC ) X ) Z WHERE Z.ORDEN BETWEEN :P_DESDE AND :P_HASTA "

                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                .Parameters.Add("P_DESDE", NpgsqlDbType.Bigint).Value = pnDesde
                .Parameters.Add("P_HASTA", NpgsqlDbType.Bigint).Value = pnHasta
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
