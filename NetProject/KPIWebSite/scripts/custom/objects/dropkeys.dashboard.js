(function ($) {

    $.widget('dropkeys.dashboardItem', {

        // opciones predeterminadas
        options: {
            data: '',
            'style': 'dashboards-items'
        },

        _create: function () {
            var self = this,
                opciones = this.options;

            this.element.addClass(opciones.style).data('iddashboard', opciones.data.dashboardid);

            this._link = $('<a>').attr('href', '#').addClass('dashboard-menu-item')
                                     .attr('title', 'Dropkeys dashboard')
                                    .appendTo(this.element);
            $('<i>').addClass('fa fa-lg fa-fw fa-bar-chart-o').appendTo(this._link);
            $('<span>').addClass('menu-item-parent max-width-dashboard').html(opciones.data.titulo).appendTo(this._link);

            
            this._list = $('<ul>').addClass('widget-list').data('iddashboard',opciones.data.dashboardid).appendTo(this.element);
            this._liToolbar = $('<li>').appendTo(this._list);
            $('<a>').attr('href', '#').addClass('txt-color-blue').html('<i class="fa fa-sitemap"></i>&nbsp;Dashboard options<b class="collapse-sign"><em class="fa fa-plus-square-o"></em></b>').appendTo(this._liToolbar);

            this._sublist = $('<ul>').appendTo(this._liToolbar);

            this._liEdit = $('<li>').appendTo(this._sublist);
            this._linkEdit = $('<a>').attr('href', '#').addClass('edit-dashboard')
                                     .attr('title', 'Change dashboard name').data('iddashboard', opciones.data.dashboardid).on({
                                         'click': function (event) {
                                             var elt = $(event.currentTarget),
                                                 dashboardid = elt.data('iddashboard');
                                             self._trigger('editar', event, dashboardid);
                                         }
                                     }).appendTo(this._liEdit);
            $('<i>').addClass('fa fa-lg fa-fw fa-pencil txt-color-blue').appendTo(this._linkEdit);
            $('<span>').addClass('menu-item-parent max-width-dashboard').html('Rename dashboard').appendTo(this._linkEdit);

            this._liDelete = $('<li>').appendTo(this._sublist);
            this._linkDelete = $('<a>').attr('href', '#').addClass('delete-dashboard')
                                     .attr('title', 'Delete current dashboard').data('iddashboard', opciones.data.dashboardid).on({
                                         'click': function (event) {
                                             var elt = $(event.currentTarget),
                                                 dashboardid = elt.data('iddashboard');
                                             self._trigger('eliminar', event, dashboardid);
                                         }
                                     }).appendTo(this._liDelete);
            $('<i>').addClass('fa fa-lg fa-fw fa-times txt-color-redLight').appendTo(this._linkDelete);
            $('<span>').addClass('menu-item-parent max-width-dashboard').html('Delete dashboard').appendTo(this._linkDelete);

            this._liInsert = $('<li>').appendTo(this._sublist);
            this._linkInsert = $('<a>').attr('href', '#').addClass('insert-widget')
                                     .attr('title', 'Insert new graph').data('iddashboard', opciones.data.dashboardid).on({
                                         'click': function (event) {
                                             var elt = $(event.currentTarget),
                                                 dashboardid = elt.data('iddashboard');
                                             self._trigger('insertar', event, dashboardid);
                                         }
                                     }).appendTo(this._liDelete);
            $('<i>').addClass('fa fa-lg fa-fw fa-plus txt-color-greenLight').appendTo(this._linkInsert);
            $('<span>').addClass('menu-item-parent max-width-dashboard').html('Insert graph').appendTo(this._linkInsert);
        },

        _destroy: function () {
            this.element.empty();
        }

    });
})(jQuery);