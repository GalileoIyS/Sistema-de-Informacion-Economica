Public Class cKPI_MESES
    Inherits cBase
    Implements IDisposable, ITemporal

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT MES, TRIMESTRE, SEMESTRE, NOMBRE " & _
                                "FROM KPI_MESES " & _
                                "WHERE 1=1 "
#End Region

#Region "Variables Privadas"
    Private _MES As Nullable(Of Integer)
    Private _TRIMESTRE As  Nullable(Of Integer)
    Private _SEMESTRE As  Nullable(Of Integer)
    Private _NOMBRE As String

    Private _USERID As Nullable(Of Integer)
    Private _DATASETID As Nullable(Of Integer)
    Private _INDICATORID As Nullable(Of Integer)
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "KPI_MESES"
        End Get
    End Property
    Public Property mes As Nullable(Of Integer)
        Get
            Return _MES
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _MES = value
        End Set
    End Property
    Public Property trimestre As  Nullable(Of Integer)
        Get
            Return _TRIMESTRE
        End Get
        Set(ByVal value As  Nullable(Of Integer))
            _TRIMESTRE = value
        End Set
    End Property
    Public Property semestre As  Nullable(Of Integer)
        Get
            Return _SEMESTRE
        End Get
        Set(ByVal value As  Nullable(Of Integer))
            _SEMESTRE = value
        End Set
    End Property
    Public Property nombre As String
        Get
            Return _NOMBRE
        End Get
        Set(ByVal value As String)
            _NOMBRE = value
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
        Me._MES = Nothing
        Me._TRIMESTRE = Nothing
        Me._SEMESTRE = Nothing
        Me._NOMBRE = String.Empty
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
                If Me._MES.HasValue Then
                    .CommandText = .CommandText & " AND MES = :P_MES "
                    .Parameters.Add("P_MES", NpgsqlDbType.Bigint).Value = Me._MES.Value
                End If
                If Me._TRIMESTRE.HasValue Then
                    .CommandText = .CommandText & " AND TRIMESTRE = :P_TRIMESTRE "
                    .Parameters.Add("P_TRIMESTRE", NpgsqlDbType.Bigint).Value = Me._TRIMESTRE.Value
                End If
                If Me._SEMESTRE.HasValue Then
                    .CommandText = .CommandText & " AND SEMESTRE = :P_SEMESTRE "
                    .Parameters.Add("P_SEMESTRE", NpgsqlDbType.Bigint).Value = Me._SEMESTRE.Value
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
                If Not IsDBNull(.GetValue(.GetOrdinal("MES"))) Then
                    Me._MES = Convert.ToInt64(.GetValue(.GetOrdinal("MES")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("TRIMESTRE"))) Then
                    Me._TRIMESTRE = Convert.ToInt64(.GetValue(.GetOrdinal("TRIMESTRE")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("SEMESTRE"))) Then
                    Me._SEMESTRE = Convert.ToInt64(.GetValue(.GetOrdinal("SEMESTRE")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("NOMBRE"))) Then
                    Me._NOMBRE = .GetValue(.GetOrdinal("NOMBRE"))
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

                If Me._MES.HasValue Then
                    .CommandText = .CommandText & " AND MES = :P_MES "
                    .Parameters.Add("P_MES", NpgsqlDbType.Bigint).Value = Me._MES.Value
                End If
                If Me._TRIMESTRE.HasValue Then
                    .CommandText = .CommandText & " AND TRIMESTRE = :P_TRIMESTRE "
                    .Parameters.Add("P_TRIMESTRE", NpgsqlDbType.Bigint).Value = Me._TRIMESTRE.Value
                End If
                If Me._SEMESTRE.HasValue Then
                    .CommandText = .CommandText & " AND SEMESTRE = :P_SEMESTRE "
                    .Parameters.Add("P_SEMESTRE", NpgsqlDbType.Bigint).Value = Me._SEMESTRE.Value
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

                If Me._MES.HasValue Then
                    .CommandText = .CommandText & " AND MES = :P_MES "
                    .Parameters.Add("P_MES", NpgsqlDbType.Bigint).Value = Me._MES.Value
                End If
                If Me._TRIMESTRE.HasValue Then
                    .CommandText = .CommandText & " AND TRIMESTRE = :P_TRIMESTRE "
                    .Parameters.Add("P_TRIMESTRE", NpgsqlDbType.Bigint).Value = Me._TRIMESTRE.Value
                End If
                If Me._SEMESTRE.HasValue Then
                    .CommandText = .CommandText & " AND SEMESTRE = :P_SEMESTRE "
                    .Parameters.Add("P_SEMESTRE", NpgsqlDbType.Bigint).Value = Me._SEMESTRE.Value
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
                .CommandText = "SELECT Z.EJERCICIO, Z.MES, Z.NOMBRE_MES, Z.VALOR FROM ( " & _
                               "SELECT ROW_NUMBER() OVER() ORDEN, X.EJERCICIO, X.MES, X.NOMBRE_MES, X.VALOR FROM ( " & _
                               "SELECT A.EJERCICIO, A.MES, A.NOMBRE NOMBRE_MES, B.VALOR " & _
                               "FROM (SELECT EJERCICIO, MES, NOMBRE FROM KPI_EJERCICIOS, KPI_MESES WHERE TO_DATE (MES || '/' || EJERCICIO, 'MM/YYYY') <= CURRENT_DATE) A " & _
                               "LEFT JOIN KPI_DATASET_VALUES B ON A.EJERCICIO = B.EJERCICIO AND A.MES = B.MES " & _
                               "AND B.DATASETID = :P_DATASETID " & _
                               "ORDER BY A.EJERCICIO DESC, A.MES DESC ) X ) Z WHERE Z.ORDEN BETWEEN :P_DESDE AND :P_HASTA "

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
