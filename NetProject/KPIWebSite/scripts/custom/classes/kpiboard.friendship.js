var friendObject = (function ($) {

    //Variables privates
    var _touserid,
        _fromuserid,
        _aceptado = '',
        _filter = '',
        _pageSize = 15,
        _currentPage = 1,
        _totalPages = 0,
        _totalItems = 0,
        _orderby = '';

    function _setTotalPages(response) {
        _totalItems = response;
        _totalPages = Math.floor((response + _pageSize - 1) / _pageSize);
    }

    // Return an object exposed to the public
    return {

        // Peticion de amistad
        insert: function (funcOK, funcFALSE) {
            var dataString = "{touserid : " + _touserid + "}";
            proxy.addFriend(dataString, funcOK, funcFALSE);
        },

        // Aceptacion de amistad
        accept: function (funcOK, funcFALSE) {
            var dataString = "{fromuserid : " + _fromuserid + "}";
            proxy.acceptFriend(dataString, funcOK, funcFALSE);
        },

        //cancelamos la amistad
        remove: function (funcOK, funcFALSE) {
            var dataString = "{touserid : " + _touserid + "}";
            proxy.delFriend(dataString, funcOK, funcFALSE);
        },

        // Contamos las solicitudes de amistad
        count: function (funcOK, funcFALSE) {
            var dataString = "{aceptado : '" + _aceptado + "'}";
            proxy.countFriend(dataString, funcOK, funcFALSE);
        },

        //obtenemos las solicitudes pendientes
        populate: function (funcOK, funcFALSE) {
            var dataString = "{aceptado : '" + _aceptado + "', filterBy : '" + _filter + "', pageSize : " + _pageSize + ", currentPage : " + _currentPage + ", orderby : '" + _orderby + "'}";
            proxy.getFriends(dataString, funcOK, funcFALSE);
        },

        //obtenemos los amigos por indicador
        getFriendsList: function (indicatorid, funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + indicatorid + ", aceptado : '" + _aceptado + "', filterBy : '" + _filter + "', pageSize : " + _pageSize + ", currentPage : " + _currentPage + ", orderby : '" + _orderby + "'}";
            proxy.getFriendsList(dataString, funcOK, funcFALSE);
        },

        setToUserId: function (data) {
            _touserid = data;
        },

        setFromUserId: function (data) {
            _fromuserid = data;
        },

        setAceptado: function (data) {
            _aceptado = data;
        },

        setCurrentPage: function (data) {
            _currentPage = data;
        },

        setFilter: function (data) {
            _filter = data;
        },

        setOrderBy: function (data) {
            _orderby = data;
        },

        getTotalPages: function () {
            return _totalPages;
        },

        getTotalItems: function () {
            return _totalItems;
        },

        getPageSize: function () {
            return _pageSize;
        },

        getCurrentPage: function () {
            return _currentPage;
        }
    };
} (jQuery));
