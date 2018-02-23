var columnsObject = (function ($) {

    //Variables privates
    var _arrayColumns = [];

    function _columnInfo(column, name, attrid) {
        this.column = column;
        this.name = name;
        this.attrid = attrid;
    }

    function _yaExiste(id) {
        var Pos = -1;
        $.each(_arrayColumns, function (i, elem) {
            if (elem.attrid === id)
                Pos = i;
        });
        return Pos;
    }

    // Return an object exposed to the public
    return {

        // Inserta
        add: function (i, name, attrid) {
            _arrayColumns.push(new _columnInfo(i, name, attrid));
        },

        // Actualiza
        update: function (i, nombre, attrid) {
            if (attrid < -1) {
                var PosId = _yaExiste(attrid);
                if (PosId >= 0) {
                    _arrayColumns[PosId].name = '-- none --';
                    _arrayColumns[PosId].attrid = -1;
                }
            }
            _arrayColumns[i].column = i;
            _arrayColumns[i].name = nombre;
            _arrayColumns[i].attrid = attrid;
        },

        reset: function()
        {
            _arrayColumns=[];
        },

        getName: function (i) {
            return _arrayColumns[i].name;
        },

        getAttributeId: function (i) {
            return _arrayColumns[i].attrid;
        },

        getNames: function () {
            var columns = [];
            $.each(_arrayColumns, function (i, elem) {
                columns.push(elem.name);
            });
            return columns;
        },

        hasAttributeId: function (attrid) {
            var exists = false;
            $.each(_arrayColumns, function (i, elem) {
                if (elem.attrid === attrid) {
                    exists = true;
                    return false;
                }
            });
            return exists;
        },

        getColumnId: function (attrid) {
            var numCol = -1;
            $.each(_arrayColumns, function (i, elem) {
                if (elem.attrid === attrid) {
                    numCol = i;
                    return false;
                }
            });
            return numCol;
        },

        serialize: function () {
            var resultado='';
            var coma='';
            $.each(_arrayColumns, function (i, elem) {
                resultado = resultado + coma + JSON.stringify(elem);
                coma = ',';
            });
            return '[' + resultado + ']';
        },
    };
} (jQuery));
