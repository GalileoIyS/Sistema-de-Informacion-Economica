Public Class cKPI_WIDGETS
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT A.IDWIDGET, A.IDDASHBOARD, A.IDCOLUMN, A.TITULO, A.ORDEN, A.ESTILO, B.USERID, A.SIZE, " & _
                                "A.DIMENSION, A.GRAFICO, A.COLAPSED, TO_CHAR(A.FECHA_INICIO,'DD-MM-YYYY') FECHA_INICIO, TO_CHAR(A.FECHA_FIN,'DD-MM-YYYY') FECHA_FIN " & _
                                "FROM KPI_WIDGETS A INNER JOIN KPI_DASHBOARDS B ON A.IDDASHBOARD = B.IDDASHBOARD " & _
                                "WHERE 1=1 "

    Private Const strInserta = "kpis.add_kpi_widgets"
    Private Const strModifica = "kpis.upd_kpi_widgets"
    Private Const strElimina = "kpis.del_kpi_widgets"
#End Region

#Region "Variables Privadas"
    Private _IDWIDGET As Nullable(Of Integer)
    Private _IDDASHBOARD As Nullable(Of Integer)
    Private _IDCOLUMN As Nullable(Of Integer)
    Private _TITULO As String
    Private _ORDEN As Nullable(Of Integer)
    Private _ESTILO As String
    Private _DIMENSION As String
    Private _GRAFICO As String
    Private _COLAPSED As String
    Private _FECHA_INICIO As Nullable(Of DateTime)
    Private _FECHA_FIN As Nullable(Of DateTime)
    Private _SIZE As String

    Private _USERID As Nullable(Of Integer)
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "KPI_WIDGETS"
        End Get
    End Property
    Public Property idwidget As Nullable(Of Integer)
        Get
            Return _IDWIDGET
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IDWIDGET = value
        End Set
    End Property
    Public Property iddashboard As Nullable(Of Integer)
        Get
            Return _IDDASHBOARD
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IDDASHBOARD = value
        End Set
    End Property
    Public Property idcolumn As String
        Get
            If _IDCOLUMN.HasValue Then
                Return _IDCOLUMN.Value.ToString()
            Else
                Return 1
            End If
        End Get
        Set(ByVal value As String)
            Select Case value
                Case "column1" : _IDCOLUMN = 1
                Case "column2" : _IDCOLUMN = 2
                Case "column3" : _IDCOLUMN = 3
                Case Else
                    _IDCOLUMN = 1
            End Select
        End Set
    End Property
    Public Property titulo As String
        Get
            Return _TITULO
        End Get
        Set(ByVal value As String)
            _TITULO = value
        End Set
    End Property
    Public Property orden As Nullable(Of Integer)
        Get
            Return _ORDEN
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _ORDEN = value
        End Set
    End Property
    Public Property estilo As String
        Get
            Return _ESTILO
        End Get
        Set(ByVal value As String)
            _ESTILO = value
        End Set
    End Property
    Public ReadOnly Property dimension_texto As String
        Get
            Select Case _DIMENSION
                Case "D" : Return "Diario"
                Case "s" : Return "Semanal"
                Case "Q" : Return "Quincenal"
                Case "M" : Return "Mensual"
                Case "T" : Return "Trimestral"
                Case "S" : Return "Semestral"
                Case "A" : Return "Anual"
                Case Else
                    Return "Dimensión"
            End Select
        End Get
    End Property
    Public Property dimension As String
        Get
            Return _DIMENSION
        End Get
        Set(ByVal value As String)
            _DIMENSION = value
        End Set
    End Property
    Public Property grafico As String
        Get
            Return _GRAFICO
        End Get
        Set(ByVal value As String)
            _GRAFICO = value
        End Set
    End Property
    Public Property colapsado As String
        Get
            Return _COLAPSED
        End Get
        Set(ByVal value As String)
            _COLAPSED = value
        End Set
    End Property
    Public Property fecha_inicio As Nullable(Of DateTime)
        Get
            Return _FECHA_INICIO
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FECHA_INICIO = value
        End Set
    End Property
    Public Property fecha_fin As Nullable(Of DateTime)
        Get
            Return _FECHA_FIN
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FECHA_FIN = value
        End Set
    End Property
    Public Property size As String
        Get
            Return _SIZE
        End Get
        Set(ByVal value As String)
            _SIZE = value
        End Set
    End Property

    Public Property userid As Nullable(Of Integer)
        Get
            Return _USERID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _USERID = value
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
        Me._IDWIDGET = Nothing
        Me._IDDASHBOARD = Nothing
        Me._IDCOLUMN = Nothing
        Me._TITULO = String.Empty
        Me._ORDEN = Nothing
        Me._ESTILO = String.Empty
        Me._DIMENSION = String.Empty
        Me._GRAFICO = String.Empty
        Me._COLAPSED = String.Empty
        Me._FECHA_INICIO = Nothing
        Me._FECHA_FIN = Nothing
        Me._SIZE = String.Empty
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

                If Me._IDWIDGET.HasValue Then
                    .CommandText = .CommandText & " AND A.IDWIDGET = :P_IDWIDGET "
                    .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
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
                If Not IsDBNull(.GetValue(.GetOrdinal("IDWIDGET"))) Then
                    Me._IDWIDGET = Convert.ToInt64(.GetValue(.GetOrdinal("IDWIDGET")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("IDDASHBOARD"))) Then
                    Me._IDDASHBOARD = Convert.ToInt64(.GetValue(.GetOrdinal("IDDASHBOARD")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("IDCOLUMN"))) Then
                    Me._IDCOLUMN = Convert.ToInt64(.GetValue(.GetOrdinal("IDCOLUMN")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("TITULO"))) Then
                    Me._TITULO = .GetValue(.GetOrdinal("TITULO"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("ORDEN"))) Then
                    Me._ORDEN = Convert.ToInt64(.GetValue(.GetOrdinal("ORDEN")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("ESTILO"))) Then
                    Me._ESTILO = .GetValue(.GetOrdinal("ESTILO"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("DIMENSION"))) Then
                    Me._DIMENSION = .GetValue(.GetOrdinal("DIMENSION"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("GRAFICO"))) Then
                    Me._GRAFICO = .GetValue(.GetOrdinal("GRAFICO"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("COLAPSED"))) Then
                    Me._COLAPSED = .GetValue(.GetOrdinal("COLAPSED"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("FECHA_INICIO"))) Then
                    Me._FECHA_INICIO = Convert.ToDateTime(.GetValue(.GetOrdinal("FECHA_INICIO")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("FECHA_FIN"))) Then
                    Me._FECHA_FIN = Convert.ToDateTime(.GetValue(.GetOrdinal("FECHA_FIN")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("SIZE"))) Then
                    Me._SIZE = .GetValue(.GetOrdinal("SIZE"))
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
                If Me._IDDASHBOARD.HasValue Then
                    .Parameters.Add("P_IDDASHBOARD", NpgsqlDbType.Bigint).Value = Me._IDDASHBOARD.Value
                Else
                    .Parameters.Add("P_IDDASHBOARD", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_TITULO", NpgsqlDbType.Varchar, 100).Value = Me._TITULO
                .Parameters.Add("P_ESTILO", NpgsqlDbType.Varchar, 100).Value = Me._ESTILO
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._IDWIDGET = nResultado
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            nResultado = -1
        Finally
            dbconexion.Close()
        End Try

        If nResultado >= 0 Then
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
                If Me._IDDASHBOARD.HasValue Then
                    .Parameters.Add("P_IDDASHBOARD", NpgsqlDbType.Bigint).Value = Me._IDDASHBOARD.Value
                Else
                    .Parameters.Add("P_IDDASHBOARD", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_TITULO", NpgsqlDbType.Varchar, 100).Value = Me._TITULO
                .Parameters.Add("P_ESTILO", NpgsqlDbType.Varchar, 100).Value = Me._ESTILO
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._IDWIDGET = nResultado
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            nResultado = -1
        End Try

        If nResultado >= 0 Then
            Return True
        Else
            Return False
        End If

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
                .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                .Parameters.Add("P_TITULO", NpgsqlDbType.Varchar, 100).Value = Me._TITULO
                .Parameters.Add("P_ESTILO", NpgsqlDbType.Varchar, 100).Value = Me._ESTILO
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
    Public Function bModificar(ByVal dbConexion As NpgsqlConnection, ByVal dbTransaccion As NpgsqlTransaction) As Boolean
        Dim nResultado As Int64 = 0

        Try
            Dim SqlModifica As New NpgsqlCommand

            With SqlModifica
                .Connection = dbConexion
                .Transaction = dbTransaccion
                .CommandText = strModifica
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                .Parameters.Add("P_TITULO", NpgsqlDbType.Varchar, 100).Value = Me._TITULO
                .Parameters.Add("P_ESTILO", NpgsqlDbType.Varchar, 100).Value = Me._ESTILO
                .Parameters.Add("P_DIMENSION", NpgsqlDbType.Char, 1).Value = Me._DIMENSION
                .Parameters.Add("P_GRAFICO", NpgsqlDbType.Char, 1).Value = Me._GRAFICO
                .Parameters.Add("P_COLAPSED", NpgsqlDbType.Varchar, 1).Value = Me._COLAPSED
                If Me._FECHA_INICIO.HasValue Then
                    .Parameters.Add("P_FECHA_INICIO", NpgsqlDbType.Date).Value = Me._FECHA_INICIO.Value
                Else
                    .Parameters.Add("P_FECHA_INICIO", NpgsqlDbType.Date).Value = DBNull.Value
                End If
                If Me._FECHA_FIN.HasValue Then
                    .Parameters.Add("P_FECHA_FIN", NpgsqlDbType.Date).Value = Me._FECHA_FIN.Value
                Else
                    .Parameters.Add("P_FECHA_FIN", NpgsqlDbType.Date).Value = DBNull.Value
                End If
            End With

            SqlModifica.ExecuteNonQuery()
            nResultado = SqlModifica.Parameters("RETURN_VALUE").Value
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Modificación sobre la Base de Datos." & Chr(13) & _
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
                If Me._IDWIDGET.HasValue Then
                    .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                Else
                    .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
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
                .Connection = dbConexion
                .Transaction = dbTransaccion
                .CommandText = strElimina
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                If Me._IDWIDGET.HasValue Then
                    .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                Else
                    .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
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

                If Me._IDWIDGET.HasValue Then
                    .CommandText = .CommandText & " AND A.IDWIDGET = :P_IDWIDGET "
                    .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                End If
                If Me._IDDASHBOARD.HasValue Then
                    .CommandText = .CommandText & " AND A.IDDASHBOARD = :P_IDDASHBOARD "
                    .Parameters.Add("P_IDDASHBOARD", NpgsqlDbType.Bigint).Value = Me._IDDASHBOARD.Value
                End If
                If Me._IDCOLUMN.HasValue Then
                    .CommandText = .CommandText & " AND A.IDCOLUMN = :P_IDCOLUMN "
                    .Parameters.Add("P_IDCOLUMN", NpgsqlDbType.Bigint).Value = Me._IDCOLUMN.Value
                End If
                .CommandText = .CommandText & " ORDER BY A.ORDEN ASC"
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

                If Me._IDWIDGET.HasValue Then
                    .CommandText = .CommandText & " AND A.IDWIDGET = :P_IDWIDGET "
                    .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                End If
                If Me._IDDASHBOARD.HasValue Then
                    .CommandText = .CommandText & " AND A.IDDASHBOARD = :P_IDDASHBOARD "
                    .Parameters.Add("P_IDDASHBOARD", NpgsqlDbType.Bigint).Value = Me._IDDASHBOARD.Value
                End If
                If Me._IDCOLUMN.HasValue Then
                    .CommandText = .CommandText & " AND A.IDCOLUMN = :P_IDCOLUMN "
                    .Parameters.Add("P_IDCOLUMN", NpgsqlDbType.Bigint).Value = Me._IDCOLUMN.Value
                End If
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND B.USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
                .CommandText = .CommandText & " ORDER BY A.ORDEN ASC"
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

#Region "Otras funciones"
    Public Function bModificarPosicion() As Boolean
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
                .CommandText = "kpis.UPD_KPI_WIDGETS_POSITION"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                .Parameters.Add("P_IDCOLUMN", NpgsqlDbType.Bigint).Value = Me._IDCOLUMN.Value
                .Parameters.Add("P_ORDEN", NpgsqlDbType.Bigint).Value = Me._ORDEN.Value
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
    Public Function bModificarColapsado() As Boolean
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
                .CommandText = "kpis.UPD_KPI_WIDGETS_COLAPSED"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                .Parameters.Add("P_COLAPSED", NpgsqlDbType.Varchar, 1).Value = Me._COLAPSED
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
#End Region

#Region "Funciones de Consulta de Datos"
    Public Function ObtenerPie() As DataTable
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
            Dim Transaccion As NpgsqlTransaction = dbconexion.BeginTransaction()

            With SqlConsulta
                .Connection = dbconexion
                .CommandText = "FORMULAS.GETPERCENT"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                .Parameters.Add("P_TIPO", NpgsqlDbType.Char, 1).Value = Me._GRAFICO
                If Me._FECHA_INICIO.HasValue Then
                    .Parameters.Add("P_FECHA_INICIO", NpgsqlDbType.Date).Value = Me._FECHA_INICIO.Value
                Else
                    .Parameters.Add("P_FECHA_INICIO", NpgsqlDbType.Date).Value = DBNull.Value
                End If
                If Me._FECHA_FIN.HasValue Then
                    .Parameters.Add("P_FECHA_FIN", NpgsqlDbType.Date).Value = Me._FECHA_FIN.Value
                Else
                    .Parameters.Add("P_FECHA_FIN", NpgsqlDbType.Date).Value = DBNull.Value
                End If
            End With

            daData.SelectCommand = SqlConsulta
            daData.Fill(_dtData)

            Transaccion.Commit()

        Catch excp As NpgsqlException
            MsgBox("No se ha podido obtener los resultados de la consulta de la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return _dtData

    End Function
    Public Function ObtenerLineaTemporal() As DataTable
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
            Dim Transaccion As NpgsqlTransaction = dbconexion.BeginTransaction()

            With SqlConsulta
                .Connection = dbconexion
                .CommandText = "FORMULAS.GETTIMELINE"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                .Parameters.Add("P_DIMENSION", NpgsqlDbType.Char, 1).Value = Me._DIMENSION
                .Parameters.Add("P_TIPO", NpgsqlDbType.Char, 1).Value = Me._GRAFICO
                If Me._FECHA_INICIO.HasValue Then
                    .Parameters.Add("P_FECHA_INICIO", NpgsqlDbType.Date).Value = Me._FECHA_INICIO.Value
                Else
                    .Parameters.Add("P_FECHA_INICIO", NpgsqlDbType.Date).Value = DBNull.Value
                End If
                If Me._FECHA_FIN.HasValue Then
                    .Parameters.Add("P_FECHA_FIN", NpgsqlDbType.Date).Value = Me._FECHA_FIN.Value
                Else
                    .Parameters.Add("P_FECHA_FIN", NpgsqlDbType.Date).Value = DBNull.Value
                End If
            End With

            daData.SelectCommand = SqlConsulta
            daData.Fill(_dtData)

            Transaccion.Commit()

        Catch excp As NpgsqlException
            MsgBox("No se ha podido obtener los resultados de la consulta de la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return _dtData

    End Function
    Public Function ObtenerResumen(ByVal psTodos As Boolean) As DataTable
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
            Dim Transaccion As NpgsqlTransaction = dbconexion.BeginTransaction()

            With SqlConsulta
                .Connection = dbconexion
                .CommandText = "FORMULAS.GETCOUNTRESULT"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                If psTodos Then
                    .Parameters.Add("P_TODOS", NpgsqlDbType.Char, 1).Value = "S"
                Else
                    .Parameters.Add("P_TODOS", NpgsqlDbType.Char, 1).Value = "N"
                End If
            End With

            daData.SelectCommand = SqlConsulta
            daData.Fill(_dtData)

            Transaccion.Commit()

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
