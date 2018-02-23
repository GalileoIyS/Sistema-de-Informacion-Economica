Public Class cKPI_WIDGET_FORMULAS
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT IDFORMULA, IDWIDGET, FECHA, FORMULA, " & _
                                "NOMBRE, COLOR, ORIGINAL, VALIDATED, DISPLAY " & _
                                "FROM KPI_WIDGET_FORMULAS " & _
                                "WHERE 1=1 "

    Private Const strInserta = "kpis.ADD_KPI_WIDGET_FORMULAS"
    Private Const strModifica = "kpis.UPD_KPI_WIDGET_FORMULAS"
    Private Const strElimina = "kpis.DEL_KPI_WIDGET_FORMULAS"
#End Region

#Region "Variables Privadas"
    Private _IDFORMULA As Nullable(Of Integer)
    Private _IDWIDGET As Nullable(Of Integer)
    Private _FECHA As Nullable(Of DateTime)
    Private _FORMULA As String
    Private _NOMBRE As String
    Private _COLOR As String
    Private _ORIGINAL As String
    Private _VALIDATED As Nullable(Of Integer)
    Private _DISPLAY As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "KPI_WIDGET_FORMULAS"
        End Get
    End Property
    Public Property idformula As Nullable(Of Integer)
        Get
            Return _IDFORMULA
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IDFORMULA = value
        End Set
    End Property
    Public Property idwidget As Nullable(Of Integer)
        Get
            Return _IDWIDGET
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IDWIDGET = value
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
    Public Property formula As String
        Get
            Return _FORMULA
        End Get
        Set(ByVal value As String)
            If (value.Length > 500) Then
                _FORMULA = value.Substring(0, 500)
            Else
                _FORMULA = value
            End If
        End Set
    End Property
    Public Property nombre As String
        Get
            Return _NOMBRE
        End Get
        Set(ByVal value As String)
            If (value.Length > 23) Then
                _NOMBRE = value.Substring(0, 23)
            Else
                _NOMBRE = value
            End If
        End Set
    End Property
    Public Property color As String
        Get
            Return _COLOR
        End Get
        Set(ByVal value As String)
            If (value.Length > 10) Then
                _COLOR = value.Substring(0, 10)
            Else
                _COLOR = value
            End If
        End Set
    End Property
    Public Property original As String
        Get
            Return _ORIGINAL
        End Get
        Set(ByVal value As String)
            If (value.Length > 500) Then
                _ORIGINAL = value.Substring(0, 500)
            Else
                _ORIGINAL = value
            End If
        End Set
    End Property
    Public Property validado As Boolean
        Get
            If _VALIDATED.HasValue Then
                If _VALIDATED.Value = 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _VALIDATED = 0
            Else
                _VALIDATED = 1
            End If
        End Set
    End Property
    Public Property display As String
        Get
            Return _DISPLAY
        End Get
        Set(ByVal value As String)
            If (value.Length > 500) Then
                _DISPLAY = value.Substring(0, 500)
            Else
                _DISPLAY = value
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
        Me._IDFORMULA = Nothing
        Me._IDWIDGET = Nothing
        Me._FECHA = Nothing
        Me._FORMULA = String.Empty
        Me._NOMBRE = String.Empty
        Me._COLOR = String.Empty
        Me._ORIGINAL = String.Empty
        Me._VALIDATED = 0
        Me._DISPLAY = String.Empty
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

                If Me._IDFORMULA.HasValue Then
                    .CommandText = .CommandText & " AND IDFORMULA = :P_IDFORMULA "
                    .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = Me._IDFORMULA.Value
                End If
                If Me._VALIDATED.HasValue Then
                    .CommandText = .CommandText & " AND VALIDATED = :P_VALIDATED "
                    .Parameters.Add("P_VALIDATED", NpgsqlDbType.Bigint).Value = Me._VALIDATED.Value
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
                If Not IsDBNull(.GetValue(.GetOrdinal("IDFORMULA"))) Then
                    Me._IDFORMULA = Convert.ToInt64(.GetValue(.GetOrdinal("IDFORMULA")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("IDWIDGET"))) Then
                    Me._IDWIDGET = Convert.ToInt64(.GetValue(.GetOrdinal("IDWIDGET")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("FECHA"))) Then
                    Me._FECHA = Convert.ToDateTime(.GetValue(.GetOrdinal("FECHA")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("FORMULA"))) Then
                    Me._FORMULA = .GetValue(.GetOrdinal("FORMULA"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("NOMBRE"))) Then
                    Me._NOMBRE = .GetValue(.GetOrdinal("NOMBRE"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("COLOR"))) Then
                    Me._COLOR = .GetValue(.GetOrdinal("COLOR"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("ORIGINAL"))) Then
                    Me._ORIGINAL = .GetValue(.GetOrdinal("ORIGINAL"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("VALIDATED"))) Then
                    Me._VALIDATED = Convert.ToInt64(.GetValue(.GetOrdinal("VALIDATED")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("DISPLAY"))) Then
                    Me._DISPLAY = .GetValue(.GetOrdinal("DISPLAY"))
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
                .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 23).Value = Me._NOMBRE.Trim()
                .Parameters.Add("P_FORMULA", NpgsqlDbType.Varchar, 500).Value = Me._FORMULA.Trim()
                .Parameters.Add("P_COLOR", NpgsqlDbType.Varchar, 10).Value = Me._COLOR.Trim()
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._IDFORMULA = nResultado
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
                .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 23).Value = Me._NOMBRE.Trim()
                .Parameters.Add("P_FORMULA", NpgsqlDbType.Varchar, 500).Value = Me._FORMULA.Trim()
                .Parameters.Add("P_COLOR", NpgsqlDbType.Varchar, 10).Value = Me._COLOR.Trim()
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._IDFORMULA = nResultado
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
                .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = Me._IDFORMULA.Value
                .Parameters.Add("P_FORMULA", NpgsqlDbType.Varchar, 500).Value = Me._FORMULA.Trim()
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 23).Value = Me._NOMBRE.Trim()
                .Parameters.Add("P_COLOR", NpgsqlDbType.Varchar, 10).Value = Me._COLOR.Trim()
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
                .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = Me._IDFORMULA.Value
                .Parameters.Add("P_FORMULA", NpgsqlDbType.Varchar, 500).Value = Me._FORMULA.Trim()
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 23).Value = Me._NOMBRE.Trim()
                .Parameters.Add("P_COLOR", NpgsqlDbType.Varchar, 10).Value = Me._COLOR.Trim()
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
                .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = Me._IDFORMULA.Value
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
                .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = Me._IDFORMULA.Value
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

                If Me._IDFORMULA.HasValue Then
                    .CommandText = .CommandText & " AND IDFORMULA = :P_IDFORMULA "
                    .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = Me._IDFORMULA.Value
                End If
                If Me._IDWIDGET.HasValue Then
                    .CommandText = .CommandText & " AND IDWIDGET = :P_IDWIDGET "
                    .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                End If
                .CommandText = .CommandText & " ORDER BY IDFORMULA ASC"
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

                If Me._IDFORMULA.HasValue Then
                    .CommandText = .CommandText & " AND IDFORMULA = :P_IDFORMULA "
                    .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = Me._IDFORMULA.Value
                End If
                If Me._IDWIDGET.HasValue Then
                    .CommandText = .CommandText & " AND IDWIDGET = :P_IDWIDGET "
                    .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                End If
                .CommandText = .CommandText & " ORDER BY IDFORMULA ASC"
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

#Region "Otras funciones Publicas de la Clase"
    Public Function bCopiar() As Boolean
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
                .Parameters.Add("P_IDWIDGET", NpgsqlDbType.Bigint).Value = Me._IDWIDGET.Value
                .Parameters.Add("P_IDFORMULA", NpgsqlDbType.Bigint).Value = Me._IDFORMULA.Value
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 23).Value = Me._NOMBRE.Trim()
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._IDFORMULA = nResultado
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
