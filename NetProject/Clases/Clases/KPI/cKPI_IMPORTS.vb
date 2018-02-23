Public Class cKPI_IMPORTS
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT A.IMPORTID, A.USERID, A.INDICATORID, A.FECHA, " & _
                                "A.NOMBRE, A.DESCRIPCION, A.NUM_DATASETS, A.NUM_DATA_OK, " & _
                                "A.NUM_DATA_ERROR, A.NUM_DATA_LOAD, A.FINALIZADO " & _
                                "FROM KPI_IMPORTS A " & _
                                "WHERE 1=1 "

    Private Const strInserta = "kpis.ADD_KPI_IMPORT"
    Private Const strModifica = "kpis.UPD_KPI_IMPORT"
    Private Const strElimina = "kpis.DEL_KPI_IMPORT"
#End Region

#Region "Variables Privadas"
    Private _IMPORTID As Nullable(Of Integer)
    Private _USERID As Nullable(Of Integer)
    Private _INDICATORID As Nullable(Of Integer)
    Private _FECHA As  Nullable(Of DateTime)
    Private _NOMBRE As String
    Private _DESCRIPCION As String
    Private _NUM_DATASETS As  Nullable(Of Integer)
    Private _NUM_DATA_OK As Nullable(Of Integer)
    Private _NUM_DATA_LOAD As Nullable(Of Integer)
    Private _NUM_DATA_ERROR As Nullable(Of Integer)
    Private _FINALIZADO As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "KPI_IMPORTS"
        End Get
    End Property
    Public Property importid As Nullable(Of Integer)
        Get
            Return _IMPORTID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IMPORTID = value
        End Set
    End Property
    Public Property userid As  Nullable(Of Integer)
        Get
            Return _USERID
        End Get
        Set(ByVal value As  Nullable(Of Integer))
            _USERID = value
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
    Public Property fecha As  Nullable(Of DateTime)
        Get
            Return _FECHA
        End Get
        Set(ByVal value As  Nullable(Of DateTime))
            _FECHA = value
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
    Public Property descripcion As String
        Get
            Return _DESCRIPCION
        End Get
        Set(ByVal value As String)
            _DESCRIPCION = value
        End Set
    End Property
    Public Property num_datasets As  Nullable(Of Integer)
        Get
            Return _NUM_DATASETS
        End Get
        Set(ByVal value As  Nullable(Of Integer))
            _NUM_DATASETS = value
        End Set
    End Property
    Public Property num_data_ok As  Nullable(Of Integer)
        Get
            Return _NUM_DATA_OK
        End Get
        Set(ByVal value As  Nullable(Of Integer))
            _NUM_DATA_OK = value
        End Set
    End Property
    Public Property num_data_load As Nullable(Of Integer)
        Get
            Return _NUM_DATA_LOAD
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _NUM_DATA_LOAD = value
        End Set
    End Property
    Public Property num_data_error As  Nullable(Of Integer)
        Get
            Return _NUM_DATA_ERROR
        End Get
        Set(ByVal value As  Nullable(Of Integer))
            _NUM_DATA_ERROR = value
        End Set
    End Property
    Public Property finalizado As Boolean
        Get
            If _FINALIZADO = "S" Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _FINALIZADO = "S"
            Else
                _FINALIZADO = "N"
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
        Me._IMPORTID = Nothing
        Me._USERID = Nothing
        Me._INDICATORID = Nothing
        Me._FECHA = Nothing
        Me._NOMBRE = String.Empty
        Me._DESCRIPCION = String.Empty
        Me._NUM_DATASETS = Nothing
        Me._NUM_DATA_OK = Nothing
        Me._NUM_DATA_LOAD = Nothing
        Me._NUM_DATA_ERROR = Nothing
        Me._FINALIZADO = String.Empty
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

                If Me._IMPORTID.HasValue Then
                    .CommandText = .CommandText & " AND IMPORTID = :P_IMPORTID "
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
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
                If Not IsDBNull(.GetValue(.GetOrdinal("IMPORTID"))) Then
                    Me._IMPORTID = Convert.ToInt64(.GetValue(.GetOrdinal("IMPORTID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("USERID"))) Then
                    Me._USERID = Convert.ToInt64(.GetValue(.GetOrdinal("USERID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("INDICATORID"))) Then
                    Me._INDICATORID = Convert.ToInt64(.GetValue(.GetOrdinal("INDICATORID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("FECHA"))) Then
                    Me._FECHA = Convert.ToDateTime(.GetValue(.GetOrdinal("FECHA")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("NOMBRE"))) Then
                    Me._NOMBRE = .GetValue(.GetOrdinal("NOMBRE"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("DESCRIPCION"))) Then
                    Me._DESCRIPCION = .GetValue(.GetOrdinal("DESCRIPCION"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("NUM_DATASETS"))) Then
                    Me._NUM_DATASETS = Convert.ToInt64(.GetValue(.GetOrdinal("NUM_DATASETS")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("NUM_DATA_OK"))) Then
                    Me._NUM_DATA_OK = Convert.ToInt64(.GetValue(.GetOrdinal("NUM_DATA_OK")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("NUM_DATA_LOAD"))) Then
                    Me._NUM_DATA_LOAD = Convert.ToInt64(.GetValue(.GetOrdinal("NUM_DATA_LOAD")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("NUM_DATA_ERROR"))) Then
                    Me._NUM_DATA_ERROR = Convert.ToInt64(.GetValue(.GetOrdinal("NUM_DATA_ERROR")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("FINALIZADO"))) Then
                    Me._FINALIZADO = .GetValue(.GetOrdinal("FINALIZADO"))
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
                .CommandText = "SELECT COUNT(IMPORTID) " & _
                               "FROM KPI_IMPORTS " & _
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
                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 100).Value = Me._NOMBRE
                .Parameters.Add("P_DESCRIPCION", NpgsqlDbType.Varchar, 500).Value = Me._DESCRIPCION
                .Parameters.Add("P_FINALIZADO", NpgsqlDbType.Char, 1).Value = "N"
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._IMPORTID = nResultado
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            Me._IMPORTID = -1
        Finally
            dbconexion.Close()
        End Try

        If Me._IMPORTID >= 0 Then
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
                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 100).Value = Me._NOMBRE
                .Parameters.Add("P_DESCRIPCION", NpgsqlDbType.Varchar, 500).Value = Me._DESCRIPCION
                .Parameters.Add("P_FINALIZADO", NpgsqlDbType.Char, 1).Value = "N"
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._IMPORTID = nResultado
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            Me._IMPORTID = -1
        End Try

        If Me._IMPORTID >= 0 Then
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
                .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
                If Me._NUM_DATASETS.HasValue Then
                    .Parameters.Add("P_NUM_DATASETS", NpgsqlDbType.Bigint).Value = Me._NUM_DATASETS.Value
                Else
                    .Parameters.Add("P_NUM_DATASETS", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._NUM_DATA_OK.HasValue Then
                    .Parameters.Add("P_NUM_DATA_OK", NpgsqlDbType.Bigint).Value = Me._NUM_DATA_OK.Value
                Else
                    .Parameters.Add("P_NUM_DATA_OK", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._NUM_DATA_ERROR.HasValue Then
                    .Parameters.Add("P_NUM_DATA_ERROR", NpgsqlDbType.Bigint).Value = Me._NUM_DATA_ERROR.Value
                Else
                    .Parameters.Add("P_NUM_DATA_ERROR", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_FINALIZADO", NpgsqlDbType.Char, 1).Value = Me._FINALIZADO
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
                .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
                If Me._NUM_DATASETS.HasValue Then
                    .Parameters.Add("P_NUM_DATASETS", NpgsqlDbType.Bigint).Value = Me._NUM_DATASETS.Value
                Else
                    .Parameters.Add("P_NUM_DATASETS", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._NUM_DATA_OK.HasValue Then
                    .Parameters.Add("P_NUM_DATA_OK", NpgsqlDbType.Bigint).Value = Me._NUM_DATA_OK.Value
                Else
                    .Parameters.Add("P_NUM_DATA_OK", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._NUM_DATA_ERROR.HasValue Then
                    .Parameters.Add("P_NUM_DATA_ERROR", NpgsqlDbType.Bigint).Value = Me._NUM_DATA_ERROR.Value
                Else
                    .Parameters.Add("P_NUM_DATA_ERROR", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_FINALIZADO", NpgsqlDbType.Char, 1).Value = Me._FINALIZADO
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
                .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
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
                .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
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
                .CommandText = strConsulta
                .CommandType = CommandType.Text

                If Me._IMPORTID.HasValue Then
                    .CommandText = .CommandText & " AND A.IMPORTID = :P_IMPORTID "
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
                End If
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Not String.IsNullOrEmpty(Me._NOMBRE) Then
                    .CommandText = .CommandText & " AND LOWER(A.NOMBRE) LIKE :P_NOMBRE "
                    .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 250).Value = "%" & Me._NOMBRE.ToLower() & "%"
                End If
                .CommandText = .CommandText & " ORDER BY A.FECHA DESC "
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
    Public Function ObtenerDatos(ByVal pageSize As Integer, ByVal currentPage As Integer, ByVal psFiltro As String, ByVal psOrderBy As String) As DataTable
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
                .CommandText = strConsulta

                If Me._IMPORTID.HasValue Then
                    .CommandText = .CommandText & " AND A.IMPORTID = :P_IMPORTID "
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
                End If
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Not String.IsNullOrEmpty(Me._NOMBRE) Then
                    .CommandText = .CommandText & " AND LOWER(A.NOMBRE) LIKE :P_NOMBRE "
                    .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 250).Value = "%" & Me._NOMBRE.ToLower() & "%"
                End If
                If Not String.IsNullOrEmpty(psFiltro) Then
                    .CommandText = .CommandText & psFiltro
                End If
                If Not String.IsNullOrEmpty(psOrderBy) Then
                    .CommandText = .CommandText & " ORDER BY " & psOrderBy
                Else
                    .CommandText = .CommandText & " ORDER BY A.FECHA DESC "
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
