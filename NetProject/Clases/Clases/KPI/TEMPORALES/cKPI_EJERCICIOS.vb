Public Class cKPI_EJERCICIOS
    Inherits cBase
    Implements IDisposable, ITemporal

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT EJERCICIO " & _
                                "FROM KPI_EJERCICIOS " & _
                                "WHERE 1=1 "
#End Region

#Region "Variables Privadas"
    Private _EJERCICIO As Nullable(Of Integer)

    Private _USERID As Nullable(Of Integer)
    Private _DATASETID As Nullable(Of Integer)
    Private _INDICATORID As Nullable(Of Integer)
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "KPI_EJERCICIOS"
        End Get
    End Property
    Public Property ejercicio As Nullable(Of Integer)
        Get
            Return _EJERCICIO
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _EJERCICIO = value
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
        Me._EJERCICIO = Nothing
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

                If Me._EJERCICIO.HasValue Then
                    .CommandText = .CommandText & " AND EJERCICIO = :P_EJERCICIO "
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = Me._EJERCICIO.Value
                End If
                If Me._DATASETID.HasValue Then
                    .CommandText = .CommandText & " AND DATASETID = :P_DATASETID "
                    .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
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
                If Not IsDBNull(.GetValue(.GetOrdinal("EJERCICIO"))) Then
                    Me._EJERCICIO = Convert.ToInt64(.GetValue(.GetOrdinal("EJERCICIO")))
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

                If Me._EJERCICIO.HasValue Then
                    .CommandText = .CommandText & " AND EJERCICIO = :P_EJERCICIO "
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = Me._EJERCICIO.Value
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

                If Me._EJERCICIO.HasValue Then
                    .CommandText = .CommandText & " AND EJERCICIO = :P_EJERCICIO "
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = Me._EJERCICIO.Value
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
                .CommandText = "SELECT Z.EJERCICIO, Z.VALOR FROM (" & _
                               "SELECT ROW_NUMBER() OVER() ORDEN, X.EJERCICIO, X.VALOR FROM " & _
                               "(SELECT A.EJERCICIO, B.VALOR " & _
                               "FROM (SELECT EJERCICIO FROM KPI_EJERCICIOS WHERE TO_DATE('01/' || EJERCICIO, 'MM/YYYY') <= CURRENT_DATE) A " & _
                               "LEFT JOIN KPI_DATASET_VALUES B ON A.EJERCICIO = B.EJERCICIO " & _
                               "AND B.DATASETID = :P_DATASETID " & _
                               "ORDER BY A.EJERCICIO DESC ) X ) Z WHERE Z.ORDEN BETWEEN :P_DESDE AND :P_HASTA "

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
