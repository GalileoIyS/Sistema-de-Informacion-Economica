(function ($) {

    $.widget('dropkeys.kpieditfilter', {

        // opciones predeterminadas
        options: {
            data: '',
            cmbDim: [],
            cmbOpe: [],
            'style': 'kpi-filter-panel'
        },

        _create: function () {
            var self = this,
                opciones = this.options;

            //Global container
            this.element.addClass(opciones.style).data('filtroid', opciones.data.filterid);

            //Show container
            this._show = $('<div>').attr('class', 'alert alert-info show-panel-filter')
                                   .data('filtroid', opciones.data.filterid)
                                   .appendTo(this.element);

            //Edit container
            this._edit = $('<div>').attr('class', 'well bg-color-blueLight text-center edit-panel-filter hidden')
                                   .data('filtroid', opciones.data.filterid)
                                   .appendTo(this.element);

            //Show Buttons
            this._btnedit = $('<a>').addClass('kpi-edit-filter txt-color-blueMarine').attr('href', '#').data('filtroid', opciones.data.filterid).html('<i class="fa-fw fa fa-pencil"></i>').appendTo(this._show);
            this._btnedit.on({
                'click': function (event) {
                    var elt = $(event.currentTarget),
                        idfiltro = elt.data('filtroid');
                    elt.parents('.kpi-filter-panel').find('.show-panel-filter').addClass('hidden');
                    elt.parents('.kpi-filter-panel').find('.edit-panel-filter').removeClass('hidden');
                    self._trigger('editar', event, { idfilter: idfiltro });
                }
            });

            this._btndelete = $('<a>').addClass('kpi-delete-filter txt-color-blueMarine').attr('href', '#').data('filtroid', opciones.data.filterid).html('<i class="fa-fw fa fa-times"></i>').appendTo(this._show);
            this._btndelete.on({
                'click': function (event) {
                    var elt = $(event.currentTarget),
                        idfiltro = elt.data('filtroid');
                    elt.parents('.kpi-filter-panel').fadeToggle();
                    self._trigger('eliminar', event, { idfilter: idfiltro });
                }
            });

            this._divFilterText = $('<div>').addClass('form-inline filterText').appendTo(this._edit);

            //Añadimos el cuadro de selección de características
            this._divAttributes = $('<div>').addClass('form-group').appendTo(this._divFilterText);
            $('<label>').addClass('sr-only').appendTo(this._divAttributes);
            this._cmbDimensiones = $('<select />').addClass('lst-dimensiones form-control margin-right-10').kpicombo({ datos: opciones.cmbDim }).appendTo(this._divAttributes);
            this._cmbDimensiones.val(opciones.data.dimensionid);
            this._cmbDimensiones.on({
                'change': function (event) {
                    var elt = $(event.currentTarget);
                    var optionOperadores = elt.parents('.filterText').find('select.lst-operadores').find(":selected");
                    var valueSelected = optionOperadores.val();
                    if ((valueSelected == 'Lista') || (valueSelected == 'NoLista')) {
                        var optionDimensions = elt.parents('.form-inline').find('select.lst-dimensiones').find(":selected");
                        elt.parents('.edit-panel-filter').find('.filterList').kpilistfilters({ dimensionid: optionDimensions.val() });
                    }
                }
            });

            //Añadimos el cuadro de selección de operadores
            this._divOperadores = $('<div>').addClass('form-group').appendTo(this._divFilterText);
            $('<label>').addClass('sr-only').appendTo(this._divOperadores);
            this._cmbOperadores = $('<select />').addClass('lst-operadores form-control margin-right-10').kpicombo({ datos: opciones.cmbOpe }).appendTo(this._divOperadores);
            this._cmbOperadores.val(opciones.data.filtro);
            this._cmbOperadores.on({
                'change': function (event) {
                    var elt = $(event.currentTarget);
                    var optionOperadores = elt.parent().find('select.lst-operadores').find(":selected");
                    var valueSelected = optionOperadores.val();
                    if ((valueSelected == 'Lista') || (valueSelected == 'NoLista')) {
                        var optionDimensions = elt.parents('.form-inline').find('select.lst-dimensiones').find(":selected");
                        elt.parents('.edit-panel-filter').find('.filterList').kpilistfilters({ dimensionid: optionDimensions.val() });
                        elt.parents('.edit-panel-filter').find('.filterText').find('.filterInput').addClass('hidden');
                        elt.parents('.edit-panel-filter').find('.filterList').removeClass('hidden');
                    }
                    else {
                        elt.parents('.edit-panel-filter').find('.filterText').find('.filterInput').removeClass('hidden');
                        elt.parents('.edit-panel-filter').find('.filterList').addClass('hidden');
                    }
                }
            });

            this._show.append($('<span />', {
                "class": "kpi-filter-dim",
                "text": this._cmbDimensiones.find(":selected").text()
            })).append($('<span />', {
                "class": "kpi-filter-oper",
                "text": this._cmbOperadores.find(":selected").text()
            })).append($('<span />', {
                "class": "kpi-filter-val",
                "text": opciones.data.valor
            }));

            //Añadimos el cuadro de texto con el valor del filtro
            this._divTextbox = $('<div>').addClass('form-group filterInput').appendTo(this._divFilterText);
            $('<label>').addClass('sr-only').appendTo(this._divTextbox);
            this._filterText = $('<input />', { 'type': 'text', 'value': opciones.data.valor, 'class': 'kpi-filter-text form-control margin-right-10' }).appendTo(this._divTextbox);

            this._divFilterList = $('<div>').addClass('form filterList').appendTo(this._edit);
            this._divFilterList.kpilistfilters({ filterid: opciones.data.filterid }).addClass('hidden');

            this._btncancel = $('<a>').addClass('btn btn-default').attr('href', '#').data('filtroid', opciones.data.filterid).html('<i class="fa-fw fa fa-undo"></i>').appendTo(this._divFilterText);
            this._btncancel.on({
                'click': function (event) {
                    var elt = $(event.currentTarget),
                        idfiltro = elt.data('filtroid');
                    elt.parents('.kpi-filter-panel').find('.show-panel-filter').removeClass('hidden');
                    elt.parents('.kpi-filter-panel').find('.edit-panel-filter').addClass('hidden');
                    self._trigger('cancelar', event, { idfilter: idfiltro });
                }
            });

            this._btnaccept = $('<a>').addClass('btn btn-default').attr('href', '#').data('filtroid', opciones.data.filterid).html('<i class="fa-fw fa fa-check"></i>').appendTo(this._divFilterText);
            this._btnaccept.on({
                'click': function (event) {
                    var elt = $(event.currentTarget),
                        idfiltro = elt.data('filtroid');
                    elt.parents('.kpi-filter-panel').find('.show-panel-filter').removeClass('hidden');
                    elt.parents('.kpi-filter-panel').find('.edit-panel-filter').addClass('hidden');
                    var coma = '';
                    var valores = [];
                    var txtvalores = '';
                    var optionDimensions = elt.parents('.edit-panel-filter').find('select.lst-dimensiones').find(":selected");
                    var optionOperadores = elt.parents('.edit-panel-filter').find('select.lst-operadores').find(":selected");
                    var valueSelected = optionOperadores.val();
                    if ((valueSelected == 'Lista') || (valueSelected == 'NoLista')) {
                        elt.parents('.edit-panel-filter').find('select.filter-select').find('option').each(function (i, selected) {
                            valores.push($.trim($(selected).text()));
                            txtvalores = txtvalores + coma + $.trim($(selected).text());
                            coma = ', ';
                        });
                    }
                    else {
                        valores.push(elt.parents('.filterText').find('.filterInput').find('input').val());
                        txtvalores = elt.parents('.filterText').find('.filterInput').find('input').val();
                    }
                    elt.parents('.kpi-filter-panel').find('span.kpi-filter-dim').text(optionDimensions.text());
                    elt.parents('.kpi-filter-panel').find('span.kpi-filter-oper').text(optionOperadores.text());
                    elt.parents('.kpi-filter-panel').find('span.kpi-filter-val').text(txtvalores);
                    self._trigger('aceptar', event, { idfilter: idfiltro, dimid: optionDimensions.val(), dimop: optionOperadores.val(), value: valores });
                }
            });

            if ((opciones.data.filtro == 'Lista') || (opciones.data.filtro == 'NoLista')) {
                this._divTextbox.addClass('hidden');
                this._divFilterList.removeClass('hidden');
                this._divFilterList.kpilistfilters({
                    dimensionid: opciones.data.dimensionid,
                    filterid: opciones.data.filterid
                });
            }
            else {
                this._divTextbox.removeClass('hidden');
                this._divFilterList.addClass('hidden');
            }
        },

        _setOption: function (key, value) {
            switch (key) {
                case "data":
                    this.options.data = value;
                    this._create();
                    break;
                case "cmbDim":
                    this.options.cmbDim = value;
                    break;
                case "cmbOpe":
                    this.options.cmbOpe = value;
                    break;
            }
        },

        _destroy: function () {
            this.element.empty();
        }

    });
})(jQuery);