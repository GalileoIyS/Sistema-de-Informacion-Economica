Public Class cASPNET_USERSINROLES
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT A.USERID, A.ROLEID, B.USERNAME, C.ROLENAME " & _
                                "FROM ASPNET_USERSINROLES A " & _
                                "LEFT JOIN ASPNET_USERS B ON A.USERID = B.USERID " & _
                                "LEFT JOIN ASPNET_ROLES C ON A.ROLEID = C.ROLEID " & _
                                "WHERE 1=1 "
    Private Const strElimina As String = "pkdelete. del_aspnet_usersfromroles"
#End Region

#Region "Variables Privadas"
    Private _USERID As Nullable(Of Integer)
    Private _ROLEID As Nullable(Of Integer)
#End Region

#Region "Variables Foraneas"
    Private _USERNAME As String
    Private _ROLENAME As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "ASPNET_USERSINROLES"
        End Get
    End Property
    Public Property UserId() As Nullable(Of Integer)
        Get
            Return _USERID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _USERID = value
        End Set
    End Property
    Public Property RoleId() As Nullable(Of Integer)
        Get
            Return _ROLEID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _ROLEID = value
        End Set
    End Property
    Public Property Username() As String
        Get
            Return _USERNAME
        End Get
        Set(ByVal value As String)
            _USERNAME = value
        End Set
    End Property
    Public Property Rolename() As String
        Get
            Return _ROLENAME
        End Get
        Set(ByVal value As String)
            _ROLENAME = value
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
        Me._USERID = Nothing
        Me._ROLEID = Nothing
        Me._USERNAME = String.Empty
        Me._ROLENAME = String.Empty
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
            Return False
        End Try

        Try
            With SqlConsulta
                .Connection = dbconexion
                .CommandText = strConsulta
                .CommandType = CommandType.Text

                If (Me._USERID.HasValue) Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
                If (Me._ROLEID.HasValue) Then
                    .CommandText = .CommandText & " AND A.ROLEID = :P_ROLEID "
                    .Parameters.Add("P_ROLEID", NpgsqlDbType.Bigint).Value = Me._ROLEID.Value
                End If
            End With

            drDATOS = SqlConsulta.ExecuteReader()

        Catch excp As NpgsqlException
            dbconexion.Close()
            dbconexion.Dispose()
            Return False
        End Try

        With drDATOS
            Me.Inicializar()
            If .Read Then
                If Not IsDBNull(.GetValue(.GetOrdinal("USERID"))) Then
                    Me._USERID = Convert.ToInt32(.GetValue(.GetOrdinal("USERID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("ROLEID"))) Then
                    Me._ROLEID = Convert.ToInt32(.GetValue(.GetOrdinal("ROLEID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("USERNAME"))) Then
                    Me._USERNAME = .GetValue(.GetOrdinal("USERNAME")).ToString()
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("ROLENAME"))) Then
                    Me._ROLENAME = .GetValue(.GetOrdinal("ROLENAME")).ToString()
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
    Public Function bEliminar() As Boolean
        Dim dbconexion As New NpgsqlConnection
        Dim nResultado As Int64 = 0

        dbconexion = Me.ObtenerConexion()

        'Intentamos abrir la conexión
        Try
            dbconexion.Open()
        Catch excp As NpgsqlException
            Return False
        End Try

        Try
            Dim SqlElimina As New NpgsqlCommand

            With SqlElimina
                .Connection = dbconexion
                .CommandText = strElimina
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                .Parameters.Add("P_USERNAME", NpgsqlDbType.Varchar, 40).Value = Me._USERNAME
                .Parameters.Add("P_ROLENAME", NpgsqlDbType.Varchar, 40).Value = Me._ROLENAME
            End With

            SqlElimina.ExecuteNonQuery()
            nResultado = SqlElimina.Parameters("RETURN_VALUE").Value
        Catch excp As NpgsqlException
            Return False
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
            Return Nothing
        End Try

        Try
            Dim SqlConsulta As New NpgsqlCommand
            Dim daData As New NpgsqlDataAdapter

            With SqlConsulta
                .Connection = dbconexion
                .CommandText = strConsulta
                .CommandType = CommandType.Text

                If (Me._USERID.HasValue) Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
                If (Me._ROLEID.HasValue) Then
                    .CommandText = .CommandText & " AND A.ROLEID = :P_ROLEID "
                    .Parameters.Add("P_ROLEID", NpgsqlDbType.Bigint).Value = Me._ROLEID.Value
                End If
                If (Not String.IsNullOrEmpty(Me._USERNAME)) Then
                    .CommandText = .CommandText & " AND B.USERNAME = :P_USERNAME "
                    .Parameters.Add("P_USERNAME", NpgsqlDbType.Varchar, 40).Value = Me._USERNAME
                End If
                If (Not String.IsNullOrEmpty(Me._ROLENAME)) Then
                    .CommandText = .CommandText & " AND C.ROLENAME = :P_ROLENAME "
                    .Parameters.Add("P_ROLENAME", NpgsqlDbType.Varchar, 40).Value = Me._ROLENAME
                End If
            End With

            daData.SelectCommand = SqlConsulta
            daData.Fill(_dtData)

        Catch excp As NpgsqlException
            Return Nothing
        Finally
            dbconexion.Close()
        End Try

        Return _dtData

    End Function
#End Region

#Region "Soporte a la Interfaz IDisposable "
    Private disposedValue As Boolean = False        ' Para detectar llamadas redundantes)

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
