Public Class cASPNET_FRIENDSHIP
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT X.RELATION, X.FECHA, X.ACCEPTED, X.MYUSERID, X.USERID, X.NOMBRE, X.APELLIDOS, X.IMAGEURL " & _
                                "FROM VW_FRIENDS X " & _
                                "WHERE 1=1 "

    Private Const strInserta = "aspnet.add_aspnet_friendship"
    Private Const strModifica = "aspnet.upd_aspnet_friendship"
    Private Const strElimina = "aspnet.del_aspnet_friendship"
#End Region

#Region "Variables Privadas"
    Private _RELATION As Nullable(Of Integer)
    Private _FROMUSERID As Nullable(Of Integer)
    Private _TOUSERID As Nullable(Of Integer)
    Private _ACCEPTED As String
    Private _FECHA As Nullable(Of DateTime)
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "aspnet_friendship"
        End Get
    End Property
    Public ReadOnly Property relacion As Nullable(Of Integer)
        Get
            Return _RELATION
        End Get
    End Property
    Public Property fromuserid As Nullable(Of Integer)
        Get
            Return _FROMUSERID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _FROMUSERID = value
        End Set
    End Property
    Public Property touserid As Nullable(Of Integer)
        Get
            Return _TOUSERID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _TOUSERID = value
        End Set
    End Property
    Public Property aceptado As Boolean
        Get
            If _ACCEPTED = "S" Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _ACCEPTED = "S"
            Else
                _ACCEPTED = "N"
            End If
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
#End Region

#Region "Funciones Publicas de la Clase"
    Public Sub New()
        'Realiza la generación del ejemplar estándar.
        MyBase.New()
        'Inicializa las variables privadas de la clase.
        Inicializar()
    End Sub
    Public Sub Inicializar()
        Me._RELATION = Nothing
        Me._FROMUSERID = Nothing
        Me._TOUSERID = Nothing
        Me._ACCEPTED = String.Empty
        Me._FECHA = Nothing
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

                If Me._FROMUSERID.HasValue Then
                    .CommandText = .CommandText & " AND X.MYUSERID = :P_MYUSERID "
                    .Parameters.Add("P_MYUSERID", NpgsqlDbType.Bigint).Value = Me._FROMUSERID.Value
                End If
                If Me._TOUSERID.HasValue Then
                    .CommandText = .CommandText & " AND X.USERID = :P_TOUSERID "
                    .Parameters.Add("P_TOUSERID", NpgsqlDbType.Bigint).Value = Me._TOUSERID.Value
                End If
                If Not String.IsNullOrEmpty(Me._ACCEPTED) Then
                    .CommandText = .CommandText & " AND X.ACCEPTED = :P_ACCEPTED "
                    .Parameters.Add("P_ACCEPTED", NpgsqlDbType.Char, 1).Value = Me._ACCEPTED
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
                If Not IsDBNull(.GetValue(.GetOrdinal("RELATION"))) Then
                    Me._RELATION = Convert.ToInt64(.GetValue(.GetOrdinal("RELATION")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("MYUSERID"))) Then
                    Me._FROMUSERID = Convert.ToInt64(.GetValue(.GetOrdinal("MYUSERID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("USERID"))) Then
                    Me._TOUSERID = Convert.ToInt64(.GetValue(.GetOrdinal("USERID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("ACCEPTED"))) Then
                    Me._ACCEPTED = .GetValue(.GetOrdinal("ACCEPTED"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("FECHA"))) Then
                    Me._FECHA = Convert.ToDateTime(.GetValue(.GetOrdinal("FECHA")))
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
    Public Function nRecuento() As Integer
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
                .CommandText = "SELECT COUNT(1) FROM " & _
                                "(SELECT 0 RELATION, A.FECHA, A.ACCEPTED, A.TOUSERID MYUSERID, B.USERID, B.NOMBRE, B.APELLIDOS, B.IMAGEURL FROM ASPNET_FRIENDSHIP A LEFT JOIN ASPNET_INFO_USUARIO B ON A.FROMUSERID = B.USERID " & _
                                "UNION " & _
                                "SELECT 1 RELATION, A.FECHA, A.ACCEPTED, A.FROMUSERID MYUSERID, B.USERID, B.NOMBRE, B.APELLIDOS, B.IMAGEURL FROM ASPNET_FRIENDSHIP A LEFT JOIN ASPNET_INFO_USUARIO B ON A.TOUSERID = B.USERID ) X " & _
                                "WHERE 1=1 "

                If Me._FROMUSERID.HasValue Then
                    .CommandText = .CommandText & " AND X.MYUSERID = :P_MYUSERID "
                    .Parameters.Add("P_MYUSERID", NpgsqlDbType.Bigint).Value = Me._FROMUSERID.Value
                End If
                If Me._TOUSERID.HasValue Then
                    .CommandText = .CommandText & " AND X.USERID = :P_TOUSERID "
                    .Parameters.Add("P_TOUSERID", NpgsqlDbType.Bigint).Value = Me._TOUSERID.Value
                End If
                If Not String.IsNullOrEmpty(Me._ACCEPTED) Then
                    .CommandText = .CommandText & " AND X.ACCEPTED = :P_ACCEPTED "
                    .Parameters.Add("P_ACCEPTED", NpgsqlDbType.Char, 1).Value = Me._ACCEPTED
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
                .Parameters.Add("P_FROMUSERID", NpgsqlDbType.Bigint).Value = Me._FROMUSERID.Value
                .Parameters.Add("P_TOUSERID", NpgsqlDbType.Bigint).Value = Me._TOUSERID.Value
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
                .Parameters.Add("P_FROMUSERID", NpgsqlDbType.Bigint).Value = Me._FROMUSERID.Value
                .Parameters.Add("P_TOUSERID", NpgsqlDbType.Bigint).Value = Me._TOUSERID.Value
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
                .Parameters.Add("P_FROMUSERID", NpgsqlDbType.Bigint).Value = Me._FROMUSERID.Value
                .Parameters.Add("P_TOUSERID", NpgsqlDbType.Bigint).Value = Me._TOUSERID.Value
                .Parameters.Add("P_ACCEPTED", NpgsqlDbType.Varchar).Value = Me._ACCEPTED
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
                .Parameters.Add("P_FROMUSERID", NpgsqlDbType.Bigint).Value = Me._FROMUSERID.Value
                .Parameters.Add("P_TOUSERID", NpgsqlDbType.Bigint).Value = Me._TOUSERID.Value
                .Parameters.Add("P_ACCEPTED", NpgsqlDbType.Varchar).Value = Me._ACCEPTED
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
                .Parameters.Add("P_FROMUSERID", NpgsqlDbType.Bigint).Value = Me._FROMUSERID.Value
                .Parameters.Add("P_TOUSERID", NpgsqlDbType.Bigint).Value = Me._TOUSERID.Value
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
                .Parameters.Add("P_FROMUSERID", NpgsqlDbType.Bigint).Value = Me._FROMUSERID.Value
                .Parameters.Add("P_TOUSERID", NpgsqlDbType.Bigint).Value = Me._TOUSERID.Value
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
    Public Function ObtenerDatos(ByVal pageSize As Integer, ByVal currentPage As Integer, ByVal psOrderBy As String) As DataTable
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

                If Me._FROMUSERID.HasValue Then
                    .CommandText = .CommandText & " AND X.MYUSERID = :P_MYUSERID "
                    .Parameters.Add("P_MYUSERID", NpgsqlDbType.Bigint).Value = Me._FROMUSERID.Value
                End If
                If Me._TOUSERID.HasValue Then
                    .CommandText = .CommandText & " AND X.USERID = :P_TOUSERID "
                    .Parameters.Add("P_TOUSERID", NpgsqlDbType.Bigint).Value = Me._TOUSERID.Value
                End If
                If Not String.IsNullOrEmpty(Me._ACCEPTED) Then
                    .CommandText = .CommandText & " AND X.ACCEPTED = :P_ACCEPTED "
                    .Parameters.Add("P_ACCEPTED", NpgsqlDbType.Char, 1).Value = Me._ACCEPTED
                End If
                If Not String.IsNullOrEmpty(psOrderBy) Then
                    .CommandText = .CommandText & " ORDER BY " & psOrderBy
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
    Public Function ObtenerDatos(ByVal psFiltro As String, ByVal pageSize As Integer, ByVal currentPage As Integer, ByVal psOrderBy As String) As DataTable
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

                If Me._FROMUSERID.HasValue Then
                    .CommandText = .CommandText & " AND X.MYUSERID = :P_MYUSERID "
                    .Parameters.Add("P_MYUSERID", NpgsqlDbType.Bigint).Value = Me._FROMUSERID.Value
                End If
                If Me._TOUSERID.HasValue Then
                    .CommandText = .CommandText & " AND X.USERID = :P_TOUSERID "
                    .Parameters.Add("P_TOUSERID", NpgsqlDbType.Bigint).Value = Me._TOUSERID.Value
                End If
                If Not String.IsNullOrEmpty(Me._ACCEPTED) Then
                    .CommandText = .CommandText & " AND X.ACCEPTED = :P_ACCEPTED "
                    .Parameters.Add("P_ACCEPTED", NpgsqlDbType.Char, 1).Value = Me._ACCEPTED
                End If
                If Not String.IsNullOrEmpty(psFiltro) Then
                    .CommandText = .CommandText & psFiltro
                End If
                If Not String.IsNullOrEmpty(psOrderBy) Then
                    .CommandText = .CommandText & " ORDER BY " & psOrderBy
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

#Region "Otras funciones de la clase"
    Public Function CommonUsers(ByVal pageSize As Integer, ByVal currentPage As Integer, ByVal nIndicatorId As Integer, ByVal psFilterBy As String, ByVal psOrderBy As String) As DataTable
        '******************************************************************************************
        'Funcion para obtener los amigos que tienen un determinado indicador 
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
                .CommandText = "SELECT A.USERID, A.NOMBRE, A.APELLIDOS, A.IMAGEURL " & _
                               "FROM VW_FRIENDS A " & _
                               "WHERE A.MYUSERID = :P_MYUSERID " & _
                               "AND A.ACCEPTED = 'S' "
                               

                .Parameters.Add("P_MYUSERID", NpgsqlDbType.Bigint).Value = Me._FROMUSERID.Value
                .Parameters.Add("P_ACCEPTED", NpgsqlDbType.Char, 1).Value = Me._ACCEPTED

                If (nIndicatorId > -1) Then
                    .CommandText = .CommandText & "AND EXISTS (SELECT 1 FROM KPI_INDICATOR_USERS C " & _
                               "            WHERE A.USERID = C.USERID " & _
                               "            AND C.INDICATORID = :P_INDICATORID) "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = nIndicatorId
                End If
                If Not String.IsNullOrEmpty(psFilterBy) Then
                    .CommandText = .CommandText & " AND UPPER(A.NOMBRE) LIKE '" & psFilterBy.ToUpper() & "%'"
                End If
                If Not String.IsNullOrEmpty(psOrderBy) Then
                    .CommandText = .CommandText & " ORDER BY " & psOrderBy
                Else
                    .CommandText = .CommandText & " ORDER BY A.NOMBRE ASC, A.APELLIDOS"
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
