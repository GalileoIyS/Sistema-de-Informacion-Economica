(function ($) {

    $.widget('dropkeys.indicatorItem', {

        // opciones predeterminadas
        options: {
            'datos': [],
            'style': 'search'
        },

        addItem: function (elem) {
            var self = this,
                opciones = this.options;

            this._liElement = $('<div>').addClass(opciones.style).appendTo(this.element);

            //Panel de la imagen izquierda
            this._colIzda = $('<div>').addClass("col-md-2")
                                      .appendTo(this._liElement);
            this._imageDiv = $('<div>').addClass('circular width80').attr('style', 'border: 5px solid ' + elem.estilo + ';')
                                      .appendTo(this._colIzda);
            $('<img>').attr('src', elem.imageurl).attr('alt', elem.titulo).attr('style','width:100px;border-width:0px;').appendTo(this._imageDiv);

            //Título
            this._colDcha = $('<div>').addClass("col-md-10")
                                      .appendTo(this._liElement);
            this._h4Elem = $('<h4>').appendTo(this._colDcha);
            $('<a>').attr('title', elem.titulo).attr('href', 'indicator.aspx?indicatorid=' + elem.indicatorid).html(elem.titulo).appendTo(this._h4Elem);

            //Subtítulo
            $('<p>').html(elem.resumen).appendTo(this._colDcha);

            //Resto de iconos
            this._iconsDiv = $('<div>').addClass("iconos hidden-xs")
                                      .appendTo(this._colDcha);
            this._iconsDiv.append('<span class="glyphicon glyphicon-calendar"></span>&nbsp;');
            $('<span>').attr('title', 'Creation date').attr('data-livestamp', elem.fecha_alta).appendTo(this._iconsDiv);
            this._iconsDiv.append('&nbsp;&nbsp;&nbsp;&nbsp;<span class="glyphicon glyphicon-eye-open"></span>&nbsp;');
            $('<span>').attr('title', 'Latest data modified').attr('data-livestamp', elem.ultima_fecha).appendTo(this._iconsDiv);
            this._iconsDiv.append('&nbsp;&nbsp;&nbsp;&nbsp;');
            $('<span>').attr('title', 'Number of users sharing this indicator').append('<i class="fa fa-group"></i>&nbsp;' + elem.num_usuarios).appendTo(this._iconsDiv);
            this._iconsDiv.append('&nbsp;&nbsp;&nbsp;&nbsp;');
            $('<span>').attr('title', 'Number of comments').append('<span class="glyphicon glyphicon-comment"></span>&nbsp;' + elem.num_comentarios).appendTo(this._iconsDiv);
            this._iconsDiv.append('&nbsp;&nbsp;&nbsp;&nbsp;');
            $('<span>').attr('title', 'Current rating').append('<span class="glyphicon glyphicon-star"></span>&nbsp;' + elem.rating).appendTo(this._iconsDiv);
            this._iconsDiv.append('&nbsp;&nbsp;&nbsp;&nbsp;');
            $('<span>').attr('title', 'Measure unit').append('<span class="glyphicon glyphicon-signal"></span>&nbsp;' + elem.unidad).appendTo(this._iconsDiv);

            if (elem.asignado == 0) {
                $('<a>').addClass('add-indicator').attr('title', elem.titulo).attr('href', '#').data('indicadorid', elem.indicatorid).data('nombre', elem.titulo).html('<span class="glyphicon glyphicon-plus"></span>&nbsp;Add')
                    .on({ 'click': function (event) {
                        var elt = $(event.currentTarget),
                            idindicador = elt.data('indicadorid'),
                            nombreindicador = elt.data('nombre');
                        self._trigger('insertar', event, { indicadorid: idindicador, nombre: nombreindicador });
                    }
                    }).appendTo(this._colDcha);
            }
            if (elem.compartido == 'N') {
                $('<a>').addClass('share-indicator').attr('title', elem.titulo).attr('href', 'registrado/sharekpi.aspx?indicatorid=' + elem.indicatorid).html('<span class="glyphicon glyphicon-share"></span>&nbsp;Share').appendTo(this._colDcha);
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
            //self.element.empty();

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