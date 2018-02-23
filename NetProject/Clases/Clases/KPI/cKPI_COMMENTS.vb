Public Class cKPI_COMMENTS
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT A.COMMENTID, A.INDICATORID, A.USERID, A.FECHA, A.COMENTARIO, " & _
                                "A.PADREID, B.NOMBRE, B.APELLIDOS, B.IMAGEURL, C.TITULO " & _
                                "FROM KPI_COMMENTS A " & _
                                "LEFT JOIN ASPNET_INFO_USUARIO B ON A.USERID = B.USERID " & _
                                "LEFT JOIN KPI_INDICATORS C ON A.INDICATORID = C.INDICATORID " & _
                                "WHERE 1=1 "

    Private Const strInserta = "kpis.ADD_KPI_COMMENTS"
    Private Const strModifica = "kpis.UPD_KPI_COMMENTS"
    Private Const strElimina = "kpis.DEL_KPI_COMMENTS"
#End Region

#Region "Variables Privadas"
    Private _COMMENTID As Nullable(Of Integer)
    Private _INDICATORID As Nullable(Of Integer)
    Private _USERID As Nullable(Of Integer)
    Private _FECHA As Nullable(Of DateTime)
    Private _COMENTARIO As String
    Private _PADREID As Nullable(Of Integer)
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "KPI_COMMENTS"
        End Get
    End Property
    Public Property commentid As Nullable(Of Integer)
        Get
            Return _COMMENTID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _COMMENTID = value
        End Set
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
    Public Property comentario As String
        Get
            Return _COMENTARIO
        End Get
        Set(ByVal value As String)
            If (value.Length > 4000) Then
                _COMENTARIO = value.Substring(0, 4000)
            Else
                _COMENTARIO = value
            End If
        End Set
    End Property
    Public Property padreid As Nullable(Of Integer)
        Get
            Return _PADREID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _PADREID = value
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
        Me._COMMENTID = Nothing
        Me._INDICATORID = Nothing
        Me._USERID = Nothing
        Me._FECHA = Nothing
        Me._COMENTARIO = String.Empty
        Me._PADREID = Nothing
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

                If Me._COMMENTID.HasValue Then
                    .CommandText = .CommandText & " AND A.COMMENTID = :P_COMMENTID"
                    .Parameters.Add("P_COMMENTID", NpgsqlDbType.Bigint).Value = Me._COMMENTID.Value
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
                If Not IsDBNull(.GetValue(.GetOrdinal("COMMENTID"))) Then
                    Me._COMMENTID = Convert.ToInt64(.GetValue(.GetOrdinal("COMMENTID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("INDICATORID"))) Then
                    Me._INDICATORID = Convert.ToInt64(.GetValue(.GetOrdinal("INDICATORID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("USERID"))) Then
                    Me._USERID = Convert.ToInt64(.GetValue(.GetOrdinal("USERID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("FECHA"))) Then
                    Me._FECHA = Convert.ToDateTime(.GetValue(.GetOrdinal("FECHA")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("COMENTARIO"))) Then
                    Me._COMENTARIO = .GetValue(.GetOrdinal("COMENTARIO"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("PADREID"))) Then
                    Me._PADREID = Convert.ToInt64(.GetValue(.GetOrdinal("PADREID")))
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
        Dim nResultados As Integer = 0
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
                .CommandText = "SELECT COUNT(A.COMMENTID) " & _
                               "FROM KPI_COMMENTS A " & _
                               "WHERE 1=1 "

               If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID"
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID"
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
                If Me._PADREID.HasValue Then
                    .CommandText = .CommandText & " AND A.PADREID = :P_PADREID"
                    .Parameters.Add("P_PADREID", NpgsqlDbType.Bigint).Value = Me._PADREID.Value
                End If
            End With

            nResultados = SqlConsulta.ExecuteScalar()

        Catch excp As NpgsqlException
            MsgBox("No se ha podido obtener los resultados de la consulta de la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return Convert.ToInt32(nResultados)

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
                If Me._INDICATORID.HasValue Then
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                Else
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._USERID.HasValue Then
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                Else
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_COMENTARIO", NpgsqlDbType.Varchar, 4000).Value = Me._COMENTARIO
                If Me._PADREID.HasValue Then
                    .Parameters.Add("P_PADREID", NpgsqlDbType.Bigint).Value = Me._PADREID.Value
                Else
                    .Parameters.Add("P_PADREID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._COMMENTID = nResultado
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            Me._COMMENTID = -1
        Finally
            dbconexion.Close()
        End Try

        If Me._COMMENTID < 0 Then
            Return False
        Else
            Return True
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
                .Parameters.Add("P_COMMENTID", NpgsqlDbType.Bigint).Value = Me._COMMENTID
                .Parameters.Add("P_COMENTARIO", NpgsqlDbType.Varchar, 4000).Value = Me._COMENTARIO
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
                .Parameters.Add("P_COMMENTID", NpgsqlDbType.Bigint).Value = Me._COMMENTID
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
                If Me._PADREID.HasValue Then
                    If Me._PADREID = -1 Then
                        .CommandText = .CommandText & " AND A.PADREID IS NULL "
                    Else
                        .CommandText = .CommandText & " AND A.PADREID = :P_PADREID "
                        .Parameters.Add("P_PADREID", NpgsqlDbType.Bigint).Value = Me._PADREID.Value
                    End If
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

            With SqlConsulta
                .Connection = dbconexion
                .CommandText = strConsulta
                .CommandType = CommandType.Text

                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID"
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID"
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
                If Me._PADREID.HasValue Then
                    .CommandText = .CommandText & " AND A.PADREID = :P_PADREID"
                    .Parameters.Add("P_PADREID", NpgsqlDbType.Bigint).Value = Me._PADREID.Value
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
