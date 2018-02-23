(function ($) {

    $.widget('dropkeys.attributeItem', {

        // opciones predeterminadas
        options: {
            'datos': [],
            'style': 'search lidimensions'
        },

        addItem: function (elem) {
            var self = this,
                opciones = this.options;

            this._liElement = $('<div>').addClass(opciones.style).data('dimensionid', elem.dimensionid).appendTo(this.element);

            //Primera Columna
            this._colOne = $('<div>').addClass('col-md-1').appendTo(this._liElement);
            $('<input>').attr('type', 'checkbox').data('dimensionid', elem.dimensionid)
                .on({
                    'click': function (event) {
                        self._trigger('onchange', event, null);
                    }
                }).appendTo(this._colOne);

            //Segunda Columna
            this._colSecond = $('<div>').addClass('col-md-1 no-padding-left no-margin-left no-padding-right no-margin-right').appendTo(this._liElement);
            this._imageDiv = $('<div>').addClass('categoria color-secundario-background').appendTo(this._colSecond);
            $('<div>').addClass('categoria-valor').append('<span>' + elem.numdatos + '</span>').appendTo(this._imageDiv);
            $('<div>').addClass('categoria-desc').html('valores').appendTo(this._imageDiv);

            //Tercera Columna
            this._colThird = $('<div>').addClass('col-md-10').appendTo(this._liElement);
            $('<h4>').html(elem.nombre).appendTo(this._colThird);

            this._iconsDiv = $('<div>').addClass("iconos")
                                      .appendTo(this._colThird);

            this._iconsDiv.append('<span class="glyphicon glyphicon-calendar"></span>&nbsp;');
            $('<span>').attr('title', 'Creation date').attr('data-livestamp', elem.fecha).appendTo(this._iconsDiv);
            this._iconsDiv.append('&nbsp;&nbsp;&nbsp;&nbsp;');
            $('<a>').attr('title', 'Creator user').addClass('show-modal-info-user').attr('href','#').data('userid',elem.userid).append('<span class="glyphicon glyphicon-user"></span>&nbsp;' + elem.userfirstname + ' ' + elem.userlastname).appendTo(this._iconsDiv);

            this._descDiv = $('<div>').appendTo(this._colThird);
            $('<span>').html(elem.descripcion).appendTo(this._descDiv);
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