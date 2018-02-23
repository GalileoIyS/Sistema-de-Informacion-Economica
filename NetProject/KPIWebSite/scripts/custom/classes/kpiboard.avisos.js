var avisoObject = (function ($) {

    //Variables privates
    var _pagina,
        _mensaje,
        _error;

    // Return an object exposed to the public
    return {

        // Peticion de amistad
        insert: function () {
            var dataString = "{pagina : '" + _pagina + "', mensaje : '" + _mensaje + "', error : " + _error + "}";
            proxy.addAviso(dataString, null);
        },

        setPagina: function (data) {
            _pagina = data;
        },

        setMensaje: function (data) {
            _mensaje = data;
        },

        setError: function (data) {
            _error = data;
        }
    };
} (jQuery));
