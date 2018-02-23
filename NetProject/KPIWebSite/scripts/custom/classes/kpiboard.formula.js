var formulaObject = (function ($) {

    //Variables privates
    var _idformula,
        _idwidget,
        _nombre,
        _formula,
        _original,
        _color,
        _validated,
        _fecha,
        _display,
        _filter = '',
        _pageSize = 15,
        _currentPage = 1,
        _totalPages = 0,
        _totalItems = 0,
        _orderby = '';

    function _setTotalPages(response) {
        _totalItems = response;
        _totalPages = Math.floor((response + _pageSize - 1) / _pageSize);
    }

    function leer(response) {
        _idformula = response.formulaid;
        _idwidget = response.widgetid;
        _nombre = response.nombre;
        _formula = response.formula;
        _original = response.original;
        _color = response.color;
        _fecha = response.fecha;
        _validated = response.validated;
        _display = response.display;
    }

    // Return an object exposed to the public
    return {

        // Consulta
        select: function (funcFALSE) {
            var dataString = "{idformula : " + _idformula + "}";
            proxy.getFormula(dataString, leer, funcFALSE);
        },

        // Inserta
        insert: function (funcOK, funcFALSE) {
            var dataString = "{idwidget : " + _idwidget + ", nombre : '" + _nombre + "', formula : '" + _original + "', color : '" + _color + "'}";
            proxy.addFormula(dataString, funcOK, funcFALSE);
        },

        // Modifica
        update: function (funcOK, funcFALSE) {
            var dataString = "{idformula : " + _idformula + ", nombre : '" + _nombre + "', formula : '" + _original + "', color : '" + _color + "'}";
            proxy.updFormula(dataString, funcOK, funcFALSE);
        },

        // Elimina
        remove: function (data, funcOK, funcFALSE) {
            var dataString = "{formulaid : " + data.formulaid + "}";
            proxy.delFormula(dataString, funcOK, funcFALSE);
        },

        // Consulta
        populate: function (funcOK, funcFALSE) {
            var dataString = "{idwidget : " + _idwidget + "}";
            proxy.getFormulas(dataString, funcOK, funcFALSE);
        },

        //obtenemos las formulas por indicador
        getFormulasIndicator: function (indicatorid, funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + indicatorid + ", filterBy : '" + _filter + "', pageSize : " + _pageSize + ", currentPage : " + _currentPage + ", orderby : '" + _orderby + "'}";
            proxy.getFormulasIndicator(dataString, funcOK, funcFALSE);
        },

        // Copia
        copy: function (funcOK, funcFALSE) {
            var dataString = "{idwidget : " + _idwidget + ", idformula : " + _idformula + ", nombre : '" + _nombre + "'}";
            proxy.copyFormula(dataString, funcOK, funcFALSE);
        },

        setIdFormula: function (data) {
            _idformula = data;
        },

        setIdWidget: function (data) {
            _idwidget = data;
        },

        setNombre: function (data) {
            _nombre = data;
        },

        setFormula: function (data) {
            _formula = data;
        },

        setOriginal: function (data) {
            _original = data;
        },

        setColor: function (data) {
            _color = data;
        },

        setCurrentPage: function (data) {
            _currentPage = data;
        },

        setFilter: function (data) {
            _filter = data;
        },

        setOrderBy: function (data) {
            _orderby = data;
        },

        getIdFormula: function () {
            return _idformula;
        },

        getIdWidget: function () {
            return _idwidget;
        },

        getNombre: function () {
            return _nombre;
        },

        getFormula: function () {
            return _formula;
        },

        getOriginal: function () {
            return _original;
        },

        getColor: function () {
            return _color;
        },

        getFecha: function () {
            return _fecha;
        },

        getValidado: function () {
            return _validated;
        },

        getDisplay: function () {
            return _display;
        },

        getTotalPages: function () {
            return _totalPages;
        },

        getTotalItems: function () {
            return _totalItems;
        },

        getPageSize: function () {
            return _pageSize;
        },

        getCurrentPage: function () {
            return _currentPage;
        }
    };
} (jQuery));
