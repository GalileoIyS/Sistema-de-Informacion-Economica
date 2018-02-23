Public Class cKPI_DIMENSION_VALUES
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT DIMENSIONID, DATASETID, CODIGO, UPPERCODIGO " & _
                                "FROM KPI_DIMENSION_VALUES " & _
                                "WHERE 1=1 "

    Private Const strInserta = "kpis.ADD_KPI_DIMENSION_VALUES"
    Private Const strModifica = "kpis.UPD_KPI_DIMENSION_VALUES"
    Private Const strElimina = "kpis.DEL_KPI_DIMENSION_VALUES"
#End Region

#Region "Variables Privadas"
    Private _DIMENSIONID As Nullable(Of Integer)
    Private _DATASETID As Nullable(Of Integer)
    Private _CODIGO As String
    Private _UPPERCODIGO As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "KPI_DIMENSION_VALUES"
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
    Public Property datasetid As Nullable(Of Integer)
        Get
            Return _DATASETID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _DATASETID = value
        End Set
    End Property
    Public Property codigo As String
        Get
            Return _CODIGO
        End Get
        Set(ByVal value As String)
            If (value.Length > 250) Then
                _CODIGO = value.Substring(0, 250)
            Else
                _CODIGO = value
            End If
        End Set
    End Property
    Public Property uppercodigo As String
        Get
            Return _UPPERCODIGO
        End Get
        Set(ByVal value As String)
            If (value.Length > 250) Then
                _UPPERCODIGO = value.Substring(0, 250)
            Else
                _UPPERCODIGO = value
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
        Me._DATASETID = Nothing
        Me._CODIGO = String.Empty
        Me._UPPERCODIGO = String.Empty
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
                    .CommandText = .CommandText & " AND DIMENSIONID = :P_DIMENSIONID "
                    .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                End If
                If Me._DATASETID.HasValue Then
                    .CommandText = .CommandText & " AND DATASETID = :P_DATASETID "
                    .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                End If
                If Not String.IsNullOrEmpty(Me._CODIGO) Then
                    .CommandText = .CommandText & " AND CODIGO = :P_CODIGO "
                    .Parameters.Add("P_CODIGO", NpgsqlDbType.Varchar, 15).Value = Me._CODIGO
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
                If Not IsDBNull(.GetValue(.GetOrdinal("DATASETID"))) Then
                    Me._DATASETID = Convert.ToInt64(.GetValue(.GetOrdinal("DATASETID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("CODIGO"))) Then
                    Me._CODIGO = .GetValue(.GetOrdinal("CODIGO"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("UPPERCODIGO"))) Then
                    Me._UPPERCODIGO = .GetValue(.GetOrdinal("UPPERCODIGO"))
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
                .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                .Parameters.Add("P_CODIGO", NpgsqlDbType.Varchar, 250).Value = Me._CODIGO
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
    Public Function bInsertarFromImport() As Boolean
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
                .CommandText = "kpis.import_kpi_dimension_values"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                .Parameters.Add("P_CODIGO", NpgsqlDbType.Varchar, 250).Value = Me._CODIGO
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
                .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                .Parameters.Add("P_CODIGO", NpgsqlDbType.Varchar, 250).Value = Me._CODIGO
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
                .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                .Parameters.Add("P_CODIGO", NpgsqlDbType.Varchar, 250).Value = Me._CODIGO
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
                .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                .Parameters.Add("P_CODIGO", NpgsqlDbType.Varchar, 250).Value = Me._CODIGO
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
                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                .Parameters.Add("P_CODIGO", NpgsqlDbType.Varchar, 250).Value = Me._CODIGO
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

                If Me._DIMENSIONID.HasValue Then
                    .CommandText = .CommandText & " AND DIMENSIONID = :P_DIMENSIONID "
                    .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                End If
                If Me._DATASETID.HasValue Then
                    .CommandText = .CommandText & " AND DATASETID = :P_DATASETID "
                    .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                End If
                If Not String.IsNullOrEmpty(Me._CODIGO) Then
                    .CommandText = .CommandText & " AND TRIM(LOWER(CODIGO)) LIKE :P_CODIGO "
                    .Parameters.Add("P_CODIGO", NpgsqlDbType.Varchar, 50).Value = "%" & Me._CODIGO.Trim().ToLower() & "%"
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

                If Me._DIMENSIONID.HasValue Then
                    .CommandText = .CommandText & " AND DIMENSIONID = :P_DIMENSIONID "
                    .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                End If
                If Me._DATASETID.HasValue Then
                    .CommandText = .CommandText & " AND DATASETID = :P_DATASETID "
                    .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                End If
                If Not String.IsNullOrEmpty(Me._CODIGO) Then
                    .CommandText = .CommandText & " AND TRIM(LOWER(CODIGO)) LIKE :P_CODIGO "
                    .Parameters.Add("P_CODIGO", NpgsqlDbType.Varchar, 50).Value = "%" & Me._CODIGO.Trim().ToLower() & "%"
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
    Public Function Recuento() As Integer
        Dim nResultado As Integer = 0
        Dim SqlConsulta As New NpgsqlCommand
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
                .CommandType = CommandType.Text
                .CommandText = "SELECT COUNT(DIMENSIONID) " & _
                                "FROM KPI_DIMENSION_VALUES " & _
                                "WHERE 1=1 "

                If Me._DIMENSIONID.HasValue Then
                    .CommandText = .CommandText & " AND DIMENSIONID = :P_DIMENSIONID "
                    .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                End If
                If Me._DATASETID.HasValue Then
                    .CommandText = .CommandText & " AND DATASETID = :P_DATASETID "
                    .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                End If
                If Not String.IsNullOrEmpty(Me._CODIGO) Then
                    .CommandText = .CommandText & " AND CODIGO = :P_CODIGO "
                    .Parameters.Add("P_CODIGO", NpgsqlDbType.Varchar, 15).Value = Me._CODIGO
                End If
            End With
            nResultado = SqlConsulta.ExecuteScalar()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido obtener los resultados de la consulta de la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            dbconexion.Close()
            dbconexion.Dispose()
            Return -1
        Finally
            'Cerramos las Conexiones
            dbconexion.Close()
        End Try

        Return nResultado

    End Function
#End Region

#Region "Funciones específicas"
    Public Function ObtenerDatasets(ByVal psDatasets As String, ByVal pnUserID As Integer) As String
        Dim SqlConsulta As New NpgsqlCommand
        Dim drDATOS As NpgsqlDataReader
        Dim dbconexion As New NpgsqlConnection
        Dim _sDatasets As String = String.Empty
        Dim sComa As String = String.Empty

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
            With SqlConsulta
                .Connection = dbconexion
                .CommandText = "SELECT A.DATASETID FROM KPI_DIMENSION_VALUES A, KPI_DATASETS B " & _
                               "WHERE A.DATASETID = B.DATASETID " & _
                               "AND A.DIMENSIONID = :P_DIMENSIONID " & _
                               "AND A.UPPERCODIGO = :P_CODIGO " & _
                               "AND B.USERID = :P_USERID "
                .CommandType = CommandType.Text

                .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                .Parameters.Add("P_CODIGO", NpgsqlDbType.Varchar, 50).Value = Me._UPPERCODIGO.Trim().ToUpper()
                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = pnUserID
            End With
            drDATOS = SqlConsulta.ExecuteReader()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido obtener los resultados de la consulta de la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            dbconexion.Close()
            dbconexion.Dispose()
            Return String.Empty
        End Try

        With drDATOS
            While .Read
                If Not IsDBNull(.GetValue(.GetOrdinal("DATASETID"))) Then
                    _sDatasets = sComa & .GetValue(.GetOrdinal("DATASETID"))
                    sComa = ", "
                End If
            End While
        End With

        Return _sDatasets

    End Function
    Public Function ObtenerValores() As DataTable
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
                .CommandText = "SELECT CODIGO FROM KPI_DIMENSION_VALUES " & _
                               "WHERE 1=1 "
                .CommandType = CommandType.Text

                If Me._DIMENSIONID.HasValue Then
                    .CommandText = .CommandText & " AND DIMENSIONID = :P_DIMENSIONID "
                    .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                End If
                If Me._DATASETID.HasValue Then
                    .CommandText = .CommandText & " AND DATASETID = :P_DATASETID "
                    .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                End If
                If Not String.IsNullOrEmpty(Me._CODIGO) Then
                    .CommandText = .CommandText & " AND TRIM(LOWER(CODIGO)) LIKE :P_CODIGO "
                    .Parameters.Add("P_CODIGO", NpgsqlDbType.Varchar, 50).Value = "%" & Me._CODIGO.Trim().ToLower() & "%"
                End If
                .CommandText = .CommandText & " GROUP BY CODIGO ORDER BY CODIGO ASC "
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
    Public Function ObtenerValoresAsignados(ByVal pnIdFilter As Integer) As DataTable
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
                .CommandText = "SELECT A.CODIGO FROM KPI_WIDGET_FILTER_VALUES A " & _
                               "WHERE A.DIMENSIONID = :P_DIMENSIONID " & _
                               "AND A.IDFILTER = :P_IDFILTER "
                .CommandType = CommandType.Text

                .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                .Parameters.Add("P_IDFILTER", NpgsqlDbType.Bigint).Value = pnIdFilter
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
    Public Function ObtenerValoresNoAsignados(ByVal pnIdFilter As Integer) As DataTable
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
                .CommandText = "SELECT A.CODIGO FROM KPI_DIMENSION_VALUES A " & _
                               "WHERE A.DIMENSIONID = :P_DIMENSIONID " & _
                               "AND NOT EXISTS (SELECT 1 FROM KPI_WIDGET_FILTER_VALUES B WHERE B.IDFILTER = :P_IDFILTER AND B.UPPERCODIGO = A.UPPERCODIGO) " & _
                               "GROUP BY A.CODIGO ORDER BY A.CODIGO ASC "
                .CommandType = CommandType.Text

                .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                .Parameters.Add("P_IDFILTER", NpgsqlDbType.Bigint).Value = pnIdFilter
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
