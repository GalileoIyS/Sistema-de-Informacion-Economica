Public Class cKPI_WIDGET_EXPRESIONS
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT A.IDEXPRESION, A.IDFORMULA, A.INDICATORID, A.FUNCION, B.TITULO " & _
                                "FROM KPI_WIDGET_EXPRESIONS A LEFT JOIN KPI_INDICATORS B ON A.INDICATORID = B.INDICATORID " & _
                                "WHERE 1=1 "

    Private Const strInserta = "KPIFORMULAS.ADD_KPI_WIDGET_EXPRESIONS"
    Private Const strModifica = "KPIFORMULAS.UPD_KPI_WIDGET_EXPRESIONS"
    Private Const strElimina = "KPIFORMULAS.DEL_KPI_WIDGET_EXPRESIONS"
#End Region

#Region "Variables Privadas"
    Private _IDEXPRESION As  Nullable(Of Integer)
    Private _IDFORMULA As  Nullable(Of Integer)
    Private _INDICATORID As  Nullable(Of Integer)
    Private _FUNCION As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "KPI_WIDGET_EXPRESIONS"
        End Get
    End Property
    Public Property idexpresion As  Nullable(Of Integer)
        Get
            Return _IDEXPRESION
        End Get
        Set(ByVal value As  Nullable(Of Integer))
            _IDEXPRESION = value
        End Set
    End Property
    Public Property idformula As  Nullable(Of Integer)
        Get
            Return _IDFORMULA
        End Get
        Set(ByVal value As  Nullable(Of Integer))
            _IDFORMULA = value
        End Set
    End Property
    Public Property indicatorid As  Nullable(Of Integer)
        Get
            Return _INDICATORID
        End Get
        Set(ByVal value As  Nullable(Of Integer))
            _INDICATORID = value
        End Set
    End Property
    Public Property funcion As String
        Get
            Return _FUNCION
        End Get
        Set(ByVal value As String)
            _FUNCION = value
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
        Me._IDEXPRESION = Nothing
        Me._IDFORMULA = Nothing
        Me._INDICATORID = Nothing
        Me._FUNCION = String.Empty
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

                If Me._IDEXPRESION.HasValue Then
                    .CommandText = .CommandText & " AND A.IDEXPRESION = :P_IDEXPRESION "
                    .Parameters.Add("P_IDEXPRESION", NpgsqlDbType.Bigint).Value = Me._IDEXPRESION.Value
                End If
                If Me._IDFORMULA.HasValue Then
                    .CommandText = .CommandText & " AND A.IDFORMULA = :P_IDFORMULA "
                    .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = Me._IDFORMULA.Value
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
                If Not IsDBNull(.GetValue(.GetOrdinal("IDEXPRESION"))) Then
                    Me._IDEXPRESION = Convert.ToInt64(.GetValue(.GetOrdinal("IDEXPRESION")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("IDFORMULA"))) Then
                    Me._IDFORMULA = Convert.ToInt64(.GetValue(.GetOrdinal("IDFORMULA")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("INDICATORID"))) Then
                    Me._INDICATORID = Convert.ToInt64(.GetValue(.GetOrdinal("INDICATORID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("FUNCION"))) Then
                    Me._FUNCION = .GetValue(.GetOrdinal("FUNCION"))
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
                If Me._IDFORMULA.HasValue Then
                    .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = Me._IDFORMULA.Value
                Else
                    .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                Else
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_FUNCION", NpgsqlDbType.Varchar, 40).Value = Me._FUNCION
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
                If Me._IDFORMULA.HasValue Then
                    .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = Me._IDFORMULA.Value
                Else
                    .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                Else
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_FUNCION", NpgsqlDbType.Varchar, 40).Value = Me._FUNCION
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
                If Me._IDEXPRESION.HasValue Then
                    .Parameters.Add("P_IDEXPRESION", NpgsqlDbType.Bigint).Value = Me._IDEXPRESION.Value
                Else
                    .Parameters.Add("P_IDEXPRESION", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._IDFORMULA.HasValue Then
                    .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = Me._IDFORMULA.Value
                Else
                    .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                Else
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_FUNCION", NpgsqlDbType.Varchar, 40).Value = Me._FUNCION
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
                If Me._IDEXPRESION.HasValue Then
                    .Parameters.Add("P_IDEXPRESION", NpgsqlDbType.Bigint).Value = Me._IDEXPRESION.Value
                Else
                    .Parameters.Add("P_IDEXPRESION", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._IDFORMULA.HasValue Then
                    .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = Me._IDFORMULA.Value
                Else
                    .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                Else
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_FUNCION", NpgsqlDbType.Varchar, 40).Value = Me._FUNCION
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
                .Parameters.Add("P_IDEXPRESION", NpgsqlDbType.Bigint).Value = Me._IDEXPRESION.Value
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
                .Parameters.Add("P_IDEXPRESION", NpgsqlDbType.Bigint).Value = Me._IDEXPRESION.Value
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

                If Me._IDEXPRESION.HasValue Then
                    .CommandText = .CommandText & " AND A.IDEXPRESION = :P_IDEXPRESION "
                    .Parameters.Add("P_IDEXPRESION", NpgsqlDbType.Bigint).Value = Me._IDEXPRESION.Value
                End If
                If Me._IDFORMULA.HasValue Then
                    .CommandText = .CommandText & " AND A.IDFORMULA = :P_IDFORMULA "
                    .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = Me._IDFORMULA.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
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

                If Me._IDEXPRESION.HasValue Then
                    .CommandText = .CommandText & " AND A.IDEXPRESION = :P_IDEXPRESION "
                    .Parameters.Add("P_IDEXPRESION", NpgsqlDbType.Bigint).Value = Me._IDEXPRESION.Value
                End If
                If Me._IDFORMULA.HasValue Then
                    .CommandText = .CommandText & " AND A.IDFORMULA = :P_IDFORMULA "
                    .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = Me._IDFORMULA.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
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

#Region "Otras Funciones Publicas de la Clase"
    Public Function ObtenerOtrasFormulas(ByVal pageSize As Integer, ByVal currentPage As Integer, ByVal psFilterBy As String, ByVal psOrderBy As String) As DataTable
        '******************************************************************************************
        'Funcion para obtener las formulas vinculadas a un determinado indicador 
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
                .CommandText = "SELECT REPLACE (REGEXP_REPLACE (ORIGINAL, '@\[\[([-]*\d+):(\d+):(\w+)' ,'@[[-1:\2:\3','g'),',','' ), MIN(NOMBRE) NOMBRE, COUNT(IDFORMULA) CONTADOR , MIN(IDFORMULA) IDFORMULA, MIN(COLOR) COLOR " & _
                               "FROM KPI_WIDGET_FORMULAS B " & _
                               "WHERE EXISTS (SELECT 1 FROM KPI_WIDGET_EXPRESIONS A " & _
                               "              WHERE A.IDFORMULA = B.IDFORMULA AND A.INDICATORID = :P_INDICATORID) " & _
                               "AND VALIDATED = 0 " & _
                               "GROUP BY REPLACE (REGEXP_REPLACE (ORIGINAL, '@\[\[([-]*\d+):(\d+):(\w+)' ,'@[[-1:\2:\3','g'),',','' )"
                .CommandType = CommandType.Text

                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                If Not String.IsNullOrEmpty(psFilterBy) Then
                    .CommandText = .CommandText & " HAVING UPPER(MIN(NOMBRE)) LIKE '" & psFilterBy.ToUpper() & "%'"
                End If
                If Not String.IsNullOrEmpty(psOrderBy) Then
                    .CommandText = .CommandText & " ORDER BY " & psOrderBy
                Else
                    .CommandText = .CommandText & " ORDER BY NOMBRE ASC"
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
    Public Function ObtenerOtrosIndicadores() As DataTable
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
                .CommandText = "SELECT C.INDICATORID, C.TITULO " & _
                               "FROM KPI_WIDGET_EXPRESIONS A, KPI_INDICATORS C " & _
                               "WHERE A.INDICATORID = C.INDICATORID " & _
                               "AND EXISTS (SELECT 1 FROM KPI_WIDGET_EXPRESIONS B " & _
                               "                  WHERE A.IDFORMULA= B.IDFORMULA AND B.INDICATORID = :P_INDICATORID) " & _
                               "AND A.INDICATORID != :P_INDICATORID " & _
                               "GROUP BY C.INDICATORID, C.TITULO "
                .CommandType = CommandType.Text

                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
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
