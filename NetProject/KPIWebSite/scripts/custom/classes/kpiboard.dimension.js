var dimensionObject = (function ($) {

    //Variables privates
    var _dimensionid,
        _indicatorid,
        _nombre = '',
        _descripcion = '',
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
        _dimensionid = response.dimensionid;
        _indicatorid = response.indicatorid;
        _nombre = response.nombre;
        _descripcion = response.descripcion;
    }

    // Return an object exposed to the public
    return {

        // Consulta
        select: function () {
            var dataString = "{dimensionid : " + _dimensionid + "}";
            proxy.selDimension(dataString, leer, null);
        },

        // Inserta
        insert: function (funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + ", nombre : '" + _nombre + "', descripcion : '" + _descripcion + "'}";
            proxy.addAttribute(dataString, funcOK, funcFALSE);
        },

        // Elimina
        remove: function (funcOK, funcFALSE) {
            var dataString = "{dimensionid : " + _dimensionid + "}";
            proxy.delDimension(dataString, funcOK, funcFALSE);
        },

        // Cuenta
        count: function () {
            var dataString = "{indicatorid : " + _indicatorid + ", nombre : '" + _nombre + "'}";
            proxy.countAttributes(dataString, _setTotalPages, null);
        },

        // Populate
        populate: function (funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + ", nombre : '" + _nombre + "', pageSize : " + _pageSize + ", currentPage : " + _currentPage + ", orderby : '" + _orderby + "'}";
            proxy.getAttributes(dataString, funcOK, funcFALSE);
        },

        fillCombo: function (funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + "}";
            proxy.getDimensions(dataString, funcOK, funcFALSE);
        },

        setDimensionId: function (data) {
            _dimensionid = data;
        },

        setIndicatorId: function (data) {
            _indicatorid = data;
        },

        setNombre: function (data) {
            _nombre = data;
        },

        setDescripcion: function (data) {
            _descripcion = data;
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

        getDimensionId: function () {
            return _dimensionid;
        },

        getIndicatorId: function () {
            return _indicatorid;
        },

        getNombre: function () {
            return _nombre;
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
