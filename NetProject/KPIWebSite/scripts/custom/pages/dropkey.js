// global variables to track the date range
var datos = undefined,
    opciones = undefined,
    objChart = undefined,
    objTable = undefined;

/***********************************************************************************************************
**                    Funciones para animar la barra de progreso durante la carga                         **
***********************************************************************************************************/
function ShowProgressAnimation() {
    $('.widget-loading').removeClass('hidden');
}
function CloseProgressAnimation() {
    $('.widget-loading').addClass('hidden');
}

/***********************************************************************************************************
**                                  Función para mostrar la gráfica actual                                **
***********************************************************************************************************/
function muestraFormulas(idwidget) {
    var bNoError = true;
    $.fn.displayFormulas = function (response) {
        $('#graphic-leyend').formulaItem({
            showOptions: false,
            datos: response,
            checked: function (event, data) {
                drawChart(null, null);
            }
        });
    };
    $.fn.hideFormulas = function (response) {
        $('#graphic-leyend').formulaItem({ datos: null });
        bNoError = false;
    };
    formulaObject.setIdWidget(idwidget);
    formulaObject.populate($(this).displayFormulas, $(this).hideFormulas);

    return bNoError;
}
function muestraWidget(idwidget) {
    if (!idwidget) {
        showWidget(3);
        $('#desktop-message-warning').kpimessage({
            texto: 'Error',
            message: 'You must especify a valid graphic number stored in our database',
            estilo: 'alert alert-danger fade in'
        });
        return;
    };

    //PASO 1.- Consultamos información del widget
    $.fn.returnNO = function (response) {
        showWidget(3);
        $('#desktop-message-warning').kpimessage({
            texto: 'Warning',
            message: 'We could not be able to find information about this graph in our database',
            estilo: 'alert alert-warning fade in'
        });
        return;
    };
    widgetObject.setIdWidget(idwidget);
    widgetObject.select($(this).returnNO);

    //PASO 2.- Mostramos las fórmulas
    if (muestraFormulas(idwidget) === false) {
        //$('#desktop-message-warning').kpimessage({
        //    texto: 'Info',
        //    message: 'Please, you must add formulas into this graph to visualize your results',
        //    estilo: 'alert alert-info fade in'
        //});
        return;
    }

    //PASO 3.- Actualizamos el estado de los botones de tipo
    var tipografica;
    switch (widgetObject.getTipoGrafico()) {
        case 'B':
            tipografica = $('#btnBarChart');
            break;
        case 'L':
            tipografica = $('#btnLineChart');
            break;
        case 'A':
            tipografica = $('#btnAreaChart');
            break;
        case 'H':
            tipografica = $('#btnHistogram');
            break;
        case 'Q':
            tipografica = $('#btnPieChart');
            break;
        case 'T':
            tipografica = $('#btnTable');
            break;
        default:
            showWidget(1);
            $('#desktop-message-warning').kpimessage({
                texto: 'Info',
                message: 'You must assign a valid type graphic to visualize your data',
                estilo: 'alert alert-info fade in'
            });
            return;
    }
    $('label.btn-type-graph').removeClass("active");
    tipografica.parent().addClass("active");

    //PASO 4.- Actualizamos el estado de los botones de tiempo
    var tipotiempo;
    switch (widgetObject.getTipoTiempo()) {
        case 'A':
            tipotiempo = $('#btnAnual');
            break;
        case 'S':
            tipotiempo = $('#btnSemestre');
            break;
        case 'T':
            tipotiempo = $('#btnTrimestre');
            break;
        case 'M':
            tipotiempo = $('#btnMes');
            break;
        case 'Q':
            tipotiempo = $('#btnQuincena');
            break;
        case 's':
            tipotiempo = $('#btnSemana');
            break;
        case 'D':
            tipotiempo = $('#btnDia');
            break;
        default:
            showWidget(1);
            $('#desktop-message-warning').kpimessage({
                texto: 'Info',
                message: 'You must assign a valid time dimension to visualize your data',
                estilo: 'alert alert-info fade in'
            });
            return;
    }
    $('label.btn-type-time').removeClass("active");
    tipotiempo.parent().addClass("active");

    pintaGrafica();
}
function pintaGrafica() {

    ShowProgressAnimation();

    $.fn.returnOK = function (response) {
        datos = new google.visualization.DataTable(response);
    };
    $.fn.returnNO = function (response) {
        alert('Algo salió mal');
    };
    if (widgetObject.getModoDatos() === 0)
        widgetObject.GetPercentData($(this).returnOK, $(this).returnNO);
    else if (widgetObject.getModoDatos() === 1)
        widgetObject.GetAbsoluteData($(this).returnOK, $(this).returnNO);
    else {
        showWidget(1);
        $('#desktop-message-warning').kpimessage({
            texto: 'Error',
            message: 'Some unexpected error occurred',
            estilo: 'alert alert-danger fade in'
        });
        return;
    }

    //mostramos la gráfica con los resultados
    drawChart(null, null);

    CloseProgressAnimation();
}
function drawChart(posIni, posFin) {
    var colors = [],
        hiddencols = [],
        hiddenrows = [],
        i = 0,
        j = 0;

    if (datos.getNumberOfColumns() == 1) {
        $('svg').hide();
        return;
    }

    //creamos la tabla temporal o vista
    var myView = new google.visualization.DataView(datos);

    if ((posIni != null) && (posFin != null) && (posIni != undefined) && (posFin != undefined)) {
        myView.setRows(posIni, posFin);
    }

    colors.length = 0;
    hiddencols.length = 0;
    hiddenrows.length = 0;

    $("div.formula-item").each(function (index) {
        if ($(this).find('input:checkbox').is(':checked')) {
            colors[i] = $(this).find('span.categoria-small').data('color');
            i++;
        } else {
            hiddencols[j] = index + 1;
            hiddenrows[j] = index;
            j++;
        }
    });

    switch (widgetObject.getTipoGrafico()) {
        case 'B':
            opciones = {
                'width': '100%',
                'height': 500,
                'fontSize': 11,
                'colors': colors,
                'legend': { 'position': 'bottom' },
                'isStacked': 'true',
                'chartArea': { 'width': '85%', 'height': '90%' }
            };
            objChart = new google.visualization.BarChart(document.getElementById('graphic-chart'));
            myView.hideRows(hiddenrows);
            break;
        case 'L':
            opciones = {
                'width': '100%',
                'height': 500,
                'fontSize': 11,
                'colors': colors,
                'legend': { 'position': 'bottom' },
                'chartArea': { 'width': '95%', 'height': '70%' },
                'pointSize': 5,
                'animation': {
                    'duration': 1000,
                    'easing': 'in'
                }
            };
            objChart = new google.visualization.LineChart(document.getElementById('graphic-chart'));
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
                'chartArea': { 'width': '95%', 'height': '70%' }
            };
            objChart = new google.visualization.AreaChart(document.getElementById('graphic-chart'));
            myView.hideColumns(hiddencols);
            break;
        case 'H':
            opciones = {
                'width': '100%',
                'height': 500,
                'fontSize': 11,
                'colors': colors,
                'legend': { 'position': 'bottom' },
                'chartArea': { 'width': '95%', 'height': '70%' }
            };
            objChart = new google.visualization.ColumnChart(document.getElementById('graphic-chart'));
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
            objChart = new google.visualization.PieChart(document.getElementById('graphic-chart'));
            myView.hideRows(hiddenrows);
            break;
    }
    objChart.draw(myView, opciones);
}

/******************************************************************************************************
**  Convierte una tabla devuelta en JSON en datos tipo CSV                                           **
*******************************************************************************************************/
function ConvertToCSV(objArray) {
    var array = typeof objArray != 'object' ? JSON.parse(objArray) : objArray;
    var str = '';

    for (var i = 0; i < array.length; i++) {
        var line = '';
        for (var index in array[i]) {
            if (line != '') line += ','

            line += array[i][index];
        }
        str += line + '\r\n';
    }
    return str;
}

/******************************************************************************************************
**  Descarga el contenido de un CSV                                                                  **
*******************************************************************************************************/
function downloadCSV(csv_out, titulo) {
    var blob = new Blob([csv_out], { type: 'text/csv;charset=utf-8' });
    var url = window.URL || window.webkitURL;
    var link = document.createElementNS("http://www.w3.org/1999/xhtml", "a");
    link.href = url.createObjectURL(blob);
    link.download = titulo + '.csv';

    var event = document.createEvent("MouseEvents");
    event.initEvent("click", true, false);
    link.dispatchEvent(event);
}

/***********************************************************************************************************
**                                      INICIO DE LA PÁGINA                                               **
***********************************************************************************************************/
$(document).ready(function () {
    var id = document.getElementById('hdnWidgetID').value;

    $(".graphic-leyend").draggable();

    //Botones de tipo de gráfico
    $('label.btn-type-graph > input[type=radio]').change(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        widgetObject.setTipoGrafico($(this).attr('value'));

        pintaGrafica();
    });

    //Botones de la dimensión Temporal
    $('label.btn-type-time > input[type=radio]').change(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        widgetObject.setTipoTiempo($(this).attr('value'));

        pintaGrafica();
    });

    //Muestra / Oculta la leyenda
    $('#btnShowHideLegend').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        var Legend = $('#graphic-leyend');
        if (Legend.hasClass('hidden')) {
            Legend.removeClass('hidden');
            $(this).html('<i class="fa fa-eye-slash"></i>&nbsp;Hide formulas');
        }
        else {
            Legend.addClass('hidden');
            $(this).html('<i class="fa fa-eye"></i>&nbsp;Show formulas');
        }
    });

    //Exporta datos a CSV
    $('#btnDownloadCSV').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        if (datos) {
            var result = ConvertToCSV(datos);
            var titulo = $.trim($('.widget-title').text());

            downloadCSV(result, titulo);
        }
    });

    muestraWidget(id);
});
