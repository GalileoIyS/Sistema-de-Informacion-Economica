var filterObject = (function ($) {

    //Variables privates
    var _idfilter,
        _idexpresion,
        _dimensionid,
        _filtro,
        _valor,
        _pageSize = 10,
        _currentPage = 1,
        _totalPages = 0,
        _totalItems = 0,
        _orderby = '';

    function leer(response) {
        _idfilter = response.filterid;
        _dimensionid = response.dimensionid;
        _filtro = response.filtro;
        _valor = response.valor;
    }

    // Return an object exposed to the public
    return {

        // Consulta
        select: function () {
            var dataString = "{filtroid : " + _idfilter + "}";
            proxy.getFilter(dataString, leer, null);
        },

        // Inserta
        insert: function (funcOK, funcFALSE) {
            var dataString = "{idexpresion: " + _idexpresion + ", dimensionid:" + _dimensionid + ", filtro :'" + _filtro + "', valor : '" + _valor + "'}";
            proxy.addFilters(dataString, funcOK, funcFALSE);
        },

        // Elimina
        remove: function (funcOK, funcFALSE) {
            var dataString = "{filtroid:" + _idfilter + "}";
            proxy.delFilters(dataString, funcOK, funcFALSE);
        },

        // Populate
        populate: function (funcOK, funcFALSE) {
            var dataString = "{idexpresion : " + _idexpresion + ", pageSize : " + _pageSize + ", currentPage : " + _currentPage + ", orderby : '" + _orderby + "'}";
            proxy.getFilters(dataString, funcOK, funcFALSE);
        },

        setFilterId: function (data) {
            _idfilter = data;
        },

        setExpresionId: function (data) {
            _idexpresion = data;
        },

        setDimensionId: function (data) {
            _dimensionid = data;
        },

        setFiltro: function (data) {
            _filtro = data;
        },

        setValor: function (data) {
            _valor = data;
        },

        getFilterId: function () {
            return _idfilter;
        },

        getDimensionId: function () {
            return _dimensionid;
        },

        getFiltro: function () {
            return _filtro;
        },

        getValor: function () {
            return _valor;
        }
    };
} (jQuery));
