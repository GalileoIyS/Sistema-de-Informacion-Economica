Public Class cKPI_WIDGET_FILTERS
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT A.IDFILTER, A.IDEXPRESION, A.DIMENSIONID, A.FILTRO, A.VALOR, " & _
                                "B.NOMBRE " & _
                                "FROM KPI_WIDGET_FILTERS A LEFT JOIN KPI_DIMENSIONS B ON A.DIMENSIONID = B.DIMENSIONID " & _
                                "WHERE 1=1 "

    Private Const strInserta = "kpis.ADD_KPI_WIDGET_FILTERS"
    Private Const strModifica = "kpis.UPD_KPI_WIDGET_FILTERS"
    Private Const strElimina = "kpis.DEL_KPI_WIDGET_FILTERS"
#End Region

#Region "Variables Privadas"
    Private _IDFILTER As Nullable(Of Integer)
    Private _IDEXPRESION As Nullable(Of Integer)
    Private _DIMENSIONID As Nullable(Of Integer)
    Private _FILTRO As String
    Private _VALOR As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "KPI_WIDGET_FILTERS"
        End Get
    End Property
    Public Property idfilter As Nullable(Of Integer)
        Get
            Return _IDFILTER
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IDFILTER = value
        End Set
    End Property
    Public Property idexpresion As Nullable(Of Integer)
        Get
            Return _IDEXPRESION
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IDEXPRESION = value
        End Set
    End Property
    Public Property dimensionid As Nullable(Of Integer)
        Get
            Return _DIMENSIONID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _DIMENSIONID = value
        End Set
    End Property
    Public Property filtro As String
        Get
            Return _FILTRO
        End Get
        Set(ByVal value As String)
            If (value.Length > 10) Then
                _FILTRO = value.Substring(0, 10)
            Else
                _FILTRO = value
            End If
        End Set
    End Property
    Public Property valor As String
        Get
            Return _VALOR
        End Get
        Set(ByVal value As String)
            If (value.Length > 100) Then
                _VALOR = value.Substring(0, 100)
            Else
                _VALOR = value
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
        Me._IDFILTER = Nothing
        Me._IDEXPRESION = Nothing
        Me._DIMENSIONID = Nothing
        Me._FILTRO = String.Empty
        Me._VALOR = String.Empty
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

                If Me._IDEXPRESION.HasValue Then
                    .CommandText = .CommandText & " AND A.IDEXPRESION = :P_IDEXPRESION "
                    .Parameters.Add("P_IDEXPRESION", NpgsqlDbType.Bigint).Value = Me._IDEXPRESION.Value
                End If
                If Me._IDFILTER.HasValue Then
                    .CommandText = .CommandText & " AND A.IDFILTER = :P_IDFILTER "
                    .Parameters.Add("P_IDFILTER", NpgsqlDbType.Bigint).Value = Me._IDFILTER.Value
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
                If Not IsDBNull(.GetValue(.GetOrdinal("IDFILTER"))) Then
                    Me._IDFILTER = Convert.ToInt64(.GetValue(.GetOrdinal("IDFILTER")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("IDEXPRESION"))) Then
                    Me._IDEXPRESION = Convert.ToInt64(.GetValue(.GetOrdinal("IDEXPRESION")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("DIMENSIONID"))) Then
                    Me._DIMENSIONID = Convert.ToInt64(.GetValue(.GetOrdinal("DIMENSIONID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("FILTRO"))) Then
                    Me._FILTRO = .GetValue(.GetOrdinal("FILTRO"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("VALOR"))) Then
                    Me._VALOR = .GetValue(.GetOrdinal("VALOR"))
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
                .Parameters.Add("P_IDEXPRESION", NpgsqlDbType.Bigint).Value = Me._IDEXPRESION.Value
                .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                .Parameters.Add("P_FILTRO", NpgsqlDbType.Varchar, 10).Value = Me._FILTRO
                .Parameters.Add("P_VALOR", NpgsqlDbType.Varchar, 100).Value = Me._VALOR
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._IDFILTER = nResultado
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            Me._IDFILTER = -1
        Finally
            dbconexion.Close()
        End Try

        If Me._IDFILTER >= 0 Then
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
                .Parameters.Add("P_IDEXPRESION", NpgsqlDbType.Bigint).Value = Me._IDEXPRESION.Value
                .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                .Parameters.Add("P_FILTRO", NpgsqlDbType.Varchar, 10).Value = Me._FILTRO
                .Parameters.Add("P_VALOR", NpgsqlDbType.Varchar, 100).Value = Me._VALOR
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._IDFILTER = nResultado
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            Me._IDFILTER = -1
        End Try

        If Me._IDFILTER >= 0 Then
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
                .Parameters.Add("P_IDFILTER", NpgsqlDbType.Bigint).Value = Me._IDFILTER.Value
                .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                .Parameters.Add("P_FILTRO", NpgsqlDbType.Varchar, 10).Value = Me._FILTRO
                .Parameters.Add("P_VALOR", NpgsqlDbType.Varchar, 100).Value = Me._VALOR
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
                .Parameters.Add("P_IDFILTER", NpgsqlDbType.Bigint).Value = Me._IDFILTER.Value
                .Parameters.Add("P_DIMENSIONID", NpgsqlDbType.Bigint).Value = Me._DIMENSIONID.Value
                .Parameters.Add("P_FILTRO", NpgsqlDbType.Varchar, 10).Value = Me._FILTRO
                .Parameters.Add("P_VALOR", NpgsqlDbType.Varchar, 100).Value = Me._VALOR
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
                .Parameters.Add("P_IDFILTER", NpgsqlDbType.Bigint).Value = Me._IDFILTER.Value
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
    Public Function ObtenerDatos(ByVal pageSize As Integer, ByVal currentPage As Integer) As DataTable
        Dim dbconexion As New NpgsqlConnection
        Dim tblResultados As New System.Data.DataTable

        'Limpiamos el contenido de la Tabla de Datos
        tblResultados.Clear()

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

            With SqlConsulta
                .Connection = dbconexion
                .CommandText = strConsulta
                .CommandType = CommandType.Text

                If Me._IDEXPRESION.HasValue Then
                    .CommandText = .CommandText & " AND A.IDEXPRESION = :P_IDEXPRESION "
                    .Parameters.Add("P_IDEXPRESION", NpgsqlDbType.Bigint).Value = Me._IDEXPRESION.Value
                End If
                If Me._IDFILTER.HasValue Then
                    .CommandText = .CommandText & " AND A.IDFILTER = :P_IDFILTER "
                    .Parameters.Add("P_IDFILTER", NpgsqlDbType.Bigint).Value = Me._IDFILTER.Value
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
    Public Function ObtenerDatos(ByVal pageSize As Integer, ByVal currentPage As Integer, ByVal psFiltro As String, ByVal psOrderBy As String) As DataTable
        Dim dbconexion As New NpgsqlConnection
        Dim tblResultados As New System.Data.DataTable

        'Limpiamos el contenido de la Tabla de Datos
        tblResultados.Clear()

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

            With SqlConsulta
                .Connection = dbconexion
                .CommandType = CommandType.Text
                .CommandText = strConsulta

                If Me._IDEXPRESION.HasValue Then
                    .CommandText = .CommandText & " AND A.IDEXPRESION = :P_IDEXPRESION "
                    .Parameters.Add("P_IDEXPRESION", NpgsqlDbType.Bigint).Value = Me._IDEXPRESION.Value
                End If
                If Me._IDFILTER.HasValue Then
                    .CommandText = .CommandText & " AND A.IDFILTER = :P_IDFILTER "
                    .Parameters.Add("P_IDFILTER", NpgsqlDbType.Bigint).Value = Me._IDFILTER.Value
                End If
                .CommandText = .CommandText & psFiltro
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
