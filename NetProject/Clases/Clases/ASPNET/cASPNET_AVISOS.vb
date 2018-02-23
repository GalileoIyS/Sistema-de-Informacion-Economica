Public Class cASPNET_AVISOS
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT FECHA, PAGINA, MENSAJE, ERROR " & _
                                "FROM ASPNET_AVISOS " & _
                                "WHERE 1=1 "

    Private Const strInserta = "aspnet.add_aspnet_avisos"
    Private Const strElimina = "aspnet.del_aspnet_avisos"
#End Region

#Region "Variables Privadas"
    Private _FECHA As  Nullable(Of DateTime)
    Private _PAGINA As String
    Private _MENSAJE As String
    Private _ERROR As  Nullable(Of Integer)
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "ASPNET_AVISOS"
        End Get
    End Property
    Public Property fecha As  Nullable(Of DateTime)
        Get
            Return _FECHA
        End Get
        Set(ByVal value As  Nullable(Of DateTime))
            _FECHA = value
        End Set
    End Property
    Public Property pagina As String
        Get
            Return _PAGINA
        End Get
        Set(ByVal value As String)
            _PAGINA = value
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
    Public Property codigo_error As Nullable(Of Integer)
        Get
            Return _ERROR
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _ERROR = value
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
        Me._FECHA = Nothing
        Me._PAGINA = String.Empty
        Me._MENSAJE = String.Empty
        Me._ERROR = Nothing
    End Sub
    Public Sub bInsertar()
        Dim dbconexion As New NpgsqlConnection

        dbconexion = Me.ObtenerConexion()

        'Intentamos abrir la conexión
        Try
            dbconexion.Open()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido conectar con la Base de Datos." & Chr(13) & _
                   "Motivo: " & excp.Message)
            Return
        End Try

        Try
            Dim SqlInserta As New NpgsqlCommand

            With SqlInserta
                .Connection = dbconexion
                .CommandText = strInserta
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_PAGINA", NpgsqlDbType.Varchar, 200).Value = Me._PAGINA
                .Parameters.Add("P_MENSAJE", NpgsqlDbType.Text).Value = Me._MENSAJE
                If Me._ERROR.HasValue Then
                    .Parameters.Add("P_ERROR", NpgsqlDbType.Bigint).Value = Me._ERROR.Value
                Else
                    .Parameters.Add("P_ERROR", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

    End Sub
    Public Sub bInsertar(ByVal dbConexion As NpgsqlConnection, ByVal dbTransaccion As NpgsqlTransaction)

        Try
            Dim SqlInserta As New NpgsqlCommand

            With SqlInserta
                .Connection = dbConexion
                .Transaction = dbTransaccion
                .CommandText = strInserta
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_PAGINA", NpgsqlDbType.Varchar, 200).Value = Me._PAGINA
                .Parameters.Add("P_MENSAJE", NpgsqlDbType.Text).Value = Me._MENSAJE
                If Me._ERROR.HasValue Then
                    .Parameters.Add("P_ERROR", NpgsqlDbType.Bigint).Value = Me._ERROR.Value
                Else
                    .Parameters.Add("P_ERROR", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        End Try

    End Sub
    Public Sub bEliminar()
        Dim dbconexion As New NpgsqlConnection

        dbconexion = Me.ObtenerConexion()

        'Intentamos abrir la conexión
        Try
            dbconexion.Open()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido conectar con la Base de Datos." & Chr(13) & _
                   "Motivo: " & excp.Message)
            Return
        End Try

        Try
            Dim SqlElimina As New NpgsqlCommand

            With SqlElimina
                .Connection = dbconexion
                .CommandText = strElimina
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_ERROR", NpgsqlDbType.Bigint).Value = Me._ERROR.Value
            End With

            SqlElimina.ExecuteNonQuery()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Eliminación sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

    End Sub
    Public Sub bEliminar(ByVal dbConexion As NpgsqlConnection, ByVal dbTransaccion As NpgsqlTransaction)

        Try
            Dim SqlElimina As New NpgsqlCommand

            With SqlElimina
                .Connection = dbConexion
                .Transaction = dbTransaccion
                .CommandText = strElimina
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("P_ERROR", NpgsqlDbType.Bigint).Value = Me._ERROR.Value
            End With

            SqlElimina.ExecuteNonQuery()
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Eliminación sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        End Try

    End Sub
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

                If Not String.IsNullOrEmpty(Me._PAGINA) Then
                    .CommandText = .CommandText & " AND LOWER(PAGINA) LIKE :P_PAGINA "
                    .Parameters.Add("P_PAGINA", NpgsqlDbType.Varchar, 250).Value = "%" & Me._PAGINA.ToLower() & "%"
                End If
                .CommandText = .CommandText & " ORDER BY FECHA DESC"
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

            With SqlConsulta
                .Connection = dbconexion
                .CommandType = CommandType.Text
                .CommandText = strConsulta & psFiltro

                If Not String.IsNullOrEmpty(Me._PAGINA) Then
                    .CommandText = .CommandText & " AND LOWER(PAGINA) LIKE :P_PAGINA "
                    .Parameters.Add("P_PAGINA", NpgsqlDbType.Varchar, 250).Value = "%" & Me._PAGINA.ToLower() & "%"
                End If
                .CommandText = .CommandText & " ORDER BY FECHA DESC"
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
