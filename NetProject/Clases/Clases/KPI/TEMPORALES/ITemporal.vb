Public Interface ITemporal

#Region "Variables definidas en la Interfaz"
    Property userid As Nullable(Of Integer)
    Property datasetid As Nullable(Of Integer)
    Property indicatorid As Nullable(Of Integer)
#End Region

#Region "Funciones definidas en la Interfaz"
    Sub Inicializar()
    Function ObtenerValores(ByVal pnDesde As Integer, ByVal pnHasta As Integer) As DataTable
#End Region

End Interface
