Public Class cKPI_DAILY
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT DAILYID, USERID, REFID, FECHA, MESSAGE, " & _
                                "MODO, CONTENIDO, ICONO, TITULO " & _
                                "FROM KPI_DAILY " & _
                                "WHERE 1=1 "

    Private Const strInserta = "kpis.ADD_KPI_DAILY"
    Private Const strModifica = "kpis.UPD_KPI_DAILY"
    Private Const strElimina = "kpis.DEL_KPI_DAILY"
#End Region

#Region "Variables Privadas"
    Private _DAILYID As Nullable(Of Integer)
    Private _USERID As Nullable(Of Integer)
    Private _REFID As Nullable(Of Integer)
    Private _FECHA As Nullable(Of DateTime)
    Private _MESSAGE As String
    Private _MODO As Nullable(Of Integer)
    Private _CONTENIDO As String
    Private _ICONO As String
    Private _TITULO As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "kpi_daily"
        End Get
    End Property
    Public Property dailyid As Nullable(Of Integer)
        Get
            Return _DAILYID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _DAILYID = value
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
    Public Property refid As Nullable(Of Integer)
        Get
            Return _REFID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _REFID = value
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
    Public Property message As String
        Get
            Return _MESSAGE
        End Get
        Set(ByVal value As String)
            _MESSAGE = value
        End Set
    End Property
    Public Property modo As Nullable(Of Integer)
        Get
            Return _MODO
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _MODO = value
        End Set
    End Property
    Public Property contenido As String
        Get
            Return _CONTENIDO
        End Get
        Set(ByVal value As String)
            _CONTENIDO = value
        End Set
    End Property
    Public Property icono As String
        Get
            Return _ICONO
        End Get
        Set(ByVal value As String)
            _ICONO = value
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
#End Region

#Region "Funciones Publicas de la Clase"
    Public Sub New()
        'Realiza la generación del ejemplar estándar.
        MyBase.New()
        'Inicializa las variables privadas de la clase.
        Inicializar()
    End Sub
    Public Sub Inicializar()
        Me._DAILYID = Nothing
        Me._USERID = Nothing
        Me._REFID = Nothing
        Me._FECHA = Nothing
        Me._MESSAGE = String.Empty
        Me._MODO = Nothing
        Me._CONTENIDO = String.Empty
        Me._ICONO = String.Empty
        Me._TITULO = String.Empty
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

                If Me._DAILYID.HasValue Then
                    .CommandText = .CommandText & " AND DAILYID = :P_DAILYID "
                    .Parameters.Add("P_DAILYID", NpgsqlDbType.Bigint).Value = Me._DAILYID.Value
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
                If Not IsDBNull(.GetValue(.GetOrdinal("DAILYID"))) Then
                    Me._DAILYID = Convert.ToInt64(.GetValue(.GetOrdinal("DAILYID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("USERID"))) Then
                    Me._USERID = Convert.ToInt64(.GetValue(.GetOrdinal("USERID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("REFID"))) Then
                    Me._REFID = Convert.ToInt64(.GetValue(.GetOrdinal("REFID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("FECHA"))) Then
                    Me._FECHA = Convert.ToDateTime(.GetValue(.GetOrdinal("FECHA")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("MESSAGE"))) Then
                    Me._MESSAGE = .GetValue(.GetOrdinal("MESSAGE"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("MODE"))) Then
                    Me._MODO = Convert.ToInt64(.GetValue(.GetOrdinal("MODE")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("CONTENIDO"))) Then
                    Me._CONTENIDO = .GetValue(.GetOrdinal("CONTENIDO"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("ICONO"))) Then
                    Me._ICONO = .GetValue(.GetOrdinal("ICONO"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("TITULO"))) Then
                    Me._TITULO = .GetValue(.GetOrdinal("TITULO"))
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
                If Me._USERID.HasValue Then
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                Else
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._REFID.HasValue Then
                    .Parameters.Add("P_REFID", NpgsqlDbType.Bigint).Value = Me._REFID.Value
                Else
                    .Parameters.Add("P_REFID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_MESSAGE", NpgsqlDbType.Varchar).Value = Me._MESSAGE
                .Parameters.Add("P_MODO", NpgsqlDbType.Varchar).Value = Me._MODO
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._DAILYID = nResultado
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
                If Me._USERID.HasValue Then
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                Else
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._REFID.HasValue Then
                    .Parameters.Add("P_REFID", NpgsqlDbType.Bigint).Value = Me._REFID.Value
                Else
                    .Parameters.Add("P_REFID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_MESSAGE", NpgsqlDbType.Varchar).Value = Me._MESSAGE
                .Parameters.Add("P_MODO", NpgsqlDbType.Varchar).Value = Me._MODO
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._DAILYID = nResultado
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
                If Me._DAILYID.HasValue Then
                    .Parameters.Add("P_DAILYID", NpgsqlDbType.Bigint).Value = Me._DAILYID.Value
                Else
                    .Parameters.Add("P_DAILYID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._USERID.HasValue Then
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                Else
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._REFID.HasValue Then
                    .Parameters.Add("P_REFID", NpgsqlDbType.Bigint).Value = Me._REFID.Value
                Else
                    .Parameters.Add("P_REFID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_MESSAGE", NpgsqlDbType.Varchar).Value = Me._MESSAGE
                .Parameters.Add("P_MODO", NpgsqlDbType.Varchar).Value = Me._MODO
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
                If Me._DAILYID.HasValue Then
                    .Parameters.Add("P_DAILYID", NpgsqlDbType.Bigint).Value = Me._DAILYID.Value
                Else
                    .Parameters.Add("P_DAILYID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._USERID.HasValue Then
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                Else
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._REFID.HasValue Then
                    .Parameters.Add("P_REFID", NpgsqlDbType.Bigint).Value = Me._REFID.Value
                Else
                    .Parameters.Add("P_REFID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_MESSAGE", NpgsqlDbType.Varchar).Value = Me._MESSAGE
                .Parameters.Add("P_MODO", NpgsqlDbType.Varchar).Value = Me._MODO
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
                If Me._DAILYID.HasValue Then
                    .Parameters.Add("P_DAILYID", NpgsqlDbType.Bigint).Value = Me._DAILYID.Value
                Else
                    .Parameters.Add("P_DAILYID", NpgsqlDbType.Bigint).Value = DBNull.Value
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
                If Me._DAILYID.HasValue Then
                    .Parameters.Add("P_DAILYID", NpgsqlDbType.Bigint).Value = Me._DAILYID.Value
                Else
                    .Parameters.Add("P_DAILYID", NpgsqlDbType.Bigint).Value = DBNull.Value
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
    Public Function ObtenerDatos(ByVal pageSize As Integer, ByVal currentPage As Integer) As DataTable
        Dim dbconexion As New NpgsqlConnection
        Dim tblResultados As System.Data.DataTable = Nothing

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

            'Limpiamos el contenido de la Tabla de Datos
            _dtData.Clear()

            With SqlConsulta
                .Connection = dbconexion
                .CommandText = strConsulta
                .CommandType = CommandType.Text

                If Me._DAILYID.HasValue Then
                    .CommandText = .CommandText & " AND DAILYID = :P_DAILYID "
                    .Parameters.Add("P_DAILYID", NpgsqlDbType.Bigint).Value = Me._DAILYID.Value
                End If
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
                If Me._REFID.HasValue Then
                    .CommandText = .CommandText & " AND REFID = :P_REFID "
                    .Parameters.Add("P_REFID", NpgsqlDbType.Bigint).Value = Me._REFID.Value
                End If
                If Not String.IsNullOrEmpty(Me._MODO) Then
                    .CommandText = .CommandText & " AND MODO = :P_MODO "
                    .Parameters.Add("P_MODO", NpgsqlDbType.Char, 1).Value = Me._MODO
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
    Public Function ObtenerDatos(ByVal psFiltro As String, ByVal pageSize As Integer, ByVal currentPage As Integer) As DataTable
        Dim dbconexion As New NpgsqlConnection
        Dim tblResultados As System.Data.DataTable = Nothing

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

            'Limpiamos el contenido de la Tabla de Datos
            _dtData.Clear()

            If pageSize = 0 Then
                Return _dtData
            End If

            With SqlConsulta
                .Connection = dbconexion
                .CommandType = CommandType.Text
                .CommandText = strConsulta

                If Me._DAILYID.HasValue Then
                    .CommandText = .CommandText & " AND DAILYID = :P_DAILYID "
                    .Parameters.Add("P_DAILYID", NpgsqlDbType.Bigint).Value = Me._DAILYID.Value
                End If
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
                If Me._REFID.HasValue Then
                    .CommandText = .CommandText & " AND REFID = :P_REFID "
                    .Parameters.Add("P_REFID", NpgsqlDbType.Bigint).Value = Me._REFID.Value
                End If
                If Not String.IsNullOrEmpty(Me._MODO) Then
                    .CommandText = .CommandText & " AND MODO = :P_MODO "
                    .Parameters.Add("P_MODO", NpgsqlDbType.Char, 1).Value = Me._MODO
                End If
                .CommandText = .CommandText & psFiltro
            End With

            daData.SelectCommand = SqlConsulta
            daData.Fill(_dtData)

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

#Region "Otras funciones"
    Public Function ObtenerDiario(ByVal pageSize As Integer, ByVal currentPage As Integer) As DataTable
        Dim dbconexion As New NpgsqlConnection
        Dim tblResultados As System.Data.DataTable = Nothing

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

            'Limpiamos el contenido de la Tabla de Datos
            _dtData.Clear()

            With SqlConsulta
                .Connection = dbconexion
                .CommandText = "SELECT X.DAILYID, X.USERID, X.REFID, X.TITULO, X.FECHA, X.MESSAGE, X.MODO, X.CONTENIDO, X.ICONO, Y.NOMBRE, Y.APELLIDOS " & _
                               " FROM (SELECT A.DAILYID, A.USERID, A.REFID, A.TITULO, A.FECHA, A.MESSAGE, A.MODO, A.CONTENIDO, A.ICONO " & _
                               "       FROM KPI_DAILY A " & _
                               "       WHERE EXISTS (SELECT 1 FROM VW_FRIENDS B WHERE A.USERID = B.MYUSERID AND (B.USERID = :P_USERID OR B.MYUSERID = :P_USERID)) OR A.USERID = :P_USERID) X " & _
                               "INNER JOIN ASPNET_INFO_USUARIO Y ON X.USERID = Y.USERID "
                .CommandType = CommandType.Text

                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                .CommandText = .CommandText & " ORDER BY X.FECHA DESC"
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
