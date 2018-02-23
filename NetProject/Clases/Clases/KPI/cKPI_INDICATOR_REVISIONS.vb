Public Class cKPI_INDICATOR_REVISIONS
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT A.REVISIONID, A.INDICATORID, A.FECHA, A.TITULO, A.RESUMEN, " & _
                                "A.DESCRIPCION, A.UNIDAD, A.SIMBOLO, A.USERID, A.FUNCION_AGREGADA, " & _
                                "B.NOMBRE, B.APELLIDOS, B.IMAGEURL " & _
                                "FROM KPI_INDICATOR_REVISIONS A LEFT JOIN ASPNET_INFO_USUARIO B ON A.USERID = B.USERID " & _
                                "WHERE 1=1 "

    Private Const strInserta = "kpis.add_kpi_indicator_revisions"
    Private Const strModifica = "kpis.upd_kpi_indicator_revisions"
    Private Const strElimina = "kpis.del_kpi_indicator_revisions"
#End Region

#Region "Variables Privadas"
    Private _REVISIONID As Nullable(Of Integer)
    Private _INDICATORID As Nullable(Of Integer)
    Private _FECHA As Nullable(Of DateTime)
    Private _TITULO As String
    Private _RESUMEN As String
    Private _DESCRIPCION As String
    Private _UNIDAD As String
    Private _SIMBOLO As String
    Private _USERID As Nullable(Of Integer)
    Private _FUNCION_AGREGADA As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "kpi_indicator_revisions"
        End Get
    End Property
    Public Property revisionid As Nullable(Of Integer)
        Get
            Return _REVISIONID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _REVISIONID = value
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
    Public Property fecha As Nullable(Of DateTime)
        Get
            Return _FECHA
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FECHA = value
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
    Public Property resumen As String
        Get
            Return _RESUMEN
        End Get
        Set(ByVal value As String)
            _RESUMEN = value
        End Set
    End Property
    Public Property descripcion As String
        Get
            Return _DESCRIPCION
        End Get
        Set(ByVal value As String)
            _DESCRIPCION = value
        End Set
    End Property
    Public Property unidad As String
        Get
            Return _UNIDAD
        End Get
        Set(ByVal value As String)
            _UNIDAD = value
        End Set
    End Property
    Public Property simbolo As String
        Get
            Return _SIMBOLO
        End Get
        Set(ByVal value As String)
            _SIMBOLO = value
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
    Public Property funcion_agregada As String
        Get
            Return _FUNCION_AGREGADA
        End Get
        Set(ByVal value As String)
            _FUNCION_AGREGADA = value
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
        Me._REVISIONID = Nothing
        Me._INDICATORID = Nothing
        Me._FECHA = Nothing
        Me._TITULO = String.Empty
        Me._RESUMEN = String.Empty
        Me._DESCRIPCION = String.Empty
        Me._UNIDAD = String.Empty
        Me._SIMBOLO = String.Empty
        Me._USERID = Nothing
        Me._FUNCION_AGREGADA = String.Empty
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

                If Me._REVISIONID.HasValue Then
                    .CommandText = .CommandText & " AND A.REVISIONID = :P_REVISIONID "
                    .Parameters.Add("P_REVISIONID", NpgsqlDbType.Bigint).Value = Me._REVISIONID.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
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
                If Not IsDBNull(.GetValue(.GetOrdinal("REVISIONID"))) Then
                    Me._REVISIONID = Convert.ToInt64(.GetValue(.GetOrdinal("REVISIONID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("INDICATORID"))) Then
                    Me._INDICATORID = Convert.ToInt64(.GetValue(.GetOrdinal("INDICATORID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("FECHA"))) Then
                    Me._FECHA = Convert.ToDateTime(.GetValue(.GetOrdinal("FECHA")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("TITULO"))) Then
                    Me._TITULO = .GetValue(.GetOrdinal("TITULO"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("RESUMEN"))) Then
                    Me._RESUMEN = .GetValue(.GetOrdinal("RESUMEN"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("DESCRIPCION"))) Then
                    Me._DESCRIPCION = .GetValue(.GetOrdinal("DESCRIPCION"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("UNIDAD"))) Then
                    Me._UNIDAD = .GetValue(.GetOrdinal("UNIDAD"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("SIMBOLO"))) Then
                    Me._SIMBOLO = .GetValue(.GetOrdinal("SIMBOLO"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("USERID"))) Then
                    Me._USERID = Convert.ToInt64(.GetValue(.GetOrdinal("USERID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("FUNCION_AGREGADA"))) Then
                    Me._FUNCION_AGREGADA = .GetValue(.GetOrdinal("FUNCION_AGREGADA"))
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
        Dim nResultado As Integer

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
                .CommandText = "SELECT COUNT(REVISIONID) " & _
                               "FROM KPI_INDICATOR_REVISIONS " & _
                               "WHERE 1=1 "
                .CommandType = CommandType.Text

                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
            End With

            nResultado = SqlConsulta.ExecuteScalar()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido obtener los resultados de la consulta de la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
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
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID
                .Parameters.Add("P_TITULO", NpgsqlDbType.Varchar).Value = Me._TITULO
                .Parameters.Add("P_RESUMEN", NpgsqlDbType.Varchar).Value = Me._RESUMEN
                .Parameters.Add("P_DESCRIPCION", NpgsqlDbType.Text).Value = Me._DESCRIPCION
                .Parameters.Add("P_UNIDAD", NpgsqlDbType.Varchar).Value = Me._UNIDAD
                .Parameters.Add("P_SIMBOLO", NpgsqlDbType.Varchar).Value = Me._SIMBOLO
                If Me._USERID.HasValue Then
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                Else
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_FUNCION_AGREGADA", NpgsqlDbType.Varchar).Value = Me._FUNCION_AGREGADA
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._REVISIONID = nResultado
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            Me._REVISIONID = -1
        Finally
            dbconexion.Close()
        End Try

        If Me._REVISIONID < 0 Then
            Return False
        Else
            Return True
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
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID
                .Parameters.Add("P_TITULO", NpgsqlDbType.Varchar).Value = Me._TITULO
                .Parameters.Add("P_RESUMEN", NpgsqlDbType.Varchar).Value = Me._RESUMEN
                .Parameters.Add("P_DESCRIPCION", NpgsqlDbType.Text).Value = Me._DESCRIPCION
                .Parameters.Add("P_UNIDAD", NpgsqlDbType.Varchar).Value = Me._UNIDAD
                .Parameters.Add("P_SIMBOLO", NpgsqlDbType.Varchar).Value = Me._SIMBOLO
                If Me._USERID.HasValue Then
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                Else
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_FUNCION_AGREGADA", NpgsqlDbType.Varchar).Value = Me._FUNCION_AGREGADA
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._REVISIONID = nResultado
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            Me._REVISIONID = -1
        End Try

        If Me._REVISIONID < 0 Then
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
                .Parameters.Add("P_REVISIONID", NpgsqlDbType.Bigint).Value = Me._REVISIONID
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID
                .Parameters.Add("P_TITULO", NpgsqlDbType.Varchar).Value = Me._TITULO
                .Parameters.Add("P_RESUMEN", NpgsqlDbType.Varchar).Value = Me._RESUMEN
                .Parameters.Add("P_DESCRIPCION", NpgsqlDbType.Text).Value = Me._DESCRIPCION
                .Parameters.Add("P_UNIDAD", NpgsqlDbType.Varchar).Value = Me._UNIDAD
                .Parameters.Add("P_SIMBOLO", NpgsqlDbType.Varchar).Value = Me._SIMBOLO
                If Me._USERID.HasValue Then
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                Else
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_FUNCION_AGREGADA", NpgsqlDbType.Varchar).Value = Me._FUNCION_AGREGADA
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
                .Parameters.Add("P_REVISIONID", NpgsqlDbType.Bigint).Value = Me._REVISIONID
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID
                .Parameters.Add("P_TITULO", NpgsqlDbType.Varchar).Value = Me._TITULO
                .Parameters.Add("P_RESUMEN", NpgsqlDbType.Varchar).Value = Me._RESUMEN
                .Parameters.Add("P_DESCRIPCION", NpgsqlDbType.Text).Value = Me._DESCRIPCION
                .Parameters.Add("P_UNIDAD", NpgsqlDbType.Varchar).Value = Me._UNIDAD
                .Parameters.Add("P_SIMBOLO", NpgsqlDbType.Varchar).Value = Me._SIMBOLO
                If Me._USERID.HasValue Then
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                Else
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_FUNCION_AGREGADA", NpgsqlDbType.Varchar).Value = Me._FUNCION_AGREGADA
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
                .Parameters.Add("P_REVISIONID", NpgsqlDbType.Bigint).Value = Me._REVISIONID
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
                .Parameters.Add("P_REVISIONID", NpgsqlDbType.Bigint).Value = Me._REVISIONID
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

                If Me._REVISIONID.HasValue Then
                    .CommandText = .CommandText & " AND A.REVISIONID = :P_REVISIONID "
                    .Parameters.Add("P_REVISIONID", NpgsqlDbType.Bigint).Value = Me._REVISIONID.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
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
                .CommandText = strConsulta & psFiltro

                If Me._REVISIONID.HasValue Then
                    .CommandText = .CommandText & " AND A.REVISIONID = :P_REVISIONID "
                    .Parameters.Add("P_REVISIONID", NpgsqlDbType.Bigint).Value = Me._REVISIONID.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
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
