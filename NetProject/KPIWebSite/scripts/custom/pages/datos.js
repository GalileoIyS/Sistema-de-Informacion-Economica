var $container, pagIni, pagFin;

/*************************************************************
Nombre: Cargar
Descripción: Carga un objeto jSon con los datos solicitados
*************************************************************/
function Cargar() {
    var idtemporal = document.getElementById('HdnTemporal').value;
    var tituloValores = $('#HdnUnidad').val();
    $.fn.returnOK = function (result) {
        if (result) {
            switch (idtemporal) {
                case 'A':
                    $container.handsontable({
                        data: result,
                        rowHeaders: true,
                        colHeaders: ['Year', tituloValores],
                        colWidths: [80, 180],
                        currentRowClassName: 'currentRow',
                        currentColClassName: 'currentCol',
                        autoWrapRow: true,
                        stretchH: 'last',
                        columns: [
                                    { data: "ejercicio", type: 'text', readOnly: true },
                                    { data: "valor", type: 'numeric', format: '0,0.00' }
                        ]
                    });
                    break;
                case 'S':
                    $container.handsontable({
                        data: result,
                        rowHeaders: true,
                        colHeaders: ['Year', 'Semester', tituloValores],
                        colWidths: [80, 180, 100],
                        currentRowClassName: 'currentRow',
                        currentColClassName: 'currentCol',
                        autoWrapRow: true,
                        stretchH: 'last',
                        columns: [
                                    { data: "ejercicio", type: 'text', readOnly: true },
                                    { data: "nombre_semestre", type: 'text', readOnly: true },
                                    { data: "valor", type: 'numeric', format: '0,0.00' }
                        ]
                    });
                    break;
                case 'T':
                    $container.handsontable({
                        data: result,
                        rowHeaders: true,
                        colHeaders: ['Year', 'Quarter', tituloValores],
                        colWidths: [80, 180, 100],
                        currentRowClassName: 'currentRow',
                        currentColClassName: 'currentCol',
                        autoWrapRow: true,
                        stretchH: 'last',
                        columns: [
                                    { data: "ejercicio", type: 'text', readOnly: true },
                                    { data: "nombre_trimestre", type: 'text', readOnly: true },
                                    { data: "valor", type: 'numeric', format: '0,0.00' }
                        ]
                    });
                    break;
                case 'M':
                    $container.handsontable({
                        data: result,
                        rowHeaders: true,
                        colHeaders: ['Year', 'Month', 'Name', tituloValores],
                        colWidths: [80, 80, 180, 100],
                        currentRowClassName: 'currentRow',
                        currentColClassName: 'currentCol',
                        autoWrapRow: true,
                        stretchH: 'last',
                        columns: [
                                    { data: "ejercicio", type: 'text', readOnly: true },
                                    { data: "mes", type: 'text', readOnly: true },
                                    { data: "nombre_mes", type: 'text', readOnly: true },
                                    { data: "valor", type: 'numeric', format: '0,0.00' }
                        ]
                    });
                    break;
                case 'Q':
                    $container.handsontable({
                        data: result,
                        rowHeaders: true,
                        colHeaders: ['Year', 'Month', 'Fortnight', tituloValores],
                        colWidths: [80, 180, 200],
                        currentRowClassName: 'currentRow',
                        currentColClassName: 'currentCol',
                        autoWrapRow: true,
                        stretchH: 'last',
                        columns: [
                                    { data: "ejercicio", type: 'text', readOnly: true },
                                    { data: "nombre_mes", type: 'text', readOnly: true },
                                    { data: "nombre_quincena", type: 'text', readOnly: true },
                                    { data: "valor", type: 'numeric', format: '0,0.00' }
                        ]
                    });
                    break;
                case 's':
                    $container.handsontable({
                        data: result,
                        rowHeaders: true,
                        colHeaders: ['Year', 'Month', 'Week', tituloValores],
                        colWidths: [80, 180, 200],
                        currentRowClassName: 'currentRow',
                        currentColClassName: 'currentCol',
                        autoWrapRow: true,
                        stretchH: 'last',
                        columns: [
                                    { data: "ejercicio", type: 'text', readOnly: true },
                                    { data: "nombre_mes", type: 'text', readOnly: true },
                                    { data: "nombre_semana", type: 'text', readOnly: true },
                                    { data: "valor", type: 'numeric', format: '0,0.00' }
                        ]
                    });
                    break;
                case 'D':
                    $container.handsontable({
                        data: result,
                        rowHeaders: true,
                        colHeaders: ['Year', 'Month', 'Day', tituloValores],
                        colWidths: [80, 100, 80, 180],
                        currentRowClassName: 'currentRow',
                        currentColClassName: 'currentCol',
                        autoWrapRow: true,
                        stretchH: 'last',
                        columns: [
                                    { data: "ejercicio", type: 'text', readOnly: true },
                                    { data: "nombre_mes", type: 'text', readOnly: true },
                                    { data: "dia", type: 'text', readOnly: true },
                                    { data: "valor", type: 'numeric', format: '0,0.00' }
                        ]
                    });
                    break;
                case 'L':
                    $container.handsontable({
                        data: result,
                        rowHeaders: true,
                        colHeaders: ['Date (dd/mm/yyyy)', tituloValores],
                        colWidths: [140, 180],
                        currentRowClassName: 'currentRow',
                        currentColClassName: 'currentCol',
                        autoWrapRow: true,
                        minSpareRows: true,
                        stretchH: 'last',
                        columns: [
                                    { data: "fecha", type: 'date', format: 'dd/mm/yyyy' },
                                    { data: "valor", type: 'numeric', format: '0,0.00' }
                        ]
                    });
                    break;
            }
        }
    };
    $.fn.returnNO = function (response) {
        $console.text('Load error');
    };
    datasetObject.load(pagIni, pagFin, $(this).returnOK, $(this).returnNO);
}

/*************************************************************
Nombre: Guardar
Descripción: Envia los datos modificados a través de un objeto 
jSon para que sean almacenados en la base de datos.
*************************************************************/
function Guardar() {
    var handsontable = $container.data('handsontable');
    $.fn.returnNO = function (response) {
        alert('Algo salió mal');
    };
    var data = JSON.stringify(handsontable.getData());
    datasetObject.save(data, null, $(this).returnNO);
}

/*************************************************************
Nombre: Anterior
Descripción: Busca datos anteriores en la bb.dd.
*************************************************************/
function Anterior() {
    if (parseInt(pagIni) - 15 <= 0) {
        pagIni = 1;
        pagFin = 15;
    }
    else {
        pagIni = parseInt(pagIni) - 15;
        pagFin = parseInt(pagFin) - 15;
    }

    $('#txtPreviousTop').val(pagFin);
    $('#txtNextTop').val(pagIni);

    Guardar();
    Cargar();
}

/*************************************************************
Nombre: Siguiente
Descripción: Busca datos posteriores en la bb.dd.
*************************************************************/
function Siguiente() {
    pagIni = parseInt(pagIni) + 15;
    pagFin = parseInt(pagFin) + 15;

    $('#txtPreviousTop').val(pagFin);
    $('#txtNextTop').val(pagIni);

    Guardar();
    Cargar();
}

$(document).ready(function () {

    var id = document.getElementById('HdnDatasetId').value;
    if (id) {
        datasetObject.setDatasetId(id);
        dimensionvaluesObject.setDatasetId(id);
    }

    $container = $("#TablaDatos");
    pagIni = 1;
    pagFin = 15;

    Cargar();

    $("#txtPreviousTop").keyup(function (e) {
        if (e.keyCode == 13) {
            pagIni = $("#txtNextTop").val();
            pagFin = $("#txtPreviousTop").val();
            $("#txtNextBottom").val(pagIni);
            $("#txtPreviousBottom").val(pagFin);

            Cargar();
        }
    });

    $("#txtNextTop").keyup(function (e) {
        if (e.keyCode == 13) {
            pagIni = $("#txtNextTop").val();
            pagFin = $("#txtPreviousTop").val();
            $("#txtNextBottom").val(pagIni);
            $("#txtPreviousBottom").val(pagFin);

            Cargar();
        }
    });

    //    $("#searchbox").tagsInput({
    //        'width': '100%',
    //        'height': '224px',
    //        'defaultText': '(añadir)',
    //        'minChars': 0,
    //        'maxChars': 0,
    //        'placeholderColor': '#666666',
    //        autocomplete_url: 'datos.aspx/ObtenerDimensionValores',
    //        autocomplete: {
    //            source: function (request, response) {

    //                if (geocoder == null) {
    //                    geocoder = new google.maps.Geocoder();
    //                }
    //                geocoder.geocode({ 'address': request.term }, function (results, status) {
    //                    if (status == google.maps.GeocoderStatus.OK) {

    //                        var searchLoc = results[0].geometry.location;
    //                        var lat = results[0].geometry.location.lat();
    //                        var lng = results[0].geometry.location.lng();
    //                        var latlng = new google.maps.LatLng(lat, lng);
    //                        var bounds = results[0].geometry.bounds;

    //                        geocoder.geocode({ 'latLng': latlng }, function (results1, status1) {
    //                            if (status1 == google.maps.GeocoderStatus.OK) {
    //                                if (results1[1]) {

    //                                    response($.map(results1, function (loc) {
    //                                        return {
    //                                            label: loc.formatted_address,
    //                                            value: loc.formatted_address,
    //                                            bounds: loc.geometry.bounds
    //                                        }
    //                                    }));
    //                                }
    //                            }
    //                        });
    //                    }
    //                });
    //            },
    //            select: function (event, ui) {
    //                var pos = ui.item.position;
    //                var lct = ui.item.locType;
    //                var bounds = ui.item.bounds;

    //                if (bounds) {
    //                    map.fitBounds(bounds);
    //                }
    //            }
    //        },
    //        onAddTag: function (value) {
    //            var dimensionid = $(this).attr('dimensionid');
    //            var datasetid = document.getElementById('HdnDatasetId').value;
    //            var dataString = "{texto:'" + value + "', dimensionid:" + dimensionid + ", datasetid:" + datasetid + "}";

    //            var options = {
    //                type: 'POST',
    //                url: '/WebServices.aspx/AddDimensionValue',
    //                contentType: 'application/json; charset=utf-8',
    //                dataType: 'json',
    //                data: dataString,
    //                success: function (result) {
    //                    if (result.d > 0) {

    //                        $('ul.dimensions li').each(function () {
    //                            if ($(this).attr('dimensionid') == dimensionid) {
    //                                var Num = parseInt($(this).find('span').text());
    //                                $(this).find('span').text(Num + 1);
    //                            }
    //                        });

    //                        $(".tagsinput").css('background-color', '#86C59C').animate({ 'background-color': '#ffffff' }, 1000);
    //                    }
    //                    else
    //                        $(".tagsinput").css('background-color', '#F9A7A7').animate({ 'background-color': '#ffffff' }, 1000);
    //                }
    //            }

    //            $.ajax(options);
    //        },
    //        onRemoveTag: function (value) {
    //            var dimensionid = $(this).attr('dimensionid');
    //            var datasetid = document.getElementById('HdnDatasetId').value;
    //            var dataString = "{texto:'" + value + "', dimensionid:" + dimensionid + ", datasetid:" + datasetid + "}";

    //            var options = {
    //                type: 'POST',
    //                url: '/WebServices.aspx/DelDimensionValue',
    //                contentType: 'application/json; charset=utf-8',
    //                dataType: 'json',
    //                data: dataString,
    //                success: function (result) {
    //                    if (result.d > 0) {

    //                        $('ul.dimensions li').each(function () {
    //                            if ($(this).attr('dimensionid') == dimensionid) {
    //                                var Num = parseInt($(this).find('span').text());
    //                                $(this).find('span').text(Num - 1);
    //                            }
    //                        });

    //                        $(".tagsinput").css('background-color', '#86C59C').animate({ 'background-color': '#ffffff' }, 1000);
    //                    }
    //                    else
    //                        $(".tagsinput").css('background-color', '#F9A7A7').animate({ 'background-color': '#ffffff' }, 1000);
    //                }
    //            }
    //            $.ajax(options);
    //        }
    //        });

    $(".tag-prefix").tagsInput({
        'width': '100%',
        'defaultText': '(añadir)',
        'minChars': 0,
        'maxChars': 0,
        'placeholderColor': '#666666',
        autocomplete_url: '/WebServices.aspx/ObtenerDimensionValores',
        autocomplete: {
            source: function (request, response) {
                var id = this.element.parents('.panel-body:first').data('dimensionid');
                if (id) {
                    $.fn.returnOK = function (result) {
                        response($.map(result, function (item) {
                            return {
                                label: item.codigo,
                                value: item.codigo
                            }
                        }));
                    };
                    $.fn.returnNO = function (response) {
                        alert('Algo salió mal');
                    };
                    dimensionvaluesObject.setDimensionId(id);
                    dimensionvaluesObject.find(request.term, $(this).returnOK, $(this).returnNO);
                }
            }
        },
        onAddTag: function (value) {
            var id = $(this).data('dimensionid');
            if (id) {
                $.fn.returnOK = function (result) {
                    if (result) {
                        $('a.dimension-title').each(function () {
                            if ($(this).data('dimensionid') == id) {
                                var Num = parseInt($(this).find('span').text());
                                $(this).find('span').text(Num + 1);
                            }
                        });
                        $(".tagsinput").css('background-color', '#86C59C').animate({ 'background-color': '#ffffff' }, 1000);
                    }
                    else
                        $(".tagsinput").css('background-color', '#F9A7A7').animate({ 'background-color': '#ffffff' }, 1000);
                };
                $.fn.returnNO = function (response) {
                    $(".tagsinput").css('background-color', '#F9A7A7').animate({ 'background-color': '#ffffff' }, 1000);
                };
                dimensionvaluesObject.setDimensionId(id);
                dimensionvaluesObject.insert(value, $(this).returnOK, $(this).returnNO);
            }
        },
        onRemoveTag: function (value) {
            var id = $(this).data('dimensionid');
            if (id) {
                $.fn.returnOK = function (result) {
                    if (result) {
                        $('a.dimension-title').each(function () {
                            if ($(this).data('dimensionid') == id) {
                                var Num = parseInt($(this).find('span').text());
                                $(this).find('span').text(Num - 1);
                            }
                        });
                        $(".tagsinput").css('background-color', '#86C59C').animate({ 'background-color': '#ffffff' }, 1000);
                    }
                    else
                        $(".tagsinput").css('background-color', '#F9A7A7').animate({ 'background-color': '#ffffff' }, 1000);
                };
                $.fn.returnNO = function (response) {
                    $(".tagsinput").css('background-color', '#F9A7A7').animate({ 'background-color': '#ffffff' }, 1000);
                };
                dimensionvaluesObject.setDimensionId(id);
                dimensionvaluesObject.remove(value, $(this).returnOK, $(this).returnNO);
            }
        }
    });
});

