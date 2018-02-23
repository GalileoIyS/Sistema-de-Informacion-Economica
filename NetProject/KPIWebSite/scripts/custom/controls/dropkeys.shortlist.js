(function ($) {

    var _ulList, _divSearch;

    $.widget('dropkeys.shortlist', {

        // opciones predeterminadas
        options: {
            'datos': [],
            'styleContainer': 'dropkeys-shortlist-container',
            'styleSearch': 'dropkeys-shortlist-search',
            'styleElem': '',
            'txtValue': '',
            'txtBadge': '',
            'notFound':'-- No matches found --'
        },

        addItem: function (elem) {
            var self = this,
                opciones = this.options;

            this._liElement = $('<li>').addClass(opciones.styleElem).data('id', elem.id).appendTo(this._ulList);
            this._Link = $('<a>').data('id', elem.id).attr('href', '#').on({
                'click': function (event) {
                    var elt = $(event.currentTarget),
                        id = elt.data('id');
                    self._trigger('onClick', event, id);
                }
            }).appendTo(this._liElement);

            if (elem.imageurl)
                $('<img>').attr('src', elem.imageurl).appendTo(this._Link);
            $('<span>').html(elem.name).appendTo(this._Link);
            if (elem.value)
                $('<span>').addClass('badge bg-color-blueMedium pull-right').attr('title', opciones.txtBadge).html(elem.value).appendTo(this._Link);
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

            //rellenamos los elementos
            this._divContainer = $('<div>').addClass(opciones.styleContainer).appendTo(this.element);
            this._ulList = $('<ul>').addClass('dropkeys-shortlist').appendTo(this._divContainer);

            if (data) {
                $.each(data, function (i, item) {
                    self.addItem(item);
                })
            }

            //rellenamos el buscador
            this._divSearch = $('<div>').addClass(opciones.styleSearch).appendTo(this.element);
            this._addSearch();
        },

        _addSearch: function (data) {
            var self = this,
            opciones = this.options;

            this._divGroup = $('<div>').addClass('control-group').appendTo(this._divSearch);
            this._divForm = $('<div>').addClass('smart-form').appendTo(this._divGroup);
            this._divLabel = $('<label>').addClass('input').appendTo(this._divForm);
            this._InputSearch = $('<input>').attr('type', 'text').attr('placeholder', 'Search...').attr('value', opciones.txtValue).on({
                'keyup': function (event) {
                    if (event.which == 13) {
                        var elt = $(event.currentTarget),
                            txtFilter = elt.val();

                        //Prevents the default action to be triggered. 
                        event.preventDefault();

                        if (txtFilter == '') 
                            self._trigger('onEnter', event, '%');
                        else
                            self._trigger('onEnter', event, txtFilter);
                    }
                }
            }).appendTo(this._divLabel);
        },

        _setOption: function (key, value) {
            switch (key) {
                case "datos":
                    this._populate(value);
                    break;
                case "txtValue":
                    if (value != '%')
                        this._InputSearch.val(value);
                    else
                        this._InputSearch.val('');
                    break;
            }
        },

        _destroy: function () {
            this.element.empty();
        }

    });
})(jQuery);