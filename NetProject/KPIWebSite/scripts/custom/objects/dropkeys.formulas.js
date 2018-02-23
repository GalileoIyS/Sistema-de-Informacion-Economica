(function ($) {

    $.widget('dropkeys.formulaItem', {

        // opciones predeterminadas
        options: {
            'showOptions': true,
            'datos': [],
            'style': 'well margin-top-10 margin-left-10 margin-right-10 font-sm'
        },

        addItem: function (elem) {
            var self = this,
                opciones = this.options;

            this._lstGroup = $('<div>').addClass('list-group list-dropkeys')
                                      .appendTo(this.element);

            this._smartForm = $('<div>').addClass('smart-form formula-item').data('formulaid', elem.formulaid)
                                      .appendTo(this._lstGroup);

            this._label = $('<label>').addClass('toggle')
                                      .appendTo(this._smartForm);

            //Checkbox de activacion
            this._input = $('<input>').attr('type', 'checkbox').attr('checked', 'checked').attr('name', 'checkbox-toggle').data('formulaid', elem.formulaid).data('widgetid', elem.widgetid)
                .on({
                    'click': function (event) {
                        var elt = $(event.currentTarget),
                            idformula = elt.data('formulaid'),
                            idwidget = elt.data('widgetid');
                        self._trigger('checked', event, { formulaid: idformula, widgetid: idwidget });
                    }
                }).appendTo(this._label);
            $('<i>').attr('data-swchon-text', 'ON').attr('data-swchoff-text', 'OFF').appendTo(this._label);

            //Botonera de opciones
            if (opciones.showOptions) {
                this._lnkMenu = $('<a>').addClass('btn btn-default dropdown-toggle').attr('data-toggle', 'dropdown').attr('href', '#').html('<span class="caret"></span>').appendTo(this._smartForm);
                this._lnkMenu.dropdown();

                this._ulList = $('<ul>').addClass('dropdown-menu').appendTo(this._smartForm);
                this._liEdit = $('<li>').appendTo(this._ulList);
                $('<a>').addClass('btn-edit-formula').data('formulaid', elem.formulaid).attr('href', '#').html('<span class="glyphicon glyphicon-pencil"></span>&nbsp;Edit...').on({
                    'click': function (event) {
                        var elt = $(event.currentTarget),
                            idformula = elt.data('formulaid');
                        self._trigger('editar', event, { formulaid: idformula });
                    }
                }).appendTo(this._liEdit);
                this._liFilter = $('<li>').appendTo(this._ulList);
                $('<a>').addClass('btn-filter-formula').data('formulaid', elem.formulaid).attr('href', '#').html('<span class="glyphicon glyphicon-filter"></span>&nbsp;Filters...').on({
                    'click': function (event) {
                        var elt = $(event.currentTarget),
                            idformula = elt.data('formulaid');
                        self._trigger('filtrar', event, { formulaid: idformula });
                    }
                }).appendTo(this._liFilter);
                $('<li>').addClass('divider').appendTo(this._ulList);
                this._liDelete = $('<li>').appendTo(this._ulList);
                $('<a>').addClass('btn-remove-formula').data('formulaid', elem.formulaid).attr('href', '#').html('<span class="glyphicon glyphicon-remove"></span>&nbsp;Delete').on({
                    'click': function (event) {
                        var elt = $(event.currentTarget),
                            idformula = elt.data('formulaid');
                        self._trigger('eliminar', event, { formulaid: idformula });
                    }
                }).appendTo(this._liDelete);
            }
            this._colorPanel = $('<span>').attr('style', 'background-color:' + elem.color).data('color', elem.color).addClass('categoria-small max-width-dashboard').appendTo(this._smartForm);
            if (elem.validated >= 0)
                this._colorPanel.after('<span class="formula-text">' + elem.nombre + '</span>');
            else
                this._colorPanel.after('<span class="formula-text no-validated">' + elem.nombre + '</span>');
        },

        updItem: function (elem) {
            var self = this,
                opciones = this.options;

            var actual = this.element.find('div').filter(function () {
                return $(this).data("formulaid") == elem.formulaid
            });

            if (actual) {
                //Span de color
                actual.find('span.categoria-small').attr('style', 'background-color:' + elem.color).data('color', elem.color);

                //Span del texto
                if (elem.validated >= 0)
                    actual.find('span.formula-text').removeClass('no-validated').text(elem.nombre);
                else
                    actual.find('span.formula-text').addClass('no-validated').text(elem.nombre);
            }
        },

        _create: function () {
            var self = this,
                opciones = this.options;

            this.element.addClass(opciones.style);

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