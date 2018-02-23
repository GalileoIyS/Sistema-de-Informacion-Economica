Public Class cKPI_DIMENSIONS
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT A.DIMENSIONID, A.INDICATORID, A.USERID, A.FECHA, A.NOMBRE, A.DESCRIPCION, " & _
                                "COALESCE(C.NUMDATOS,0) NUMDATOS, D.NOMBRE USERFIRSTNAME, D.APELLIDOS USERLASTNAME " & _
                                "FROM KPI_DIMENSIONS A " & _
                                "LEFT JOIN (SELECT Z.DIMENSIONID, COUNT(Z.UPPERCODIGO) NUMDATOS FROM (SELECT DIMENSIONID, UPPERCODIGO FROM KPI_DIMENSION_VALUES " & _
                                "                                                                GROUP BY DIMENSIONID, UPPERCODIGO ) Z GROUP BY Z.DIMENSIONID ) C " & _
                                "           ON C.DIMENSIONID = A.DIMENSIONID " & _
                                "LEFT JOIN ASPNET_INFO_USUARIO D ON D.USERID = A.USERID " & _
                                "WHERE 1=1 "

    Private Const strInserta = "kpis.ADD_KPI_DIMENSIONS"
    Private Const strModifica = "kpis.UPD_KPI_DIMENSIONS"
    Private Const strElimina = "kpis.DEL_KPI_DIMENSIONS"
#End Region

#Region "Variables Privadas"
    Private _DIMENSIONID As Nullable(Of Integer)
    Private _INDICATORID As Nullable(Of Integer)
    Private _USERID As Nullable(Of Integer)
    Private _FECHA As Nullable(Of DateTime)
    Private _NOMBRE As String
    Private _DESCRIPCION As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "KPI_DIMENSIONS"
        End Get
    End Property
    Public Property dimensionid As Nullable(Of Integer)
        Get
            Return _DIMENSIONID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _DIMENSIONID = value
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
            If (value.Length > 100) Then
                _NOMBRE = value.Substring(0, 100)
            Else
                _NOMBRE = value
            End If
        End Set
    End Property
    Public Property descripcion As String
        Get
            Return _DESCRIPCION
        End Get
        Set(ByVal value As String)
            If (value.Length > 500) Then
                _DESCRIPCION = value.Substring(0, 500)
            Else
                _DESCRIPCION = value
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
        Me._DIMENSIONID = Nothing
        Me._INDICATORID = Nothing
        Me._USERID = Nothing
        Me._FECHA = Nothing
        Me._NOMBRE = String.Empty
        Me._DESCRIPCION = String.Empty
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

                If Me._DIMENSIONID.HasValue Then
                    .CommandText = .CommandText & " AND A.DIMENSIONID = :P_DIMENSIONID "
                    .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                End If
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
                If Not IsDBNull(.GetValue(.GetOrdinal("DIMENSIONID"))) Then
                    Me._DIMENSIONID = Convert.ToInt64(.GetValue(.GetOrdinal("DIMENSIONID")))
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
                If Not IsDBNull(.GetValue(.GetOrdinal("NOMBRE"))) Then
                    Me._NOMBRE = .GetValue(.GetOrdinal("NOMBRE"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("DESCRIPCION"))) Then
                    Me._DESCRIPCION = .GetValue(.GetOrdinal("DESCRIPCION"))
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
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 100).Value = Me._NOMBRE
                .Parameters.Add("P_DESCRIPCION", NpgsqlDbType.Varchar, 500).Value = Me._DESCRIPCION
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._DIMENSIONID = nResultado
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
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 100).Value = Me._NOMBRE
                .Parameters.Add("P_DESCRIPCION", NpgsqlDbType.Varchar, 500).Value = Me._DESCRIPCION
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._DIMENSIONID = nResultado
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
                .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 100).Value = Me._NOMBRE
                .Parameters.Add("P_DESCRIPCION", NpgsqlDbType.Varchar, 500).Value = Me._DESCRIPCION
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
                .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 100).Value = Me._NOMBRE
                .Parameters.Add("P_DESCRIPCION", NpgsqlDbType.Varchar, 500).Value = Me._DESCRIPCION
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
                .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
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
                .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
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

                If Me._DIMENSIONID.HasValue Then
                    .CommandText = .CommandText & " AND A.DIMENSIONID = :P_DIMENSIONID "
                    .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
                If Not String.IsNullOrEmpty(Me._NOMBRE) Then
                    .CommandText = .CommandText & " AND LOWER(A.NOMBRE) LIKE :P_NOMBRE "
                    .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 250).Value = "%" & Me._NOMBRE.ToLower() & "%"
                End If
                .CommandText = .CommandText & " ORDER BY A.FECHA DESC "
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

                If Me._DIMENSIONID.HasValue Then
                    .CommandText = .CommandText & " AND A.DIMENSIONID = :P_DIMENSIONID "
                    .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
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
    Public Function Recuento() As Integer
        Dim dbconexion As New NpgsqlConnection
        Dim nResultado As Int64 = 0

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
                .CommandText = "SELECT COUNT(A.DIMENSIONID) CONTADOR " & _
                                "FROM KPI_DIMENSIONS A " & _
                                "WHERE 1=1 "
                .CommandType = CommandType.Text

                If Me._DIMENSIONID.HasValue Then
                    .CommandText = .CommandText & " AND A.DIMENSIONID = :P_DIMENSIONID "
                    .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
                If Not String.IsNullOrEmpty(Me._NOMBRE) Then
                    .CommandText = .CommandText & " AND LOWER(A.NOMBRE) LIKE :P_NOMBRE "
                    .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 250).Value = "%" & Me._NOMBRE.ToLower() & "%"
                End If
            End With

            nResultado = Convert.ToDecimal(SqlConsulta.ExecuteScalar())

        Catch excp As NpgsqlException
            MsgBox("No se ha podido obtener los resultados de la consulta de la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return nResultado

    End Function
#End Region

#Region "Otras funciones específicas"
    Public Function ObtenerRecuento() As DataTable
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
                .CommandText = "SELECT * FROM KPI_DIMENSIONS A, " & _
                               "(SELECT DIMENSIONID, COUNT(1) FROM KPI_DIMENSION_VALUES B GROUP BY DIMENSIONID) B " & _
                               "WHERE A.DIMENSIONID = B.DIMENSIONID "
                .CommandType = CommandType.Text

                If Me._DIMENSIONID.HasValue Then
                    .CommandText = .CommandText & " AND A.DIMENSIONID = :P_DIMENSIONID "
                    .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
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
