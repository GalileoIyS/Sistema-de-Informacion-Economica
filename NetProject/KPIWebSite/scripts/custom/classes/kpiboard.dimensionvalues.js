var dimensionvaluesObject = (function ($) {

    //Variables privates
    var _dimensionid,
        _datasetid,
        _codigo;

    function leer(response) {
        _dimensionid = response.dimensionid;
        _datasetid = response.datasetid;
        _codigo = response.codigo;
    }

    // Return an object exposed to the public
    return {

        // Consulta
        find: function (texto, funcOK, funcFALSE) {
            var dataString = "{dimensionid : " + _dimensionid + ", texto : '" + texto + "'}";
            proxy.getDimensionValues(dataString, funcOK, funcFALSE);
        },

        // Inserta
        insert: function (texto, funcOK, funcFALSE) {
            var dataString = "{dimensionid : " + _dimensionid + ", datasetid : " + _datasetid + ", texto : '" + texto + "'}";
            proxy.addDimensionValue(dataString, funcOK, funcFALSE);
        },

        // Elimina
        remove: function (texto, funcOK, funcFALSE) {
            var dataString = "{dimensionid : " + _dimensionid + ", datasetid : " + _datasetid + ", texto : '" + texto + "'}";
            proxy.delDimensionValue(dataString, funcOK, funcFALSE);
        },

        // Consulta
        asigned: function (pfilterid, funcOK, funcFALSE) {
            var dataString = "{dimensionid : " + _dimensionid + ", idfilter : " + pfilterid + "}";
            proxy.getDimensionValuesAsigned(dataString, funcOK, funcFALSE);
        },

        // No asignados
        notAsigned: function (pfilterid, funcOK, funcFALSE) {
            var dataString = "{dimensionid : " + _dimensionid + ", idfilter : " + pfilterid + "}";
            proxy.getDimensionValuesNotAsigned(dataString, funcOK, funcFALSE);
        },

        setDimensionId: function (data) {
            _dimensionid = data;
        },

        setDatasetId: function (data) {
            _datasetid = data;
        },

        setCodigo: function (data) {
            _codigo = data;
        },

        getDimensionId: function () {
            return _dimensionid;
        },

        getDatasetId: function () {
            return _datasetid;
        },

        getCodigo: function () {
            return _codigo;
        }
    };
} (jQuery));
