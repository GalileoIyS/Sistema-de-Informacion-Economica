Public Class cKPI_CATEGORIES
    Inherits cBase
    Implements IDisposable

#Region "Constantes Privadas"
    Private Const strConsulta = "SELECT A.CATEGORYID, A.NOMBRE, A.DESCRIPCION, A.ORDEN, A.ESTILO, COALESCE(B.RECUENTO,0) RECUENTO " & _
                                "FROM KPI_CATEGORIES A LEFT JOIN (SELECT CATEGORYID, COUNT(1) RECUENTO FROM KPI_INDICATORS GROUP BY CATEGORYID) B ON A.CATEGORYID=B.CATEGORYID " & _
                                "WHERE 1=1 "

    Private Const strInserta = "kpis.ADD_KPI_CATEGORIES"
    Private Const strModifica = "kpis.UPD_KPI_CATEGORIES"
    Private Const strElimina = "kpis.DEL_KPI_CATEGORIES"
#End Region

#Region "Variables Privadas"
    Private _CATEGORYID As Nullable(Of Integer)
    Private _NOMBRE As String
    Private _DESCRIPCION As String
    Private _ORDEN As Nullable(Of Integer)
    Private _ESTILO As String
#End Region

#Region "Propiedades Publicas"
    Public ReadOnly Property Tabla() As String
        Get
            Return "KPI_CATEGORIES"
        End Get
    End Property
    Public Property categoryid As Nullable(Of Integer)
        Get
            Return _CATEGORYID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _CATEGORYID = value
        End Set
    End Property
    Public Property nombre As String
        Get
            Return _NOMBRE
        End Get
        Set(ByVal value As String)
            If (value.Length > 250) Then
                _NOMBRE = value.Substring(0, 250)
            Else
                _NOMBRE = value
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
    Public Property orden As Nullable(Of Integer)
        Get
            Return _ORDEN
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _ORDEN = value
        End Set
    End Property
    Public Property estilo As String
        Get
            Return _ESTILO
        End Get
        Set(ByVal value As String)
            If (value.Length > 40) Then
                _ESTILO = value.Substring(0, 40)
            Else
                _ESTILO = value
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
        Me._CATEGORYID = Nothing
        Me._NOMBRE = String.Empty
        Me._DESCRIPCION = String.Empty
        Me._ORDEN = Nothing
        Me._ESTILO = String.Empty
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

                If Me._CATEGORYID.HasValue Then
                    .CommandText = .CommandText & " AND A.CATEGORYID = :P_CATEGORYID "
                    .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = Me._CATEGORYID.Value
                End If
                If Not String.IsNullOrEmpty(Me._NOMBRE) Then
                    .CommandText = .CommandText & " AND A.NOMBRE = :P_NOMBRE "
                    .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 250).Value = Me._NOMBRE
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
                If Not IsDBNull(.GetValue(.GetOrdinal("CATEGORYID"))) Then
                    Me._CATEGORYID = Convert.ToInt64(.GetValue(.GetOrdinal("CATEGORYID")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("NOMBRE"))) Then
                    Me._NOMBRE = .GetValue(.GetOrdinal("NOMBRE"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("DESCRIPCION"))) Then
                    Me._DESCRIPCION = .GetValue(.GetOrdinal("DESCRIPCION"))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("ORDEN"))) Then
                    Me._ORDEN = Convert.ToInt64(.GetValue(.GetOrdinal("ORDEN")))
                End If
                If Not IsDBNull(.GetValue(.GetOrdinal("ESTILO"))) Then
                    Me._ESTILO = .GetValue(.GetOrdinal("ESTILO"))
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
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 250).Value = Me._NOMBRE
                .Parameters.Add("P_DESCRIPCION", NpgsqlDbType.Varchar, 1000).Value = Me._DESCRIPCION
                If Me._ORDEN.HasValue Then
                    .Parameters.Add("P_ORDEN", NpgsqlDbType.Bigint).Value = Me._ORDEN.Value
                Else
                    .Parameters.Add("P_ORDEN", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
            End With

            SqlInserta.ExecuteNonQuery()
            nResultado = SqlInserta.Parameters("RETURN_VALUE").Value
            Me._CATEGORYID = nResultado
        Catch excp As NpgsqlException
            MsgBox("No se ha podido ejecutar la Inserción sobre la Base de Datos." & Chr(13) & _
                    "Motivo: " & excp.Message)
            Me._CATEGORYID = -1
        Finally
            dbconexion.Close()
        End Try

        If Me._CATEGORYID < 0 Then
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
                .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = Me._CATEGORYID.Value
                .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 250).Value = Me._NOMBRE
                .Parameters.Add("P_DESCRIPCION", NpgsqlDbType.Varchar, 1000).Value = Me._DESCRIPCION
                .Parameters.Add("P_ESTILO", NpgsqlDbType.Varchar, 40).Value = Me._ESTILO
                If Me._ORDEN.HasValue Then
                    .Parameters.Add("P_ORDEN", NpgsqlDbType.Bigint).Value = Me._ORDEN.Value
                Else
                    .Parameters.Add("P_ORDEN", NpgsqlDbType.Bigint).Value = DBNull.Value
                End If
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
                .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = Me._CATEGORYID.Value
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

                If Me._CATEGORYID.HasValue Then
                    .CommandText = .CommandText & " AND A.CATEGORYID = :P_CATEGORYID"
                    .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = Me._CATEGORYID.Value
                End If
                If Not String.IsNullOrEmpty(Me._NOMBRE) Then
                    .CommandText = .CommandText & " AND LOWER(A.NOMBRE) LIKE :P_NOMBRE "
                    .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 250).Value = "%" & Me._NOMBRE.ToLower() & "%"
                End If
                .CommandText = .CommandText & " ORDER BY A.ORDEN ASC "
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

                If Me._CATEGORYID.HasValue Then
                    .CommandText = .CommandText & " AND A.CATEGORYID = :P_CATEGORYID"
                    .Parameters.Add("P_CATEGORYID", NpgsqlDbType.Bigint).Value = Me._CATEGORYID.Value
                End If
                If Not String.IsNullOrEmpty(Me._NOMBRE) Then
                    .CommandText = .CommandText & " AND LOWER(A.NOMBRE) LIKE :P_NOMBRE "
                    .Parameters.Add("P_NOMBRE", NpgsqlDbType.Varchar, 250).Value = "%" & Me._NOMBRE.ToLower() & "%"
                End If
                .CommandText = .CommandText & " ORDER BY A.ORDEN ASC "
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

#Region "Otras funciones"
    Public Function ObtenerTodos(ByVal pnUserId As Nullable(Of Integer)) As DataTable
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
                If pnUserId.HasValue Then
                    .CommandText = "SELECT * FROM ( " & _
                                        "SELECT NULL SUBCATEGORYID, A.CATEGORYID, A.NOMBRE, A.DESCRIPCION, A.ORDEN, A.ESTILO, 'licategory' CLASE, COALESCE(B.RECUENTO,0) RECUENTO " & _
                                        "FROM KPI_CATEGORIES A LEFT JOIN (SELECT CATEGORYID, COUNT(1) RECUENTO FROM KPI_INDICATORS GROUP BY CATEGORYID) B ON A.CATEGORYID = B.CATEGORYID " & _
                                        "WHERE A.CATEGORYID > 1 " & _
                                        "UNION " & _
                                        "SELECT A.SUBCATEGORYID, A.CATEGORYID, A.NOMBRE, NULL DESCRIPCION, NULL ORDEN, C.ESTILO ESTILO, 'lisubcategory' CLASE, COALESCE(B.RECUENTO,0) RECUENTO " & _
                                        "FROM KPI_SUBCATEGORIES A LEFT JOIN (SELECT SUBCATEGORYID, COUNT(1) RECUENTO FROM KPI_INDICATORS GROUP BY SUBCATEGORYID) B ON A.SUBCATEGORYID = B.SUBCATEGORYID " & _
                                        "LEFT JOIN KPI_CATEGORIES C ON C.CATEGORYID = A.CATEGORYID " & _
                                        "UNION " & _
                                        "SELECT NULL SUBCATEGORYID, A.CATEGORYID, A.NOMBRE, A.DESCRIPCION, A.ORDEN, A.ESTILO, 'licategory' CLASE, COALESCE(B.RECUENTO,0) RECUENTO " & _
                                        "FROM KPI_CATEGORIES A LEFT JOIN (SELECT CATEGORYID, COUNT(1) RECUENTO FROM KPI_INDICATORS WHERE USERID = :P_USERID AND CATEGORYID = 1 GROUP BY CATEGORYID) B ON A.CATEGORYID = B.CATEGORYID " & _
                                        "WHERE A.CATEGORYID = 1 " & _
                                    ") X   "
                    .Parameters.Add("P_USERID", NpgsqlDbType.Bigint).Value = pnUserId.Value
                Else
                    .CommandText = "SELECT * FROM ( " & _
                                        "SELECT NULL SUBCATEGORYID, A.CATEGORYID, A.NOMBRE, A.DESCRIPCION, A.ORDEN, A.ESTILO, 'licategory' CLASE, COALESCE(B.RECUENTO,0) RECUENTO " & _
                                        "FROM KPI_CATEGORIES A LEFT JOIN (SELECT CATEGORYID, COUNT(1) RECUENTO FROM KPI_INDICATORS GROUP BY CATEGORYID) B ON A.CATEGORYID = B.CATEGORYID " & _
                                        "UNION " & _
                                        "SELECT A.SUBCATEGORYID, A.CATEGORYID, A.NOMBRE, NULL DESCRIPCION, NULL ORDEN, C.ESTILO ESTILO, 'lisubcategory' CLASE, COALESCE(B.RECUENTO,0) RECUENTO " & _
                                        "FROM KPI_SUBCATEGORIES A LEFT JOIN (SELECT SUBCATEGORYID, COUNT(1) RECUENTO FROM KPI_INDICATORS GROUP BY SUBCATEGORYID) B ON A.SUBCATEGORYID = B.SUBCATEGORYID " & _
                                        "LEFT JOIN KPI_CATEGORIES C ON C.CATEGORYID = A.CATEGORYID " & _
                                    ") X WHERE CATEGORYID > 1 "
                End If
                .CommandType = CommandType.Text
                .CommandText = .CommandText & " ORDER BY X.CATEGORYID, X.ORDEN, X.NOMBRE "
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
