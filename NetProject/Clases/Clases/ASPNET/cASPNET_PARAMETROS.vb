Public Class cASPNET_PARAMETROS
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT PARAMETROID, TIPO, VALOR, LOWEREDVALOR " & _
                                "FROM ASPNET_PARAMETROS " & _
                                "WHERE 1=1 "

    Private Const strInserta = "ADD_ASPNET_PARAMETROS"
    Private Const strModifica = "UPD_ASPNET_PARAMETROS"
    Private Const strElimina = "DEL_ASPNET_PARAMETROS"
#End Region

#Region "Variables Privadas"
    Private _PARAMETROID As Nullable(Of Integer)
    Private _TIPO As  Nullable(Of Integer)
    Private _VALOR As String
    Private _LOWEREDVALOR As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "ASPNET_PARAMETROS"
        End Get
    End Property
    Public Property parametroid As Nullable(Of Integer)
        Get
            Return _PARAMETROID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _PARAMETROID = value
        End Set
    End Property
    Public Property tipo As  Nullable(Of Integer)
        Get
            Return _TIPO
        End Get
        Set(ByVal value As  Nullable(Of Integer))
            _TIPO = value
        End Set
    End Property
    Public Property valor As String
        Get
            Return _VALOR
        End Get
        Set(ByVal value As String)
            _VALOR = value
        End Set
    End Property
    Public Property loweredvalor As String
        Get
            Return _LOWEREDVALOR
        End Get
        Set(ByVal value As String)
            _LOWEREDVALOR = value
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
        Me._PARAMETROID = Nothing
        Me._TIPO = Nothing
        Me._VALOR = String.Empty
        Me._LOWEREDVALOR = String.Empty
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

                If Me._PARAMETROID.HasValue Then
                    .CommandText = .CommandText & " AND PARAMETROID = :P_PARAMETROID"
                    .Parameters.Add("P_PARAMETROID", NpgsqlDbType.Bigint).Value = Me._PARAMETROID.Value
                End If
                If Me._TIPO.HasValue Then
                    .CommandText = .CommandText & " AND TIPO = :P_TIPO"
                    .Parameters.Add("P_TIPO", NpgsqlDbType.Bigint).Value = Me._TIPO.Value
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
                If Not IsDBNull(.GetValue(.GetOrdinal("PARAMETROID"))) Then
                    Me._PARAMETROID = Convert.ToInt64(.GetValue(.GetOrdinal("PARAMETROID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("TIPO"))) Then
                    Me._TIPO = Convert.ToInt64(.GetValue(.GetOrdinal("TIPO")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("VALOR"))) Then
                    Me._VALOR = .GetValue(.GetOrdinal("VALOR"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("LOWEREDVALOR"))) Then
                    Me._LOWEREDVALOR = .GetValue(.GetOrdinal("LOWEREDVALOR"))
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
            Dim SqlInserta As New NpgsqlCommand

            With SqlInserta
                .Connection = dbconexion
                .CommandText = strInserta
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                If Me._TIPO.HasValue Then
                    .Parameters.Add("P_TIPO", NpgsqlDbType.Bigint).Value = Me._TIPO.Value
                Else
                    .Parameters.Add("P_TIPO", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_VALOR", NpgsqlDbType.Varchar, 250).Value = Me._VALOR
                .Parameters.Add("P_LOWEREDVALOR", NpgsqlDbType.Varchar, 250).Value = Me._LOWEREDVALOR
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
    Public Function bModificar() As Boolean
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
            Dim SqlModifica As New NpgsqlCommand

            With SqlModifica
                .Connection = dbconexion
                .CommandText = strModifica
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                .Parameters.Add("P_PARAMETROID", NpgsqlDbType.Bigint).Value = Me._PARAMETROID.Value
                If Me._TIPO.HasValue Then
                    .Parameters.Add("P_TIPO", NpgsqlDbType.Bigint).Value = Me._TIPO.Value
                Else
                    .Parameters.Add("P_TIPO", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_VALOR", NpgsqlDbType.Varchar, 250).Value = Me._VALOR
                .Parameters.Add("P_LOWEREDVALOR", NpgsqlDbType.Varchar, 250).Value = Me._LOWEREDVALOR
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
            Dim SqlElimina As New NpgsqlCommand

            With SqlElimina
                .Connection = dbconexion
                .CommandText = strElimina
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                .Parameters.Add("P_PARAMETROID", NpgsqlDbType.Bigint).Value = Me._PARAMETROID.Value
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

                If Me.parametroid.HasValue Then
                    .CommandText = .CommandText & " AND PARAMETROID = :P_PARAMETROID"
                    .Parameters.Add("P_PARAMETROID", NpgsqlDbType.Bigint).Value = Me._PARAMETROID.Value
                End If
                If Me._TIPO.HasValue Then
                    .CommandText = .CommandText & " AND TIPO = :P_TIPO"
                    .Parameters.Add("P_TIPO", NpgsqlDbType.Bigint).Value = Me._TIPO.Value
                End If
                If Not String.IsNullOrEmpty(Me._LOWEREDVALOR) Then
                    .CommandText = .CommandText & " AND LOWEREDVALOR LIKE :P_LOWEREDVALOR "
                    .Parameters.Add("P_LOWEREDVALOR", NpgsqlDbType.Varchar, 250).Value = "%" & Me._LOWEREDVALOR & "%"
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

                If Me.parametroid.HasValue Then
                    .CommandText = .CommandText & " AND PARAMETROID = :P_PARAMETROID"
                    .Parameters.Add("P_PARAMETROID", NpgsqlDbType.Bigint).Value = Me._PARAMETROID.Value
                End If
                If Me._TIPO.HasValue Then
                    .CommandText = .CommandText & " AND TIPO = :P_TIPO"
                    .Parameters.Add("P_TIPO", NpgsqlDbType.Bigint).Value = Me._TIPO.Value
                End If
                If Not String.IsNullOrEmpty(Me._LOWEREDVALOR) Then
                    .CommandText = .CommandText & " AND LOWEREDVALOR LIKE :P_VALOR "
                    .Parameters.Add("P_VALOR", NpgsqlDbType.Varchar, 250).Value = "%" & Me._VALOR.ToLower() & "%"
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
