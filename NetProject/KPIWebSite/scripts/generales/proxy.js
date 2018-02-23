//Constructors With Prototypes

ServiceProxy = function ()
{
    this._async = false;
    this._baseURL = "/WebServices.aspx/";
};

ServiceProxy.prototype =
{
    _defaultErrorHandler: function (xhr, status, error) {
        avisoObject.setPagina("undefined");
        avisoObject.setMensaje(xhr.statusText + ' -- ' + error);
        avisoObject.setError(3);
        avisoObject.insert();
        return false;
    },

    _getAjax: function (method, data, fnSuccess, fnError) {
        if (!data) data = {};

        if (!fnError) fnError = this._defaultErrorHandler;

        var options = {
            type: 'POST',
            url: this._baseURL + method,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: this._async,
            data: data,
            success: fnSuccess,
            error: fnError,
            dataFilter: function (data) {
                var response;

                if (typeof (JSON) !== "undefined" && typeof (JSON.parse) === "function")
                    response = JSON.parse(data);
                else
                    response = val("(" + data + ")");

                if (response.hasOwnProperty("d"))
                    return response.d;
                else
                    return response;
            }
        };
        $.ajax(options);
    },

    _setAjax: function (method, data, fnSuccess, fnError) {
        if (!data) data = {};

        if (!fnError) fnError = this._defaultErrorHandler;

        var options = {
            type: 'POST',
            url: this._baseURL + method,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: this._async,
            data: data,
            success: fnSuccess,
            error: fnError,
            dataFilter: function (data) {
                var response;

                if (typeof (JSON) !== "undefined" && typeof (JSON.parse) === "function") {
                    response = JSON.parse(data);
                }
                else
                    response = val("(" + data + ")");

                if (response.hasOwnProperty("d"))
                    return response.d;
                else
                    return response;
            }
        };
        $.ajax(options);
    },

    _delAjax: function (method, data, fnSuccess, fnError) {
        if (!data) data = {};

        if (!fnError) fnError = this._defaultErrorHandler;

        var options = {
            type: 'POST',
            url: this._baseURL + method,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            async: this._async,
            data: data,
            success: fnSuccess,
            error: fnError
        };
        $.ajax(options);
    },

    setAsync: function (value) {
        this._async = value;
    },

    //Obtenemos los datos porcentuales para las gráficas de Quesito y Histograma
    getPercentData: function (dataString, success, error) {
        this._getAjax("PercentChart", dataString, success, error);
    },

    //Obtenemos los datos absolutos para el resto de gráficas
    getAbsoluteData: function (dataString, success, error) {
        this._getAjax("TimeChart", dataString, success, error);
    },

    //Obtenemos las diferentes formulas definidas en un widget
    getFormulas: function (dataString, success, error) {
        this._getAjax("ObtenerFormulasDelWidget", dataString, success, error);
    },

    //Obtenemos las diferentes dimensiones de un indicador
    getDimensions: function (dataString, success, error) {
        this._getAjax("ObtenerDimensionesDeIndicador", dataString, success, error);
    },

    //Obtenemos las diferentes dimensiones de un indicador
    getImportDimensions: function (dataString, success, error) {
        this._getAjax("ObtenerDimensionesDeIndicadorParaImportar", dataString, success, error);
    },

    //Obtenemos los diferentes valores de una dimension
    getDimensionValues: function (dataString, success, error) {
        this._getAjax("ObtenerDimensionValores", dataString, success, error);
    },

    //Obtenemos los diferentes valores de una dimension que han sido asinados al filtro
    getDimensionValuesAsigned: function (dataString, success, error) {
        this._getAjax("ObtenerDimensionValoresAsignados", dataString, success, error);
    },

    //Obtenemos los diferentes valores de una dimension que aun no han sido asinados al filtro
    getDimensionValuesNotAsigned: function (dataString, success, error) {
        this._getAjax("ObtenerDimensionValoresNoAsignados", dataString, success, error);
    },

    //Obtenemos una lista de indicadores
    getIndicators: function (dataString, success, error) {
        this._getAjax("ObtenerIndicadores", dataString, success, error);
    },

    //Obtenemos una lista de datasets
    getDatasets: function (dataString, success, error) {
        this._getAjax("ObtenerDatasets", dataString, success, error);
    },

    //Obtenemos una lista de usuarios
    getUsers: function (dataString, success, error) {
        this._getAjax("ObtenerUsuarios", dataString, success, error);
    },

    //Obtenemos una lista de atributos
    getAttributes: function (dataString, success, error) {
        this._getAjax("ObtenerAtributos", dataString, success, error);
    },

    //Obtenemos los datos asociados a un dataset
    getDatasetValues: function (dataString, success, error) {
        this._getAjax("ObtenerDatasetValues", dataString, success, error);
    },

    //Obtenemos el separador de un archivo csv
    getSeparatorCsv: function (dataString, success, error) {
        this._delAjax("GetCSVSeparator", dataString, success, error);
    },

    //Obtenemos los datos de un archivo csv
    getDataCsv: function (dataString, success, error) {
        this._getAjax("LeerDesdeCSV", dataString, success, error);
    },

    //Obtenemos el formato fecha de un archivo csv
    getDateCsv: function (dataString, success, error) {
        this._getAjax("FechaDesdeCSV", dataString, success, error);
    },

    //Obtenemos el formato fecha de un archivo xls
    getDateXls: function (dataString, success, error) {
        this._getAjax("FechaDesdeXLS", dataString, success, error);
    },

    //Obtenemos los datos de un archivo xls
    getDataXls: function (dataString, success, error) {
        this._getAjax("LeerDatosDesdeXLS", dataString, success, error);
    },

    //Obtenemos los datos de un archivo json
    getDataJson: function (dataString, success, error) {
        this._getAjax("LeerDesdeJSON", dataString, success, error);
    },

    //Obtenemos los datos de un archivo xml
    getDataXml: function (dataString, success, error) {
        this._getAjax("LeerDesdeXML", dataString, success, error);
    },

    //Obtenemos las hojas de un archivo xls
    getSheetsXls: function (dataString, success, error) {
        this._getAjax("LeerHojasDesdeXLS", dataString, success, error);
    },

    //Obtenemos los amigos que comparten un determinado indicador
    getFriendsList: function (dataString, success, error) {
        this._getAjax("ObtenerListadoAmigos", dataString, success, error);
    },

    //Obtenemos las formulas empleadas con un determinado indicador
    getFormulasIndicator: function (dataString, success, error) {
        this._getAjax("ObtenerFormulasXIndicador", dataString, success, error);
    },

    //Obtenemos las diferentes subcategorías encontradas dentro de una categoría determinada
    getSubcategories: function (dataString, success, error) {
        this._getAjax("ObtenerSubcategorias", dataString, success, error);
    },

    //Importamos los datos de un archivo csv
    writeDataCsv: function (dataString, success, error) {
        this._getAjax("ImportarDesdeCSV", dataString, success, error);
    },

    //Importamos los datos de un archivo xls
    writeDataXls: function (dataString, success, error) {
        this._getAjax("ImportarDesdeXLS", dataString, success, error);
    },

    //Importamos los datos de un archivo json
    writeDataJson: function (dataString, success, error) {
        this._getAjax("ImportarDesdeJSON", dataString, success, error);
    },

    //Importamos los datos de un archivo json
    writeDataXml: function (dataString, success, error) {
        this._getAjax("ImportarDesdeXML", dataString, success, error);
    },

    //Importamos los datos de una carga manual
    writeDataTable: function (dataString, success, error) {
        this._getAjax("ImportarDesdeTabla", dataString, success, error);
    },

    //Obtenemos los diferentes cuadros de mando definidos por un usuario
    getDashboards: function (success, error) {
        this._getAjax("ObtenerDashboards", null, success, error);
    },

    //Obtenemos los diferentes widgets de un determinado cuadro de mando
    getWidgets: function (dataString, success, error) {
        this._getAjax("ObtenerWidgets", dataString, success, error);
    },

    //Obtenemos una lista de importaciones
    getImports: function (dataString, success, error) {
        this._getAjax("ObtenerImports", dataString, success, error);
    },

    //Obtenemos los diferentes cuadros de mando definidos por un usuario
    getFunctions: function (success, error) {
        this._getAjax("ObtenerFunciones", null, success, error);
    },

    //Obtenemos los diferentes tipos de filtro
    getTypeFilters: function (dataString, success, error) {
        this._getAjax("ObtenerTiposFiltros", dataString, success, error);
    },

    //Obtenemos los diferentes filtros de una expresión
    getFilters: function (dataString, success, error) {
        this._getAjax("ObtenerFiltrosDeExpresion", dataString, success, error);
    },

    //Obtenemos las diferentes expresiones de una fórmula
    getExpresions: function (dataString, success, error) {
        this._getAjax("ObtenerExpresionesDeFormula", dataString, success, error);
    },

    //Obtenemos las diferentes solicitudes de amistad
    getFriends: function (dataString, success, error) {
        this._getAjax("ObtenerAmistades", dataString, success, error);
    },

    //Obtenemos los datos de una formula
    getAnyIndicators: function (dataString, success, error) {
        this._getAjax("ObtenerCualquierIndicador", dataString, success, error);
    },

    //Obtenemos los datos de un atributo de un indicador
    selDimension: function (dataString, success, error) {
        this._getAjax("SelectAtributte", dataString, success, error);
    },

    //Obtenemos los datos de un widget
    selWidget: function (dataString, success, error) {
        this._getAjax("SelectWidget", dataString, success, error);
    },

    //Obtenemos los datos de un usuario
    selUser: function (dataString, success, error) {
        this._getAjax("SelectUser", dataString, success, error);
    },

    //Obtenemos los datos de una formula
    getFormula: function (dataString, success, error) {
        this._getAjax("SelectFormula", dataString, success, error);
    },

    //Obtenemos los datos de un indicador
    getIndicator: function (dataString, success, error) {
        this._getAjax("SelectIndicator", dataString, success, error);
    },

    //Obtenemos los datos de un filtro
    getFilter: function (dataString, success, error) {
        this._getAjax("SelectFiltro", dataString, success, error);
    },

    //Obtenemos el resumen de los datos utilizados
    getWidgetResume: function (dataString, success, error) {
        this._getAjax("ObtenerResumenDeWidget", dataString, success, error);
    },

    //Obtenemos una nueva contraseña
    getNewPassword: function (dataString, success, error) {
        this._getAjax("RecuperarPassword", dataString, success, error);
    },

    //Puntuamos un indicador
    voteIndicator: function (dataString, success, error) {
        this._delAjax("VoteIndicator", dataString, success, error);
    },

    //Obtenemos el resumen de los datos disponibles de un indicador
    getIndicatorResume: function (dataString, success, error) {
        this._getAjax("ObtenerResumenDeIndicador", dataString, success, error);
    },

    //Obtenemos la gráfica lineal de los datos disponibles de un indicador
    getIndicatorLineChart: function (dataString, success, error) {
        this._getAjax("ObtenerIndicatorLineChart", dataString, success, error);
    },

    //Obtenemos la gráfica de barras de los datos disponibles de un indicador
    getIndicatorBarChart: function (dataString, success, error) {
        this._getAjax("ObtenerIndicatorBarChart", dataString, success, error);
    },

    getDatasetLastChart: function (dataString, success, error) {
        this._getAjax("ObtenerDatasetLastChart", dataString, success, error);
    },

    //Exportamos los datos del dataset especificado
    expDataset: function (dataString, success, error) {
        this._getAjax("ExportDataset", dataString, success, error);
    },

    //Guardamos los datos asociados a un dataset
    saveDatasetValues: function (dataString, success, error) {
        this._setAjax("GuardarDatasetValue", dataString, success, error);
    },

    //Asigna un indicador al usuario actual
    addIndicatorUser: function (dataString, success, error) {
        this._setAjax("AsignIndicator", dataString, success, error);
    },

    //Cambia la visibilidad del usuario respecto a este indicador
    changeIndicatorUser: function (dataString, success, error) {
        this._setAjax("ChangeIndicatorUser", dataString, success, error);
    },

    changeUserPassword: function (dataString, success, error) {
        this._setAjax("ChangePasswordUser", dataString, success, error);
    },

    //Insertamos un nuevo filtro asociado a una expresión
    addFilters: function (dataString, success, error) {
        this._setAjax("InsertFilter", dataString, success, error);
    },

    //Insertamos una nueva fórmula a un indicador
    addFormula: function (dataString, success, error) {
        this._setAjax("InsertFormula", dataString, success, error);
    },

    //Insertamos un nuevo filtro asociado a una expresión
    addDashboard: function (dataString, success, error) {
        this._setAjax("InsertDashboard", dataString, success, error);
    },

    //Insertamos un nuevo widget asociado a un dashboard
    addWidget: function (dataString, success, error) {
        this._setAjax("InsertWidget", dataString, success, error);
    },

    //Insertamos un nuevo indicador
    addIndicator: function (dataString, success, error) {
        this._setAjax("InsertIndicator", dataString, success, error);
    },

    //Insertamos un nuevo dataset vacío y lo asociamos al indicador
    addDataset: function (dataString, success, error) {
        this._setAjax("InsertDataset", dataString, success, error);
    },

    //Insertamos una nueva caracteristica y la asociamos al indicador
    addAttribute: function (dataString, success, error) {
        this._setAjax("InsertAtributo", dataString, success, error);
    },

    //Insertamos un nuevo valor para la dimensión X de un dataset Y
    addDimensionValue: function (dataString, success, error) {
        this._setAjax("InsertDimensionValue", dataString, success, error);
    },

    //Añade una nueva etiqueta a un indicador
    addTag: function (dataString, success, error) {
        this._setAjax("InsertEtiqueta", dataString, success, error);
    },

    //Insertamos un aviso de incidencia/error en la bb.dd.
    addAviso: function (dataString) {
        this._setAjax("InsertIncidencia", dataString, null, null);
    },

    //Insertamos la solicitud de followers 
    addFriend: function (dataString, success, error) {
        this._setAjax("InsertFollower", dataString, success, error);
    },

    //Insertamos un nuevo comentario asociado a un indicador
    addComment: function (dataString, success, error) {
        this._setAjax("InsertComment", dataString, success, error);
    },

    //Aceptamos la peticion de followers de algún otro usuario
    acceptFriend: function (dataString, success, error) {
        this._setAjax("AcceptFollower", dataString, success, error);
    },

    //Registra al usuario mediante Facebook
    loginFacebook: function (dataString, success, error) {
        this._delAjax("LoginFacebookUser", dataString, success, error);
    },

    //Insertamos un nuevo filtro asociado a una expresión
    copyFormula: function (dataString, success, error) {
        this._setAjax("CopyFormula", dataString, success, error);
    },

    shareIndicator: function (dataString, success, error) {
        this._setAjax("ShareIndicator", dataString, success, error);
    },

    //Actualizamos la nueva imagen asociada al indicador
    changeIndicatorImage: function (dataString, success, error) {
        this._delAjax("ChangeIndicatorImage", dataString, success, error);
    },

    //Actualizamos la nueva imagen asociada al usuario
    changeProfileImage: function (dataString, success, error) {
        this._delAjax("ChangeProfileImage", dataString, success, error);
    },

    //Actualizamos el titlo y estilo de la gráfica
    updWidget: function (dataString, success, error) {
        this._setAjax("UpdateWidget", dataString, success, error);
    },

    //Actualizamos el filtro asociado a una expresión
    updFilters: function (dataString, success, error) {
        this._setAjax("UpdateFilter", dataString, success, error);
    },

    //Actualizamos la fórmula de un widget
    updFormula: function (dataString, success, error) {
        this._setAjax("UpdateFormula", dataString, success, error);
    },

    //Actualizamos el dashboard
    updDashboard: function (dataString, success, error) {
        this._setAjax("UpdateDashboard", dataString, success, error);
    },

    //Actualizamos el nombre del dataset
    updDataset: function (dataString, success, error) {
        this._setAjax("UpdateDataset", dataString, success, error);
    },

    //Eliminamos un indicador del usuario
    delIndicator: function (dataString, success, error) {
        this._delAjax("DeleteIndicator", dataString, success, error);
    },

    //Elimina la etiqueta de un indicador
    delTag: function (dataString, success, error) {
        this._delAjax("DeleteEtiqueta", dataString, success, error);
    },

    //Eliminamos la revision del indicador
    delRevision: function (dataString, success, error) {
        this._delAjax("DeleteRevision", dataString, success, error);
    },

    //Eliminamos el dataset del indicador
    delDataset: function (dataString, success, error) {
        this._delAjax("DeleteDataset", dataString, success, error);
    },

    //Eliminamos la dimension del indicador (si se puede)
    delDimension: function (dataString, success, error) {
        this._delAjax("DeleteDimension", dataString, success, error);
    },

    //Eliminamos un valor de la dimension X del dataset Y
    delDimensionValue: function (dataString, success, error) {
        this._delAjax("DeleteDimensionValue", dataString, success, error);
    },

    //Eliminamos la fórmula del widget
    delFormula: function (dataString, success, error) {
        this._delAjax("DeleteFormula", dataString, success, error);
    },

    //Eliminamos el filtro asociado a una expresión
    delFilters: function (dataString, success, error) {
        this._setAjax("DeleteFiltro", dataString, success, error);
    },

    //Eliminamos un dashboard y todo su contenido asociado
    delDashboard: function (dataString, success, error) {
        this._delAjax("DeleteDashboard", dataString, success, error);
    },

    //Eliminamos el grafico y todo su contenido asociado
    delWidget: function (dataString, success, error) {
        this._delAjax("DeleteWidget", dataString, success, error);
    },

    //Eliminamos la importación y sus datos asociados
    delImport: function (dataString, success, error) {
        this._delAjax("DeleteImport", dataString, success, error);
    },

    //Eliminamos el seguimiento de un usuario a otro
    delFriend: function (dataString, success, error) {
        this._delAjax("DeleteFollower", dataString, success, error);
    },

    //Eliminamos un comentario
    delComment: function (dataString, success, error) {
        this._delAjax("DeleteComment", dataString, success, error);
    },

    //Contamos el numero de Indicadores
    countIndicators: function (dataString, success, error) {
        this._getAjax("CountIndicators", dataString, success, error);
    },

    //Contamos el numero de Atributos de un indicador
    countAttributes: function (dataString, success, error) {
        this._getAjax("CountAttribute", dataString, success, error);
    },

    //Contamos el numero de Datasets de un indicador
    countDataset: function (dataString, success, error) {
        this._getAjax("CountDataset", dataString, success, error);
    },

    //Contamos el numero de Importaciones de un indicador
    countImport: function (dataString, success, error) {
        this._getAjax("CountImport", dataString, success, error);
    },

    //Contamos el numero de Usuarios
    countUsers: function (dataString, success, error) {
        this._getAjax("CountUsers", dataString, success, error);
    },

    //Contamos el numero de Solicitudes de amistad
    countFriend: function (dataString, success, error) {
        this._getAjax("CountFriend", dataString, success, error);
    },

    //Contamos el numero de Revisiones pendientes de aprobar
    countRevisions: function (dataString, success, error) {
    this._getAjax("CountRevision", dataString, success, error);
    }

};

var proxy = new ServiceProxy();