(function ($) {

    $.widget('dropkeys.userItem', {

        // opciones predeterminadas
        options: {
            'datos': [],
            'style': 'search liuser'
        },

        addItem: function (elem) {
            var self = this,
                opciones = this.options;

            this._liElement = $('<div>').addClass(opciones.style).data('userid', elem.userid).appendTo(this.element);

            //Primera Columna
            this._colOne = $('<div>').addClass('col-md-1 no-padding-left no-margin-left no-padding-right no-margin-right').appendTo(this._liElement);
            this._imgContainer = $('<div>').addClass('friends-list').appendTo(this._colOne);
            $('<img>').attr('src', elem.imageurl).attr('alt', elem.nombre).attr('style', 'border-width:0px;').appendTo(this._imgContainer);

            //Segunda Columna
            this._colSecond = $('<div>').addClass('col-md-10').appendTo(this._liElement);
            this._userLink = $('<h6>').addClass('no-margin').appendTo(this._colSecond);
            $('<a>').attr('href', '#').addClass('show-modal-info-user').data('userid', elem.userid).on({
                'click': function (event) {
                    var elt = $(event.currentTarget),
                        iduser = elt.data('userid');
                    self._trigger('view', event, { userid: iduser });
                }
            }).html(elem.nombre + ', ' + elem.apellidos).appendTo(this._userLink);

            this._iconsDiv = $('<div>').addClass("iconos hidden-xs")
                                      .appendTo(this._colSecond);
            $('<span>').attr('title', 'Number of datasets').append('<i class="fa fa-dot-circle-o"></i>&nbsp;' + elem.num_datasets + '&nbsp;datasets').appendTo(this._iconsDiv);
            this._iconsDiv.append('&nbsp;&nbsp;&nbsp;&nbsp;');
            $('<span>').attr('title', 'Number of data values').append('<i class="fa fa-cube"></i>&nbsp;' + elem.num_datos + '&nbsp;data values').appendTo(this._iconsDiv);
 
        },

        _create: function () {
            var self = this,
                opciones = this.options;

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