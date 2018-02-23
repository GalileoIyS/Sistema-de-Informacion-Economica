Public Class cKPI_ETIQUETAS
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT ETIQUETAID, NOMBRE, UPPERNOMBRE " & _
                                "FROM KPI_ETIQUETAS " & _
                                "WHERE 1=1 "

    Private Const strInserta = "kpis.ADD_KPI_ETIQUETAS"
    Private Const strModifica = "kpis.UPD_KPI_ETIQUETAS"
    Private Const strElimina = "kpis.DEL_KPI_ETIQUETAS"
#End Region

#Region "Variables Privadas"
    Private _ETIQUETAID As Nullable(Of Integer)
    Private _NOMBRE As String
    Private _UPPERNOMBRE As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "WR_ETIQUETAS"
        End Get
    End Property
    Public Property etiquetaid As Nullable(Of Integer)
        Get
            Return _ETIQUETAID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _ETIQUETAID = value
        End Set
    End Property
    Public Property nombre As String
        Get
            Return _NOMBRE
        End Get
        Set(ByVal value As String)
            If (value.Length > 100) Then
                _NOMBRE = value.Substring(0, 100)
            Else
                _NOMBRE = value
            End If
        End Set
    End Property
    Public Property uppernombre As String
        Get
            Return _UPPERNOMBRE
        End Get
        Set(ByVal value As String)
            If (value.Length > 100) Then
                _UPPERNOMBRE = value.Substring(0, 100)
            Else
                _UPPERNOMBRE = value
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
        Me._ETIQUETAID = Nothing
        Me._NOMBRE = String.Empty
        Me._UPPERNOMBRE = String.Empty
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

                If Me._ETIQUETAID.HasValue Then
                    .CommandText = .CommandText & " AND ETIQUETAID = :P_ETIQUETAID"
                    .Parameters.Add("P_ETIQUETAID", NpgsqlDbType.Bigint).Value = Me._ETIQUETAID.Value
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
                If Not IsDBNull(.GetValue(.GetOrdinal("ETIQUETAID"))) Then
                    Me._ETIQUETAID = Convert.ToInt64(.GetValue(.GetOrdinal("ETIQUETAID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("NOMBRE"))) Then
                    Me._NOMBRE = .GetValue(.GetOrdinal("NOMBRE"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("UPPERNOMBRE"))) Then
                    Me._UPPERNOMBRE = .GetValue(.GetOrdinal("UPPERNOMBRE"))
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
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 100).Value = Me._NOMBRE
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._ETIQUETAID = nResultado
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            Me._ETIQUETAID = -1
        Finally
            dbconexion.Close()
        End Try

        If Me._ETIQUETAID >= 0 Then
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
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 100).Value = Me._NOMBRE
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._ETIQUETAID = nResultado
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            Me._ETIQUETAID = -1
        End Try

        If Me._ETIQUETAID >= 0 Then
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
                .Parameters.Add("P_ETIQUETAID", NpgsqlDbType.Bigint).Value = Me._ETIQUETAID.Value
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 100).Value = Me._NOMBRE
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
                .Parameters.Add("P_ETIQUETAID", NpgsqlDbType.Bigint).Value = Me._ETIQUETAID.Value
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 100).Value = Me._NOMBRE
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
                .Parameters.Add("P_ETIQUETAID", NpgsqlDbType.Bigint).Value = Me._ETIQUETAID.Value
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
                .Parameters.Add("P_ETIQUETAID", NpgsqlDbType.Bigint).Value = Me._ETIQUETAID.Value
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

                If Me.ETIQUETAID.HasValue Then
                    .CommandText = .CommandText & " AND ETIQUETAID = :P_ETIQUETAID"
                    .Parameters.Add("P_ETIQUETAID", NpgsqlDbType.Bigint).Value = Me._ETIQUETAID.Value
                End If
                If Not String.IsNullOrEmpty(Me._NOMBRE) Then
                    .CommandText = .CommandText & " AND LOWER(NOMBRE) LIKE :P_NOMBRE "
                    .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 250).Value = "%" & Me._NOMBRE.ToLower() & "%"
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

                If Me.ETIQUETAID.HasValue Then
                    .CommandText = .CommandText & " AND ETIQUETAID = :P_ETIQUETAID"
                    .Parameters.Add("P_ETIQUETAID", NpgsqlDbType.Bigint).Value = Me._ETIQUETAID.Value
                End If
                If Not String.IsNullOrEmpty(Me._NOMBRE) Then
                    .CommandText = .CommandText & " AND LOWER(NOMBRE) LIKE :P_NOMBRE "
                    .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 250).Value = "%" & Me._NOMBRE.ToLower() & "%"
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

#Region "Otras Funciones"
    Public Function TopXUtilizadas(ByVal pnPrimeros As Integer) As DataTable
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
                .CommandText = "SELECT X.ETIQUETAID, X.NOMBRE, X.CONTADOR , get_random_number(1, 10) TAMANO FROM " & _
                               "(SELECT A.ETIQUETAID, A.NOMBRE, COUNT(B.ETIQUETAID) CONTADOR FROM KPI_ETIQUETAS A " & _
                               "LEFT JOIN KPI_INDICATOR_ETIQUETAS B ON A.ETIQUETAID = B.ETIQUETAID " & _
                               "GROUP BY A.ETIQUETAID, A.NOMBRE " & _
                               "ORDER BY CONTADOR DESC) X " & _
                               "LIMIT :P_ROWNUM "
                .CommandType = CommandType.Text

                .Parameters.Add("P_ROWNUM", NpgsqlDbType.Bigint).Value = pnPrimeros
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
