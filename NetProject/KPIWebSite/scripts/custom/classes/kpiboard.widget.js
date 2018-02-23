var widgetObject = (function ($) {

    //Variables privates
    var _idwidget,
        _iddashboard,
        _titulo,
        _fecha_ini,
        _fecha_fin,
        _dimension,
        _tipo_grafico,
        _estilo;

    function leer(response) {
        _iddashboard = response.dashboardid;
        _idwidget = response.idwidget;
        _titulo = response.titulo;
        _fecha_ini = response.fecha_inicio;
        _fecha_fin = response.fecha_fin;
        _dimension = response.dimension;
        _tipo_grafico = response.grafico;
        _estilo = response.estilo;
    }

    // Return an object exposed to the public
    return {

        // Consulta
        select: function (funcOK, funcFALSE) {
            var dataString = "{idwidget : " + _idwidget + "}";
            proxy.selWidget(dataString, leer, funcFALSE);
        },

        // Inserta
        insert: function (funcOK, funcFALSE) {
            var dataString = "{iddashboard : " + _iddashboard + ", titulo : '" + _titulo + "'}";
            proxy.addWidget(dataString, funcOK, funcFALSE);
        },

        // Modifica
        update: function (funcOK, funcFALSE) {
            var dataString = "{idwidget : " + _idwidget + ", titulo : '" + _titulo + "', estilo : '" + _estilo + "'}";
            proxy.updWidget(dataString, funcOK, funcFALSE);
        },

        // Elimina
        remove: function (funcOK, funcFALSE) {
            var dataString = "{idwidget : " + _idwidget + "}";
            proxy.delWidget(dataString, funcOK, funcFALSE);
        },

        // Rellena
        populate: function (funcOK, funcFALSE) {
            var dataString = "{iddashboard : " + _iddashboard + "}";
            proxy.getWidgets(dataString, funcOK, funcFALSE);
        },

        // Especial
        getResumen: function (funcOK, funcFALSE) {
            var dataString = "{widgetid : " + _idwidget + "}";
            proxy.getWidgetResume(dataString, funcOK, funcFALSE);
        },

        GetPercentData: function (funcOK, funcFALSE) {
            var dataString = "{widgetid : " + _idwidget + ", psTipo : '" + _tipo_grafico + "', pdDesde : '" + _fecha_ini + "', pdHasta : '" + _fecha_fin + "'}";
            proxy.getPercentData(dataString, funcOK, funcFALSE);
        },

        GetAbsoluteData: function (funcOK, funcFALSE) {
            var dataString = "{widgetid : " + _idwidget + ", pDimension:'" + _dimension + "', psTipo : '" + _tipo_grafico + "', pdDesde : '" + _fecha_ini + "', pdHasta : '" + _fecha_fin + "'}";
            proxy.getAbsoluteData(dataString, funcOK, funcFALSE);
        },

        setIdWidget: function (data) {
            _idwidget = data;
        },

        setIdDashboard: function (data) {
            _iddashboard = data;
        },

        setTitulo: function (data) {
            _titulo = data;
        },

        setTipoGrafico: function (data) {
            _tipo_grafico = data;
        },

        setTipoTiempo: function (data) {
            _dimension = data;
        },

        setFechaIni: function (data) {
            _fecha_ini = data;
        },

        setFechaFin: function (data) {
            _fecha_fin = data;
        },

        getIdWidget: function () {
            return _idwidget;
        },

        getIdDashboard: function () {
            return _iddashboard;
        },

        getTitulo: function () {
            return _titulo;
        },

        getFechaIni: function () {
            return _fecha_ini;
        },

        getFechaFin: function () {
            return _fecha_fin;
        },

        getTipoTiempo: function () {
            return _dimension;
        },

        getTipoGrafico: function () {
            return _tipo_grafico;
        },

        getEstilo: function () {
            return _estilo;
        },

        getModoDatos: function () {
            switch(_tipo_grafico)
            {
                case 'Q':
                    return 0;
                    break;
                case 'B':
                    return 0;
                    break;
                case 'L':
                    return 1;
                    break;
                case 'A':
                    return 1;
                    break;
                case 'T':
                    return 1;
                    break;
                case 'H':
                    return 1;
                    break;
                default:
                    return -1;
                    break;
            }
            return -1;
        }

    };
} (jQuery));
