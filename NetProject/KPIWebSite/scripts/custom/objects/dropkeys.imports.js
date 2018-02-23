(function ($) {

    $.widget('dropkeys.importItem', {

        // opciones predeterminadas
        options: {
            'datos': [],
            'style': 'search liimports'
        },

        addItem: function (elem) {
            var self = this,
                opciones = this.options;

            this._liElement = $('<div>').addClass(opciones.style).data('importid', elem.importid).appendTo(this.element);

            //Primera Columna
            this._colOne = $('<div>').addClass('col-md-1').appendTo(this._liElement);
            $('<input>').attr('type', 'checkbox').data('importid', elem.importid)
                .on({ 'click': function (event) {
                    self._trigger('onchange', event, null);
                }
                }).appendTo(this._colOne);

            //Segunda Columna
            this._colSecond = $('<div>').addClass('col-md-1 no-padding-left no-margin-left no-padding-right no-margin-right').appendTo(this._liElement);
            this._imageDiv = $('<div>').addClass('categoria color-secundario-background').appendTo(this._colSecond);
            $('<div>').addClass('categoria-valor').append('<span>' + elem.num_datasets + '</span>').appendTo(this._imageDiv);
            $('<div>').addClass('categoria-desc').html('datasets').appendTo(this._imageDiv);

            //Tercera Columna
            this._colThird = $('<div>').addClass('col-md-10').appendTo(this._liElement);
            $('<h4>').html(elem.nombre).appendTo(this._colThird);

            this._iconsDiv = $('<div>').addClass("iconos").appendTo(this._colThird);
            this._iconsDiv.append('<span class="glyphicon glyphicon-calendar"></span>&nbsp;');
            $('<span>').attr('title', 'Import date').attr('data-livestamp', elem.fecha).appendTo(this._iconsDiv);
            this._iconsDiv.append('&nbsp;&nbsp;&nbsp;&nbsp;');
            $('<span>').attr('title', 'Number of accepted values').append('<span class="glyphicon glyphicon-ok"></span>&nbsp;' + elem.num_data_ok + '&nbsp;imported values').appendTo(this._iconsDiv);
            this._iconsDiv.append('&nbsp;&nbsp;&nbsp;&nbsp;');
            $('<span>').attr('title', 'Number of rejected values').append('<span class="glyphicon glyphicon-remove"></span>&nbsp;' + elem.num_data_error + '&nbsp;rejected values').appendTo(this._iconsDiv);

            this._descDiv = $('<div>').appendTo(this._colThird);
            $('<span>').html(elem.descripcion).appendTo(this._descDiv);

            if (elem.finalizado === 'S') {
                $('<div>').addClass('import-finalizado').html('Processed').appendTo(this._colThird);
            }
            else {
                $('<div>').addClass('import-en-proceso').html('...processing...').appendTo(this._colThird);
            }
        },

        _create: function () {
            var self = this,
                opciones = this.options;

            this._populate(opciones.datos);
        },

        _populate: function (data) {
            var self = this;

            //limpiamos la lista
            self.element.empty();

            //rellenamos las expresiones
            if (data) {
                $.each(data, function (i, item) {
                    self.addItem(item);
                })
            }
        },

        _setOption: function (key, value) {
            switch (key) {
                case "datos":
                    this._populate(value);
                    break;
            }
        },

        _destroy: function () {
            this.element.empty();
        }

    });
})(jQuery);