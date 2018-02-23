Public Class cKPI_DATASET_VALUES
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT VALORID, DATASETID, INDICATORID, USERID, FECHA, FECHA_FIN, VALOR, EJERCICIO " & _
                                "SEMESTRE, TRIMESTRE, MES, QUINCENA, SEMANA, IMPORTID " & _
                                "FROM KPI_DATASET_VALUES " & _
                                "WHERE 1=1 "

    Private Const strInserta = "kpis.ADD_KPI_DATASET_VALUES"
    Private Const strModifica = "kpis.UPD_KPI_DATASET_VALUES"
    Private Const strElimina = "kpis.DEL_KPI_DATASET_VALUES"
#End Region

#Region "Variables Privadas"
    Private _VALORID As Nullable(Of Integer)
    Private _DATASETID As Nullable(Of Integer)
    Private _INDICATORID As Nullable(Of Integer)
    Private _USERID As Nullable(Of Integer)
    Private _FECHA As Nullable(Of DateTime)
    Private _FECHA_FIN As Nullable(Of DateTime)
    Private _VALOR As Nullable(Of Decimal)
    Private _EJERCICIO As Nullable(Of Integer)
    Private _SEMESTRE As Nullable(Of Integer)
    Private _TRIMESTRE As Nullable(Of Integer)
    Private _MES As Nullable(Of Integer)
    Private _QUINCENA As Nullable(Of Integer)
    Private _SEMANA As Nullable(Of Integer)
    Private _IMPORTID As Nullable(Of Integer)
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "KPI_DATASET_VALUES"
        End Get
    End Property
    Public Property valorid As Nullable(Of Integer)
        Get
            Return _VALORID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _VALORID = value
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
    Public Property fecha_final As Nullable(Of DateTime)
        Get
            Return _FECHA_FIN
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FECHA_FIN = value
        End Set
    End Property
    Public Property valor As Nullable(Of Decimal)
        Get
            Return _VALOR
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _VALOR = value
        End Set
    End Property
    Public Property ejercicio As Nullable(Of Integer)
        Get
            Return _EJERCICIO
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _EJERCICIO = value
        End Set
    End Property
    Public Property semestre As Nullable(Of Integer)
        Get
            Return _SEMESTRE
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _SEMESTRE = value
        End Set
    End Property
    Public Property trimestre As Nullable(Of Integer)
        Get
            Return _TRIMESTRE
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _TRIMESTRE = value
        End Set
    End Property
    Public Property mes As Nullable(Of Integer)
        Get
            Return _MES
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _MES = value
        End Set
    End Property
    Public Property quincena As Nullable(Of Integer)
        Get
            Return _QUINCENA
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _QUINCENA = value
        End Set
    End Property
    Public Property semana As Nullable(Of Integer)
        Get
            Return _SEMANA
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _SEMANA = value
        End Set
    End Property
    Public Property importid As Nullable(Of Integer)
        Get
            Return _IMPORTID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IMPORTID = value
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
        Me._VALORID = Nothing
        Me._DATASETID = Nothing
        Me._INDICATORID = Nothing
        Me._USERID = Nothing
        Me._FECHA = Nothing
        Me._FECHA_FIN = Nothing
        Me._VALOR = Nothing
        Me._EJERCICIO = Nothing
        Me._SEMESTRE = Nothing
        Me._TRIMESTRE = Nothing
        Me._MES = Nothing
        Me._QUINCENA = Nothing
        Me._SEMANA = Nothing
        Me._IMPORTID = Nothing
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

                If Me._VALORID.HasValue Then
                    .CommandText = .CommandText & " AND VALORID = :P_VALORID "
                    .Parameters.Add("P_VALORID", NpgsqlDbType.Bigint).Value = Me._VALORID.Value
                End If
                If Me._DATASETID.HasValue Then
                    .CommandText = .CommandText & " AND DATASETID = :P_DATASETID "
                    .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
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
                If Not IsDBNull(.GetValue(.GetOrdinal("VALORID"))) Then
                    Me._VALORID = Convert.ToInt64(.GetValue(.GetOrdinal("VALORID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("DATASETID"))) Then
                    Me._DATASETID = Convert.ToInt64(.GetValue(.GetOrdinal("DATASETID")))
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
                If Not IsDBNull(.GetValue(.GetOrdinal("FECHA_FIN"))) Then
                    Me._FECHA_FIN = Convert.ToDateTime(.GetValue(.GetOrdinal("FECHA_FIN")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("VALOR"))) Then
                    Me._VALOR = Convert.ToDecimal(.GetValue(.GetOrdinal("VALOR")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("EJERCICIO"))) Then
                    Me._EJERCICIO = Convert.ToInt64(.GetValue(.GetOrdinal("EJERCICIO")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("SEMESTRE"))) Then
                    Me._SEMESTRE = Convert.ToInt64(.GetValue(.GetOrdinal("SEMESTRE")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("TRIMESTRE"))) Then
                    Me._TRIMESTRE = Convert.ToInt64(.GetValue(.GetOrdinal("TRIMESTRE")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("MES"))) Then
                    Me._MES = Convert.ToInt64(.GetValue(.GetOrdinal("MES")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("QUINCENA"))) Then
                    Me._QUINCENA = Convert.ToInt64(.GetValue(.GetOrdinal("QUINCENA")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("SEMANA"))) Then
                    Me._SEMANA = Convert.ToInt64(.GetValue(.GetOrdinal("SEMANA")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("IMPORTID"))) Then
                    Me._IMPORTID = Convert.ToInt64(.GetValue(.GetOrdinal("IMPORTID")))
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
                If Me._DATASETID.HasValue Then
                    .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                Else
                    .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
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
                If Me._FECHA.HasValue Then
                    .Parameters.Add("P_FECHA", NpgsqlDbType.Date).Value = Me._FECHA.Value
                Else
                    .Parameters.Add("P_FECHA", NpgsqlDbType.Date).Value = DBNull.Value
                End If
                If Me._FECHA_FIN.HasValue Then
                    .Parameters.Add("P_FECHA_FIN", NpgsqlDbType.Date).Value = Me._FECHA_FIN.Value
                Else
                    .Parameters.Add("P_FECHA_FIN", NpgsqlDbType.Date).Value = DBNull.Value
                End If
                If Me._VALOR.HasValue Then
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = Me._VALOR.Value
                Else
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = DBNull.Value
                End If
                If Me._VALORID.HasValue Then
                    .Parameters.Add("P_VALORID", NpgsqlDbType.Bigint).Value = Me._VALORID.Value
                Else
                    .Parameters.Add("P_VALORID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._IMPORTID.HasValue Then
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
                Else
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._VALORID = nResultado
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            Me._VALORID = -1
        Finally
            dbconexion.Close()
        End Try

        If Me._VALORID >= 0 Then
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
                If Me._DATASETID.HasValue Then
                    .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                Else
                    .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._VALORID.HasValue Then
                    .Parameters.Add("P_VALORID", NpgsqlDbType.Bigint).Value = Me._VALORID.Value
                Else
                    .Parameters.Add("P_VALORID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
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
                If Me._FECHA.HasValue Then
                    .Parameters.Add("P_FECHA", NpgsqlDbType.Date).Value = Me._FECHA.Value
                Else
                    .Parameters.Add("P_FECHA", NpgsqlDbType.Date).Value = DBNull.Value
                End If
                If Me._FECHA_FIN.HasValue Then
                    .Parameters.Add("P_FECHA_FIN", NpgsqlDbType.Date).Value = Me._FECHA_FIN.Value
                Else
                    .Parameters.Add("P_FECHA_FIN", NpgsqlDbType.Date).Value = DBNull.Value
                End If
                If Me._VALOR.HasValue Then
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = Me._VALOR.Value
                Else
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = DBNull.Value
                End If
                If Me._VALORID.HasValue Then
                    .Parameters.Add("P_VALORID", NpgsqlDbType.Bigint).Value = Me._VALORID.Value
                Else
                    .Parameters.Add("P_VALORID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._IMPORTID.HasValue Then
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
                Else
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._VALORID = nResultado
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            Me._VALORID = -1
        End Try

        If Me._VALORID >= 0 Then
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
                If Me._VALORID.HasValue Then
                    .Parameters.Add("P_VALORID", NpgsqlDbType.Bigint).Value = Me._VALORID.Value
                Else
                    .Parameters.Add("P_VALORID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._FECHA.HasValue Then
                    .Parameters.Add("P_FECHA", NpgsqlDbType.Date).Value = Me._FECHA.Value
                Else
                    .Parameters.Add("P_FECHA", NpgsqlDbType.Date).Value = DBNull.Value
                End If
                If Me._VALOR.HasValue Then
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = Me._VALOR.Value
                Else
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = DBNull.Value
                End If
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
                If Me._VALORID.HasValue Then
                    .Parameters.Add("P_VALORID", NpgsqlDbType.Bigint).Value = Me._VALORID.Value
                Else
                    .Parameters.Add("P_VALORID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._FECHA.HasValue Then
                    .Parameters.Add("P_FECHA", NpgsqlDbType.Date).Value = Me._FECHA.Value
                Else
                    .Parameters.Add("P_FECHA", NpgsqlDbType.Date).Value = DBNull.Value
                End If
                If Me._VALOR.HasValue Then
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = Me._VALOR.Value
                Else
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = DBNull.Value
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
        Dim nResultado As Int64

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
                .Parameters.Add("P_VALORID", NpgsqlDbType.Bigint).Value = Me._VALORID.Value
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
                .Parameters.Add("P_VALORID", NpgsqlDbType.Bigint).Value = Me._VALORID.Value
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

                If Me._VALORID.HasValue Then
                    .CommandText = .CommandText & " AND VALORID = :P_VALORID "
                    .Parameters.Add("P_VALORID", NpgsqlDbType.Bigint).Value = Me._VALORID.Value
                End If
                If Me._DATASETID.HasValue Then
                    .CommandText = .CommandText & " AND DATASETID = :P_DATASETID "
                    .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND USERID = :P_USERID "
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

                If Me._VALORID.HasValue Then
                    .CommandText = .CommandText & " AND VALORID = :P_VALORID "
                    .Parameters.Add("P_VALORID", NpgsqlDbType.Bigint).Value = Me._VALORID.Value
                End If
                If Me._DATASETID.HasValue Then
                    .CommandText = .CommandText & " AND DATASETID = :P_DATASETID "
                    .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND USERID = :P_USERID "
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

#Region "Otras Funciones Publicas de la Clase"
    Public Function NumeroValores() As Integer
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
                .CommandText = "SELECT COUNT(VALORID) " & _
                               "FROM KPI_DATASET_VALUES " & _
                               "WHERE 1=1 "
                .CommandType = CommandType.Text

                If Me._VALORID.HasValue Then
                    .CommandText = .CommandText & " AND VALORID = :P_VALORID "
                    .Parameters.Add("P_VALORID", NpgsqlDbType.Bigint).Value = Me._VALORID.Value
                End If
                If Me._DATASETID.HasValue Then
                    .CommandText = .CommandText & " AND DATASETID = :P_DATASETID "
                    .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                End If
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
    Public Function ObtenerGraficoIndicador() As DataTable
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
                .CommandText = "formulas.GETINDICATORTIMELINE"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
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
    Public Function ObtenerDatosPorEjercicio() As DataTable
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
                .CommandText = "SELECT TO_CHAR(EJERCICIO,'9999') ""Year "", COUNT(VALORID) ""Number of values """ & _
                               "FROM KPI_DATASET_VALUES " & _
                               "WHERE 1=1 "
                .CommandType = CommandType.Text

                If Me._VALORID.HasValue Then
                    .CommandText = .CommandText & " AND VALORID = :P_VALORID "
                    .Parameters.Add("P_VALORID", NpgsqlDbType.Bigint).Value = Me._VALORID.Value
                End If
                If Me._DATASETID.HasValue Then
                    .CommandText = .CommandText & " AND DATASETID = :P_DATASETID "
                    .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                End If
                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
                .CommandText = .CommandText & " GROUP BY EJERCICIO ORDER BY EJERCICIO DESC "
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
    Public Function ObtenerUltimosDatos(ByVal pnTop As Integer) As DataTable
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
                .CommandText = "SELECT VALOR FROM (SELECT FECHA_FIN, ROUND(VALOR,0) VALOR FROM KPI_DATASET_VALUES " & _
                               "WHERE DATASETID = :P_DATASETID " & _
                               "ORDER BY FECHA_FIN DESC " & _
                               "LIMIT :P_ROWNUM ) AS Z ORDER BY FECHA_FIN ASC"
                .CommandType = CommandType.Text

                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                .Parameters.Add("P_ROWNUM", NpgsqlDbType.Bigint).Value = pnTop
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

    Public Function bGuardarEjercicio(ByVal pModo As Integer) As Boolean
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
                .CommandText = "kpis.saveejercicio"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                If Me._EJERCICIO.HasValue Then
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = Me._EJERCICIO.Value
                Else
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._VALOR.HasValue Then
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = Me._VALOR.Value
                Else
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = DBNull.Value
                End If
                .Parameters.Add("P_ADDMODE", NpgsqlDbType.Bigint).Value = pModo
                If Me._IMPORTID.HasValue Then
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
                Else
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return True

    End Function
    Public Function bEliminarEjercicio() As Boolean
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
                .CommandText = "kpis.DELETEEJERCICIO"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                If Me._EJERCICIO.HasValue Then
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = Me._EJERCICIO.Value
                Else
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return True

    End Function
    Public Function bGuardarSemestre(ByVal pModo As Integer) As Boolean
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
                .CommandText = "kpis.SAVESEMESTRE"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                If Me._EJERCICIO.HasValue Then
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = Me._EJERCICIO.Value
                Else
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._SEMESTRE.HasValue Then
                    .Parameters.Add("P_SEMESTRE", NpgsqlDbType.Bigint).Value = Me._SEMESTRE.Value
                Else
                    .Parameters.Add("P_SEMESTRE", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._VALOR.HasValue Then
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = Me._VALOR.Value
                Else
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = DBNull.Value
                End If
                .Parameters.Add("P_ADDMODE", NpgsqlDbType.Bigint).Value = pModo
                If Me._IMPORTID.HasValue Then
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
                Else
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return True

    End Function
    Public Function bEliminarSemestre() As Boolean
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
                .CommandText = "kpis.DELETESEMESTRE"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                If Me._EJERCICIO.HasValue Then
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = Me._EJERCICIO.Value
                Else
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._SEMESTRE.HasValue Then
                    .Parameters.Add("P_SEMESTRE", NpgsqlDbType.Bigint).Value = Me._SEMESTRE.Value
                Else
                    .Parameters.Add("P_SEMESTRE", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return True

    End Function
    Public Function bGuardarTrimestre(ByVal pModo As Integer) As Boolean
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
                .CommandText = "kpis.SAVETRIMESTRE"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                If Me._EJERCICIO.HasValue Then
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = Me._EJERCICIO.Value
                Else
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._TRIMESTRE.HasValue Then
                    .Parameters.Add("P_TRIMESTRE", NpgsqlDbType.Bigint).Value = Me._TRIMESTRE.Value
                Else
                    .Parameters.Add("P_TRIMESTRE", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._VALOR.HasValue Then
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = Me._VALOR.Value
                Else
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = DBNull.Value
                End If
                .Parameters.Add("P_ADDMODE", NpgsqlDbType.Bigint).Value = pModo
                If Me._IMPORTID.HasValue Then
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
                Else
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return True

    End Function
    Public Function bEliminarTrimestre() As Boolean
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
                .CommandText = "kpis.DELETETRIMESTRE"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                If Me._EJERCICIO.HasValue Then
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = Me._EJERCICIO.Value
                Else
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._TRIMESTRE.HasValue Then
                    .Parameters.Add("P_TRIMESTRE", NpgsqlDbType.Bigint).Value = Me._TRIMESTRE.Value
                Else
                    .Parameters.Add("P_TRIMESTRE", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return True

    End Function
    Public Function bGuardarMes(ByVal pModo As Integer) As Boolean
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
                .CommandText = "kpis.SAVEMES"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                If Me._EJERCICIO.HasValue Then
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = Me._EJERCICIO.Value
                Else
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._MES.HasValue Then
                    .Parameters.Add("P_MES", NpgsqlDbType.Bigint).Value = Me._MES.Value
                Else
                    .Parameters.Add("P_MES", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._VALOR.HasValue Then
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = Me._VALOR.Value
                Else
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = DBNull.Value
                End If
                .Parameters.Add("P_ADDMODE", NpgsqlDbType.Bigint).Value = pModo
                If Me._IMPORTID.HasValue Then
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
                Else
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return True

    End Function
    Public Function bEliminarMes() As Boolean
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
                .CommandText = "kpis.DELETEMES"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                If Me._EJERCICIO.HasValue Then
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = Me._EJERCICIO.Value
                Else
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._MES.HasValue Then
                    .Parameters.Add("P_MES", NpgsqlDbType.Bigint).Value = Me._MES.Value
                Else
                    .Parameters.Add("P_MES", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return True

    End Function
    Public Function bGuardarQuincena(ByVal pModo As Integer) As Boolean
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
                .CommandText = "kpis.SAVEQUINCENA"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                If Me._EJERCICIO.HasValue Then
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = Me._EJERCICIO.Value
                Else
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._MES.HasValue Then
                    .Parameters.Add("P_MES", NpgsqlDbType.Bigint).Value = Me._MES.Value
                Else
                    .Parameters.Add("P_MES", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._QUINCENA.HasValue Then
                    .Parameters.Add("P_QUINCENA", NpgsqlDbType.Bigint).Value = Me._QUINCENA.Value
                Else
                    .Parameters.Add("P_QUINCENA", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._VALOR.HasValue Then
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = Me._VALOR.Value
                Else
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = DBNull.Value
                End If
                .Parameters.Add("P_ADDMODE", NpgsqlDbType.Bigint).Value = pModo
                If Me._IMPORTID.HasValue Then
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
                Else
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return True

    End Function
    Public Function bEliminarQuincena() As Boolean
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
                .CommandText = "kpis.DELETEQUINCENA"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                If Me._EJERCICIO.HasValue Then
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = Me._EJERCICIO.Value
                Else
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._MES.HasValue Then
                    .Parameters.Add("P_MES", NpgsqlDbType.Bigint).Value = Me._MES.Value
                Else
                    .Parameters.Add("P_MES", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._QUINCENA.HasValue Then
                    .Parameters.Add("P_QUINCENA", NpgsqlDbType.Bigint).Value = Me._QUINCENA.Value
                Else
                    .Parameters.Add("P_QUINCENA", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return True

    End Function
    Public Function bGuardarSemana(ByVal pModo As Integer) As Boolean
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
                .CommandText = "kpis.SAVESEMANA"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                If Me._EJERCICIO.HasValue Then
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = Me._EJERCICIO.Value
                Else
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._MES.HasValue Then
                    .Parameters.Add("P_MES", NpgsqlDbType.Bigint).Value = Me._MES.Value
                Else
                    .Parameters.Add("P_MES", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._SEMANA.HasValue Then
                    .Parameters.Add("P_SEMANA", NpgsqlDbType.Bigint).Value = Me._SEMANA.Value
                Else
                    .Parameters.Add("P_SEMANA", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._VALOR.HasValue Then
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = Me._VALOR.Value
                Else
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = DBNull.Value
                End If
                .Parameters.Add("P_ADDMODE", NpgsqlDbType.Bigint).Value = pModo
                If Me._IMPORTID.HasValue Then
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
                Else
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return True

    End Function
    Public Function bEliminarSemana() As Boolean
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
                .CommandText = "kpis.DELETESEMANA"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                If Me._EJERCICIO.HasValue Then
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = Me._EJERCICIO.Value
                Else
                    .Parameters.Add("P_EJERCICIO", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._MES.HasValue Then
                    .Parameters.Add("P_MES", NpgsqlDbType.Bigint).Value = Me._MES.Value
                Else
                    .Parameters.Add("P_MES", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._SEMANA.HasValue Then
                    .Parameters.Add("P_SEMANA", NpgsqlDbType.Bigint).Value = Me._SEMANA.Value
                Else
                    .Parameters.Add("P_SEMANA", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return True

    End Function
    Public Function bGuardarLibre(ByVal pModo As Integer) As Boolean
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
                .CommandText = "kpis.SAVELIBRE"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                If Me._FECHA.HasValue Then
                    .Parameters.Add("P_FECHA", NpgsqlDbType.Date).Value = Me._FECHA.Value
                Else
                    .Parameters.Add("P_FECHA", NpgsqlDbType.Date).Value = DBNull.Value
                End If
                If Me._VALOR.HasValue Then
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = Me._VALOR.Value
                Else
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Numeric).Value = DBNull.Value
                End If
                .Parameters.Add("P_ADDMODE", NpgsqlDbType.Bigint).Value = pModo
                If Me._IMPORTID.HasValue Then
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
                Else
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return True

    End Function
    Public Function bEliminarLibre() As Boolean
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
                .CommandText = "kpis.DELETELIBRE"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_DATASETID", NpgsqlDbType.Bigint).Value = Me._DATASETID.Value
                If Me._FECHA.HasValue Then
                    .Parameters.Add("P_FECHA", NpgsqlDbType.Date).Value = Me._FECHA.Value
                Else
                    .Parameters.Add("P_FECHA", NpgsqlDbType.Date).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return True

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
