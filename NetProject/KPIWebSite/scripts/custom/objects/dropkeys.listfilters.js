(function ($) {

    $.widget('dropkeys.kpilistfilters', {

        // opciones predeterminadas
        options: {
            dimensionid: undefined,
            filterid: undefined,
            size: 5
        },

        _populateAll: function () {
            var self = this,
                opciones = this.options;

            var lista = self.element.find('.filter-all');
            $.fn.returnOK = function (result) {
                lista.empty();
                $.each(result, function (i, item) {
                    lista.append("<option value='" + item.codigo + "'> " + item.codigo + " </option>");
                });
            };
            $.fn.returnNO = function (response) {
                alert('Algo salió mal');
            };
            dimensionvaluesObject.setDimensionId(opciones.dimensionid);
            dimensionvaluesObject.notAsigned(opciones.filterid, $(this).returnOK, $(this).returnNO);
        },

        _populateAsigned: function () {
            var self = this,
                opciones = this.options;

            var lista = self.element.find('.filter-select');
            $.fn.returnOK = function (result) {
                lista.empty();
                $.each(result, function (i, item) {
                    lista.append("<option value='" + item.codigo + "'> " + item.codigo + " </option>");
                });
            };
            $.fn.returnNO = function (response) {
                alert('Algo salió mal');
            };
            dimensionvaluesObject.setDimensionId(opciones.dimensionid);
            dimensionvaluesObject.asigned(opciones.filterid, $(this).returnOK, $(this).returnNO);
        },

        _create: function () {
            var self = this,
                opciones = this.options;

            //Show container
            this._divRow = $('<div>').addClass('row margin-top-10').appendTo(this.element);

            //First column
            this._divColumnOne = $('<div>').addClass('col col-5 col-lg-5').appendTo(this._divRow);
            this._divGroup11 = $('<div>').addClass('form-group').appendTo(this._divColumnOne);
            this._divIcon11 = $('<div>').addClass('icon-addon addon-sm').appendTo(this._divGroup11);
            $('<input>').attr('placeholder', 'Search...').addClass('form-control').attr('type', 'text').appendTo(this._divIcon11);
            $('<label>').addClass('glyphicon glyphicon-search').appendTo(this._divIcon11);
            this._divGroup12 = $('<div>').addClass('form-group').appendTo(this._divColumnOne);
            this._filterAll = $('<select/>').addClass('filter-all form-control custom-scroll').attr('size', opciones.size).prop('multiple', 'multiple').appendTo(this._divGroup12);

            //Second column
            this._divColumnSecond = $('<div>').addClass('col col-2 col-lg-2 text-center').appendTo(this._divRow);
            this._divButton1 = $('<div>').addClass('margin-top-5').appendTo(this._divColumnSecond);
            this._btnAddAll = $('<a>').addClass('btn btn-default kpi-list-filter-addall').html('<i class="fa fa-angle-double-right"></i>').appendTo(this._divButton1);
            this._btnAddAll.on({
                'click': function (event) {
                    var elt = $(event.currentTarget),
                        selectedItem = elt.parents('.filterList').find('select.filter-all').find("option");
                    elt.parents('.filterList').find('select.filter-select').append(selectedItem);
                }
            });
            this._divButton2 = $('<div>').addClass('margin-top-5').appendTo(this._divColumnSecond);
            this._btnAdd = $('<a>').addClass('btn btn-default kpi-list-filter-add').html('<i class="fa fa-angle-right"></i>').appendTo(this._divButton2);
            this._btnAdd.on({
                'click': function (event) {
                    var elt = $(event.currentTarget),
                        selectedItem = elt.parents('.filterList').find('select.filter-all').find(":selected");
                    elt.parents('.filterList').find('select.filter-select').append(selectedItem);
                }
            });
            this._divButton3 = $('<div>').addClass('margin-top-5').appendTo(this._divColumnSecond);
            this._btnRemove = $('<a>').addClass('btn btn-default kpi-list-filter-remove').html('<i class="fa fa-angle-left"></i>').appendTo(this._divButton3);
            this._btnRemove.on({
                'click': function (event) {
                    var elt = $(event.currentTarget),
                        selectedItem = elt.parents('.filterList').find('select.filter-select').find(":selected");
                    elt.parents('.filterList').find('select.filter-all').append(selectedItem);
                }
            });
            this._divButton4 = $('<div>').addClass('margin-top-5').appendTo(this._divColumnSecond);
            this._btnRemoveAll = $('<a>').addClass('btn btn-default kpi-list-filter-removeall').html('<i class="fa fa-angle-double-left"></i>').appendTo(this._divButton4);
            this._btnRemoveAll.on({
                'click': function (event) {
                    var elt = $(event.currentTarget),
                        selectedItem = elt.parents('.filterList').find('select.filter-select').find("option");
                    elt.parents('.filterList').find('select.filter-all').append(selectedItem);
                }
            });

            //Third column
            this._divColumnThird = $('<div>').addClass('col col-5 col-lg-5').appendTo(this._divRow);
            this._divGroup31 = $('<div>').addClass('form-group').appendTo(this._divColumnThird);
            this._divIcon31 = $('<div>').addClass('icon-addon addon-sm').appendTo(this._divGroup31);
            $('<input>').attr('placeholder', 'Search...').addClass('form-control').attr('type', 'text').appendTo(this._divIcon31);
            $('<label>').addClass('glyphicon glyphicon-search').appendTo(this._divIcon31);

            this._divGroup32 = $('<div>').addClass('form-group').appendTo(this._divColumnThird);
            this._filterAll = $('<select/>').addClass('filter-select form-control custom-scroll').attr('size', opciones.size).prop('multiple', 'multiple').appendTo(this._divGroup32);

            //this._divRow2 = $('<div>').addClass('row').appendTo(this._divList);
            //this._divColumnEnd = $('<div>').addClass('col col-12 col-lg-12 margin-top-5').appendTo(this._divRow2);
            //this._btnaccept = $('<a>').addClass('btn btn-default pull-right').html('<i class="fa fa-check"></i>').appendTo(this._divColumnEnd);
            //this._btnaccept.on({
            //    'click': function (event) {
            //        var elt = $(event.currentTarget);
            //        var valores = [];
            //        var optionDimensions = $('select#cmbDimensiones').find(":selected");
            //        var optionOperadores = $('select#cmbOperadores').find(":selected");
            //        var valueSelected = optionOperadores.val();
            //        alert('ES LISTA');
            //        elt.parents('.filterList').find('select.filter-select').find('option').each(function (i, selected) {
            //            alert($(selected).text());
            //            valores.push($.trim($(selected).text()));
            //        });
            //        self._trigger('aceptarlista', event, { idfilter: -1, dimid: optionDimensions.val(), dimop: optionOperadores.val(), value: valores });
            //    }
            //});
        },

        _setOption: function (key, value) {
            switch (key) {
                case "filterid":
                    this.options.filterid = value;
                    break;
                case "dimensionid":
                    this.options.dimensionid = value;
                    this._populateAll();
                    this._populateAsigned();
                    break;
            }
        },

        _destroy: function () {
            this.element.empty();
        }

    });
})(jQuery);