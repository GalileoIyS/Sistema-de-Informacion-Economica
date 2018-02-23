var revisionObject = (function ($) {

    //Variables privates
    var _revisionid,
        _indicatorid,
        _titulo = '',
        _resumen = '',
        _descripcion = '',
        _unidad = '',
        _simbolo = '',
        _funcion = '',
        _userid = -1,
        _pageSize = 10,
        _currentPage = 1,
        _totalPages = 0,
        _totalItems = 0,
        _orderby = '';

    function _setTotalPages(response) {
        _totalItems = response;
        _totalPages = Math.floor((response + _pageSize - 1) / _pageSize);
    }

    function leer(response) {
        _revisionid = response.revisionid;
        _indicatorid = response.indicatorid;
        _titulo = response.titulo;
        _resumen = response.resumen;
        _descripcion = response.descripcion;
        _unidad = response.unidad;
        _simbolo = response.simbolo;
        _funcion = response.funcion;
        _userid = response.userid;
    }

    // Return an object exposed to the public
    return {

        // Consulta
        select: function () {
            var dataString = "{revisionid : " + _revisionid + "}";
            proxy.selRevision(dataString, leer, null);
        },

        // Elimina
        remove: function (funcOK, funcFALSE) {
            var dataString = "{revisionid : " + _revisionid + "}";
            proxy.delRevision(dataString, funcOK, funcFALSE);
        },

        // Cuenta
        count: function () {
            var dataString = "{indicatorid : " + _indicatorid + "}";
            proxy.countRevisions(dataString, _setTotalPages, null);
        },

        // Populate
        populate: function (funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + ", pageSize : " + _pageSize + ", currentPage : " + _currentPage + ", orderby : '" + _orderby + "'}";
            proxy.getRevisions(dataString, funcOK, funcFALSE);
        },

        setRevisionId: function (data) {
            _revisionid = data;
        },

        setIndicatorId: function (data) {
            _indicatorid = data;
        },

        setTitulo: function (data) {
            _titulo = data;
        },

        setResumen: function (data) {
            _resumen = data;
        },

        setDescripcion: function (data) {
            _descripcion = data;
        },

        setUnidad: function (data) {
            _unidad = data;
        },

        setSimbolo: function (data) {
            _simbolo = data;
        },

        setFuncion: function (data) {
            _funcion = data;
        },

        setUserid: function (data) {
            _userid = data;
        },

        setCurrentPage: function (data) {
            _currentPage = data;
        },

        setPageSize: function (data) {
            _pageSize = data;
        },

        setOrderBy: function (data) {
            _orderby = data;
        },

        getRevisionId: function () {
            return _revisionid;
        },

        getIndicatorId: function () {
            return _indicatorid;
        },

        getTitulo: function () {
            return _titulo;
        },

        getDescripcion: function () {
            return _descripcion;
        },

        getTotalPages: function () {
            return _totalPages;
        },

        getTotalItems: function () {
            return _totalItems;
        }
    };
} (jQuery));
