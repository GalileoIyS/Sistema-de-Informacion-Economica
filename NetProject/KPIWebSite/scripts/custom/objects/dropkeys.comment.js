(function ($) {

    $.widget('dropkeys.commentItem', {

        // opciones predeterminadas
        options: {
            data: '',
            'style': 'message message-reply'
        },

        _create: function () {
            var self = this,
                opciones = this.options;

            this._divSeparator = $('<div>').addClass('timeline-seperator text-center').appendTo(this.element);
            $('<span>').html('Today').appendTo(this._divSeparator);

            this._divChatBody = $('<div>').addClass('chat-body no-padding profile-message').data('commentid', opciones.data.commentid).appendTo(this.element);
            this._ulElement = $('<ul>').appendTo(this._divChatBody);
            this._liElement = $('<li>').addClass('message').appendTo(this._ulElement);

            $('<img>').attr('src', opciones.data.imageurl).attr('alt', opciones.data.nombre).attr('style', 'width:50px;').appendTo(this._liElement);

            this._spanMessage = $('<span>').addClass('message-text').appendTo(this._liElement);
            $('<a>').addClass('username show-modal-info-user').attr('href', '#').data('userid', opciones.data.userid).html(opciones.data.nombre + '&nbsp;' + opciones.data.apellidos + '<small class="text-muted pull-right ultra-light">Less than a minute</small>').appendTo(this._spanMessage);
            $('<div>').html(opciones.data.comentario).appendTo(this._spanMessage);

            this._ulOptions = $('<ul>').addClass('list-inline font-xs').appendTo(this._liElement);
            this._liLike = $('<li>').appendTo(this._ulOptions);
            $('<a>').addClass('text-danger').attr('href', '#').html('<i class="fa fa-thumbs-up"></i>Like').appendTo(this._liLike);
            this._liEdit = $('<li>').appendTo(this._ulOptions);
            $('<a>').addClass('text-primary').attr('href', '#').html('Edit').appendTo(this._liEdit);
            this._liDelete = $('<li>').appendTo(this._ulOptions);
            $('<a>').addClass('text-danger').attr('href', '#').html('Delete').appendTo(this._liDelete);

        },

        _destroy: function () {
            this.element.empty();
        }

    });
})(jQuery);