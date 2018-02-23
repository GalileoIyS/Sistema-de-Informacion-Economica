Public Class cKPI_INDICATOR_TIPOS
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT IDTIPO, ORDEN, NOMBRE " & _
                                "FROM KPI_INDICATOR_TIPOS " & _
                                "WHERE 1=1 "
#End Region

#Region "Variables Privadas"
    Private _IDTIPO As String
    Private _ORDEN As  Nullable(Of Integer)
    Private _NOMBRE As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "KPI_INDICATOR_TIPOS"
        End Get
    End Property
    Public Property idtipo As String
        Get
            Return _IDTIPO
        End Get
        Set(ByVal value As String)
            _IDTIPO = value
        End Set
    End Property
    Public Property orden As  Nullable(Of Integer)
        Get
            Return _ORDEN
        End Get
        Set(ByVal value As  Nullable(Of Integer))
            _ORDEN = value
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
#End Region

#Region "Funciones Publicas de la Clase"
    Public Sub New()
        'Realiza la generación del ejemplar estándar.
        MyBase.New()
        'Inicializa las variables privadas de la clase.
        Inicializar()
    End Sub
    Public Sub Inicializar()
        Me._IDTIPO = String.Empty
        Me._ORDEN = Nothing
        Me._NOMBRE = String.Empty
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
                If Not IsDBNull(.GetValue(.GetOrdinal("IDTIPO"))) Then
                    Me._IDTIPO = .GetValue(.GetOrdinal("IDTIPO"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("ORDEN"))) Then
                    Me._ORDEN = Convert.ToInt64(.GetValue(.GetOrdinal("ORDEN")))
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

                If Not String.IsNullOrEmpty(Me._IDTIPO) Then
                    .CommandText = .CommandText & " AND A.IDTIPO = :P_IDTIPO "
                    .Parameters.Add("P_IDTIPO", NpgsqlDbType.Varchar, 10).Value = Me._IDTIPO
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

                If Not String.IsNullOrEmpty(Me._IDTIPO) Then
                    .CommandText = .CommandText & " AND A.IDTIPO = :P_IDTIPO "
                    .Parameters.Add("P_IDTIPO", NpgsqlDbType.Varchar, 10).Value = Me._IDTIPO
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

#Region "Otras Funciones de consulta"
    Public Function ObtenerTipos(ByVal pnIdWidget As Integer, ByVal pnIdIndicator As Integer) As DataTable
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
                .CommandText = "SELECT A.IDTIPO, " & pnIdIndicator.ToString() + " INDICATORID, A.NOMBRE, A.ORDEN, DECODE(B.IDTIPO, NULL, 'false','true') ASIGNADO, " & _
                               "B.INDICATORID " & _
                               "FROM KPI_INDICATOR_TIPOS A " & _
                               "LEFT JOIN (SELECT INDICATORID, IDTIPO FROM KPI_INDICATOR_WIDGETS WHERE IDWIDGET = :P_IDWIDGET AND INDICATORID = :P_INDICATORID) B ON A.IDTIPO = B.IDTIPO " & _
                               "ORDER BY ORDEN ASC "
                .CommandType = CommandType.Text

                .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = pnIdWidget
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = pnIdIndicator
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
