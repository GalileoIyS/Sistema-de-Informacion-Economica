var dashboardObject = (function ($) {

    //Variables privates
    var _iddashboard,
        _titulo;

    function leer(response) {
        _iddashboard = response.dashboardid;
        _titulo = response.titulo;
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
        remove: function (funcOK, funcFALSE) {
            var dataString = "{dashboardid : " + _iddashboard + "}";
            proxy.delDashboard(dataString, funcOK, funcFALSE);
        },

        // Rellena
        populate: function (funcOK, funcFALSE) {
            proxy.getDashboards(funcOK, funcFALSE);
        },

        setIdDashboard: function (data) {
            _iddashboard = data;
        },

        setTitulo: function (data) {
            _titulo = data;
        },

        getIdDashboard: function () {
            return _iddashboard;
        },

        getTitulo: function () {
            return _titulo;
        }

    };
} (jQuery));
