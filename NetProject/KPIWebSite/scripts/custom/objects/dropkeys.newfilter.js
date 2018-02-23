(function ($) {

    $.widget('dropkeys.newfilter', {

        // opciones predeterminadas
        options: {
            data: '',
            cmbDim: [],
            cmbOpe: [],
            'style': 'kpi-new-filter'
        },

        populateDim: function (elem) {
            var self = this,
                opciones = this.options;

            if (elem != null) {
                $('.' + opciones.style).removeClass('hidden');
                $('#cmbDimensiones').kpicombo({ datos: elem });
            }
            else {
                $('.' + opciones.style).addClass('hidden');
            }
        },

        populateOpe: function (elem) {
            var self = this,
                opciones = this.options;

            $('#cmbOperadores').kpicombo({ datos: elem });
        },

        restore: function () {
            var self = this,
                opciones = this.options;

            this._cmbOperadores.val('Igual');
            this._cmbOperadores.trigger('change');
            this._filterText.val('');
        },

        _create: function () {
            var self = this,
                opciones = this.options;

            this._divFilterText = $('<div>').addClass('form-inline filterText').appendTo(this.element);

            //Añadimos el cuadro de selección de características
            this._divAttributes = $('<div>').addClass('form-group').appendTo(this._divFilterText);
            $('<label>').addClass('sr-only').appendTo(this._divAttributes);
            this._cmbDimensiones = $('<select />').addClass('form-control margin-right-10').attr('id', 'cmbDimensiones').appendTo(this._divAttributes);
            this._cmbDimensiones.on({ 'change': function (event) {
                var elt = $(event.currentTarget);
                var optionOperadores = $('select#cmbOperadores').find(":selected");
                var valueSelected = optionOperadores.val();
                if ((valueSelected == 'Lista') || (valueSelected == 'NoLista')) {
                    var optionDimensions = $('select#cmbDimensiones').find(":selected");
                    elt.closest('.kpi-new-filter').find('.filterList').kpilistfilters({ dimensionid: optionDimensions.val() });
                }
            }
            });

            //Añadimos el cuadro de selección de operadores
            this._divOperadores = $('<div>').addClass('form-group').appendTo(this._divFilterText);
            $('<label>').addClass('sr-only').appendTo(this._divOperadores);
            this._cmbOperadores = $('<select />').addClass('form-control margin-right-10').attr('id', 'cmbOperadores').appendTo(this._divOperadores);
            this._cmbOperadores.on({ 'change': function (event) {
                var elt = $(event.currentTarget);
                var optionOperadores = $('select#cmbOperadores').find(":selected");
                var valueSelected = optionOperadores.val();
                var padre = elt.closest('.kpi-new-filter');

                if ((valueSelected == 'Lista') || (valueSelected == 'NoLista')) {
                    var optionDimensions = $('select#cmbDimensiones').find(":selected");                   
                    padre.find('.filterList').kpilistfilters({ dimensionid: optionDimensions.val() });
                    padre.find('.filterText').find('.filterInput').addClass('hidden');
                    padre.find('.filterList').removeClass('hidden');
                }
                else {
                    padre.find('.filterText').find('.filterInput').removeClass('hidden');
                    padre.find('.filterList').addClass('hidden');
                }
            }
            });

            //Añadimos el cuadro de texto con el valor del filtro
            this._divTextbox = $('<div>').addClass('form-group filterInput').appendTo(this._divFilterText);
            $('<label>').addClass('sr-only').appendTo(this._divTextbox);
            this._filterText = $('<input />', { 'type': 'text', 'class': 'form-control margin-right-10' }).appendTo(this._divTextbox);

            this._divFilterList = $('<div>').addClass('form filterList').appendTo(this.element);
            this._divFilterList.kpilistfilters({ filterid: -1 }).addClass('hidden');

            this._btnaccept = $('<a>').addClass('btn btn-default').html('<i class="fa fa-check"></i>').appendTo(this._divFilterText);
            this._btnaccept.on({
                'click': function (event) {
                    var elt = $(event.currentTarget);
                    var valores = [];
                    var optionDimensions = $('select#cmbDimensiones').find(":selected");
                    var optionOperadores = $('select#cmbOperadores').find(":selected");
                    var valueSelected = optionOperadores.val();
                    if ((valueSelected == 'Lista') || (valueSelected == 'NoLista')) {
                        elt.parents('.kpi-new-filter').find('select.filter-select').find('option').each(function (i, selected) {
                            valores.push($.trim($(selected).text()));
                        });
                    }
                    else {
                        valores.push(elt.parent().find('input').val());
                    }
                    self._trigger('aceptar', event, { idfilter: -1, dimid: optionDimensions.val(), dimop: optionOperadores.val(), value: valores });
                }
            });
        },

        _destroy: function () {
            this.element.fadeOut('slow');
            this.element.removeClass('kpi-filter-panel');
            this.element.empty();
        }

    });
})(jQuery);