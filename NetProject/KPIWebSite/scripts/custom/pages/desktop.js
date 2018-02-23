// global variables to track the date range
var cur = -1,
    prv = -1,
    primerClick = true,
    datos = undefined,
    opciones = undefined,
    objChart = undefined,
    objTable = undefined;

/***********************************************************************************************************
**                  Función para mostrar la selección entre un rango de fechas                            **
***********************************************************************************************************/
$.datepicker._defaults.hideOnSelect = false;
$.datepicker._defaults.onAfterUpdate = null;
$.datepicker._selectDate = function (id, dateStr) {
    var target = $(id);
    var inst = this._getInst(target[0]);
    dateStr = (dateStr != null ? dateStr : this._formatDate(inst));
    if (inst.input)
        inst.input.val(dateStr);
    this._updateAlternate(inst);
    var onSelect = this._get(inst, 'onSelect');
    var hideOnSelect = this._get(inst, 'hideOnSelect');

    if (onSelect)
        onSelect.apply((inst.input ? inst.input[0] : null), [dateStr, inst]);  // trigger custom callback
    else if (inst.input)
        inst.input.trigger('change'); // fire the change event
    if (inst.inline || !hideOnSelect)
        this._updateDatepicker(inst);
    else {
        this._hideDatepicker();
        this._lastInput = inst.input[0];
        if (typeof (inst.input[0]) != 'object')
            inst.input.focus(); // restore focus
        this._lastInput = null;

    }
}
var datepicker__updateDatepicker = $.datepicker._updateDatepicker;
$.datepicker._updateDatepicker = function (inst) {
    datepicker__updateDatepicker.call(this, inst);

    var onAfterUpdate = this._get(inst, 'onAfterUpdate');
    if (onAfterUpdate)
        onAfterUpdate.apply((inst.input ? inst.input[0] : null),
         [(inst.input ? inst.input.val() : ''), inst]);
}

/******************************************************************************************************
**  Dibuja una mapa con valores aleatorios para la demo del Dublin Summit                            **
*******************************************************************************************************/
function fillMap()
{
    data_array = {
        "US": 4977,
        "AU": 4873,
        "IN": 3671,
        "BR": 2476,
        "TR": 1476,
        "CN": 146,
        "CA": 134,
        "BD": 100
    };

    $('#vector-map').vectorMap({
        map: 'world_mill_en',
        backgroundColor: '#fff',
        regionStyle: {
            initial: {
                fill: '#c4c4c4'
            },
            hover: {
                "fill-opacity": 1
            }
        },
        series: {
            regions: [{
                values: data_array,
                scale: ['#85a8b6', '#4d7686'],
                normalizeFunction: 'polynomial'
            }]
        },
        onRegionLabelShow: function (e, el, code) {
            if (typeof data_array[code] == 'undefined') {
                e.preventDefault();
            } else {
                var countrylbl = data_array[code];
                el.html(el.html() + ': ' + countrylbl + ' visits');
            }
        }
    });
}

/******************************************************************************************************
**  Dibuja una lista con los amigos que tienen este indicador en su biblioteca                       **
*******************************************************************************************************/
function fillFriends(txtFilter) {
    var filtro = txtFilter;
    $.fn.displayFriends = function (response) {
        $("#chat-container").shortlist({
            datos: response,
            onClick: function (event, data) {
                // Prevents the default action to be triggered. 
                event.preventDefault();

                //Mostramos información del usuario
                //showUserInfo(data);
            },
            onEnter: function (event, data) {
                fillFriends(data);
            },
            txtValue: filtro
        });
    };
    $.fn.hideFriends = function (response) {
        alert('Algo salió mal');
    };
    friendObject.setAceptado('S');
    friendObject.setFilter(filtro);
    friendObject.getFriendsList(-1, $(this).displayFriends, $(this).hideFriends);
}

/***********************************************************************************************************
**                                  Función para mostrar la gráfica actual                                **
***********************************************************************************************************/
function muestraFiltros() {
    var id = expressionObject.getIdExpresion();
    $('mtext').attr('class', 'negrita');
    $('mtext span').each(function (index) {
        if ($(this).data('id') == id) {
            $(this).parent().prev().attr('class', 'txt-color-pinkRed');
        }
    });

    $.fn.displayDimensions = function (response) {
        $('.kpi-no-filter').hide();
        $('.kpi-new-filter').newfilter('populateDim', response);
    };
    $.fn.hideDimensions = function (response) {
        $('.kpi-no-filter').show();
        $('.kpi-new-filter').newfilter('populateDim', null);
    };
    dimensionObject.setIndicatorId(expressionObject.getIdIndicator());
    dimensionObject.setPageSize(0);
    dimensionObject.fillCombo($(this).displayDimensions, $(this).hideDimensions);

    $.fn.displayOperators = function (response) {
        $('.kpi-new-filter').newfilter('populateOpe', response);
    };
    $.fn.hideOperators = function (response) {
        $('.kpi-new-filter').newfilter('populateOpe', null);
    };
    proxy.getTypeFilters(null, $(this).displayOperators, $(this).hideOperators);

    $('.kpi-new-filter').newfilter('restore');

    $.fn.displayFilters = function (response) {
        var operatorItems = $('#cmbOperadores').kpicombo("getData", null);
        var dimensionItems = $('#cmbDimensiones').kpicombo("getData", null);

        //limpiamos la lista previa que exista
        $('#filter-list').empty();
        //rellenamos la nueva lista

        $.each(response, function (i, item) {
            $('<div>').appendTo('#filter-list').kpieditfilter({
                data: item,
                cmbDim: dimensionItems,
                cmbOpe: operatorItems,
                eliminar: function (event, data) {
                    var dataString = "{filtroid:" + data.idfilter + "}";
                    proxy.delFilters(dataString, null, null);
                },
                aceptar: function (event, data) {
                    var dataString = "{filtroid:" + data.idfilter + ", dimensionid:" + data.dimid + ", filtro :'" + data.dimop + "', valor : '" + data.value + "'}";
                    proxy.updFilters(dataString, null, null);
                }
            });
        });
    };
    $.fn.errorFilters = function (response) {
        //limpiamos la lista previa que exista
        $('#filter-list').empty();
    };
    filterObject.setExpresionId(id);
    filterObject.populate($(this).displayFilters, $(this).errorFilters);
}
function muestraFormulas(idwidget) {
    var bNoError = true;
    $.fn.displayFormulas = function (response) {
        $('#graphic-leyend').formulaItem({
            datos: response,
            checked: function (event, data) {
                drawChart(null, null);
            },
            editar: function (event, data) {
                formulaObject.setIdFormula(data.formulaid);
                formulaObject.select(data.formulaid);
                $('#lbNombreDeFormula').text(formulaObject.getNombre());
                $('#txtNombreFormula').val(formulaObject.getNombre());
                $('#txtValorFormula').importTags(formulaObject.getOriginal());
                $('#txtFormulaColor').val(formulaObject.getColor());
                $('#txtSearchIndicators').val('');
                $('.pickercolor').colorpicker('setValue', formulaObject.getColor());

                $('#frmAddFormula').modal('show');
            },
            filtrar: function (event, data) {
                $.fn.displayExpresions = function (response) {
                    $('#cmbFilterIndicators').kpicombo({ datos: response });

                    //dibujamos la fórmula
                    formulaObject.setIdFormula(data.formulaid);
                    formulaObject.select(null);
                    $('#formulagraphic').text(formulaObject.getDisplay());
                    var miSpan = document.getElementById('formulagraphic');
                    M.parseMath(miSpan);

                    var selected = $('#cmbFilterIndicators').find('option:selected');
                    if (selected) {
                        expressionObject.setIdExpresion(selected.val());
                        expressionObject.setIdIndicator(selected.data('extra'));
                        muestraFiltros();
                    }
                };
                $.fn.hideExpresions = function (response) {
                    //$("#graphic-list-expresions").empty();
                };
                expressionObject.setIdFormula(data.formulaid);
                expressionObject.populate($(this).displayExpresions, $(this).hideExpresions);
                $('#frmAddFilter').modal('show', {}
                    );
            },
            eliminar: function (event, data) {
                $.SmartMessageBox({
                    title: '<i class="fa fa-times" style="color:#ed1c24"></i>&nbsp;Remove current formula?',
                    content: "Warning: This action cannot be undone",
                    buttons: '[No][Yes]'
                }, function (ButtonPressed) {
                    if (ButtonPressed === "Yes") {
                        $.fn.returnOK = function (response) {
                            $('.formula-item').filter(function () {
                                return ($(this).data('formulaid') == response.d)
                            }).parent().remove();
                            $.smallBox({
                                title: "Delete complete",
                                content: "<i class='fa fa-clock-o'></i> <i>1 formula successfully deleted</i>",
                                color: "#659265",
                                iconSmall: "fa fa-check fa-2x fadeInRight animated",
                                timeout: 4000
                            });
                            pintaGrafica();
                        };
                        $.fn.returnNO = function (response) {
                            alert('Error: rellenaFormulas');
                        };
                        formulaObject.remove(data, $(this).returnOK, $(this).returnNO);
                    }
                })
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

    //PASO 1.- Marcamos el elemento activo
    $('a.widget-menu-item').parent().removeClass('active');
    var currentWidget = $('a.widget-menu-item').filter(function () {
        return $(this).data("idwidget") == idwidget;
    }).parent().addClass('active');

    //PASO 2.- Actualizamos la barra superior con la ruta
    $('#lbCurrentDashboard').html(currentWidget.parents('ul').prev().find('.menu-item-parent').html());
    $('#lbCurrentWidget').html('&nbsp;>&nbsp;' + currentWidget.find('span').html());
    $('.widget-title').html(currentWidget.find('span').html());

    //PASO 3.- Consultamos información del widget
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

    //PASO 4.- Asignamos el nombre por si lo queremos renombrar
    $('#txtRenameWidget').val(widgetObject.getTitulo());

    //PASO 5.- Establecemos la fecha del gráfico
    $('#txtDateRange').val(widgetObject.getFechaIni() + ' - ' + widgetObject.getFechaFin());
    if (widgetObject.getFechaIni()) {
        prv = (new Date(widgetObject.getFechaIni())).getTime();
    }
    else {
        prv = -1;
    }
    if (widgetObject.getFechaFin()) {
        cur = (new Date(widgetObject.getFechaFin())).getTime();
    }
    else {
        cur = -1;
    }

    //PASO 6.- Mostramos las fórmulas
    if (muestraFormulas(idwidget) === false) {
        showWidget(2);
        $('#desktop-message-warning').kpimessage({
            texto: 'Info',
            message: 'Please, you must add formulas into this graph to visualize your results',
            estilo: 'alert alert-info fade in'
        });
        return;
    }

    //PASO 7.- Actualizamos el estado de los botones de tipo
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

    //PASO 8.- Actualizamos el estado de los botones de tiempo
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
function showWidget(situacion) {
    switch (situacion) {
        case 0:
            //Mostramos TODO
            $('#content').removeClass('hidden');
            $('#content-none').addClass('hidden');
            $('.widget-body').removeClass('hidden');
            $('.tabbable.tabs-below').removeClass('hidden');
            break;
        case 1:
            //Solo la barra del widget
            $('#content').removeClass('hidden');
            $('#content-none').addClass('hidden');
            $('.widget-body').removeClass('hidden');
            $('.tabbable.tabs-below').addClass('hidden');
            break;
        case 2:
            //Incluimos la barra de Herramientas
            $('#content').removeClass('hidden');
            $('#content-none').addClass('hidden');
            $('.widget-body').addClass('hidden');
            $('.tabbable.tabs-below').addClass('hidden');
            break;
        case 3:
            //Ocultamos TODO
            $('#content').addClass('hidden');
            $('#content-none').removeClass('hidden');
            $('.widget-body').addClass('hidden');
            $('.tabbable.tabs-below').addClass('hidden');
            break;
    }
}

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
**                                  Función para pintar la gráfica actual                                **
***********************************************************************************************************/
//Funcion para mostrar el resumen de los datos
function mostrarResumen() {
    $.fn.returnOK = function (response) {
        $('span.userPercent').text(response.usuariosP);
        $('div.userPercent').data('easyPieChart').update(response.usuariosP);
        $('span.userIncluded').text(response.usuariosV);
        if (response.usuariosV === 0)
            $('span.userIncluded').removeClass('bg-color-greenLight').addClass('bg-color-red');
        else
            $('span.userIncluded').removeClass('bg-color-red').addClass('bg-color-greenLight');
        $('span.userAvailable').text(response.usuariosAll);
        if (response.usuariosAll === 0)
            $('span.userAvailable').removeClass('bg-color-greenLight').addClass('bg-color-red');
        else
            $('span.userAvailable').removeClass('bg-color-red').addClass('bg-color-greenLight');

        $('span.datasetPercent').text(response.datasetsP);
        $('div.datasetPercent').data('easyPieChart').update(response.datasetsP);
        $('span.datasetIncluded').text(response.datasetsV);
        if (response.datasetsV === 0)
            $('span.datasetIncluded').removeClass('bg-color-greenLight').addClass('bg-color-red');
        else
            $('span.datasetIncluded').removeClass('bg-color-red').addClass('bg-color-greenLight');
        $('span.datasetAvailable').text(response.datasetsAll);
        if (response.datasetsAll === 0)
            $('span.datasetAvailable').removeClass('bg-color-greenLight').addClass('bg-color-red');
        else
            $('span.datasetAvailable').removeClass('bg-color-red').addClass('bg-color-greenLight');

        $('span.valuePercent').text(response.datosP);
        $('div.valuePercent').data('easyPieChart').update(response.datosP);
        $('span.valueIncluded').text(response.datosV);
        if (response.datosV === 0)
            $('span.valueIncluded').removeClass('bg-color-greenLight').addClass('bg-color-red');
        else
            $('span.valueIncluded').removeClass('bg-color-red').addClass('bg-color-greenLight');
        $('span.valueAvailable').text(response.datosAll);
        if (response.datosAll === 0)
            $('span.valueAvailable').removeClass('bg-color-greenLight').addClass('bg-color-red');
        else
            $('span.valueAvailable').removeClass('bg-color-red').addClass('bg-color-greenLight');
    };
    $.fn.returnFALSE = function (response) {
        alert('Error: mostrarResumen');
    };

    widgetObject.getResumen($(this).returnOK, $(this).returnFALSE);
}
function mostrarTimeSlider() {

    var myTimes = [];
    for (var pos = 0; pos < datos.getNumberOfRows() ; pos++) {
        var currentValue = datos.getValue(pos, 0);
        myTimes.push(currentValue);
    }
    var maxVal = myTimes.length - 1;

    $('#time-slider').ionRangeSlider("update", {
        min: 0,
        max: maxVal,
        from: 0,
        to: maxVal,
        values: myTimes
    });
}
function pintaGrafica() {

    showWidget(0);

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

    //Mostramos el resumen
    mostrarResumen();

    //Muestra la barra temporal inferior
    mostrarTimeSlider();

    //mostramos la gráfica con los resultados
    drawChart(null, null);

    $('#desktop-message-warning').kpimessage('hide');

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

    objTable = new google.visualization.Table(document.getElementById('graphic-table'));
    objTable.draw(myView, {
        'page': 'enable',
        'pageSize': 20
    });
}

/***********************************************************************************************************
**                         Funciónes para insertar/eliminar elementos                                     **
***********************************************************************************************************/
function InsertaDashboard() {
    if (!dashboardObject.getIdDashboard()) {
        $.fn.returnAddOK = function (response) {
            $('#txtNewDashboardName').val('');
            var dashboardlist = $('nav.dashboards').children('ul');
            var newDashboard = $('<li>').dashboardItem({
                data: response,
                editar: function (event, data) {
                    dashboardObject.setIdDashboard(data);
                    $('#frmAddDashboard').modal('show');
                },
                eliminar: function (event, data) {
                    EliminaDashboard(data);
                },
                insertar: function (event, data) {
                    InsertaWidget(data);
                }
            });

            newDashboard.jarvismenuitem({
                accordion: true,
                speed: $.menu_speed,
                closedSign: '<em class="fa fa-plus-square-o"></em>',
                openedSign: '<em class="fa fa-minus-square-o"></em>'
            });

            dashboardlist.append(newDashboard);

            $('#frmAddDashboard').modal('hide');
        };
        $.fn.returnAddCancel = function (response) {
            alert('Algo salió mal');
            $('#frmAddDashboard').modal('hide');
        };
        dashboardObject.setTitulo($('#txtNewDashboardName').val());
        dashboardObject.insert($(this).returnAddOK, $(this).returnAddCancel);
    }
    else {
        $.fn.returnEditOK = function (response) {
            $('#txtNewDashboardName').val('');
            $('li.dashboards-items').filter(function () {
                return $(this).data("iddashboard") == response.dashboardid;
            }).find('a.dashboard-menu-item > span').text(response.titulo);

            $('#frmAddDashboard').modal('hide');
        };
        $.fn.returnEditCancel = function (response) {
            alert('Algo salió mal');
            $('#frmAddDashboard').modal('hide');
        };
        dashboardObject.setTitulo($('#txtNewDashboardName').val());
        dashboardObject.update($(this).returnEditOK, $(this).returnEditCancel);
    }
}
function EliminaDashboard(iddashboard) {
    $.SmartMessageBox({
        title: '<i class="fa fa-times" style="color:#ed1c24"></i>&nbsp;Remove dashboard?',
        content: "Warning: This action cannot be undone",
        buttons: '[No][Yes]'
    }, function (ButtonPressed) {
        if (ButtonPressed === "Yes") {
            $.fn.returnOK = function (response) {
                $('li.dashboards-items').filter(function () {
                    return $(this).data("iddashboard") == response.d;
                }).remove();
                $.smallBox({
                    title: "Delete complete",
                    content: "<i class='fa fa-clock-o'></i> <i>1 dashboard successfully deleted</i>",
                    color: "#659265",
                    iconSmall: "fa fa-check fa-2x fadeInRight animated",
                    timeout: 4000
                });
                showWidget(3);
            };
            $.fn.returnNO = function (response) {
                $.smallBox({
                    title: "Cannot delete dashboard",
                    content: "<i class='fa fa-clock-o'></i> <i>Sorry but we couldn't delete the selected dashboard</i>",
                    color: "#C46A69",
                    iconSmall: "fa fa-times fa-2x fadeInRight animated",
                    timeout: 4000
                });
            };
            dashboardObject.setIdDashboard(iddashboard);
            dashboardObject.remove($(this).returnOK, $(this).returnNO);
        }
    });
}
function InsertaWidget(iddashboard) {
    $.fn.returnOK = function (response) {
        var dashboardlist = $('ul.widget-list').filter(function () {
            return $(this).data("iddashboard") == response.iddashboard;
        });
        var newWidget = $('<li>').widgetItem({
            data: response,
            onchange: function (event, data) {
                muestraWidget(data);
            }
        });
        dashboardlist.append(newWidget);
    };
    $.fn.returnNO = function (response) {
        alert('Algo salió mal');
    };
    widgetObject.setIdDashboard(iddashboard);
    widgetObject.setTitulo('Untitled');
    widgetObject.insert($(this).returnOK, $(this).returnNO);
}
function eliminaWidget() {

    if (widgetObject.getIdWidget()) {
        $.SmartMessageBox({
            title: '<i class="fa fa-times" style="color:#ed1c24"></i>&nbsp;Delete graph?',
            content: "Warning: This action cannot be undone",
            buttons: '[No][Yes]'
        }, function (ButtonPressed) {
            if (ButtonPressed === "Yes") {
                $.fn.returnOK = function (response) {
                    $('a.widget-menu-item').filter(function () {
                        return $(this).data("idwidget") == response.d;
                    }).parent().remove();
                    $.smallBox({
                        title: "Delete complete",
                        content: "<i class='fa fa-clock-o'></i> <i>1 graph successfully deleted</i>",
                        color: "#659265",
                        iconSmall: "fa fa-check fa-2x fadeInRight animated",
                        timeout: 4000
                    });
                    showWidget(3);
                };
                $.fn.returnNO = function (response) {
                    $.smallBox({
                        title: "Cannot delete graph",
                        content: "<i class='fa fa-clock-o'></i> <i>Sorry but we couldn't delete the graph</i>",
                        color: "#C46A69",
                        iconSmall: "fa fa-times fa-2x fadeInRight animated",
                        timeout: 4000
                    });
                };
                widgetObject.remove($(this).returnOK, $(this).returnNO);
            }
        });
    }
}
function renameWidget(newtitulo) {
    widgetObject.setTitulo(newtitulo);

    $.fn.returnOK = function (response) {
        $('a.widget-menu-item').filter(function () {
            return $(this).data("idwidget") == response;
        }).html('<span class="menu-item-parent max-width-dashboard">' + newtitulo + '</span>');
        $.smallBox({
            title: "Rename complete",
            content: "<i class='fa fa-clock-o'></i> <i>1 graph successfully renamed</i>",
            color: "#659265",
            iconSmall: "fa fa-check fa-2x fadeInRight animated",
            timeout: 4000
        });
        $('.jarviswidget-editbox').removeClass('display-block');
    };
    $.fn.returnNO = function (response) {
        $.smallBox({
            title: "Cannot rename graph",
            content: "<i class='fa fa-clock-o'></i> <i>Sorry but we couldn't rename the graph</i>",
            color: "#C46A69",
            iconSmall: "fa fa-times fa-2x fadeInRight animated",
            timeout: 4000
        });
    };
    widgetObject.update($(this).returnOK, $(this).returnNO);
}
function InsertaFormula() {

    var sformula = $('#txtValorFormula').returnTags();
    formulaObject.setNombre($('#txtNombreFormula').val());
    formulaObject.setColor($('#txtFormulaColor').val());
    formulaObject.setOriginal(sformula);
    if (formulaObject.getIdFormula() == -1) {
        $.fn.returnOK = function (response) {
            $('#graphic-leyend').formulaItem('addItem', response);
            $('#frmAddFormula').modal('hide');
            pintaGrafica();
        };
        $.fn.returnNO = function (response) {
            alert('Algo salió mal');
        };
        formulaObject.insert($(this).returnOK, $(this).returnNO);
    }
    else {
        $.fn.returnOK = function (response) {
            $('#graphic-leyend').formulaItem('updItem', response);
            $('#frmAddFormula').modal('hide');
            pintaGrafica();
        };
        $.fn.returnNO = function (response) {
            alert('Algo salió mal');
        };
        formulaObject.update($(this).returnOK, $(this).returnNO);
    }
}

/***********************************************************************************************************
**                                      OTRAS FUNCIONES                                                   **
***********************************************************************************************************/
//Funcion para convertir los Datos a formato interoperable CSV
function dataTableToCSV(dataTable_arg) {
    var dt_cols = dataTable_arg.getNumberOfColumns();
    var dt_rows = dataTable_arg.getNumberOfRows();
    var csv_cols = [];
    var csv_out;
    // Iterate columns
    for (var i = 0; i < dt_cols; i++) {
        // Replace any commas in column labels
        csv_cols.push(dataTable_arg.getColumnLabel(i).replace(/,/g, ""));
    }
    // Create column row of CSV
    csv_out = csv_cols.join(",") + "\r\n";
    // Iterate rows
    for (i = 0; i < dt_rows; i++) {
        var raw_col = [];
        for (var j = 0; j < dt_cols; j++) {
            // Replace any commas in row values
            raw_col.push(dataTable_arg.getFormattedValue(i, j, 'label').replace(/,/g, ""));
        }
        // Add row to CSV text
        csv_out += raw_col.join(",") + "\r\n";
    }
    return csv_out;
}

//Comprueba si se han cumplido los pre-requisitos para insertar una fórmula                     
function validarFormula() {
    var inputNombre = $('#txtNombreFormula');
    if ((!inputNombre.val()) || (inputNombre.val().length == 0)) {
        inputNombre.parent().addClass('has-error');
        return false;
    }
    return true;
}

/***********************************************************************************************************
**                                      INICIO DE LA PÁGINA                                               **
***********************************************************************************************************/
$(document).ready(function () {
    $('#imgDesktop').addClass("fadeIn");

    $("a[elem='desktop']").css("color", "#759dae;");
    $(window).resize(function () {
        drawChart();
    });
    //Función para pintar cada widget de la lista
    $('.widget-menu-item').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        //Llamamos al pintado del widget
        muestraWidget($(this).data('idwidget'));
    });

    $('.add-dashboard').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        dashboardObject.setIdDashboard(null);
        $('#frmAddDashboard').modal('show');
    });
    $('.edit-dashboard').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        dashboardObject.setIdDashboard($(this).data('iddashboard'));
        $('#txtNewDashboardName').val($(this).closest('.dashboards-items').find('a.dashboard-menu-item > span').text());
        $('#frmAddDashboard').modal('show');
    });
    $('.delete-dashboard').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        EliminaDashboard($(this).data('iddashboard'));
    });

    $('.insert-widget').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        InsertaWidget($(this).data('iddashboard'));
    });

    $('#btnSaveNewDashboard').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        if ($('#txtNewDashboardName').val()) {
            InsertaDashboard();
        }
    });

    $('nav.dashboards ul li:first').addClass('active open');

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

    $('#cmbFilterIndicators').change(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        var selected = $(this).find('option:selected');
        if (selected) {
            expressionObject.setIdExpresion(selected.val());
            expressionObject.setIdIndicator(selected.data('extra'));
            muestraFiltros();
        }
    });

    //Muestra el diálogo para crear una nueva fórmula
    $('#btnNewFormulaDialog').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        //formulaObject.setIdFormula(null);
        $('#lbNombreDeFormula').text('Define new formula');
        $('#txtNombreFormula').val('');
        $('#txtSearchIndicators').val('');
        $('#txtValorFormula').importTags('');
        formulaObject.setIdFormula(-1);

        $('#frmAddFormula').modal('show');
    });

    //Muestra el diálogo para crear una nueva fórmula
    $('#btnRenameWidget').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        $('.jarviswidget-editbox').addClass('display-block');
    });
    $('#btnSaveRenameWidget').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        renameWidget($('#txtRenameWidget').val());
    });
    $('#btnCancelRenameWidget').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        $('.jarviswidget-editbox').removeClass('display-block');
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
            var result = dataTableToCSV(datos);
            var titulo = $.trim($('.widget-title').text());

            downloadCSV(result, titulo);
        }
    });

    //Elimina el widget actual
    $('#btnDeleteWidget').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        eliminaWidget();
    });

    //Comparte el widget actual
    $('#btnShareWidget').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        var idwidget = widgetObject.getIdWidget();
        if (idwidget) {
            var newURL = window.location.protocol + "//" + window.location.host
            $('#txtShareWidget').val('<iframe style="width: 600px; height: 400px; border: 1px solid #cdcdcd;" src="' + newURL + '/kpiboard.aspx?widgetid=' + idwidget + '" />');

            $('#frmShareWidget').modal('show');
        }
    });

    //Generar el código del iFrame
    $('#btnGenerateiFrame').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        var newURL = window.location.protocol + "//" + window.location.host
        var idwidget = widgetObject.getIdWidget();
        if (!idwidget) {
            return;
        }
        var width = $('#txtiFrameWidth').val();
        if (!width) {
            $('#txtiFrameWidth').closest('.form-group').addClass('has-error');
            $('#txtiFrameWidth').focus();
            return;
        }
        var height = $('#txtiFrameHeight').val();
        if (!height) {
            $('#txtiFrameHeight').closest('.form-group').addClass('has-error');
            $('#txtiFrameHeight').focus();
            return;
        }
        $('#txtShareWidget').val('<iframe style="width: ' + width + 'px; height: ' + height + 'px; border: 1px solid #cdcdcd;" src="' + newURL + '/kpiboard.aspx?widgetid=' + idwidget + '" />');
    });

    //Añade un indicador a la fórmula
    $('#btnAddIndicator').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        if (indicatorObject.getIndicatorId()) {
            var data = { 'indicatorid': indicatorObject.getIndicatorId(), 'titulo': indicatorObject.getTitulo(), 'tipo': $('#lstAggreggateFunction').val(), 'expresionid': -1 };
            $('#txtValorFormula').addTag(data);
        }
    });

    //Limpia el buscador de indicadores
    $('#btnClearIndicator').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        $('#txtSearchIndicators').val('');
    });

    //Inserta una nueva fórmula en la gráfica
    $('#btnSaveFormula').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        if (validarFormula()) {
            InsertaFormula();
        }
    });
    //Cierra la ventana modal y refresca los datos del widget
    $('#btnExitFilter').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        $('#frmAddFilter').modal('hide');
        pintaGrafica();
    });

    $('#txtSearchFriends').keypress(function (event) {
        if (event.which == 13) {
            fillFriends($(this).val());

            //Prevents the default action to be triggered. 
            event.preventDefault();
        }
    });

    $("#graphic-leyend").draggable();

    $('#time-slider').ionRangeSlider({
        type: 'double',
        hasGrid: true,
        values: ["a", "b", "c", "d", "e", "f", "g"],
        from: 3,
        to: 5,
        onFinish: function (obj) {
            drawChart(obj.fromNumber, obj.toNumber)
        }
    });

    //Libreria para la selección de un rango de fechas
    $('#txtDateRange').datepicker({
        numberOfMonths: 3,
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        dateFormat: "dd-mm-yy",
        onSelect: function (dateText, inst) {
            var d1, d2;

            if (primerClick) {
                prv = -1;
                cur = (new Date(inst.selectedYear, inst.selectedMonth, inst.selectedDay)).getTime();
            }
            else {
                prv = cur;
                cur = (new Date(inst.selectedYear, inst.selectedMonth, inst.selectedDay)).getTime();
            }

            if (prv == -1 || prv == cur) {
                prv = cur;
                $('#txtDateRange').val(dateText);
                widgetObject.setFechaIni(dateText);
                widgetObject.setFechaFin(dateText);
            } else {
                d1 = $.datepicker.formatDate('dd/mm/yy', new Date(Math.min(prv, cur)), {});
                d2 = $.datepicker.formatDate('dd/mm/yy', new Date(Math.max(prv, cur)), {});
                $('#txtDateRange').val(d1 + ' - ' + d2);
                widgetObject.setFechaIni(d1);
                widgetObject.setFechaFin(d2);
            }

            primerClick = !primerClick;

            pintaGrafica();
        },
        beforeShowDay: function (date) {
            return [true, ((date.getTime() >= Math.min(prv, cur) && date.getTime() <= Math.max(prv, cur)) ? 'date-range-selected' : '')];
        }
    });

    //Librería para la introducción de la fórmula
    $('#txtValorFormula').tagsInput({
        'unique': false,
        'width': '100%',
        'height': '200px'
    });

    //Librería para la búsqueda de indicadores
    $('#txtSearchIndicators').typeahead({
        source: function (query, process) {
            $.fn.returnOK = function (result) {
                map = {};
                states = [];
                $.map(result, function (data, item) {
                    map[data.titulo] = data.indicatorid;
                    states.push(data.titulo);
                });
                process(states);
            };
            $.fn.returnNO = function (response) {
                $('#txtSearchIndicators').parent().addClass('has-error');
            };
            var dataString = "{nombre:'" + query + "'}";
            proxy.getAnyIndicators(dataString, $(this).returnOK, $(this).returnNO);
        },
        updater: function (item) {
            $('#txtSearchIndicators').parent().removeClass('has-error');
            indicatorObject.setIdIndicator(map[item]);
            indicatorObject.setTitulo(item);
            return item;
        },
        minLength: 3,
        limit: 10
    });

    /*Especificación del Color*/
    $('.pickercolor').colorpicker();

    //Botón que se activa para añadir un nuevo filtro
    $('.kpi-new-filter').newfilter({
        aceptar: function (event, data) {
            $.fn.displayFilter = function (response) {
                var dimensionItems = $('#cmbDimensiones').kpicombo("getData", null);
                var operatorItems = $('#cmbOperadores').kpicombo("getData", null);
                $('<div>').appendTo('#filter-list').kpieditfilter({
                    data: response,
                    cmbDim: dimensionItems,
                    cmbOpe: operatorItems,
                    eliminar: function (event, data) {
                        var dataString = "{filtroid:" + data.idfilter + "}";
                        proxy.delFilters(dataString, null, null);
                    },
                    aceptar: function (event, data) {
                        var dataString = "{filtroid:" + data.idfilter + ", dimensionid:" + data.dimid + ", filtro :'" + data.dimop + "', valor : '" + data.value + "'}";
                        proxy.updFilters(dataString, null, null);
                    }
                });
            };

            filterObject.setExpresionId(expressionObject.getIdExpresion());
            filterObject.setDimensionId(data.dimid);
            filterObject.setFiltro(data.dimop);
            filterObject.setValor(data.value);
            filterObject.insert($(this).displayFilter, null);

            $('.kpi-new-filter').newfilter('restore', null);
        }
    });

    $('#desktop-message-warning').kpimessage();

    // open chat list
    $.chat_list_btn = $('#chat-container > .chat-list-open-close');
    $.chat_list_btn.click(function () {
        $(this).parent('#chat-container').toggleClass('open');
    })

    //Para la demo
    fillMap();
    
    pageSetUp(); //IMPORTANTE!!
});

$(window).scroll(function () {
    var topOfWindow = $(window).scrollTop();

    var imagePos1 = $('#imgColLeft').offset().top;
    var imagePos2 = $('#imgColMed').offset().top;
    var imagePos3 = $('#imgColRight').offset().top;
    var imagePos4 = $('#imgiFrame').offset().top;

    if (imagePos1 < topOfWindow + 600) {
        $('#imgColLeft').addClass("slideRight");
    };
    if (imagePos2 < topOfWindow + 600) {
        $('#imgColMed').addClass("fadeIn");
    };
    if (imagePos3 < topOfWindow + 600) {
        $('#imgColRight').addClass("slideLeft");
    };
    if (imagePos4 < topOfWindow + 600) {
        $('#imgiFrame').addClass("fadeIn");
    };
});