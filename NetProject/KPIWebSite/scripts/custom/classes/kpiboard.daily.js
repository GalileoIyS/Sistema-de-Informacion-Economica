var dailyObject = (function ($) {

    //Variables privates
    var _iddaily,
        _iduser,
        _fecha,
        _comment;

    function leer(response) {
        _iddaily = response.dailyid;
        _iduser = response.userid;
        _fecha = response.fecha;
        _comment = response.comment;
    }

    // Return an object exposed to the public
    return {

        // Consulta
        populate: function (id) {
            var dataString = "{iddaily : " + _iddaily + "}";
            proxy.getDailyComments(dataString, leer, null);
        },

        // Consulta
        select: function (funcOK, funcFALSE) {
            var dataString = "{idformula : " + _idformula + "}";
            proxy.getFormula(dataString, funcOK, funcFALSE);
        },

        // Inserta
        insert: function (funcOK, funcFALSE) {
            var dataString = "{iddaily : " + _iddaily + ", comentario : '" + _comment + "'}";
            proxy.addDailyComment(dataString, funcOK, funcFALSE);
        },

        setIdDaily: function (data) {
            _iddaily = data;
        },

        setComentario: function (data) {
            _comment = data;
        }

    };
} (jQuery));
