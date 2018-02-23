var functionObject = (function ($) {

    //Variables privates
    var _idfunction,
        _nombre;

    function leer(response) {
        _idfunction = response.tipoid;
        _nombre = response.nombre;
    }

    // Return an object exposed to the public
    return {

        // Rellena
        populate: function (funcOK, funcFALSE) {
            proxy.getFunctions(funcOK, funcFALSE);
        }
    };
} (jQuery));
