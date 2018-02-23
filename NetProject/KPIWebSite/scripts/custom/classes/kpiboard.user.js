var userObject = (function ($) {

    //Variables privates
    var _userid,
        _indicatorid,
        _nombre = '',
        _apellidos = '',
        _email = '',
        _pageSize = 10,
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

        // Consulta
        select: function (funcOK, funcFALSE) {
            var dataString = "{userid : " + _userid + "}";
            proxy.selUser(dataString, funcOK, funcFALSE);
        },

        // Cuenta
        count: function () {
            var dataString = "{indicatorid : " + _indicatorid + ", nombre : '" + _nombre + "'}";
            proxy.countUsers(dataString, _setTotalPages, null);
        },

        // Login
        loginUsingFacebook: function (funcOK, funcFALSE) {
            var dataString = "{userid : '" + _userid + "', email : '" + _email + "', nombre : '" + _nombre + "', apellidos : '" + _apellidos + "'}";
            proxy.loginFacebook(dataString, funcOK, funcFALSE);
        },

        // Populate
        populate: function (funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + ", nombre : '" + _nombre + "', pageSize : " + _pageSize + ", currentPage : " + _currentPage + ", orderby : '" + _orderby + "'}";
            proxy.getUsers(dataString, funcOK, funcFALSE);
        },

        // Asignar una etiqueta
        changeImage: function (pic, funcOK, funcFALSE) {
            var dataString = "{imageData:'" + pic + "'}";
            proxy.changeProfileImage(dataString, funcOK, funcFALSE);
        },

        // Asignar una etiqueta
        changePassword: function (oldpassword, newpassword, funcOK, funcFALSE) {
            var dataString = "{oldpwd:'" + oldpassword + "', newpdw : '" + newpassword + "'}";
            proxy.changeUserPassword(dataString, funcOK, funcFALSE);
        },

        setUserId: function (data) {
            _userid = data;
        },

        setIndicatorId: function (data) {
            _indicatorid = data;
        },

        setNombre: function (data) {
            _nombre = data;
        },

        setApellidos: function (data) {
            _apellidos = data;
        },

        setEmail: function (data) {
            _email = data;
        },

        setCurrentPage: function (data) {
            _currentPage = data;
        },

        setPageSize: function (data) {
            _pageSize = data;
        },

        setOrderBy: function (data) {
            _orderby = data;
        },

        getTotalPages: function () {
            return _totalPages;
        },

        getTotalItems: function () {
            return _totalItems;
        }
    };
} (jQuery));
