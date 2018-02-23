var indicatorObject = (function ($) {

    //Variables privates
    var _indicatorid,
        _titulo = '',
        _categoryid = -1,
        _subcategoryid = -1,
        _imageurl = '',
        _color = '',
        _unidad = '',
        _simbolo = '',
        _funcion = '',
        _resumen = '',
        _pageSize = 15,
        _currentPage = 1,
        _totalPages = 0,
        _totalItems = 0,
        _orderby = '';

    function _setTotalPages(response) {
        _totalItems = response;
        _totalPages = Math.floor((response + _pageSize - 1) / _pageSize);
    }

    function leer(response) {
        _indicatorid = response.indicatorid;
        _titulo = response.titulo;
        _categoryid = response.categoryid;
        _subcategoryid = response.subcategoryid;
        _imageurl = response.imageurl;
    }

    // Return an object exposed to the public
    return {

        // Consulta
        select: function () {
            var dataString = "{indicatorid : " + _indicatorid + "}";
            proxy.getIndicator(dataString, leer, null);
        },

        // Inserta
        insert: function (attributes, funcOK, funcFALSE) {
            var dataString = "{titulo : '" + _titulo + "', resumen : '" + _resumen + "', unidad : '" + _unidad + "', simbolo : '" + _simbolo + "', funcion : '" + _funcion + "', atributos : '" + attributes + "'}";
            proxy.addIndicator(dataString, funcOK, funcFALSE);
        },

        //cancelamos la amistad
        remove: function (funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + "}";
            proxy.delIndicator(dataString, funcOK, funcFALSE);
        },

        // Asignar a un usuario
        addIndicatorUser: function (funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + "}";
            proxy.addIndicatorUser(dataString, funcOK, funcFALSE);
        },

        // Cambia el estado del usuario respecto a este indicador
        changeIndicatorUser: function (isanonymous, funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + ", isanonymous : '" + isanonymous + "'}";
            proxy.changeIndicatorUser(dataString, funcOK, funcFALSE);
        },

        // Compartir el indicador
        shareIndicator: function (nombre, funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + ", imageurl : '" + _imageurl + "', categoryid : " + _categoryid + ", subcategoryid : " + _subcategoryid + ", nombre : '" + nombre + "'}";
            proxy.shareIndicator(dataString, funcOK, funcFALSE);
        },

        // Votar
        vote: function (valor, funcOK, funcFALSE) {
            var dataString = "{indicatorid:" + _indicatorid + ", score:" + valor + "}";
            proxy.voteIndicator(dataString, funcOK, funcFALSE);
        },

        // Asignar una etiqueta
        addTag: function (tag, funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + ", etiqueta:'" + tag + "'}";
            proxy.addTag(dataString, funcOK, funcFALSE);
        },

        // Asignar una etiqueta
        changeImage: function (pic, funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + ", imageData:'" + pic + "'}";
            proxy.changeIndicatorImage(dataString, funcOK, funcFALSE);
        },

        // Eliminar una etiqueta
        delTag: function (tag, funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + ", etiqueta:'" + tag + "'}";
            proxy.delTag(dataString, funcOK, funcFALSE);
        },

        // Obtiene el número de usuarios, Datasets y Datos disponibles de un indicador
        getResumen: function (funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + "}";
            proxy.getIndicatorResume(dataString, funcOK, funcFALSE);
        },

        // Obtiene la gráfica lineal por ejercicios de los datos del indicador
        getLineChart: function (funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + "}";
            proxy.getIndicatorLineChart(dataString, funcOK, funcFALSE);
        },

        // Obtiene la gráfica de barras del número de datos por ejercicio del indicador
        getBarChart: function (funcOK, funcFALSE) {
            var dataString = "{indicatorid : " + _indicatorid + "}";
            proxy.getIndicatorBarChart(dataString, funcOK, funcFALSE);
        },

        // Cuenta
        count: function () {
            var dataString = "{titulo : '" + _titulo + "', categoryid : " + _categoryid + ", subcategoryid : " + _subcategoryid + "}";
            proxy.countIndicators(dataString, _setTotalPages, null);
        },

        // Populate
        populate: function (funcOK, funcFALSE) {
            var dataString = "{titulo : '" + _titulo + "', categoryid : " + _categoryid + ", subcategoryid : " + _subcategoryid + ", pageSize : " + _pageSize + ", currentPage : " + _currentPage + ", orderby : '" + _orderby + "'}";
            proxy.getIndicators(dataString, funcOK, funcFALSE);
        },

        setIdIndicator: function (data) {
            _indicatorid = data;
        },

        setCategoryId: function (data) {
            _categoryid = data;
        },

        setSubcategoryId: function (data) {
            _subcategoryid = data;
        },

        setTitulo: function (data) {
            _titulo = data;
        },

        setImageUrl: function (data) {
            _imageurl = data;
        },

        setColor: function (data) {
            _color = data;
        },

        setResumen: function (data) {
            _resumen = data;
        },

        setUnidad: function (data) {
            _unidad = data;
        },

        setSimbolo: function (data) {
            _simbolo = data;
        },

        setFuncion: function (data) {
            _funcion = data;
        },

        setCurrentPage: function (data) {
            _currentPage = data;
        },

        setOrderBy: function (data) {
            _orderby = data;
        },

        getIndicatorId: function () {
            return _indicatorid;
        },

        getTitulo: function () {
            return _titulo;
        },

        getCategoryId: function () {
            return _subcategoryid;
        },

        getSubcategoryId: function () {
            return _categoryid;
        },

        getImageUrl: function () {
            return _imageurl;
        },

        getColor: function () {
            return _color;
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
