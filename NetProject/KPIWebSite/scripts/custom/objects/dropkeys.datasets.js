(function ($) {

    $.widget('dropkeys.datasetItem', {

        // opciones predeterminadas
        options: {
            'datos': [],
            'style': 'search lidatasets'
        },

        addItem: function (elem) {
            var self = this,
                opciones = this.options;

            this._liElement = $('<div>').addClass(opciones.style).data('datasetid', elem.datasetid).appendTo(this.element);

            //Primera Columna
            this._colOne = $('<div>').addClass('col-md-1').appendTo(this._liElement);
            $('<input>').attr('type', 'checkbox').data('datasetid', elem.datasetid)
                .on({ 'click': function (event) {
                    self._trigger('onchange', event, null);
                }
                }).appendTo(this._colOne);

            //Segunda Columna
            this._colSecond = $('<div>').addClass('col-md-1 no-padding-left no-margin-left no-padding-right no-margin-right').appendTo(this._liElement);
            this._imageDiv = $('<div>').addClass('categoria color-secundario-background').appendTo(this._colSecond);
            $('<div>').addClass('categoria-valor').append('<span>' + elem.numdatos + '</span>').appendTo(this._imageDiv);
            $('<div>').addClass('categoria-desc').html('datos').appendTo(this._imageDiv);


            //Tercera Columna
            this._colThird = $('<div>').addClass('col-md-8').appendTo(this._liElement);
            $('<h4>').html(elem.nombre).appendTo(this._colThird);

            this._iconsDiv = $('<div>').addClass("iconos")
                                      .appendTo(this._colThird);
            $('<span>').attr('title', 'Data frequency sampling').append('<span class="glyphicon glyphicon-time"></span>&nbsp;' + elem.descripcion).appendTo(this._iconsDiv);
            this._iconsDiv.append('&nbsp;&nbsp;&nbsp;&nbsp;');
            $('<span>').attr('title', 'Number of used attributes').append('<i class="fa fa-bullseye"></i>&nbsp;' + elem.numdimensions + '&nbsp;attributes').appendTo(this._iconsDiv);

            this._optionsDiv = $('<div>').addClass("iconos")
                                      .appendTo(this._colThird);

            //Boton de editar dataset
            this._dataspan = $('<span>').appendTo(this._optionsDiv);
            $('<a>').attr('href', '#').attr('title', 'Edit the dataset and its values').data('datasetid', elem.datasetid).html('<span class="glyphicon glyphicon-pencil"></span>&nbsp;data')
                            .on({ 'click': function (event) {
                                var elt = $(event.currentTarget),
                                    iddataset = elt.data('datasetid');
                                self._trigger('editar', event, { datasetid: iddataset });
                            }
                            }).appendTo(this._dataspan);
            this._optionsDiv.append('&nbsp;&nbsp;&nbsp;&nbsp;');

            //Boton de eliminar dataset
            this._deletespan = $('<span>').appendTo(this._optionsDiv);
            $('<a>').attr('href','#').attr('title', 'Delete the dataset and all its associated data').data('datasetid', elem.datasetid).html('<span class="glyphicon glyphicon-remove"></span>&nbsp;delete')
                            .on({ 'click': function (event) {
                                var elt = $(event.currentTarget),
                                    iddataset = elt.data('datasetid');
                                self._trigger('eliminar', event, { datasetid: iddataset });
                            }
                            }).appendTo(this._deletespan);
            this._optionsDiv.append('&nbsp;&nbsp;&nbsp;&nbsp;');

            //Boton de exportar dataset
            this._exportspan = $('<span>').appendTo(this._optionsDiv);
            $('<a>').attr('href', '#').attr('title', 'Export all the values').data('datasetid', elem.datasetid).html('<span class="glyphicon glyphicon-open"></span>&nbsp;export')
                            .on({
                                'click': function (event) {
                                    var elt = $(event.currentTarget),
                                        iddataset = elt.data('datasetid');
                                    self._trigger('exportar', event, { datasetid: iddataset });
                                }
                            }).appendTo(this._exportspan);
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