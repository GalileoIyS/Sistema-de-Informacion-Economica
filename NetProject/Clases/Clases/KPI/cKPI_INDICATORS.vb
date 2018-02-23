Public Class cKPI_INDICATORS
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT INDICATORID, TITULO, RESUMEN, DESCRIPCION, FECHA_ALTA, FECHA_MODIFICACION, " & _
                                "USERID, USERNAME, SIMBOLO, UNIDAD, RATING_VOTES, " & _
                                "CASE WHEN RATING_VOTES > 0 THEN ROUND(RATING_VALUES / RATING_VOTES) ELSE 0 END RATING_VALUES, " & _
                                "COMPARTIDO, FUNCION_AGREGADA, ESNOVEDAD, SUBCATEGORYID, " & _
                                "CATEGORYID, IMAGEURL, PUBLICADO, SISTEMA " & _
                                "FROM KPI_INDICATORS " & _
                                "WHERE 1=1 "

    Private Const strInserta = "kpis.ADD_KPI_INDICATORS"
    Private Const strModifica = "kpis.UPD_KPI_INDICATORS"
    Private Const strElimina = "kpis.DEL_KPI_INDICATORS"
    Private Const strPublicar = "kpis.SHARE_KPI_INDICATORS"
#End Region

#Region "Variables Privadas"
    Private _INDICATORID As Nullable(Of Integer)
    Private _TITULO As String
    Private _RESUMEN As String
    Private _DESCRIPCION As String
    Private _FECHA_ALTA As Nullable(Of DateTime)
    Private _FECHA_MODIFICACION As Nullable(Of DateTime)
    Private _USERID As Nullable(Of Integer)
    Private _USERNAME As String
    Private _UNIDAD As String
    Private _SIMBOLO As String
    Private _COMPARTIDO As String
    Private _FUNCION_AGREGADA As String
    Private _ESNOVEDAD As String
    Private _RATING_VOTES As Nullable(Of Integer)
    Private _RATING_VALUES As Nullable(Of Integer)
    Private _CATEGORYID As Nullable(Of Integer)
    Private _SUBCATEGORYID As Nullable(Of Integer)
    Private _IMAGEURL As String
    Private _PUBLICADO As String
    Private _SISTEMA As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "KPI_INDICATORS"
        End Get
    End Property
    Public Property indicatorid As Nullable(Of Integer)
        Get
            Return _INDICATORID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _INDICATORID = value
        End Set
    End Property
    Public Property titulo As String
        Get
            Return _TITULO
        End Get
        Set(ByVal value As String)
            If (value.Length > 250) Then
                _TITULO = value.Substring(0, 250)
            Else
                _TITULO = value
            End If
        End Set
    End Property
    Public Property resumen As String
        Get
            Return _RESUMEN
        End Get
        Set(ByVal value As String)
            If (value.Length > 1000) Then
                _RESUMEN = value.Substring(0, 1000)
            Else
                _RESUMEN = value
            End If
        End Set
    End Property
    Public Property descripcion As String
        Get
            Return _DESCRIPCION
        End Get
        Set(ByVal value As String)
            _DESCRIPCION = value
        End Set
    End Property
    Public Property fecha_alta As Nullable(Of DateTime)
        Get
            Return _FECHA_ALTA
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FECHA_ALTA = value
        End Set
    End Property
    Public Property fecha_modificacion As Nullable(Of DateTime)
        Get
            Return _FECHA_MODIFICACION
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FECHA_MODIFICACION = value
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
    Public Property usuario As String
        Get
            Return _USERNAME
        End Get
        Set(ByVal value As String)
            _USERNAME = value
        End Set
    End Property
    Public Property unidad As String
        Get
            Return _UNIDAD
        End Get
        Set(ByVal value As String)
            If (value.Length > 100) Then
                _UNIDAD = value.Substring(0, 100)
            Else
                _UNIDAD = value
            End If
        End Set
    End Property
    Public Property simbolo As String
        Get
            Return _SIMBOLO
        End Get
        Set(ByVal value As String)
            If (value.Length > 5) Then
                _SIMBOLO = value.Substring(0, 5)
            Else
                _SIMBOLO = value
            End If
        End Set
    End Property
    Public Property compartido As Boolean
        Get
            If _COMPARTIDO = "S" Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _COMPARTIDO = "S"
            Else
                _COMPARTIDO = "N"
            End If
        End Set
    End Property
    Public Property funcion_agregada As String
        Get
            Return _FUNCION_AGREGADA
        End Get
        Set(ByVal value As String)
            _FUNCION_AGREGADA = value
        End Set
    End Property
    Public Property es_novedad As Boolean
        Get
            If _ESNOVEDAD = "S" Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _ESNOVEDAD = "S"
            Else
                _ESNOVEDAD = "N"
            End If
        End Set
    End Property
    Public Property RatingVotes As Nullable(Of Integer)
        Get
            Return _RATING_VOTES
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _RATING_VOTES = value
        End Set
    End Property
    Public Property RatingValues As Nullable(Of Integer)
        Get
            Return _RATING_VALUES
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _RATING_VALUES = value
        End Set
    End Property
    Public Property categoryid As Nullable(Of Integer)
        Get
            Return _CATEGORYID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _CATEGORYID = value
        End Set
    End Property
    Public Property subcategoryid As Nullable(Of Integer)
        Get
            Return _SUBCATEGORYID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _SUBCATEGORYID = value
        End Set
    End Property
    Public Property imageurl As String
        Get
            Return _IMAGEURL
        End Get
        Set(ByVal value As String)
            If (value.Length > 200) Then
                _IMAGEURL = value.Substring(0, 200)
            Else
                _IMAGEURL = value
            End If
        End Set
    End Property
    Public Property publicado As Boolean
        Get
            If _PUBLICADO = "S" Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _PUBLICADO = "S"
            Else
                _PUBLICADO = "N"
            End If
        End Set
    End Property
    Public Property es_de_sistema As Boolean
        Get
            If _SISTEMA = "S" Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            If value Then
                _SISTEMA = "S"
            Else
                _SISTEMA = "N"
            End If
        End Set
    End Property

    Public ReadOnly Property funcion_agregada_desc As String
        Get
            Select Case _FUNCION_AGREGADA
                Case "SUM" : Return "Sum"
                Case "AVG" : Return "Average"
                Case "MAX" : Return "Maximum"
                Case "MIN" : Return "Minimum"
                Case Else : Return _FUNCION_AGREGADA
            End Select
        End Get
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
        Me._INDICATORID = Nothing
        Me._TITULO = String.Empty
        Me._RESUMEN = String.Empty
        Me._DESCRIPCION = String.Empty
        Me._FECHA_ALTA = Nothing
        Me._FECHA_MODIFICACION = Nothing
        Me._USERID = Nothing
        Me._USERNAME = String.Empty
        Me._UNIDAD = String.Empty
        Me._SIMBOLO = String.Empty
        Me._COMPARTIDO = String.Empty
        Me._FUNCION_AGREGADA = String.Empty
        Me._ESNOVEDAD = String.Empty
        Me._RATING_VOTES = Nothing
        Me._RATING_VALUES = Nothing
        Me._CATEGORYID = Nothing
        Me._SUBCATEGORYID = Nothing
        Me._IMAGEURL = String.Empty
        Me._PUBLICADO = String.Empty
        Me._SISTEMA = String.Empty
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

                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND INDICATORID = :P_INDICATORID"
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
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
                If Not IsDBNull(.GetValue(.GetOrdinal("INDICATORID"))) Then
                    Me._INDICATORID = Convert.ToInt64(.GetValue(.GetOrdinal("INDICATORID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("TITULO"))) Then
                    Me._TITULO = .GetValue(.GetOrdinal("TITULO"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("RESUMEN"))) Then
                    Me._RESUMEN = .GetValue(.GetOrdinal("RESUMEN"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("DESCRIPCION"))) Then
                    Me._DESCRIPCION = .GetValue(.GetOrdinal("DESCRIPCION"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("FECHA_ALTA"))) Then
                    Me._FECHA_ALTA = Convert.ToDateTime(.GetValue(.GetOrdinal("FECHA_ALTA")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("FECHA_MODIFICACION"))) Then
                    Me._FECHA_MODIFICACION = Convert.ToDateTime(.GetValue(.GetOrdinal("FECHA_MODIFICACION")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("USERID"))) Then
                    Me._USERID = Convert.ToInt64(.GetValue(.GetOrdinal("USERID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("USERNAME"))) Then
                    Me._USERNAME = .GetValue(.GetOrdinal("USERNAME"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("UNIDAD"))) Then
                    Me._UNIDAD = .GetValue(.GetOrdinal("UNIDAD"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("SIMBOLO"))) Then
                    Me._SIMBOLO = .GetValue(.GetOrdinal("SIMBOLO"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("COMPARTIDO"))) Then
                    Me._COMPARTIDO = .GetValue(.GetOrdinal("COMPARTIDO"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("FUNCION_AGREGADA"))) Then
                    Me._FUNCION_AGREGADA = .GetValue(.GetOrdinal("FUNCION_AGREGADA"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("ESNOVEDAD"))) Then
                    Me._ESNOVEDAD = .GetValue(.GetOrdinal("ESNOVEDAD"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("RATING_VOTES"))) Then
                    Me._RATING_VOTES = Convert.ToInt32(.GetValue(.GetOrdinal("RATING_VOTES")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("RATING_VALUES"))) Then
                    Me._RATING_VALUES = Convert.ToInt32(.GetValue(.GetOrdinal("RATING_VALUES")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("CATEGORYID"))) Then
                    Me._CATEGORYID = Convert.ToInt64(.GetValue(.GetOrdinal("CATEGORYID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("SUBCATEGORYID"))) Then
                    Me._SUBCATEGORYID = Convert.ToInt64(.GetValue(.GetOrdinal("SUBCATEGORYID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("IMAGEURL"))) Then
                    Me._IMAGEURL = .GetValue(.GetOrdinal("IMAGEURL"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("PUBLICADO"))) Then
                    Me._PUBLICADO = .GetValue(.GetOrdinal("PUBLICADO"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("SISTEMA"))) Then
                    Me._SISTEMA = .GetValue(.GetOrdinal("SISTEMA"))
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
                .Parameters.Add("P_TITULO", NpgsqlDbType.Varchar, 250).Value = Me._TITULO
                .Parameters.Add("P_RESUMEN", NpgsqlDbType.Varchar, 1000).Value = Me._RESUMEN
                If Me._USERID.HasValue Then
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                Else
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_USERNAME", NpgsqlDbType.Varchar, 40).Value = Me._USERNAME
                .Parameters.Add("P_UNIDAD", NpgsqlDbType.Varchar, 100).Value = Me._UNIDAD
                .Parameters.Add("P_SIMBOLO", NpgsqlDbType.Varchar, 5).Value = Me._SIMBOLO
                .Parameters.Add("P_COMPARTIDO", NpgsqlDbType.Char, 1).Value = Me._COMPARTIDO
                .Parameters.Add("P_FUNCION_AGREGADA", NpgsqlDbType.Varchar, 10).Value = Me._FUNCION_AGREGADA
                If Me._CATEGORYID.HasValue Then
                    .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = Me._CATEGORYID.Value
                Else
                    .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_PUBLICADO", NpgsqlDbType.Char, 1).Value = Me._PUBLICADO
                .Parameters.Add("P_SISTEMA", NpgsqlDbType.Char, 1).Value = Me._SISTEMA
                .Parameters.Add("P_IMAGEURL", NpgsqlDbType.Varchar, 200).Value = Me._IMAGEURL
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._INDICATORID = nResultado
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            Me._INDICATORID = -1
        Finally
            dbconexion.Close()
        End Try

        If Me._INDICATORID < 0 Then
            Return False
        Else
            Return True
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
                .Parameters.Add("P_TITULO", NpgsqlDbType.Varchar, 250).Value = Me._TITULO
                .Parameters.Add("P_RESUMEN", NpgsqlDbType.Varchar, 1000).Value = Me._RESUMEN
                If Me._USERID.HasValue Then
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                Else
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_USERNAME", NpgsqlDbType.Varchar, 40).Value = Me._USERNAME
                .Parameters.Add("P_UNIDAD", NpgsqlDbType.Varchar, 100).Value = Me._UNIDAD
                .Parameters.Add("P_SIMBOLO", NpgsqlDbType.Varchar, 5).Value = Me._SIMBOLO
                .Parameters.Add("P_COMPARTIDO", NpgsqlDbType.Char, 1).Value = Me._COMPARTIDO
                .Parameters.Add("P_FUNCION_AGREGADA", NpgsqlDbType.Varchar, 10).Value = Me._FUNCION_AGREGADA
                If Me._CATEGORYID.HasValue Then
                    .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = Me._CATEGORYID.Value
                Else
                    .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_PUBLICADO", NpgsqlDbType.Char, 1).Value = Me._PUBLICADO
                .Parameters.Add("P_SISTEMA", NpgsqlDbType.Char, 1).Value = Me._SISTEMA
                .Parameters.Add("P_IMAGEURL", NpgsqlDbType.Varchar, 200).Value = Me._IMAGEURL
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._INDICATORID = nResultado
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            Me._INDICATORID = -1
        End Try

        If Me._INDICATORID < 0 Then
            Return False
        Else
            Return True
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
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                .Parameters.Add("P_TITULO", NpgsqlDbType.Varchar, 250).Value = Me._TITULO
                .Parameters.Add("P_RESUMEN", NpgsqlDbType.Varchar, 1000).Value = Me._RESUMEN
                .Parameters.Add("P_DESCRIPCION", NpgsqlDbType.Text).Value = Me._DESCRIPCION
                .Parameters.Add("P_UNIDAD", NpgsqlDbType.Varchar, 100).Value = Me._UNIDAD
                .Parameters.Add("P_SIMBOLO", NpgsqlDbType.Varchar, 5).Value = Me._SIMBOLO
                .Parameters.Add("P_COMPARTIDO", NpgsqlDbType.Char, 1).Value = Me._COMPARTIDO
                .Parameters.Add("P_FUNCION_AGREGADA", NpgsqlDbType.Varchar, 10).Value = Me._FUNCION_AGREGADA
                If Me._CATEGORYID.HasValue Then
                    .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = Me._CATEGORYID.Value
                Else
                    .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_IMAGEURL", NpgsqlDbType.Varchar, 200).Value = Me._IMAGEURL
                .Parameters.Add("P_PUBLICADO", NpgsqlDbType.Char, 1).Value = Me._PUBLICADO
                .Parameters.Add("P_SISTEMA", NpgsqlDbType.Char, 1).Value = Me._SISTEMA
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
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                .Parameters.Add("P_TITULO", NpgsqlDbType.Varchar, 250).Value = Me._TITULO
                .Parameters.Add("P_RESUMEN", NpgsqlDbType.Varchar, 1000).Value = Me._RESUMEN
                .Parameters.Add("P_DESCRIPCION", NpgsqlDbType.Text).Value = Me._DESCRIPCION
                .Parameters.Add("P_UNIDAD", NpgsqlDbType.Varchar, 100).Value = Me._UNIDAD
                .Parameters.Add("P_SIMBOLO", NpgsqlDbType.Varchar, 5).Value = Me._SIMBOLO
                .Parameters.Add("P_COMPARTIDO", NpgsqlDbType.Char, 1).Value = Me._COMPARTIDO
                .Parameters.Add("P_FUNCION_AGREGADA", NpgsqlDbType.Varchar, 10).Value = Me._FUNCION_AGREGADA
                If Me._CATEGORYID.HasValue Then
                    .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = Me._CATEGORYID.Value
                Else
                    .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_IMAGEURL", NpgsqlDbType.Varchar, 200).Value = Me._IMAGEURL
                .Parameters.Add("P_PUBLICADO", NpgsqlDbType.Char, 1).Value = Me._PUBLICADO
                .Parameters.Add("P_SISTEMA", NpgsqlDbType.Char, 1).Value = Me._SISTEMA
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
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Array).Value = Me._INDICATORID.Value
                .Parameters.Add("P_USERID", NpgsqlDbType.Array).Value = Me._USERID.Value
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
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                .Parameters.Add("P_USERID", NpgsqlDbType.Array).Value = Me._USERID.Value
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

                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Not String.IsNullOrEmpty(Me._TITULO) Then
                    .CommandText = .CommandText & " AND TITULO LIKE :P_TITULO "
                    .Parameters.Add("P_TITULO", NpgsqlDbType.Varchar).Value = "%" & Me._TITULO & "%"
                End If
                If Me._CATEGORYID.HasValue Then
                    .CommandText = .CommandText & " AND CATEGORYID = :P_CATEGORYID "
                    .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = Me._CATEGORYID.Value
                End If
                If Me._SUBCATEGORYID.HasValue Then
                    .CommandText = .CommandText & " AND SUBCATEGORYID = :P_SUBCATEGORYID "
                    .Parameters.Add("P_SUBCATEGORYID", NpgsqlDbType.Bigint).Value = Me._SUBCATEGORYID.Value
                End If
                If Not String.IsNullOrEmpty(Me._USERNAME) Then
                    .CommandText = .CommandText & " AND USERNAME LIKE :P_USERNAME "
                    .Parameters.Add("P_USERNAME", NpgsqlDbType.Varchar, 40).Value = Me._USERNAME
                End If
                If Not String.IsNullOrEmpty(Me._PUBLICADO) Then
                    .CommandText = .CommandText & " AND PUBLICADO = :P_PUBLICADO "
                    .Parameters.Add("P_PUBLICADO", NpgsqlDbType.Char).Value = Me._PUBLICADO
                End If
                If Not String.IsNullOrEmpty(Me._SISTEMA) Then
                    .CommandText = .CommandText & " AND SISTEMA = :P_SISTEMA "
                    .Parameters.Add("P_SISTEMA", NpgsqlDbType.Char).Value = Me._SISTEMA
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

                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Not String.IsNullOrEmpty(Me._TITULO) Then
                    .CommandText = .CommandText & " AND TITULO LIKE :P_TITULO "
                    .Parameters.Add("P_TITULO", NpgsqlDbType.Varchar).Value = "%" & Me._TITULO & "%"
                End If
                If Me._CATEGORYID.HasValue Then
                    .CommandText = .CommandText & " AND CATEGORYID = :P_CATEGORYID "
                    .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = Me._CATEGORYID.Value
                End If
                If Me._SUBCATEGORYID.HasValue Then
                    .CommandText = .CommandText & " AND SUBCATEGORYID = :P_SUBCATEGORYID "
                    .Parameters.Add("P_SUBCATEGORYID", NpgsqlDbType.Bigint).Value = Me._SUBCATEGORYID.Value
                End If
                If Not String.IsNullOrEmpty(Me._USERNAME) Then
                    .CommandText = .CommandText & " AND USERNAME LIKE :P_USERNAME "
                    .Parameters.Add("P_USERNAME", NpgsqlDbType.Varchar, 40).Value = Me._USERNAME
                End If
                If Not String.IsNullOrEmpty(Me._PUBLICADO) Then
                    .CommandText = .CommandText & " AND PUBLICADO = :P_PUBLICADO "
                    .Parameters.Add("P_PUBLICADO", NpgsqlDbType.Char).Value = Me._PUBLICADO
                End If
                If Not String.IsNullOrEmpty(Me._SISTEMA) Then
                    .CommandText = .CommandText & " AND SISTEMA = :P_SISTEMA "
                    .Parameters.Add("P_SISTEMA", NpgsqlDbType.Char).Value = Me._SISTEMA
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

#Region "Otras funciones de búsqueda"
    Public Function BuscarIndicadores(ByVal pageSize As Integer, ByVal currentPage As Integer, ByVal psOrderBy As String) As DataTable
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
            Dim Transaccion As NpgsqlTransaction = dbconexion.BeginTransaction()
            Dim SqlConsulta As New NpgsqlCommand
            Dim daData As New NpgsqlDataAdapter
            Dim _dtData As New DataTable

            With SqlConsulta
                .Connection = dbconexion
                .CommandText = "KPIS.SEARCHINDICATORS"
                .CommandType = CommandType.StoredProcedure

                If Me._USERID.HasValue Then
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                Else
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = -1
                End If
                .Parameters.Add("P_TITULO", NpgsqlDbType.Text).Value = Me._TITULO
                If Me._CATEGORYID.HasValue Then
                    .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = Me._CATEGORYID.Value
                Else
                    .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                If Me._SUBCATEGORYID.HasValue Then
                    .Parameters.Add("P_SUBCATEGORYID", NpgsqlDbType.Bigint).Value = Me._SUBCATEGORYID.Value
                Else
                    .Parameters.Add("P_SUBCATEGORYID", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
                .Parameters.Add("P_ORDERBY", NpgsqlDbType.Text).Value = psOrderBy
            End With

            daData.SelectCommand = SqlConsulta
            daData.Fill(_dtData)

            Transaccion.Commit()

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
    Public Function ObtenerDatosBuscador(ByVal psOrden As String) As DataTable
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
                If Me._USERID.HasValue Then
                    .CommandText = "SELECT A.INDICATORID, A.TITULO, A.RESUMEN, " & _
                               "COALESCE(E.ULTIMA_FECHA, A.FECHA_ALTA) ULTIMA_FECHA, A.FECHA_ALTA, COMPARTIDO, " & _
                               "A.RATING_VOTES, A.RATING_VALUES, CASE WHEN RATING_VOTES > 0 THEN ROUND(RATING_VALUES / RATING_VOTES) ELSE 0 END RATING, " & _
                               "COALESCE(D.NUM_COMENTARIOS,0) NUM_COMENTARIOS, COALESCE(C.NUM_USUARIOS,0) NUM_USUARIOS, " & _
                               "COALESCE(E.NUM_VALORES,0) NUM_VALORES, F.ESTILO, COALESCE(G.ASIGNADO,0) ASIGNADO, A.IMAGEURL, UNIDAD " & _
                               "FROM KPI_INDICATORS A " & _
                               "LEFT JOIN (SELECT AUX.INDICATORID, COUNT(AUX.USERID) NUM_USUARIOS FROM KPI_INDICATOR_USERS AUX GROUP BY AUX.INDICATORID) C ON A.INDICATORID = C.INDICATORID " & _
                               "LEFT JOIN (SELECT AUX2.INDICATORID, COUNT(AUX2.INDICATORID) NUM_COMENTARIOS FROM KPI_COMMENTS AUX2 GROUP BY AUX2.INDICATORID) D ON A.INDICATORID = D.INDICATORID " & _
                               "LEFT JOIN (SELECT AUX3.INDICATORID, COUNT(AUX3.INDICATORID) NUM_VALORES, MAX(AUX3.FECHA) ULTIMA_FECHA FROM KPI_DATASETS AUX3 GROUP BY AUX3.INDICATORID) E ON A.INDICATORID = E.INDICATORID " & _
                               "LEFT JOIN (SELECT AUX4.INDICATORID, COUNT(AUX4.INDICATORID) ASIGNADO FROM KPI_INDICATOR_USERS AUX4 WHERE AUX4.USERID = :P_USERID1 GROUP BY AUX4.INDICATORID) G ON A.INDICATORID = G.INDICATORID " & _
                               "LEFT JOIN KPI_CATEGORIES F ON A.CATEGORYID = F.CATEGORYID " & _
                               "WHERE (A.COMPARTIDO = 'S' OR (A.COMPARTIDO = 'N' AND A.USERID = :P_USERID2)) "
                    .Parameters.Add("P_USERID1", NpgsqlDbType.Bigint).Value = Me._USERID
                    .Parameters.Add("P_USERID2", NpgsqlDbType.Bigint).Value = Me._USERID
                Else
                    .CommandText = "SELECT A.INDICATORID, A.TITULO, A.RESUMEN, " & _
                               "COALESCE(E.ULTIMA_FECHA, A.FECHA_ALTA) ULTIMA_FECHA, A.FECHA_ALTA, COMPARTIDO, " & _
                               "A.RATING_VOTES, A.RATING_VALUES, CASE WHEN RATING_VOTES > 0 THEN ROUND(RATING_VALUES / RATING_VOTES) ELSE 0 END RATING, " & _
                               "COALESCE(D.NUM_COMENTARIOS,0) NUM_COMENTARIOS, COALESCE(C.NUM_USUARIOS,0) NUM_USUARIOS, " & _
                               "NUM_VALORES, F.ESTILO, 0, A.IMAGEURL, UNIDAD " & _
                               "FROM KPI_INDICATORS A " & _
                               "LEFT JOIN (SELECT AUX.INDICATORID, COUNT(AUX.USERID) NUM_USUARIOS FROM KPI_INDICATOR_USERS AUX GROUP BY AUX.INDICATORID) C ON A.INDICATORID = C.INDICATORID " & _
                               "LEFT JOIN (SELECT AUX2.INDICATORID, COUNT(AUX2.INDICATORID) NUM_COMENTARIOS FROM KPI_COMMENTS AUX2 GROUP BY AUX2.INDICATORID) D ON A.INDICATORID = D.INDICATORID " & _
                               "LEFT JOIN (SELECT AUX3.INDICATORID, COUNT(AUX3.INDICATORID) NUM_VALORES, MAX(AUX3.FECHA) ULTIMA_FECHA FROM KPI_DATASETS AUX3 GROUP BY AUX3.INDICATORID) E ON A.INDICATORID = E.INDICATORID " & _
                               "LEFT JOIN KPI_CATEGORIES F ON A.CATEGORYID = F.CATEGORYID " & _
                               "WHERE A.COMPARTIDO = 'S' "
                End If
                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID"
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Not String.IsNullOrEmpty(Me._TITULO) Then
                    .CommandText = .CommandText & " AND LOWER(A.TITULO) LIKE :P_TITULO "
                    .Parameters.Add("P_TITULO", NpgsqlDbType.Varchar, 250).Value = "%" & Me._TITULO.ToLower() & "%"
                End If
                If Me._CATEGORYID.HasValue Then
                    .CommandText = .CommandText & " AND A.CATEGORYID = :P_CATEGORYID "
                    .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = Me._CATEGORYID.Value
                End If
                If Me._SUBCATEGORYID.HasValue Then
                    .CommandText = .CommandText & " AND A.SUBCATEGORYID = :P_SUBCATEGORYID "
                    .Parameters.Add("P_SUBCATEGORYID", NpgsqlDbType.Bigint).Value = Me._SUBCATEGORYID.Value
                End If
                If Not String.IsNullOrEmpty(Me._PUBLICADO) Then
                    .CommandText = .CommandText & " AND A.PUBLICADO = :P_PUBLICADO "
                    .Parameters.Add("P_PUBLICADO", NpgsqlDbType.Char).Value = Me._PUBLICADO
                End If
                If Not String.IsNullOrEmpty(Me._SISTEMA) Then
                    .CommandText = .CommandText & " AND A.SISTEMA = :P_SISTEMA "
                    .Parameters.Add("P_SISTEMA", NpgsqlDbType.Char).Value = Me._SISTEMA
                End If
                .CommandText = .CommandText & psOrden
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
    Public Function ObtenerParecidos(ByVal sBusqueda As String) As DataTable
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
                .CommandType = CommandType.StoredProcedure
                .CommandText = "kpis.getsearch"

                .Parameters.Add("P_BUSQUEDA", NpgsqlDbType.Varchar, 250).Value = sBusqueda
                '.Parameters.Add("result_cursor", NpgsqlDbType.Refcursor).Direction = ParameterDirection.Output
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
    Public Function RecuentoBuscador() As Integer
        Dim dbconexion As New NpgsqlConnection
        Dim nResultados As Integer = 0
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
                .CommandText = "SELECT COUNT(A.INDICATORID) " & _
                               "FROM KPI_INDICATORS A " & _
                               "LEFT JOIN (SELECT AUX.INDICATORID, COUNT(AUX.INDICATORID) NUM_VISITAS, TO_CHAR(MAX(FECHA),'dd/MM/yyyy hh24:mi:ss') ULTIMA_VISITA, MAX(FECHA) ULTIMA_FECHA FROM KPI_VISITAS AUX GROUP BY AUX.INDICATORID) C ON A.INDICATORID = C.INDICATORID " & _
                               "LEFT JOIN (SELECT AUX2.INDICATORID, COUNT(AUX2.INDICATORID) NUM_COMENTARIOS FROM KPI_COMMENTS AUX2 GROUP BY AUX2.INDICATORID) D ON A.INDICATORID = D.INDICATORID " & _
                               "WHERE 1=1 "

                If Me._INDICATORID.HasValue Then
                    .CommandText = .CommandText & " AND A.INDICATORID = :P_INDICATORID "
                    .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                End If
                If Not String.IsNullOrEmpty(Me._TITULO) Then
                    .CommandText = .CommandText & " AND LOWER(A.TITULO) LIKE :P_TITULO "
                    .Parameters.Add("P_TITULO", NpgsqlDbType.Varchar, 250).Value = "%" & Me._TITULO.ToLower() & "%"
                End If
                If Me._CATEGORYID.HasValue Then
                    .CommandText = .CommandText & " AND A.CATEGORYID = :P_CATEGORYID "
                    .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = Me._CATEGORYID.Value
                End If
                If Me._SUBCATEGORYID.HasValue Then
                    .CommandText = .CommandText & " AND SUBCATEGORYID = :P_SUBCATEGORYID "
                    .Parameters.Add("P_SUBCATEGORYID", NpgsqlDbType.Bigint).Value = Me._SUBCATEGORYID.Value
                End If
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND (A.COMPARTIDO = 'S' OR (A.COMPARTIDO = 'N' AND A.USERID = :P_USERID)) "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID
                Else
                    .CommandText = .CommandText & " AND A.COMPARTIDO = 'S' "
                End If
                If Not String.IsNullOrEmpty(Me._PUBLICADO) Then
                    .CommandText = .CommandText & " AND A.PUBLICADO = :P_PUBLICADO "
                    .Parameters.Add("P_PUBLICADO", NpgsqlDbType.Char).Value = Me._PUBLICADO
                End If
                If Not String.IsNullOrEmpty(Me._SISTEMA) Then
                    .CommandText = .CommandText & " AND A.SISTEMA = :P_SISTEMA "
                    .Parameters.Add("P_SISTEMA", NpgsqlDbType.Char).Value = Me._SISTEMA
                End If
            End With

            nResultados = SqlConsulta.ExecuteScalar()

        Catch excp As NpgsqlException
            MsgBox("No se ha podido obtener los resultados de la consulta de la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return Convert.ToInt32(nResultados)

    End Function
#End Region

#Region "Otras Funciones"
    Public Function bCompartir(ByVal psNombre As String) As Boolean
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
                .CommandText = strPublicar
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                .Parameters.Add("P_USERNAME", NpgsqlDbType.Varchar, 40).Value = Me._USERNAME
                .Parameters.Add("P_IMAGEURL", NpgsqlDbType.Varchar, 200).Value = Me._IMAGEURL
                .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = Me._CATEGORYID.Value
                .Parameters.Add("P_SUBCATEGORYID", NpgsqlDbType.Bigint).Value = Me._SUBCATEGORYID.Value
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 250).Value = psNombre
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
    Public Function bModificarRanking(ByVal pnValor As Integer) As Integer
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
                .CommandText = "kpis.UPD_KPI_RATING"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
                .Parameters.Add("P_VALUE", NpgsqlDbType.Bigint).Value = pnValor
            End With

            SqlModifica.ExecuteNonQuery()
            nResultado = SqlModifica.Parameters("RETURN_VALUE").Value
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Modificación sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            nResultado = 0
        Finally
            dbconexion.Close()
        End Try

        Return nResultado

    End Function
    Public Function bReiniciarRanking() As Boolean
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
                .CommandText = "wrdelete.del_wr_rating"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("RETURN_VALUE", NpgsqlDbType.Bigint).Direction = System.Data.ParameterDirection.Output
                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
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
    Public Function NumeroIndicadores() As Integer
        Dim dbconexion As New NpgsqlConnection
        Dim Numero As Integer = 0

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

            With SqlConsulta
                .Connection = dbconexion
                .CommandText = "SELECT COUNT(A.INDICATORID) " & _
                               "FROM KPI_INDICATORS A " & _
                               "WHERE 1=1 "
                .CommandType = CommandType.Text

                If Not String.IsNullOrEmpty(Me._PUBLICADO) Then
                    .CommandText = .CommandText & " AND A.PUBLICADO = :P_PUBLICADO "
                    .Parameters.Add("P_PUBLICADO", NpgsqlDbType.Char).Value = Me._PUBLICADO
                End If
                If Me._USERID.HasValue Then
                    .CommandText = .CommandText & " AND A.USERID = :P_USERID "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = Me._USERID.Value
                End If
            End With

            Numero = SqlConsulta.ExecuteScalar()

        Catch excp As NpgsqlException
            MsgBox("No se ha podido obtener los resultados de la consulta de la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
        Finally
            dbconexion.Close()
        End Try

        Return Numero

    End Function
    Public Function ObtenerResumen() As DataTable
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
                .CommandText = "SELECT 'USERID' CAMPO, COUNT(USERID) RESULTADO FROM KPI_INDICATOR_USERS " & _
                               " WHERE INDICATORID = :P_INDICATORID " & _
                               " UNION " & _
                               "SELECT 'DATASETID' CAMPO, COUNT(DATASETID) RESULTADO FROM KPI_DATASETS " & _
                               " WHERE INDICATORID = :P_INDICATORID " & _
                               " UNION " & _
                               "SELECT 'VALORID' CAMPO, COUNT(VALORID) RESULTADO FROM KPI_DATASET_VALUES " & _
                               " WHERE INDICATORID = :P_INDICATORID "
                .CommandType = CommandType.Text

                .Parameters.Add("P_INDICATORID", NpgsqlDbType.Bigint).Value = Me._INDICATORID.Value
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
