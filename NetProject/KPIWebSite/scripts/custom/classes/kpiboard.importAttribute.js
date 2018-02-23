var attributeObject = (function ($) {

    //Variables privates
    var _arrayElement = [];

    function _elementInfo(idValue, nameValue) {
        this.id = idValue;
        this.name = nameValue;
    }

    function _yaExiste(idValue) {
        var Pos = -1;
        $.each(_arrayElement, function (i, elem) {
            if (elem.id === idValue)
                Pos = i;
        });
        return Pos;
    }

    // Return an object exposed to the public
    return {

        // Inserta
        add: function (idValue, nameValue) {
            _arrayElement.push(new _elementInfo(idValue, nameValue));
        },

        // Actualiza
        update: function (idValue, nameValue) {
            var PosId = _yaExiste(idValue);
            if (PosId >= 0) {
                _arrayElement[PosId].name = nameValue;
            }
        },

        reset: function () {
            _arrayElement = [];
        },

        getId: function (i) {
            return _arrayElement[i].id;
        },

        getName: function (i) {
            return _arrayElement[i].name;
        },

        count: function()
        {
            return _arrayElement.length;
        },

        serialize: function () {
            var resultado = '';
            var coma = '';
            $.each(_arrayElement, function (i, elem) {
                resultado = resultado + coma + JSON.stringify(elem);
                coma = ',';
            });
            return '[' + resultado + ']';
        },
    };
}(jQuery));
