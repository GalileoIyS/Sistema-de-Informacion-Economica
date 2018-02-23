var subcategoryObject = (function ($) {

    //Variables privates
    var _idsubcategory,
        _idcategory,
        _nombre,  
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

        // Consulta
        populate: function (funcOK, funcFALSE) {
            var dataString = "{categoryid : " + _idcategory + ", filterBy : '" + _filter + "', pageSize : " + _pageSize + ", currentPage : " + _currentPage + ", orderby : '" + _orderby + "'}";
            proxy.getSubcategories(dataString, funcOK, funcFALSE);
        },

        setIdSubcategory: function (data) {
            _idsubcategory = data;
        },

        setIdCategory: function (data) {
            _idcategory = data;
        },

        setNombre: function (data) {
            _nombre = data;
        },

        setCurrentPage: function (data) {
            _currentPage = data;
        },

        setFilter: function (data) {
            _filter = data;
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
        },

        getPageSize: function () {
            return _pageSize;
        },

        getCurrentPage: function () {
            return _currentPage;
        }
    };
} (jQuery));
