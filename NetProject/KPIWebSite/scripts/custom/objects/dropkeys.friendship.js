(function ($) {

    var _liFriends, _liMessages;

    $.widget('dropkeys.friendship', {

        // opciones predeterminadas
        options: {
            'datos': [],
            'stylefriends': 'notification-body friend-list-items',
            'stylemessages': 'notification-body message-list-items'
        },

        showFriends: function () {
            this._liFriends.removeClass('hidden');
            this._liMessages.addClass('hidden');
        },

        countFriends: function () {
            return (this._liFriends.children("li").length);
        },

        showMessages: function () {
            this._liFriends.addClass('hidden');
            this._liMessages.removeClass('hidden');
        },

        countMessages: function () {
            return (this._liMessages.children("li").length);
        },

        addItem: function (elem) {
            var self = this,
                opciones = this.options;
            this._liElement = $('<li>').addClass('friend-item').data('myuserid', elem.myuserid).data('userid', elem.userid).appendTo(this.element);
            //Primera Columna
            this._spanRow = $('<span>').addClass('unread').appendTo(this._liElement);
            //this._Link = $('<a>').attr('href', '#').attr('title', 'Edit the dataset and its values').addClass('msg').appendTo(this._spanRow);
            this._Link = $('<div>').addClass('msg').appendTo(this._spanRow);
            $('<img>').addClass('air air-top-left margin-top-5').attr('src', elem.imageurl).attr('width', '40').attr('height', '40').appendTo(this._Link);
            $('<span>').addClass('from').html(elem.nombre + ' ' + elem.apellidos + '<i class="icon-paperclip"></i>').appendTo(this._Link);
            $('<time>').html('<span data-livestamp="' + elem.fecha + '"></span>').appendTo(this._Link);

            if (elem.relation === 1) {
                $('<a>').attr('href', '#').addClass('btn btn-danger btn-xs btn-notifications').data('myuserid', elem.myuserid).data('userid', elem.userid).html('<i class="fa fa-times"></i>&nbsp;Cancel').on({
                    'click': function (event) {
                        var elt = $(event.currentTarget),
                            myiduser = elt.data('myuserid'),
                            iduser = elt.data('userid');
                        self._trigger('cancel', event, { userid: iduser, myuserid: myiduser });
                    }
                }).appendTo(this._Link);
            }
            else {
                $('<a>').attr('href', '#').addClass('btn btn-success btn-xs btn-notifications').data('myuserid', elem.myuserid).data('userid', elem.userid).html('<i class="fa fa-check"></i>&nbspAccept').on({
                    'click': function (event) {
                        var elt = $(event.currentTarget),
                            myiduser = elt.data('myuserid'),
                            iduser = elt.data('userid');
                        self._trigger('accept', event, { userid: iduser, myuserid: myiduser });
                    }
                }).appendTo(this._Link);
                $('<a>').attr('href', '#').addClass('btn bg-color-blueLight btn-xs btn-notifications').data('myuserid', elem.myuserid).data('userid', elem.userid).html('<i class="fa fa-times"></i>&nbsp;Reject').on({
                    'click': function (event) {
                        var elt = $(event.currentTarget),
                            myiduser = elt.data('myuserid'),
                            iduser = elt.data('userid');
                        self._trigger('reject', event, { userid: iduser, myuserid: myiduser });
                    }
                }).appendTo(this._Link);
            }

            this._liFriends.append(this._liElement);
        },

        _create: function () {
            var self = this,
                opciones = this.options;

            this._populate(opciones.datos);
        },

        _populate: function (data) {
            var self = this,
            opciones = this.options;

            //limpiamos la lista
            self.element.empty();

            this._liFriends = $('<ul>').addClass(opciones.stylefriends).appendTo(this.element);
            this._liMessages = $('<ul>').addClass(opciones.stylemessages).appendTo(this.element);

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