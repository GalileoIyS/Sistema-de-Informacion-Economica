var datasetObject = (function ($) {

    //Variables privates
    var _datasetid,
        _indicatorid,
        _nombre = '',
        _dimension = '',
        _pageSize = 10,
        _currentPage = 1,
        _totalPages = 0,
        _totalItems = 0,
        _orderby = '';

    function _setTotalPages(response) {
        _totalItems = response;
        _totalPages = Math.floor((response + _pageSize - 1) / _pageSize);
    }

    // Return an object exposed to the public
    return {

        // Inserta
        insert: function (funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + ", nombre : '" + _nombre + "', dimension : '" + _dimension + "'}";
            proxy.addDataset(dataString, funcOK, funcFALSE);
        },

        // Modifica
        update: function (funcOK, funcFALSE) {
            var dataString = "{datasetid : " + _datasetid + ", nombre : '" + _nombre + "'}";
            proxy.updDataset(dataString, funcOK, funcFALSE);
        },

        // Cuenta
        count: function () {
            var dataString = "{indicatorid : " + _indicatorid + ", nombre : '" + _nombre + "'}";
            proxy.countDataset(dataString, _setTotalPages, null);
        },

        // Populate
        populate: function (funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + ", nombre : '" + _nombre + "', pageSize : " + _pageSize + ", currentPage : " + _currentPage + ", orderby : '" + _orderby + "'}";
            proxy.getDatasets(dataString, funcOK, funcFALSE);
        },

        // Carga
        load: function (ini, fin, funcOK, funcFALSE) {
            var dataString = "{datasetid : " + _datasetid + ", pini : '" + ini + "', pfin : '" + fin + "'}";
            proxy.getDatasetValues(dataString, funcOK, funcFALSE);
        },

        // Guarda
        save: function (data, funcOK, funcFALSE) {
            var dataString = "{datasetid : " + _datasetid + ", data : '" + data + "'}";
            proxy.saveDatasetValues(dataString, funcOK, funcFALSE);
        },

        // Elimina
        remove: function (funcOK, funcFALSE) {
            var dataString = "{datasetid : " + _datasetid + "}";
            proxy.delDataset(dataString, funcOK, funcFALSE);
        },

        // Exporta
        exportData: function (funcOK, funcFALSE) {
            var dataString = "{datasetid : " + _datasetid + "}";
            proxy.expDataset(dataString, funcOK, funcFALSE);
        },

        // Obtiene la gráfica de barras del número de datos por ejercicio del indicador
        getLastChart: function (funcOK, funcFALSE) {
            var dataString = "{datasetid : " + _datasetid + "}";
            proxy.getDatasetLastChart(dataString, funcOK, funcFALSE);
        },

        setDatasetId: function (data) {
            _datasetid = data;
        },

        setIndicatorId: function (data) {
            _indicatorid = data;
        },

        setNombre: function (data) {
            _nombre = data;
        },

        setDimension: function (data) {
            _dimension = data;
        },

        setCurrentPage: function (data) {
            _currentPage = data;
        },

        setOrderBy: function (data) {
            _orderby = data;
        },

        getDatasetId: function () {
            return _datasetid;
        },

        getIndicatorId: function () {
            return _indicatorid;
        },

        getNombre: function () {
            return _nombre;
        },

        getDimension: function () {
            return _dimension;
        },

        getTotalPages: function () {
            return _totalPages;
        },

        getTotalItems: function () {
            return _totalItems;
        }

    };
} (jQuery));
