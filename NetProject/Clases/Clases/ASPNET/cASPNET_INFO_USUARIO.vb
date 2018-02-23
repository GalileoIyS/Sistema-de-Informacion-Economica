Public Class cASPNET_INFO_USUARIO
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT A.USERID, A.VALIDATIONCODE, A.VALIDATED, A.SUSCRITO, A.VALIDATIONEMAILSENT, " & _
                                "A.NOMBRE, A.APELLIDOS, A.PUESTO, A.RESUMEN, A.IMAGEURL " & _
                                "FROM ASPNET_INFO_USUARIO A " & _
                                "WHERE 1=1 "

    Private Const strModificaDatos = "aspnet.update_aspnet_usuario"

#End Region

#Region "Variables Privadas"
    Private _USERID As Nullable(Of Integer)
    Private _VALIDATIONEMAILSENT As String
    Private _VALIDATED As String
    Private _VALIDATIONCODE As String
    Private _SUSCRITO As String
    Private _NOMBRE As String
    Private _APELLIDOS As String
    Private _PUESTO As String
    Private _RESUMEN As String
    Private _IMAGEURL As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "ASPNET_INFO_USUARIO"
        End Get
    End Property
    Public Property userid As Nullable(Of Integer)
        Get
            Return _USERID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _USERID = value
        End Set
    End Property
    Public Property mensaje_validacion As Boolean
        Get
            If _VALIDATIONEMAILSENT = "S" Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _VALIDATIONEMAILSENT = "S"
            Else
                _VALIDATIONEMAILSENT = "N"
            End If
        End Set
    End Property
    Public Property validado As Boolean
        Get
            If _VALIDATED = "S" Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _VALIDATED = "S"
            Else
                _VALIDATED = "N"
            End If
        End Set
    End Property
    Public Property codigo_validacion As String
        Get
            Return _VALIDATIONCODE
        End Get
        Set(ByVal value As String)
            If (value.Length > 100) Then
                _VALIDATIONCODE = value.Substring(0, 100)
            Else
                _VALIDATIONCODE = value
            End If
        End Set
    End Property
    Public Property suscrito As Boolean
        Get
            If _SUSCRITO = "S" Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _SUSCRITO = "S"
            Else
                _SUSCRITO = "N"
            End If
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
    Public Property apellidos As String
        Get
            Return _APELLIDOS
        End Get
        Set(ByVal value As String)
            _APELLIDOS = value
        End Set
    End Property
    Public Property puesto As String
        Get
            Return _PUESTO
        End Get
        Set(ByVal value As String)
            _PUESTO = value
        End Set
    End Property
    Public Property resumen As String
        Get
            Return _RESUMEN
        End Get
        Set(ByVal value As String)
            _RESUMEN = value
        End Set
    End Property
    Public Property imageurl As String
        Get
            Return _IMAGEURL
        End Get
        Set(ByVal value As String)
            _IMAGEURL = value
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
        Me._VALIDATIONEMAILSENT = String.Empty
        Me._VALIDATED = String.Empty
        Me._VALIDATIONCODE = String.Empty
        Me._SUSCRITO = String.Empty
        Me._NOMBRE = String.Empty
        Me._APELLIDOS = String.Empty
        Me._PUESTO = String.Empty
        Me._RESUMEN = String.Empty
        Me._IMAGEURL = String.Empty
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

                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID"
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
                If Not String.IsNullOrEmpty(Me._VALIDATIONEMAILSENT) Then
                    .CommandText = .CommandText & " AND A.VALIDATIONEMAILSENT = :P_VALIDATIONEMAILSENT "
                    .Parameters.Add("P_VALIDATIONEMAILSENT", NpgsqlDbType.Char, 1).Value = Me._VALIDATIONEMAILSENT
                End If
                If Not String.IsNullOrEmpty(Me._VALIDATED) Then
                    .CommandText = .CommandText & " AND A.VALIDATED = :P_VALIDATED "
                    .Parameters.Add("P_VALIDATED", NpgsqlDbType.Char, 1).Value = Me._VALIDATED
                End If
                If Not String.IsNullOrEmpty(Me._VALIDATIONCODE) Then
                    .CommandText = .CommandText & " AND A.VALIDATIONCODE = :P_VALIDATIONCODE "
                    .Parameters.Add("P_VALIDATIONCODE", NpgsqlDbType.Varchar, 100).Value = Me._VALIDATIONCODE
                End If
                If Not String.IsNullOrEmpty(Me._SUSCRITO) Then
                    .CommandText = .CommandText & " AND A.SUSCRITO = :P_SUSCRITO "
                    .Parameters.Add("P_SUSCRITO", NpgsqlDbType.Char, 1).Value = Me._SUSCRITO
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
                If Not IsDBNull(.GetValue(.GetOrdinal("USERID"))) Then
                    Me._USERID = Convert.ToInt64(.GetValue(.GetOrdinal("USERID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("VALIDATIONEMAILSENT"))) Then
                    Me._VALIDATIONEMAILSENT = .GetValue(.GetOrdinal("VALIDATIONEMAILSENT"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("VALIDATED"))) Then
                    Me._VALIDATED = .GetValue(.GetOrdinal("VALIDATED"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("VALIDATIONCODE"))) Then
                    Me._VALIDATIONCODE = .GetValue(.GetOrdinal("VALIDATIONCODE"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("SUSCRITO"))) Then
                    Me._SUSCRITO = .GetValue(.GetOrdinal("SUSCRITO"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("NOMBRE"))) Then
                    Me._NOMBRE = .GetValue(.GetOrdinal("NOMBRE"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("APELLIDOS"))) Then
                    Me._APELLIDOS = .GetValue(.GetOrdinal("APELLIDOS"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("PUESTO"))) Then
                    Me._PUESTO = .GetValue(.GetOrdinal("PUESTO"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("RESUMEN"))) Then
                    Me._RESUMEN = .GetValue(.GetOrdinal("RESUMEN"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("IMAGEURL"))) Then
                    Me._IMAGEURL = .GetValue(.GetOrdinal("IMAGEURL"))
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
                .CommandText = strModificaDatos
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Integer).Direction = System.Data.ParameterDirection.ReturnValue
                .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID
                .Parameters.Add("P_VALIDATIONEMAILSENT", NpgsqlDbType.Char, 1).Value = Me._VALIDATIONEMAILSENT
                .Parameters.Add("P_VALIDATIONCODE", NpgsqlDbType.Varchar, 100).Value = Me._VALIDATIONCODE
                .Parameters.Add("P_VALIDATED", NpgsqlDbType.Char, 1).Value = Me._VALIDATED
                .Parameters.Add("P_SUSCRITO", NpgsqlDbType.Char, 1).Value = Me._SUSCRITO
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Text).Value = Me._NOMBRE
                .Parameters.Add("P_APELLIDOS", NpgsqlDbType.Text).Value = Me._APELLIDOS
                .Parameters.Add("P_PUESTO", NpgsqlDbType.Text).Value = Me._PUESTO
                .Parameters.Add("P_RESUMEN", NpgsqlDbType.Text).Value = Me._RESUMEN
                .Parameters.Add("P_IMAGEURL", NpgsqlDbType.Text).Value = Me._IMAGEURL
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

                If Me.userid.HasValue Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID"
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
                If Not String.IsNullOrEmpty(Me._VALIDATIONEMAILSENT) Then
                    .CommandText = .CommandText & " AND A.VALIDATIONEMAILSENT = :P_VALIDATIONEMAILSENT "
                    .Parameters.Add("P_VALIDATIONEMAILSENT", NpgsqlDbType.Char, 1).Value = Me._VALIDATIONEMAILSENT
                End If
                If Not String.IsNullOrEmpty(Me._VALIDATED) Then
                    .CommandText = .CommandText & " AND A.VALIDATED = :P_VALIDATED "
                    .Parameters.Add("P_VALIDATED", NpgsqlDbType.Char, 1).Value = Me._VALIDATED
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

                If Me.userid.HasValue Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID"
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
                If Not String.IsNullOrEmpty(Me._VALIDATIONEMAILSENT) Then
                    .CommandText = .CommandText & " AND A.VALIDATIONEMAILSENT = :P_VALIDATIONEMAILSENT "
                    .Parameters.Add("P_VALIDATIONEMAILSENT", NpgsqlDbType.Char, 1).Value = Me._VALIDATIONEMAILSENT
                End If
                If Not String.IsNullOrEmpty(Me._VALIDATED) Then
                    .CommandText = .CommandText & " AND A.VALIDATED = :P_VALIDATED "
                    .Parameters.Add("P_VALIDATED", NpgsqlDbType.Char, 1).Value = Me._VALIDATED
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
