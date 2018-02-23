var importObject = (function ($) {

    //Variables privates
    var _filename = '',
        _separator = ',',
        _dateformat = '',
        _nombre,
        _descripcion,
        _modo = 0,
        _numHoja = 0,
        _importid,
        _indicatorid,
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

        // Calcula el caracter separador csv
        readCsvSeparator: function (funcOK, funcFALSE) {
            var dataString = "{filename : '" + _filename + "'}";
            proxy.getSeparatorCsv(dataString, funcOK, funcFALSE);
        },

        // Previsualiza datos csv
        readCsvData: function (funcOK, funcFALSE) {
            var dataString = "{filename : '" + _filename + "', separator : '" + _separator + "'}";
            proxy.getDataCsv(dataString, funcOK, funcFALSE);
        },

        // Comprueba el formato Fecha
        readCsvDate: function (columnDate, funcOK, funcFALSE) {
            var dataString = "{filename : '" + _filename + "', dateColumn :" + columnDate + "}";
            proxy.getDateCsv(dataString, funcOK, funcFALSE);
        },

        // Previsualiza hojas xls
        readSheets: function (funcOK, funcFALSE) {
            var dataString = "{filename : '" + _filename + "'}";
            proxy.getSheetsXls(dataString, funcOK, funcFALSE);
        },

        // Comprueba el formato Fecha
        readXlsDate: function (columnDate, funcOK, funcFALSE) {
            var dataString = "{filename : '" + _filename + "', hoja : " + _numHoja + ", dateColumn :" + columnDate + "}";
            proxy.getDateXls(dataString, funcOK, funcFALSE);
        },

        // Previsualiza datos xls
        readXls: function (funcOK, funcFALSE) {
            var dataString = "{filename : '" + _filename + "', hoja : " + _numHoja + "}";
            proxy.getDataXls(dataString, funcOK, funcFALSE);
        },

        // Previsualiza datos json
        readJson: function (funcOK, funcFALSE) {
            var dataString = "{filename : '" + _filename + "'}";
            proxy.getDataJson(dataString, funcOK, funcFALSE);
        },

        // Previsualiza datos Xxml
        readXml: function (funcOK, funcFALSE) {
            var dataString = "{filename : '" + _filename + "'}";
            proxy.getDataXml(dataString, funcOK, funcFALSE);
        },

        //Importa datos csv
        writeCsv: function (infoColumns, infoAttributes, funcOK, funcFALSE) {
            var dataString = "{filename : '" + _filename + "', nombre : '" + _nombre + "', descripcion : '" + _descripcion + "', info : '" + infoColumns + "', attributes : '" + infoAttributes + "', indicatorid : " + _indicatorid + ", separator : '" + _separator + "', modo : " + _modo + ", datetimeFormat : '" + _dateformat + "'}";
            proxy.writeDataCsv(dataString, funcOK, funcFALSE);
        },

        //Importa datos xls
        writeXls: function (infoColumns, infoAttributes, funcOK, funcFALSE) {
            var dataString = "{filename : '" + _filename + "', nombre : '" + _nombre + "', descripcion : '" + _descripcion + "', info : '" + infoColumns + "', attributes : '" + infoAttributes + "', indicatorid : " + _indicatorid + ", hoja : '" + _numHoja + "', modo : " + _modo + ", datetimeFormat : '" + _dateformat + "'}";
            proxy.writeDataXls(dataString, funcOK, funcFALSE);
        },

        //Importa datos json
        writeJson: function (infoColumns, funcOK, funcFALSE) {
            var dataString = "{filename : '" + _filename + "', nombre : '" + _nombre + "', descripcion : '" + _descripcion + "', info : '" + infoColumns + "', attributes : '" + infoAttributes + "', indicatorid : " + _indicatorid + ", modo : " + _modo + ", datetimeFormat : '" + _dateformat + "'}";
            proxy.writeDataJson(dataString, funcOK, funcFALSE);
        },

        //Importa datos xml
        writeXml: function (infoColumns, funcOK, funcFALSE) {
            var dataString = "{filename : '" + _filename + "', nombre : '" + _nombre + "', descripcion : '" + _descripcion + "', info : '" + infoColumns + "', attributes : '" + infoAttributes + "', indicatorid : " + _indicatorid + ", modo : " + _modo + ", datetimeFormat : '" + _dateformat + "'}";
            proxy.writeDataXml(dataString, funcOK, funcFALSE);
        },

        //Importa datos manual
        writeTable: function (infoColumns, dataValues, dimension, funcOK, funcFALSE) {
            var dataString = "{nombre : '" + _nombre + "', descripcion : '" + _descripcion + "', columns : '" + infoColumns + "', rows : '" + dataValues + "', indicatorid : " + _indicatorid + ", modo : " + _modo + ", dimension : '" + dimension + "'}";
            proxy.writeDataTable(dataString, funcOK, funcFALSE);
        },

        // Populate
        populate: function (funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + ", nombre : '" + _nombre + "', pageSize : " + _pageSize + ", currentPage : " + _currentPage + ", orderby : '" + _orderby + "'}";
            proxy.getImports(dataString, funcOK, funcFALSE);
        },

        // Cuenta
        count: function () {
            var dataString = "{indicatorid : " + _indicatorid + ", nombre : '" + _nombre + "'}";
            proxy.countImport(dataString, _setTotalPages, null);
        },

        // Elimina
        remove: function (funcOK, funcFALSE) {
            var dataString = "{importid : " + _importid + "}";
            proxy.delImport(dataString, funcOK, funcFALSE);
        },

        setImportId: function (data) {
            _importid = data;
        },

        setFileName: function (data) {
            _filename = data;
        },

        setIdindicator: function (data) {
            _indicatorid = data;
        },

        setNombre: function (data) {
            _nombre = data;
        },

        setDescripcion: function (data) {
            _descripcion = data;
        },

        setModo: function (data) {
            _modo = data;
        },

        setHoja: function (data) {
            _numHoja = data;
        },

        setSeparator: function (data) {
            _separator = data;
        },

        setDateFormat: function (data) {
            _dateformat = data;
        },

        setCurrentPage: function (data) {
            _currentPage = data;
        },

        setOrderBy: function (data) {
            _orderby = data;
        },

        getFileName: function () {
            return _filename;
        },

        getSeparator: function () {
            return _separator;
        },

        getIdindicator: function () {
            return _indicatorid;
        },

        getTotalPages: function () {
            return _totalPages;
        },

        getTotalItems: function () {
            return _totalItems;
        }
    };
} (jQuery));
