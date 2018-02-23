(function ($) {

    $.widget('dropkeys.kpimessage', {

        // opciones predeterminadas
        options: {
            icontext:'Warning',
            message: 'Message of warning',
            style: 'alert alert-warning fade in margin-top-10'
        },

        hide: function () {
            this.element.addClass('hidden');
        },

        _changeIconText: function () {
            var self = this,
                opciones = this.options;

            this._iconText.html('&nbsp;' + opciones.icontext + '&nbsp;');
        },

        _changeMessage: function () {
            var self = this,
                opciones = this.options;

            this.element.removeClass('hidden');
            this._message.html(opciones.message);
        },

        _changeStyle: function () {
            var self = this,
                opciones = this.options;

            this.element.removeClass();
            this.element.addClass(opciones.style);
        },

        _create: function () {
            var self = this,
                opciones = this.options;

            //Global container
            this.element.addClass(opciones.style);
            this.element.addClass('hidden');
            //Close button
            //this._button = $('<button>').addClass('close').attr('data-dismiss', 'alert').html('×').appendTo(this.element);

            //Icon image
            this._iconImage = $('<i>').addClass('fa-fw fa fa-warning').appendTo(this.element);

            //Icon text
            this._iconText = $('<strong>').html('&nbsp;' + opciones.icontext + '&nbsp;').appendTo(this.element);

            //Text message
            this._message = $('<span>').html(opciones.message).appendTo(this.element);
        },

        _destroy: function () {
            var self = this,
                opciones = this.options;

            self.element.removeClass(opciones.style);
            self.element.empty();
        },

        _setOption: function (key, value) {
            var self = this,
                opciones = this.options;

            switch (key) {
                case "estilo":
                    opciones.style = value;
                    this._changeStyle();
                    break;
                case "texto":
                    opciones.icontext = value;
                    this._changeIconText();
                    break;
                case "message":
                    opciones.message = value;
                    this._changeMessage();
                    break;
            }
        }
    });
})(jQuery);