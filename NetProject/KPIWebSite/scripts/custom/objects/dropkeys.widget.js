(function ($) {

    $.widget('dropkeys.widgetItem', {

        // opciones predeterminadas
        options: {
            data: '',
            'style': 'widget-menu-item'
        },

        _create: function () {
            var self = this,
                opciones = this.options;

            this._link = $('<a>').attr('href', '#').attr('class', opciones.style)
                                     .attr('title', 'Dropkeys widget')
                                     .data('idwidget', opciones.data.idwidget)
                                     .on({
                                         'click': function (event) {
                                             var elt = $(event.currentTarget),
                                             id = elt.data('idwidget');
                                             self._trigger('onchange', event, id);
                                         }
                                     }).appendTo(this.element);
            $('<span>').addClass('menu-item-parent max-width-dashboard').html(opciones.data.titulo).appendTo(this._link);
            $('<span>').addClass('badge pull-right inbox-badge bg-color-pinkRed').html('new').appendTo(this._link);
        },

        _destroy: function () {
            this.element.empty();
        }

    });
})(jQuery);