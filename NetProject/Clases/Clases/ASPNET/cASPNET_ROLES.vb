Public Class cASPNET_ROLES
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT ROLEID, APPLICATIONID, ROLENAME, LOWEREDROLENAME, DESCRIPTION " & _
                                "FROM ASPNET_ROLES " & _
                                "WHERE 1=1 "

    Private Const strModifica = "aspnet.update_aspnet_roles"
#End Region

#Region "Variables Privadas"
    Private _ROLEID As Nullable(Of Integer)
    Private _APPLICATIONID As Nullable(Of Integer)
    Private _ROLENAME As String
    Private _LOWEREDROLENAME As String
    Private _DESCRIPTION As String

    Private _USERID As Nullable(Of Integer)
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "ASPNET_ROLES"
        End Get
    End Property
    Public Property RoleId() As Nullable(Of Integer)
        Get
            Return _ROLEID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _ROLEID = value
        End Set
    End Property
    Public Property ApplicationId() As Nullable(Of Integer)
        Get
            Return _APPLICATIONID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _APPLICATIONID = value
        End Set
    End Property
    Public Property RoleName() As String
        Get
            Return _ROLENAME
        End Get
        Set(ByVal value As String)
            _ROLENAME = value
        End Set
    End Property
    Public Property LoweredRoleName() As String
        Get
            Return _LOWEREDROLENAME
        End Get
        Set(ByVal value As String)
            _LOWEREDROLENAME = value
        End Set
    End Property
    Public Property Descripcion() As String
        Get
            Return _DESCRIPTION
        End Get
        Set(ByVal value As String)
            _DESCRIPTION = value
        End Set
    End Property

    Public WriteOnly Property UserId() As Nullable(Of Integer)
        Set(ByVal value As Nullable(Of Integer))
            _USERID = value
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
        Me._ROLEID = Nothing
        Me._APPLICATIONID = Nothing
        Me._ROLENAME = String.Empty
        Me._LOWEREDROLENAME = String.Empty
        Me._DESCRIPTION = String.Empty
        Me._USERID = Nothing
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

                If (Me._ROLEID.HasValue) Then
                    .CommandText = .CommandText & " AND ROLEID = :P_ROLEID "
                    .Parameters.Add("P_ROLEID", NpgsqlDbType.Bigint).Value = Me._ROLEID.Value
                End If
                If Not String.IsNullOrEmpty(Me._ROLENAME) Then
                    .CommandText = .CommandText & " AND LOWEREDROLENAME = :P_LOWEREDROLENAME "
                    .Parameters.Add("P_LOWEREDROLENAME", NpgsqlDbType.Varchar).Value = Me._ROLENAME.ToLower()
                End If
                If (Me._USERID.HasValue) Then
                    .CommandText = .CommandText & " AND ROLEID IN (SELECT ROLEID FROM ASPNET_USERSINROLES WHERE USERID = :P_USERID ) "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
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
                If Not IsDBNull(.GetValue(.GetOrdinal("ROLEID"))) Then
                    Me._ROLEID = Convert.ToInt32(.GetValue(.GetOrdinal("ROLEID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("APPLICATIONID"))) Then
                    Me._APPLICATIONID = Convert.ToInt32(.GetValue(.GetOrdinal("APPLICATIONID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("ROLENAME"))) Then
                    Me._ROLENAME = .GetValue(.GetOrdinal("ROLENAME"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("LOWEREDROLENAME"))) Then
                    Me._LOWEREDROLENAME = .GetValue(.GetOrdinal("LOWEREDROLENAME"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("DESCRIPTION"))) Then
                    Me._DESCRIPTION = .GetValue(.GetOrdinal("DESCRIPTION"))
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
    Public Function bModificar() As Boolean

        If String.IsNullOrEmpty(Me._ROLENAME) Then
            Return False
        End If

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
            Dim SqlModifica As New NpgsqlCommand

            With SqlModifica
                .Connection = dbconexion
                .CommandText = strModifica
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                .Parameters.Add("P_ROLEID", NpgsqlDbType.Bigint).Value = Me._ROLEID
                .Parameters.Add("P_ROLENAME", NpgsqlDbType.Varchar).Value = Me._ROLENAME
                .Parameters.Add("P_DESCRIPTION", NpgsqlDbType.Varchar, 250).Value = Me._DESCRIPTION
            End With

            SqlModifica.ExecuteNonQuery()
            nResultado = SqlModifica.Parameters("RETURN_VALUE").Value

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

                If Not String.IsNullOrEmpty(Me._LOWEREDROLENAME) Then
                    .CommandText = .CommandText & " AND LOWEREDROLENAME LIKE :P_LOWEREDROLENAME "
                    .Parameters.Add("P_LOWEREDROLENAME", NpgsqlDbType.Varchar).Value = "%" + Me._LOWEREDROLENAME.ToLower() + "%"
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
            Return Nothing
        End Try

        Try
            Dim SqlConsulta As New NpgsqlCommand
            Dim daData As New NpgsqlDataAdapter

            With SqlConsulta
                .Connection = dbconexion
                .CommandText = strConsulta & psFiltro
                .CommandType = CommandType.Text

                If Not String.IsNullOrEmpty(Me._LOWEREDROLENAME) Then
                    .CommandText = .CommandText & " AND LOWEREDROLENAME LIKE :P_LOWEREDROLENAME "
                    .Parameters.Add("P_LOWEREDROLENAME", NpgsqlDbType.Varchar).Value = "%" + Me._LOWEREDROLENAME.ToLower() + "%"
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
