var settings = {
    columns: '.column',
    widgetSelector: '.widget',
    handleSelector: '.widget-head',
    contentSelector: '.widget-content'
};

function obtenerWidjet(o) {
    var widget = o.parents(settings.widgetSelector);
    return widget;
}

function isDate(txtDate) {
    var currVal = txtDate;
    if ((currVal == '') || (currVal == undefined))
        return false;

    //Declare Regex  
    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray == null)
        return false;

    //Checks for dd/mm/yyyy format.
    dtDay = dtArray[1];
    dtMonth = dtArray[3];
    dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }
    return true;
}

function guardarWidget(objWidget) {
    var dataString = "{pnIdWidget:" + objWidget.attr('idwidget') + ", psClass:'" + objWidget.attr('class') + "', psTitulo:'" + objWidget.find('input.nuevo-titulo').val() + "', psDimension:'" + objWidget.attr('dimension') + "', psTipo:'" + objWidget.attr('tipo') + "', psColapsado:'" + objWidget.attr('colapsado') + "', psInicio:'" + objWidget.attr('inicio') + "', psFin:'" + objWidget.attr('fin') + "'}";
    var options = {
        type: 'POST',
        url: 'workspace.aspx/GuardaWidget',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: dataString,
        async: true,
        success: function (result) {
            return false;
        },
        error: function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }
    }
    $.ajax(options);
}

$(document).ready(function () {

    //--------------------------------------------
    //Evento al pulsar sobre el boton Colapse
    //--------------------------------------------
    $('.collapse').click(function () {
        var widget = obtenerWidjet($(this));
        var ex = $(this).parent().nextAll('.widget-content:first');

        ex.toggle('slow', function () {
            if (ex.css('display') == 'none') {
                widget.attr('colapsado', 'S');
            }
            else {
                widget.attr('colapsado', 'N');
            }
            guardarWidget(widget);
        });
        return false;
    });


    //--------------------------------------------
    //Evento al pulsar sobre el boton Edit
    //--------------------------------------------
    $('.edit').click(function () {
        var widget = obtenerWidjet($(this));
        var ex = $(this).parent().nextAll('.edit-box:first');
        ex.toggle('slow', function () {
            if (ex.css('display') != 'none') {
                ex.find('input').focus();
                return false;
            }
            else {
                guardarWidget(widget);
            }
        });
        return false;
    });

    //--------------------------------------------
    //Evento al pulsar sobre el boton Eliminar
    //--------------------------------------------
    $('.remove').click(function () {
        if (confirm('¿Seguro que quiere eliminar este componente?')) {

            var widget = obtenerWidjet($(this));
            var dataString = "{idwidget:" + widget.attr('idwidget') + "}";
            var options = {
                type: 'POST',
                url: '/WebServices.aspx/DeleteWidget',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: dataString,
                success: function (result) {
                    if (result.d > -1) {
                        widget.animate({
                            opacity: 0
                        }, function () {
                            $(this).wrap('<div/>').parent().slideUp(function () {
                                widget.remove();
                            });
                        });
                        return false;
                    }
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }
            }

            $.ajax(options);
        }
        return false;
    });

    //--------------------------------------------
    //Evento al pulsar sobre el boton Maximize
    //--------------------------------------------
    $('.maximize').click(function () {
        var widget = obtenerWidjet($(this));
        var WidgetMax = $("#dialog-form");

        WidgetMax.attr('idwidget', widget.attr('idwidget'));
        WidgetMax.attr('dimension', widget.attr('dimension'));
        WidgetMax.attr('tipo', widget.attr('tipo'));
        WidgetMax.attr('inicio', widget.attr('inicio'));
        WidgetMax.attr('fin', widget.attr('fin'));
        WidgetMax.dialog("open");

        return false;
    });

    //--------------------------------------------
    //Evento al seleccionar un Color
    //--------------------------------------------
    $('.pickcolor').click(function () {
        var widget = obtenerWidjet($(this));
        var colorStylePattern = /\bcolor_[\w]{1,}\b/,
            thisWidgetColorClass = widget.attr('class').match(colorStylePattern);

        if (thisWidgetColorClass) {
            widget.removeClass(thisWidgetColorClass[0]).addClass($(this).attr('class').match(colorStylePattern)[0]);
        }
        return false;
    });

    //--------------------------------------------
    //Evento al escribir en el TextBox del Nombre
    //--------------------------------------------
    $('input.nuevo-titulo', this).keyup(function () {
        var widget = obtenerWidjet($(this));
        widget.find('h3').text($(this).val().length > 30 ? $(this).val().substr(0, 20) + '...' : $(this).val());
    });

});