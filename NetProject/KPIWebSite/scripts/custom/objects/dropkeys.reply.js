(function ($) {

    $.widget('dropkeys.replyItem', {

        // opciones predeterminadas
        options: {
            data: '',
            'style': 'message message-reply'
        },

        _create: function () {
            var self = this,
                opciones = this.options;

            this._liElement = $('<li>').addClass(opciones.style).appendTo(this.element);

            $('<img>').attr('src', opciones.data.imageurl).attr('alt', opciones.data.nombre).attr('style', 'width:50px;').appendTo(this._liElement);

            this._spanMessage = $('<span>').addClass('message-text').appendTo(this._liElement);

            $('<a>').addClass('username show-modal-info-user').attr('href', '#').data('userid', opciones.data.userid).html(opciones.data.nombre + '&nbsp;' + opciones.data.apellidos).appendTo(this._spanMessage);
            $('<div>').html(opciones.data.comentario).appendTo(this._spanMessage);

            this._listEnd = $('<ul>').addClass('list-inline font-xs').appendTo(this._liElement);
            $('<li>').html('Less than a minute').appendTo(this._listEnd);
        },

        _destroy: function () {
            this.element.empty();
        }

    });
})(jQuery);