var expressionObject = (function ($) {

    //Variables privates
    var _idexpresion,
        _idformula,
        _indicatorid,
        _funcion;

    function leer(response) {
        _idexpresion = response.idexpresion;
        _idformula = response.idformula;
    }

    // Return an object exposed to the public
    return {

        // Consulta
        select: function (id) {
            var dataString = "{dashboardid : " + id + "}";
            proxy.getDashboard(dataString, leer, null);
        },

        // Inserta
        insert: function (funcOK, funcFALSE) {
            var dataString = "{titulo : '" + _titulo + "'}";
            proxy.addDashboard(dataString, funcOK, funcFALSE);
        },

        // Modifica
        update: function (funcOK, funcFALSE) {
            var dataString = "{dashboardid : " + _iddashboard + ", titulo : '" + _titulo + "'}";
            proxy.updDashboard(dataString, funcOK, funcFALSE);
        },

        // Elimina
        remove: function (data, funcOK, funcFALSE) {
            var dataString = "{dashboardid : " + data.iddashboard + "}";
            proxy.delDashboard(dataString, funcOK, funcFALSE);
        },

        // Rellena
        populate: function (funcOK, funcFALSE) {
            var dataString = "{idformula:" + _idformula + "}";
            proxy.getExpresions(dataString,funcOK, funcFALSE);
        },

        setIdExpresion: function (data) {
            _idexpresion = data;
        },

        setIdFormula: function (data) {
            _idformula = data;
        },

        setIdIndicator: function (data) {
            _indicatorid = data;
        },

        setFuncion: function (data) {
            _funcion = data;
        },

        getIdExpresion: function () {
            return _idexpresion;
        },

        getIdFormula: function () {
            return _idformula;
        },

        getIdIndicator: function () {
            return _indicatorid;
        },

        getFuncion: function () {
            return _funcion;
        }

    };
} (jQuery));
