Public Class cKPI_IMPORT_DETAILS
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT IMPORTID, FILA, CADENA, MENSAJE	FROM KPI_IMPORT_DETAILS " & _
                                "WHERE 1=1 "

    Private Const strInserta = "kpis.add_kpi_import_details"
#End Region

#Region "Variables Privadas"
    Private _IMPORTID As Nullable(Of Integer)
    Private _FILA As Nullable(Of Integer)
    Private _CADENA As String
    Private _MENSAJE As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "KPI_IMPORT_DETAILS"
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
    Public Property fila As Nullable(Of Integer)
        Get
            Return _FILA
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _FILA = value
        End Set
    End Property
    Public Property cadena As String
        Get
            Return _CADENA
        End Get
        Set(ByVal value As String)
            _CADENA = value
        End Set
    End Property
    Public Property mensaje As String
        Get
            Return _MENSAJE
        End Get
        Set(ByVal value As String)
            _MENSAJE = value
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
        Me._FILA = Nothing
        Me._CADENA = String.Empty
        Me._MENSAJE = String.Empty
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
                If Not IsDBNull(.GetValue(.GetOrdinal("FILA"))) Then
                    Me._FILA = Convert.ToInt64(.GetValue(.GetOrdinal("FILA")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("CADENA"))) Then
                    Me._CADENA = .GetValue(.GetOrdinal("CADENA"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("MENSAJE"))) Then
                    Me._MENSAJE = .GetValue(.GetOrdinal("MENSAJE"))
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

                If Me._IMPORTID.HasValue Then
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
                Else
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._FILA.HasValue Then
                    .Parameters.Add("P_FILA", NpgsqlDbType.Bigint).Value = Me._FILA.Value
                Else
                    .Parameters.Add("P_FILA", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_CADENA", NpgsqlDbType.Varchar).Value = Me._CADENA
                .Parameters.Add("P_MENSAJE", NpgsqlDbType.Text).Value = Me._MENSAJE
            End With

            SqlInserta.ExecuteNonQuery()
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

                If Me._IMPORTID.HasValue Then
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
                Else
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._FILA.HasValue Then
                    .Parameters.Add("P_FILA", NpgsqlDbType.Bigint).Value = Me._FILA.Value
                Else
                    .Parameters.Add("P_FILA", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_CADENA", NpgsqlDbType.Varchar).Value = Me._CADENA
                .Parameters.Add("P_MENSAJE", NpgsqlDbType.Text).Value = Me._MENSAJE
            End With

            SqlInserta.ExecuteNonQuery()
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

                If Me._IMPORTID.HasValue Then
                    .CommandText = .CommandText & " AND IMPORTID = :P_IMPORTID "
                    .Parameters.Add("P_IMPORTID", NpgsqlDbType.Bigint).Value = Me._IMPORTID.Value
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
                .CommandText = strConsulta & psFiltro

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
