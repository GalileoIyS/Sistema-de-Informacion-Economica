(function ($) {

    $.widget('dropkeys.kpicombo', {

        // opciones predeterminadas
        options: {
            id: null,
            datos: [],
            style: 'kpi-combobox'
        },

        getData: function () {
            var items = [];
            $.each(this.options.datos, function (i, item) {
                items.push({ "id": item.id, "value": item.value });
            });
            return items;
        },

        _create: function () {
            var self = this,
                opciones = this.options;

            this.element.addClass(opciones.style);

            this._populate(opciones.datos);

            if (opciones.id) {
                this.element.val(i);
            }
        },

        _populate: function (data) {
            var self = this;

            //limpiamos la lista
            self.element.empty();

            //rellenamos los filtros
            if (data) {
                $.each(data, function (i, item) {
                    var opcion = $('<option />').attr('value', item.id).text(item.value)
                    .on({
                        'click': function (event) {
                            var elt = $(event.currentTarget),
                                        id = elt.attr('value');
                            self._trigger('onselect', event, { value: id });
                        }
                    });
                    if (item.idextra)
                        opcion.data('extra', item.idextra);
                    opcion.appendTo(self.element);
                })
            }
        },

        _destroy: function () {
            var self = this,
                opciones = this.options;

            self.element.removeClass(opciones.style);
            self.element.empty();
        },

        _setOption: function (key, value) {
            var self = this,
                opciones = this.options;

            switch (key) {
                case "datos":
                    opciones.datos = value;
                    this._populate(value);
                    break;
            }
        }
    });
})(jQuery);