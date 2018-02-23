Public Class cKPI_INDICATOR_USERS
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT A.INDICATORID, A.USERID, A.FECHA, A.VISITAS, A.ANONIMO, " & _
                                "B.TITULO, B.RESUMEN, C.ESTILO, COALESCE(D.NUM_DATASETS,0) NUM_DATASETS, B.IMAGEURL " & _
                                "FROM KPI_INDICATOR_USERS A " & _
                                "INNER JOIN KPI_INDICATORS B ON A.INDICATORID = B.INDICATORID " & _
                                "LEFT JOIN KPI_CATEGORIES C ON B.CATEGORYID = C.CATEGORYID " & _
                                "LEFT JOIN (SELECT AUX.USERID, AUX.INDICATORID, COUNT(AUX.DATASETID) NUM_DATASETS FROM KPI_DATASETS AUX GROUP BY AUX.USERID, AUX.INDICATORID) D ON D.USERID = A.USERID AND D.INDICATORID = A.INDICATORID " & _
                                "WHERE 1=1 "

    Private Const strInserta = "kpis.ADD_KPI_INDICATOR_USERS"
    Private Const strModifica = "kpis.UPD_KPI_INDICATOR_USERS"
    Private Const strElimina = "kpis.DEL_KPI_INDICATOR_USERS"
#End Region

#Region "Variables Privadas"
    Private _INDICATORID As Nullable(Of Integer)
    Private _USERID As Nullable(Of Integer)
    Private _FECHA As Nullable(Of DateTime)
    Private _VISITAS As Nullable(Of Integer)
    Private _ANONIMO As String

    Private _TITULO As String
    Private _NOMBRE As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "KPI_INDICATOR_USERS"
        End Get
    End Property
    Public Property indicatorid As Nullable(Of Integer)
        Get
            Return _INDICATORID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _INDICATORID = value
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
    Public Property fecha As Nullable(Of DateTime)
        Get
            Return _FECHA
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FECHA = value
        End Set
    End Property
    Public Property visitas As Nullable(Of Integer)
        Get
            Return _VISITAS
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _VISITAS = value
        End Set
    End Property
    Public Property anonimo As Boolean
        Get
            If _ANONIMO = "S" Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _ANONIMO = "S"
            Else
                _ANONIMO = "N"
            End If
        End Set
    End Property

    Public WriteOnly Property titulo() As String
        Set(ByVal value As String)
            If (value.Length > 100) Then
                _TITULO = value.Substring(0, 100)
            Else
                _TITULO = value
            End If
        End Set
    End Property
    Public WriteOnly Property nombre() As String
        Set(ByVal value As String)
            If (value.Length > 100) Then
                _NOMBRE = value.Substring(0, 100)
            Else
                _NOMBRE = value
            End If
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
        Me._USERID = Nothing
        Me._VISITAS = Nothing
        Me._FECHA = Nothing
        Me._ANONIMO = String.Empty
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
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
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
                If Not IsDBNull(.GetValue(.GetOrdinal("USERID"))) Then
                    Me._USERID = Convert.ToInt64(.GetValue(.GetOrdinal("USERID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("VISITAS"))) Then
                    Me._VISITAS = Convert.ToDecimal(.GetValue(.GetOrdinal("VISITAS")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("FECHA"))) Then
                    Me._FECHA = Convert.ToDateTime(.GetValue(.GetOrdinal("FECHA")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("ANONIMO"))) Then
                    Me._ANONIMO = .GetValue(.GetOrdinal("ANONIMO"))
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
                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
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
                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
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
    Public Function bModificarVisitas() As Boolean
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
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                .Parameters.Add("P_VISITAS", NpgsqlDbType.Integer).Value = Me._VISITAS
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
    Public Function bModificarAnonimo() As Boolean
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
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                .Parameters.Add("P_ANONIMO", NpgsqlDbType.Char).Value = Me._ANONIMO
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
                .Connection = dbconexion
                .Transaction = dbTransaccion
                .CommandText = strModifica
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                .Parameters.Add("P_VISITAS", NpgsqlDbType.Integer).Value = Me._VISITAS
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
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
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
    Public Function bEliminarTodos() As Boolean
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
                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
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
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID
                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID
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

                If Me.indicatorid.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Me.userid.HasValue Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
                If Not String.IsNullOrEmpty(Me._TITULO) Then
                    .CommandText = .CommandText & " AND LOWER(B.TITULO) LIKE :P_TITULO "
                    .Parameters.Add("P_TITULO", NpgsqlDbType.Varchar, 100).Value = "%" & Me._TITULO.ToLower() & "%"
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
                .CommandText = strConsulta
                .CommandType = CommandType.Text

                If Me.indicatorid.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Me.userid.HasValue Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
                If Not String.IsNullOrEmpty(Me._TITULO) Then
                    .CommandText = .CommandText & " AND LOWER(B.TITULO) LIKE :P_TITULO "
                    .Parameters.Add("P_TITULO", NpgsqlDbType.Varchar, 100).Value = "%" & Me._TITULO.ToLower() & "%"
                End If
                .CommandText = .CommandText & psFiltro
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

#Region "Otras Funciones"
    Public Function nRecuentoIndicadores() As Integer
        Dim dbconexion As New NpgsqlConnection
        Dim nResultado As Integer = 0

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

            With SqlConsulta
                .Connection = dbconexion
                .CommandText = "SELECT COUNT(INDICATORID) " & _
                                "FROM KPI_INDICATOR_USERS " & _
                                "WHERE 1=1 "
                .CommandType = CommandType.Text

                If Me.indicatorid.HasValue Then
                    .CommandText = .CommandText & " AND INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Me.userid.HasValue Then
                    .CommandText = .CommandText & " AND USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
            End With

            nResultado = SqlConsulta.ExecuteScalar()

        Catch excp As NpgsqlException
            nResultado = -1
        Finally
            dbconexion.Close()
        End Try

        Return nResultado

    End Function
    Public Function nRecuentoUsers() As Integer
        Dim dbconexion As New NpgsqlConnection
        Dim nResultado As Integer = 0

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

            With SqlConsulta
                .Connection = dbconexion
                .CommandType = CommandType.Text
                .CommandText = "SELECT COUNT(A.USERID) " & _
                               "FROM KPI_INDICATOR_USERS A " & _
                               "LEFT JOIN ASPNET_INFO_USUARIO B ON A.USERID = B.USERID " & _
                               "WHERE (A.ANONIMO='N' OR (A.ANONIMO='S' AND EXISTS (SELECT 1 FROM VW_FRIENDS C WHERE A.USERID = :P_USERID))) " & _
                               "AND A.USERID != :P_USERID " & _
                               "AND A.INDICATORID = :P_INDICATORID "

                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                If Not String.IsNullOrEmpty(Me._NOMBRE) Then
                    .CommandText = .CommandText & " AND LOWER(A.NOMBRE) LIKE :P_NOMBRE "
                    .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 250).Value = "%" & Me._NOMBRE.ToLower() & "%"
                End If
            End With

            nResultado = SqlConsulta.ExecuteScalar()

        Catch excp As NpgsqlException
            nResultado = -1
        Finally
            dbconexion.Close()
        End Try

        Return nResultado

    End Function
    Public Function nRecuentoFormulas() As Integer
        Dim dbconexion As New NpgsqlConnection
        Dim nResultado As Integer = 0

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
            Dim SqlConsulta As New NpgsqlCommand

            With SqlConsulta
                .Connection = dbconexion
                .CommandType = CommandType.Text
                .CommandText = "SELECT COUNT(C.IDFORMULA) " & _
                               "FROM KPI_DASHBOARDS A " & _
                               "LEFT JOIN KPI_WIDGETS B ON A.IDDASHBOARD = B.IDDASHBOARD " & _
                               "LEFT JOIN KPI_WIDGET_FORMULAS C ON B.IDWIDGET = C.IDWIDGET " & _
                               "WHERE 1=1 "

                If Me.userid.HasValue Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
            End With

            nResultado = SqlConsulta.ExecuteScalar()

        Catch excp As NpgsqlException
            nResultado = -1
        Finally
            dbconexion.Close()
        End Try

        Return nResultado

    End Function
    Public Function nRecuentoWidgets() As Integer
        Dim dbconexion As New NpgsqlConnection
        Dim nResultado As Integer = 0

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
            Dim SqlConsulta As New NpgsqlCommand

            With SqlConsulta
                .Connection = dbconexion
                .CommandType = CommandType.Text
                .CommandText = "SELECT COUNT(B.IDWIDGET) " & _
                               "FROM KPI_DASHBOARDS A " & _
                               "LEFT JOIN KPI_WIDGETS B ON A.IDDASHBOARD = B.IDDASHBOARD " & _
                               "WHERE 1=1 "

                If Me.userid.HasValue Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
            End With

            nResultado = SqlConsulta.ExecuteScalar()

        Catch excp As NpgsqlException
            nResultado = -1
        Finally
            dbconexion.Close()
        End Try

        Return nResultado

    End Function
    Public Function nRecuentoDashboards() As Integer
        Dim dbconexion As New NpgsqlConnection
        Dim nResultado As Integer = 0

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
            Dim SqlConsulta As New NpgsqlCommand

            With SqlConsulta
                .Connection = dbconexion
                .CommandType = CommandType.Text
                .CommandText = "SELECT COUNT(A.IDDASHBOARD) " & _
                               "FROM KPI_DASHBOARDS A " & _
                               "WHERE 1=1 "

                If Me.userid.HasValue Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
            End With

            nResultado = SqlConsulta.ExecuteScalar()

        Catch excp As NpgsqlException
            nResultado = -1
        Finally
            dbconexion.Close()
        End Try

        Return nResultado

    End Function
    Public Function TopXUltimos(ByVal pnPrimeros As Integer, ByVal psCompartido As String) As DataTable
        '******************************************************************************************
        'Funcion para obtener los ultimos 8 indicadores de la librería de un usuario y mostrárselos
        'en el control KPILibrary.aspx 
        '******************************************************************************************
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
                .CommandText = "SELECT Z.INDICATORID, Z.TITULO, Z.ESTILO, Z.RECUENTO, Z.IMAGEURL FROM " & _
                               "(SELECT A.INDICATORID, B.TITULO, C.ESTILO, COALESCE(D.RECUENTO,0) RECUENTO, B.IMAGEURL, A.VISITAS " & _
                               "FROM KPI_INDICATOR_USERS A " & _
                               "INNER JOIN KPI_INDICATORS B ON A.INDICATORID = B.INDICATORID " & _
                               "LEFT JOIN KPI_CATEGORIES C ON B.CATEGORYID = C.CATEGORYID " & _
                               "LEFT JOIN (SELECT INDICATORID, COUNT(VALORID) RECUENTO FROM KPI_DATASET_VALUES WHERE USERID = :P_USERID1 GROUP BY INDICATORID) D ON A.INDICATORID = D.INDICATORID " & _
                               "WHERE A.USERID = :P_USERID2 "
                .Parameters.Add("P_USERID1", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                .Parameters.Add("P_USERID2", NpgsqlDbType.Bigint).Value = Me._USERID.Value

                If Not String.IsNullOrEmpty(psCompartido) Then
                    .CommandText = .CommandText & " AND B.COMPARTIDO = :P_COMPARTIDO "
                    .Parameters.Add("P_COMPARTIDO", NpgsqlDbType.Char, 1).Value = psCompartido
                End If

                .CommandText = .CommandText & " ORDER BY A.VISITAS DESC ) Z LIMIT :P_ROWNUM "
                .Parameters.Add("P_ROWNUM", NpgsqlDbType.Bigint).Value = pnPrimeros
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
    Public Function CommonIndicators(ByVal pageSize As Integer, ByVal currentPage As Integer, ByVal psFiltro As String, ByVal psOrderBy As String) As DataTable
        '******************************************************************************************
        'Funcion para obtener los indicadores comunes entre dos usuarios determinados 
        '******************************************************************************************
        Dim dbconexion As New NpgsqlConnection
        Dim tblResultados As New System.Data.DataTable

        'Limpiamos el contenido de la Tabla de Datos
        tblResultados.Clear()

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
            Dim _dtData As New DataTable

            With SqlConsulta
                .Connection = dbconexion
                .CommandType = CommandType.Text
                .CommandText = "SELECT A.INDICATORID, B.TITULO, B.IMAGEURL FROM KPI_INDICATOR_USERS A " & _
                               "LEFT JOIN KPI_INDICATORS B ON A.INDICATORID = B.INDICATORID " & _
                               "WHERE EXISTS (SELECT 1 FROM KPI_INDICATOR_USERS C " & _
                               "            WHERE A.INDICATORID = C.INDICATORID " & _
                               "            AND C.USERID = :P_USERID " & _
                               "            AND C.ANONIMO = 'N') "
                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value

                If Not String.IsNullOrEmpty(psFiltro) Then
                    .CommandText = .CommandText & psFiltro
                End If
                If Not String.IsNullOrEmpty(psOrderBy) Then
                    .CommandText = .CommandText & " ORDER BY " & psOrderBy
                Else
                    .CommandText = .CommandText & " ORDER BY A.VISITAS DESC "
                End If
            End With

            daData.SelectCommand = SqlConsulta
            daData.Fill(_dtData)

            If pageSize = 0 Then
                Return _dtData
            End If

            tblResultados = _dtData.Clone()
            Dim lastRows As IEnumerable(Of DataRow) = _dtData.Rows.Cast(Of DataRow).Skip((currentPage - 1) * pageSize).Take(pageSize)

            For Each Fila As System.Data.DataRow In lastRows
                tblResultados.ImportRow(Fila)
            Next

        Catch excp As NpgsqlException
            MsgBox("No se ha podido obtener los resultados de la consulta de la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return tblResultados

    End Function
    Public Function OtherUsers(ByVal pageSize As Integer, ByVal currentPage As Integer, ByVal psFiltro As String, ByVal psOrderBy As String) As DataTable
        '******************************************************************************************
        'Funcion para obtener los indicadores comunes entre dos usuarios determinados 
        '******************************************************************************************
        Dim dbconexion As New NpgsqlConnection
        Dim tblResultados As New System.Data.DataTable

        'Limpiamos el contenido de la Tabla de Datos
        tblResultados.Clear()

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
            Dim _dtData As New DataTable

            With SqlConsulta
                .Connection = dbconexion
                .CommandType = CommandType.Text
                .CommandText = "SELECT A.INDICATORID, B.USERID, B.NOMBRE, B.APELLIDOS, B.IMAGEURL, COALESCE(C.NUM_DATASETS,0) NUM_DATASETS, COALESCE(D.NUM_DATOS,0) NUM_DATOS " & _
                               "FROM KPI_INDICATOR_USERS A " & _
                               "LEFT JOIN ASPNET_INFO_USUARIO B ON A.USERID = B.USERID " & _
                               "LEFT JOIN (SELECT AUX1.USERID, AUX1.INDICATORID, COUNT(DATASETID) NUM_DATASETS FROM KPI_DATASETS AUX1 GROUP BY AUX1.USERID, AUX1.INDICATORID) C ON A.INDICATORID = C.INDICATORID AND A.USERID = C.USERID " & _
                               "LEFT JOIN (SELECT AUX2.USERID, AUX2.INDICATORID, COUNT(VALORID) NUM_DATOS FROM KPI_DATASET_VALUES AUX2 GROUP BY AUX2.USERID, AUX2.INDICATORID) D ON A.INDICATORID = D.INDICATORID AND A.USERID = D.USERID " & _
                               "WHERE (A.ANONIMO='N' OR (A.ANONIMO='S' AND EXISTS (SELECT 1 FROM VW_FRIENDS C WHERE A.USERID = :P_USERID))) " & _
                               "AND A.USERID != :P_USERID " & _
                               "AND A.INDICATORID = :P_INDICATORID "
                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value

                If Not String.IsNullOrEmpty(Me._NOMBRE) Then
                    .CommandText = .CommandText & " AND LOWER(B.NOMBRE) LIKE :P_NOMBRE "
                    .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 250).Value = "%" & Me._NOMBRE.ToLower() & "%"
                End If
                If Not String.IsNullOrEmpty(psFiltro) Then
                    .CommandText = .CommandText & psFiltro
                End If
                If Not String.IsNullOrEmpty(psOrderBy) Then
                    .CommandText = .CommandText & " ORDER BY " & psOrderBy
                Else
                    .CommandText = .CommandText & " ORDER BY D.NUM_DATOS DESC "
                End If
            End With

            daData.SelectCommand = SqlConsulta
            daData.Fill(_dtData)

            If pageSize = 0 Then
                Return _dtData
            End If

            tblResultados = _dtData.Clone()
            Dim lastRows As IEnumerable(Of DataRow) = _dtData.Rows.Cast(Of DataRow).Skip((currentPage - 1) * pageSize).Take(pageSize)

            For Each Fila As System.Data.DataRow In lastRows
                tblResultados.ImportRow(Fila)
            Next

        Catch excp As NpgsqlException
            MsgBox("No se ha podido obtener los resultados de la consulta de la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return tblResultados

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
