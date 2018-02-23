// global variables to track the date range
var opciones = undefined,
    objChart = undefined,
    objTable = undefined;
/***********************************************************************************************************
**                                  Función para mostrar la gráfica actual                                **
***********************************************************************************************************/
function muestraFormulas(objWidget) {
    var bNoError = true;
    var legend = objWidget.find('.graphic-leyend');

    $.fn.displayFormulas = function (response) {
        legend.formulaItem({
            showOptions: false,
            datos: response,
            checked: function (event, data) {
                $.fn.returnNO = function (response) {
                    $('#dashboard-message-warning').kpimessage({
                        texto: 'Warning',
                        message: 'We could not be able to find information about this graph in our database',
                        estilo: 'alert alert-warning fade in'
                    });
                    return;
                };
                widgetObject.setIdWidget(data.widgetid);
                widgetObject.select($(this).returnNO);

                drawChart(objWidget);
            }
        });
    };
    $.fn.hideFormulas = function (response) {
        legend.formulaItem({ datos: null });
        bNoError = false;
    };
    formulaObject.setIdWidget(objWidget.data('idwidget'));
    formulaObject.populate($(this).displayFormulas, $(this).hideFormulas);

    return bNoError;
}
function muestraWidget(objWidget) {
    if (!objWidget) {
        $('#dashboard-message-warning').kpimessage({
            texto: 'Error',
            message: 'You must especify a valid graphic number stored in our database',
            estilo: 'alert alert-danger fade in'
        });
        return;
    };

    //PASO 1.- Consultamos información del widget
    $.fn.returnNO = function (response) {
        $('#dashboard-message-warning').kpimessage({
            texto: 'Warning',
            message: 'We could not be able to find information about this graph in our database',
            estilo: 'alert alert-warning fade in'
        });
        return;
    };
    widgetObject.setIdWidget(objWidget.data('idwidget'));
    widgetObject.select($(this).returnNO);
    
    //PASO 2.- Mostramos las fórmulas
    if (muestraFormulas(objWidget) === false) {
        $('#desktop-message-warning').kpimessage({
            texto: 'Info',
            message: 'Please, you must add formulas into this graph to visualize your results',
            estilo: 'alert alert-info fade in'
        });
        return;
    }

    //PASO 3.- Actualizamos el estado de los botones de tipo
    var tipografica;
    switch (widgetObject.getTipoGrafico()) {
        case 'B':
            tipografica = objWidget.find('.btnBarChart');
            break;
        case 'L':
            tipografica = objWidget.find('.btnLineChart');
            break;
        case 'A':
            tipografica = objWidget.find('.btnAreaChart');
            break;
        case 'H':
            tipografica = objWidget.find('.btnHistogram');
            break;
        case 'Q':
            tipografica = objWidget.find('.btnPieChart');
            break;
        case 'T':
            tipografica = objWidget.find('.btnTable');
            break;
        default:
            $('#desktop-message-warning').kpimessage({
                texto: 'Info',
                message: 'You must assign a valid type graphic to visualize your data',
                estilo: 'alert alert-info fade in'
            });
            return;
    }
    objWidget.find('label.btn-type-graph').removeClass("active");
    tipografica.parent().addClass("active");

    //PASO 4.- Actualizamos el estado de los botones de tiempo
    var tipotiempo;
    switch (widgetObject.getTipoTiempo()) {
        case 'A':
            tipotiempo = objWidget.find('.btnAnual');
            break;
        case 'S':
            tipotiempo = objWidget.find('.btnSemestre');
            break;
        case 'T':
            tipotiempo = objWidget.find('.btnTrimestre');
            break;
        case 'M':
            tipotiempo = objWidget.find('.btnMes');
            break;
        case 'Q':
            tipotiempo = objWidget.find('.btnQuincena');
            break;
        case 's':
            tipotiempo = objWidget.find('.btnSemana');
            break;
        case 'D':
            tipotiempo = $objWidget.find('.btnDia');
            break;
        default:
            $('#desktop-message-warning').kpimessage({
                texto: 'Info',
                message: 'You must assign a valid time dimension to visualize your data',
                estilo: 'alert alert-info fade in'
            });
            return;
    }
    objWidget.find('label.btn-type-time').removeClass("active");
    tipotiempo.parent().addClass("active");

    pintaGrafica(objWidget);
}

/***********************************************************************************************************
**                    Funciones para animar la barra de progreso durante la carga                         **
***********************************************************************************************************/
function ShowProgressAnimation(object) {
    var background = object.find('.widget-loading');
    background.removeClass('hidden');
}
function CloseProgressAnimation(object) {
    var background = object.find('.widget-loading');
    background.addClass('hidden');
}

/***********************************************************************************************************
**                                  Función para pintar la gráfica actual                                **
***********************************************************************************************************/
function pintaGrafica(objWidget) {

    ShowProgressAnimation(objWidget);

    $.fn.returnOK = function (response) {
        var tmpDatos = new google.visualization.DataTable(response);
        objWidget.data('datos', tmpDatos);
    };
    $.fn.returnNO = function (response) {
        alert('Algo salió mal');
    };
    if (widgetObject.getModoDatos() === 0)
        widgetObject.GetPercentData($(this).returnOK, $(this).returnNO);
    else if (widgetObject.getModoDatos() === 1)
        widgetObject.GetAbsoluteData($(this).returnOK, $(this).returnNO);
    else {
        $('#dashboard-message-warning').kpimessage({
            texto: 'Error',
            message: 'Some unexpected error occurred',
            estilo: 'alert alert-danger fade in'
        });
        return;
    }

    //Mostramos el resumen
    //mostrarResumen();

    //Muestra la barra temporal inferior
    //mostrarTimeSlider();

    //mostramos la gráfica con los resultados
    drawChart(objWidget);

    $('#dashboard-message-warning').kpimessage('hide');

    CloseProgressAnimation(objWidget);
}
function drawChart(objWidget) {
    var colors = [],
        hiddencols = [],
        hiddenrows = [],
        i = 0,
        j = 0;

    //if (datos.getNumberOfColumns() == 1) {
    //    $('svg').hide();
    //    return;
    //}

    var mydata = objWidget.data('datos');
    if (mydata) {
        //creamos la tabla temporal o vista
        var myView = new google.visualization.DataView(mydata);

        colors.length = 0;
        hiddencols.length = 0;
        hiddenrows.length = 0;

        objWidget.find('div.formula-item').each(function (index) {
            if ($(this).find('input:checkbox').is(':checked')) {
                colors[i] = $(this).find('span.categoria-small').data('color');
                i++;
            } else {
                hiddencols[j] = index + 1;
                hiddenrows[j] = index;
                j++;
            }
        });

        var widgetElem = 'widget-graph-' + objWidget.data('idwidget');
        switch (widgetObject.getTipoGrafico()) {
            case 'B':
                opciones = {
                    'width': '100%',
                    'height': 500,
                    'fontSize': 11,
                    'colors': colors,
                    'legend': { 'position': 'bottom' },
                    'isStacked': 'true',
                    'chartArea': { 'width': '70%', 'height': '90%' }
                };
                objChart = new google.visualization.BarChart(document.getElementById(widgetElem));
                myView.hideRows(hiddenrows);
                break;
            case 'L':
                opciones = {
                    'width': '100%',
                    'height': 500,
                    'fontSize': 11,
                    'colors': colors,
                    'legend': { 'position': 'bottom' },
                    'chartArea': { 'width': '80%', 'height': '70%' },
                    'pointSize': 5,
                    'animation': {
                        'duration': 1000,
                        'easing': 'in'
                    }
                };
                objChart = new google.visualization.LineChart(document.getElementById(widgetElem));
                myView.hideColumns(hiddencols);
                break;
            case 'A':
                opciones = {
                    'width': '100%',
                    'height': 500,
                    'fontSize': 11,
                    'colors': colors,
                    'legend': { 'position': 'bottom' },
                    'pointSize': 5,
                    'chartArea': { 'width': '80%', 'height': '70%' }
                };
                objChart = new google.visualization.AreaChart(document.getElementById(widgetElem));
                myView.hideColumns(hiddencols);
                break;
            case 'H':
                opciones = {
                    'width': '100%',
                    'height': 500,
                    'fontSize': 11,
                    'colors': colors,
                    'legend': { 'position': 'bottom' },
                    'chartArea': { 'width': '80%', 'height': '70%' }
                };
                objChart = new google.visualization.ColumnChart(document.getElementById(widgetElem));
                myView.hideColumns(hiddencols);
                break;
            case 'Q':
                opciones = {
                    'width': '100%',
                    'height': 500,
                    'fontSize': 11,
                    'colors': colors,
                    'legend': { 'position': 'bottom' },
                    'chartArea': { 'width': '95%', 'height': '90%' },
                    'is3D': true,
                    'animation': {
                        'duration': 1000,
                        'easing': 'out'
                    }
                };
                objChart = new google.visualization.PieChart(document.getElementById(widgetElem));
                myView.hideRows(hiddenrows);
                break;
        }
        objChart.draw(myView, opciones);
    }
}

$(document).ready(function () {

    $("a[elem='dashboard']").css("color", "#759dae;");

    $('#dashboard-message-warning').kpimessage();

    $(".graphic-leyend").draggable();

    //Botones de tipo de gráfico
    $('label.btn-type-graph > input[type=radio]').change(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        var objWidget = $(this).closest('div.jarviswidget');

        if (objWidget) {
            $.fn.returnNO = function (response) {
                $('#dashboard-message-warning').kpimessage({
                    texto: 'Warning',
                    message: 'We could not be able to find information about this graph in our database',
                    estilo: 'alert alert-warning fade in'
                });
                return;
            };
            widgetObject.setIdWidget(objWidget.data('idwidget'));
            widgetObject.select($(this).returnNO);

            widgetObject.setTipoGrafico($(this).attr('value'));
            pintaGrafica(objWidget);
        }
    });

    //Botones de la dimensión Temporal
    $('label.btn-type-time > input[type=radio]').change(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        var objWidget = $(this).closest('div.jarviswidget');
        if (objWidget) {
            $.fn.returnNO = function (response) {
                $('#dashboard-message-warning').kpimessage({
                    texto: 'Warning',
                    message: 'We could not be able to find information about this graph in our database',
                    estilo: 'alert alert-warning fade in'
                });
                return;
            };
            widgetObject.setIdWidget(objWidget.data('idwidget'));
            widgetObject.select($(this).returnNO);

            widgetObject.setTipoTiempo($(this).attr('value'));
            pintaGrafica(objWidget);
        }
    });

    $(".jarviswidget").each(function () {
        //Llamamos al pintado del widget
        muestraWidget($(this));
    });

    pageSetUp(); //IMPORTANTE!!
});