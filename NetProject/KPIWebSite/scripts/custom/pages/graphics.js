// global variables to track the date range
var cur = -1,
    prv = -1,
    primerClick = true;

var opciones, datos;

var graphic = {
    iddashboard: -1,
    idwidget: -1,
    altura: 500,
    tipografico: undefined,
    dimension: undefined,
    fechaini: undefined,
    fechafin: undefined,
    chart: undefined,
    type_dimensiones: ["D", "s", "Q", "M", "T", "S", "A"],
    type_tipografico: ["B", "H", "L", "A", "Q", "T"],
    colors: [],
    hiddencols: [],
    hiddenrows: [],
    esvalido: function () {
        if (this.idwidget == -1)
            return false;

        if (!this.tipografico)
            return false;

        if (this.type_tipografico.indexOf(this.tipografico) == -1)
            return false;

        if (!this.dimension)
            return false;

        if (this.type_dimensiones.indexOf(this.dimension) == -1)
            return false;

        return true;
    }
};

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

$(document).ready(function () {
    /*Mostramos la opción del menú principal actual*/
    $("a[elem='escritorio']").css("color", "#fff");

    /*Tooltips*/
    $(document).tooltip();

    /*Ampliamos el largo de la barra hasta el pie de la página y animamos el panel para que se despliegue horizontalmente*/
    $('#graphic-menu').height($(document).height());
    setTimeout(function () { $('#graphic-menu').css('left', '-222px'); }, 4000);

    /*Definimos el boton para introducir nuevos dashboards */
    $('#btnAddDashboard').click(function (event) {
        event.preventDefault();
        if ($('#txtAddDashboard').val()) {

            $.fn.returnOK = function (response) {
                var newDashboard = $('<div>').attr('class', 'dashboard-menu-item').attr('iddashboard', response.dashboardid);
                var newWidgetlist = $('<ul>').attr('class', 'widgetsmenuitem');
                var spantext = $('<span>').text(response.titulo);
                newDashboard.append(spantext);

                $('nav.dashboards').append(newDashboard);
                $('nav.dashboards').append(newWidgetlist);
                $('#txtAddDashboard').val('');

                newDashboard.kpidashboards({
                    mostrar: function (event, data) {
                        $(this).next().slideToggle('fast');
                    },
                    editar: function (event, data) {
                        $(this).kpidashboards("updItem", data);
                    },
                    insertar: function (event, data) {
                        $('#frmAddWidget').bPopup({
                            follow: [false, false],
                            easing: 'easeOutBack',
                            speed: 450,
                            transition: 'slideDown',
                            onOpen: function () {
                                graphic.iddashboard = data.iddashboard;
                                $('#txtNombreWidget').val('');
                            }
                        })
                    },
                    guardar: function (event, data) {

                        $.fn.returnOK = function (response) {
                            $('.dashboard-menu-item span').filter(function () {
                                return ($(this).data('iddashboard') == response.dashboardid)
                            }).attr('contenteditable', 'false').css('border', 'none').removeClass('cursor-text').addClass('pointer').blur();
                            $('.save-dashboard-button').remove();
                            $('.edit-dashboard-button').css('display', 'inline-block');
                        };
                        $.fn.returnNO = function (response) {
                            alert('Algo salió mal');
                        };
                        dashboardObject.setIdDashboard(data.iddashboard);
                        dashboardObject.setTitulo($(this).find('span').text());
                        dashboardObject.update($(this).returnOK, $(this).returnNO);
                    },
                    eliminar: function (event, data) {
                        if (confirm('¿Seguro que desea eliminar este cuadro de mandos?')) {
                            $.fn.returnOK = function (response) {
                                $(".dashboard-menu-item").each(function (indice, valor) {
                                    if ($(this).attr('iddashboard') == response.d) {
                                        $(this).next(".widgetsmenuitem").remove();
                                        $(this).remove();
                                    }
                                });
                            };
                            $.fn.returnNO = function (response) {
                                alert('Algo salió mal');
                            };
                            dashboardObject.remove(data, $(this).returnOK, $(this).returnNO);
                        }
                    }
                });
            };
            $.fn.returnNO = function (response) {
                alert('Algo salió mal');
            };
            dashboardObject.setTitulo($('#txtAddDashboard').val());
            dashboardObject.insert($(this).returnOK, $(this).returnNO);
        }
    });

    //Botón que se activa para añadir un nuevo gráfico/widget
    $('#btnAddWidget').click(function () {
        var nombre = $('#txtNombreWidget').val();

        $.fn.returnOK = function (response) {
            var widgetlist = $('.dashboard-menu-item[iddashboard="' + response.iddashboard + '"]').next(".widgetsmenuitem");
            var widgetItem = $('<li>').kpiwidgets({ datos: response,
                mostrar: function (event, data) {
                    muestraGrafica(data);
                },
                eliminar: function (event, data) {
                    if (confirm('¿Seguro que desea eliminar esta gráfica?')) {
                        eliminaGrafica(data, $(this));
                    }
                }
            });
            widgetlist.append(widgetItem);
            $('#frmAddWidget').bPopup().close();
        };
        $.fn.returnNO = function (response) {
            alert('Algo salió mal');
        };
        widgetObject.setIdDashboard(graphic.iddashboard);
        widgetObject.setTitulo(nombre);
        widgetObject.insert($(this).returnOK, $(this).returnNO);
    });

    /*Guardamos los nuevos datos de la fórmula*/
    $('#btnSaveFormula').click(function (event) {
        if (validarFormula()) {
            var sformula = $('#txtValorFormula').returnTags();
            formulaObject.setNombre($('#txtNombreFormula').val());
            formulaObject.setColor($('#txtFormulaColor').val());
            formulaObject.setOriginal(sformula);
            if (formulaObject.getIdFormula() == -1) {
                $.fn.returnOK = function (response) {
                    $('.graphic-indicators-content').kpiformulas('addItem', response);
                    $('#graphic-formula').toggle('slide', { direction: 'left' }, 1000);
                    pintaGrafica();
                };
                $.fn.returnNO = function (response) {
                    alert('Algo salió mal');
                };
                formulaObject.insert($(this).returnOK, $(this).returnNO);
            }
            else {
                $.fn.returnOK = function (response) {
                    $('.graphic-indicators-content').kpiformulas('updItem', response);
                    $('#graphic-formula').toggle('slide', { direction: 'left' }, 1000);
                    pintaGrafica();
                };
                $.fn.returnNO = function (response) {
                    alert('Algo salió mal');
                };
                formulaObject.update($(this).returnOK, $(this).returnNO);
            }
        }
    });

    /*Cancelamos la actualización/inserción de una fórmula*/
    $('#btnCancelFormula').click(function (event) {
        $('#graphic-formula').toggle('slide', { direction: 'left' }, 1000);
    });

    /*Limpiamos los datos de la fórmula*/
    $('#btnCleanFormula').click(function (event) {
        $('#txtValorFormula').cleanTags();
    });

    /*Definimos el boton para buscar indicadores */
    $('.find-indicators').change(function () {
        $.fn.displayIndicators = function (response) {
            $("#graphic-list-indicators").kpiindicators({ datos: response,
                misdatos: function (event, data) {
                    $('#txtValorFormula').addTag(data);
                },
                medias: function (event, data) {
                    $('#txtValorFormula').addTag(data);
                },
                maximos: function (event, data) {
                    $('#txtValorFormula').addTag(data);
                },
                minimos: function (event, data) {
                    $('#txtValorFormula').addTag(data);
                }
            });
        };
        $.fn.hideIndicators = function (response) {
            $("#graphic-list-indicators").kpiindicators({ datos: null });
        };
        var dataString = "{nombre:'" + $(this).val() + "'}";
        proxy.getAnyIndicators(dataString, $(this).displayIndicators, $(this).hideIndicators);
    });

    /*Definimos el selector de rango de fechas */
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
                graphic.fechaini = dateText;
                graphic.fechafin = dateText;
            } else {
                d1 = $.datepicker.formatDate('dd/mm/yy', new Date(Math.min(prv, cur)), {});
                d2 = $.datepicker.formatDate('dd/mm/yy', new Date(Math.max(prv, cur)), {});
                $('#txtDateRange').val(d1 + ' - ' + d2);
                graphic.fechaini = d1;
                graphic.fechafin = d2;
            }

            primerClick = !primerClick;

            pintaGrafica();
        },

        beforeShowDay: function (date) {
            return [true, ((date.getTime() >= Math.min(prv, cur) && date.getTime() <= Math.max(prv, cur)) ? 'date-range-selected' : '')];
        }
    });

    /*Botonera de opciones*/
    $("#btnViewFormulas").button({ icons: { primary: "ui-icon-view-formulas" }, text: false }).click(function (event) {
        event.preventDefault();
        $('#graphic-container-left').slideToggle(500, function () {
            drawChart();
        });
    });
    $("#btnAddFormulas").button({ icons: { primary: "ui-icon-new-formulas" }, text: false }).click(function (event) {
        event.preventDefault();
        formulaObject.setIdFormula(null);
        $('#txtNombreFormula').val('');
        $('#txtValorFormula').importTags('');
        formulaObject.setIdFormula(-1);
        if (!$("#graphic-formula").is(":visible")) {
            $('#graphic-formula').toggle('slide', { direction: 'left' }, 500);
        }
    });
    $("#btnExportCsv").button({ icons: { primary: "ui-icon-export-csv" }, text: false }).click(function (event) {
        event.preventDefault();
        var result = dataTableToCSV(datos);
        var titulo = $.trim($('ul.widgetsmenuitem li.active').find('a:first').text());
        downloadCSV(result, titulo);
    });
    $("#btnRefrescar").button({ icons: { primary: "ui-icon-refresh" }, text: false }).click(function (event) {
        event.preventDefault();
        pintaGrafica();
    });
    $("#btnCompartir").button({ icons: { primary: "ui-icon-share" }, text: false }).click(function (event) {
        event.preventDefault();
        $('#frmShareWidget').bPopup({
            follow: [false, false],
            easing: 'easeOutBack',
            speed: 450,
            transition: 'slideDown',
            onOpen: function () {
                var newURL = window.location.protocol + "//" + window.location.host
                $('#txtShareWidget').val('<iframe style="width: 800px; height: 700px; border: 3px solid purple;" src="' + newURL + '/kpiboard.aspx?widgetid=' + graphic.idwidget + '" />');
            }
        })
    });

    $("#graphicbuttons").buttonset();
    $("#btnBarChart").button({ icons: { primary: 'ui-icon-barchart' }, text: false }).click(function (event) {
        event.preventDefault();
        graphic.tipografico = $(this).attr('value');
        pintaGrafica();
    });
    $("#btnLineChart").button({ icons: { primary: 'ui-icon-linechart' }, text: false }).click(function (event) {
        event.preventDefault();
        graphic.tipografico = $(this).attr('value');
        pintaGrafica();
    });
    $("#btnAreaChart").button({ icons: { primary: 'ui-icon-areachart' }, text: false }).click(function (event) {
        event.preventDefault();
        graphic.tipografico = $(this).attr('value');
        pintaGrafica();
    });
    $("#btnPieChart").button({ icons: { primary: 'ui-icon-piechart' }, text: false }).click(function (event) {
        event.preventDefault();
        graphic.tipografico = $(this).attr('value');
        pintaGrafica();
    });
    $("#btnHistogram").button({ icons: { primary: 'ui-icon-histogram' }, text: false }).click(function (event) {
        event.preventDefault();
        graphic.tipografico = $(this).attr('value');
        pintaGrafica();
    });
    $("#btnTable").button({ icons: { primary: 'ui-icon-table' }, text: false }).click(function (event) {
        event.preventDefault();
        graphic.tipografico = $(this).attr('value');
        pintaGrafica();
    });
    $("#timebuttons").buttonset();
    $("#btnDia").button().click(function (event) {
        event.preventDefault();
        graphic.dimension = $(this).attr('value');
        pintaGrafica();
    });
    $("#btnSemana").button().click(function (event) {
        event.preventDefault();
        graphic.dimension = $(this).attr('value');
        pintaGrafica();
    });
    $("#btnQuincena").button().click(function (event) {
        event.preventDefault();
        graphic.dimension = $(this).attr('value');
        pintaGrafica();
    });
    $("#btnMes").button().click(function (event) {
        event.preventDefault();
        graphic.dimension = $(this).attr('value');
        pintaGrafica();
    });
    $("#btnTrimestre").button().click(function (event) {
        event.preventDefault();
        graphic.dimension = $(this).attr('value');
        pintaGrafica();
    });
    $("#btnSemestre").button().click(function (event) {
        event.preventDefault();
        graphic.dimension = $(this).attr('value');
        pintaGrafica();
    });
    $("#btnAnual").button().click(function (event) {
        event.preventDefault();
        graphic.dimension = $(this).attr('value');
        pintaGrafica();
    });

    /* Menu lateral de los Dashboards */
    $('.dashboard-menu-item').kpidashboards({
        mostrar: function (event, data) {
            $(this).next().slideToggle('fast');
        },
        editar: function (event, data) {
            $(this).kpidashboards("updItem", data);
        },
        insertar: function (event, data) {
            $('#frmAddWidget').bPopup({
                follow: [false, false],
                easing: 'easeOutBack',
                speed: 450,
                transition: 'slideDown',
                onOpen: function () {
                    graphic.iddashboard = data.iddashboard;
                    $('#txtNombreWidget').val('');
                }
            })
        },
        guardar: function (event, data) {
            $.fn.returnOK = function (response) {
                $('.dashboard-menu-item span').filter(function () {
                    return ($(this).data('iddashboard') == response.dashboardid)
                }).attr('contenteditable', 'false').css('border', 'none').removeClass('cursor-text').addClass('pointer').blur();
                $('.save-dashboard-button').remove();
                $('.edit-dashboard-button').css('display', 'inline-block');
            };
            $.fn.returnNO = function (response) {
                alert('Algo salió mal');
            };
            dashboardObject.setIdDashboard(data.iddashboard);
            dashboardObject.setTitulo($(this).find('span').text());
            dashboardObject.update($(this).returnOK, $(this).returnNO);
        },
        eliminar: function (event, data) {
            if (confirm('¿Seguro que desea eliminar este cuadro de mandos?')) {
                $.fn.returnOK = function (response) {
                    $(".dashboard-menu-item").each(function (indice, valor) {
                        if ($(this).attr('iddashboard') == response.d) {
                            $(this).next(".widgetsmenuitem").remove();
                            $(this).remove();
                        }
                    });
                };
                $.fn.returnNO = function (response) {
                    alert('Algo salió mal');
                };
                dashboardObject.remove(data, $(this).returnOK, $(this).returnNO);
            }
        }
    });

    /* Submenu lateral de los gráficos */
    $('.widget-menu-item').kpiwidgets({
        mostrar: function (event, data) {
            muestraGrafica(data);
        },
        eliminar: function (event, data) {
            if (confirm('¿Seguro que desea eliminar esta gráfica?')) {
                eliminaGrafica(data, $(this));
            }
        }
    });

    /*Especificación del Color*/
    $('#FormulaColor').ColorPicker({
        color: '#993300',
        onShow: function (colpkr) {
            $(colpkr).fadeIn(500);
            return false;
        },
        onHide: function (colpkr) {
            $(colpkr).fadeOut(500);
            return false;
        },
        onChange: function (hsb, hex, rgb) {
            $('#FormulaColor div').css('backgroundColor', '#' + hex);
            $('#txtFormulaColor').val(hex);
        }
    });

    //Librería para la introducción de la fórmula
    $('#txtValorFormula').tagsInput({
        'unique': false,
        'width': '100%',
        'height': '200px'
    });

    //Librería para el dibujado de los porcentajes
    $(".userPercent").knob({
        readOnly: true,
        width: 75,
        height: 75,
        thickness: .2,
        fgColor: '#415360',
        'draw': function () {
            $(this.i).val(this.cv + '%')
        }
    });
    $(".datasetPercent").knob({
        readOnly: true,
        width: 75,
        height: 75,
        thickness: .2,
        fgColor: '#759dae',
        'draw': function () {
            $(this.i).val(this.cv + '%')
        }
    });
    $(".dataPercent").knob({
        readOnly: true,
        width: 75,
        height: 75,
        thickness: .2,
        fgColor: '#FC0547',
        'draw': function () {
            $(this.i).val(this.cv + '%')
        }
    });

    //Botón que se activa para añadir un nuevo filtro
    $('.kpi-new-filter').newfilter({
        aceptar: function (event, data) {
            $.fn.displayFilter = function (response) {
                var dimensionItems = $('#cmbDimensiones').kpicombo("getData", null);
                var operatorItems = $('#cmbOperadores').kpicombo("getData", null);
                $('<div>').appendTo('#filter-list').kpieditfilter({ data: response,
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
            var dataString = "{idexpresion: " + $('#graphic-list-expresions').find('div.selected').data('idexpresion') + ", dimensionid:" + data.dimid + ", filtro :'" + data.dimop + "', valor : '" + data.value + "'}";
            proxy.addFilters(dataString, $(this).displayFilter, null);
            $('.kpi-new-filter').newfilter('restore', null);
        }
    });

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
});

//--------------------------------------------------------------------
//Funciones para animar la barra de progreso durante la carga
//--------------------------------------------------------------------
function ShowProgressAnimation() {
    var background = $('.widget-loading');
    background.show();
}
function CloseProgressAnimation() {
    var background = $('.widget-loading');
    background.hide();
}

//--------------------------------------------------------------------
//Funciones para mostrar/ocultar el gráfico
//--------------------------------------------------------------------
function ShowEmptyGraphic() {
    var background = $('.graphic-empty');
    background.show();
}
function CloseEmptyGraphic() {
    var background = $('.graphic-empty');
    background.hide();
}

//--------------------------------------------------------------------
//Funcion para mostrar todos los indicadores utilidados por el widget
//--------------------------------------------------------------------
function rellenaFormulas(idwidget) {
    $.fn.displayFormulas = function (response) {
        $('.graphic-indicators-content').kpiformulas({ datos: response,
            checked: function (event, data) {
                drawChart();
            },
            editar: function (event, data) {
                if ((data.formulaid == formulaObject.getIdFormula()) || !$('#graphic-formula').is(':visible'))
                    $('#graphic-formula').toggle('slide', { direction: 'left' }, 500);
                formulaObject.populate(data.formulaid);
                $('#txtNombreFormula').val(formulaObject.getNombre());
                $('#txtValorFormula').importTags(formulaObject.getOriginal());
                $('#txtFormulaColor').val(formulaObject.getColor());
                $('#FormulaColor div').css('backgroundColor', '#' + formulaObject.getColor());
            },
            eliminar: function (event, data) {

                $.fn.returnOK = function (response) {
                    $('.graphic-indicators-content div').filter(function () {
                        return ($(this).data('formulaid') == response.d)
                    }).remove();
                    pintaGrafica();
                };
                $.fn.returnNO = function (response) {
                    alert('Error: rellenaFormulas');
                };
                formulaObject.remove(data, $(this).returnOK, $(this).returnNO);
            },
            filtrar: function (event, data) {
                rellenaExpresiones(data.formulaid);
            }
        });
    };
    $.fn.hideFormulas = function (response) {
        $('.graphic-indicators-content').kpiformulas({ datos: null });
    };
    var dataString = "{widgetid:" + idwidget + "}";
    proxy.getFormulas(dataString, $(this).displayFormulas, $(this).hideFormulas);
}

//--------------------------------------------------------------------
//Funcion para mostrar todas las expresiones de filtro de un indicador
//--------------------------------------------------------------------
function rellenaExpresiones(formulaid) {
    if (!$('#graphic-list-expresions').data('idformula') || ($('#graphic-list-expresions').data('idformula') == formulaid) || (!$('#graphic-list-expresions').is(':visible')))
        $('#graphic-expresions').toggle('slide', { direction: 'left' }, 500);

    $.fn.displayExpresions = function (response) {
        $('#graphic-list-expresions').kpiexpresions({ datos: response });
        $('#graphic-list-expresions').kpiexpresions({ idformula: formulaid });
    };
    $.fn.hideExpresions = function (response) {
        $("#graphic-list-expresions").empty();
    };
    var dataString = "{idformula:" + formulaid + "}";
    proxy.getExpresions(dataString, $(this).displayExpresions, $(this).hideExpresions);

    //dibujamos la fórmula de la primera expresión
    formulaObject.populate(formulaid);
    $('#formulagraphic').text(formulaObject.getDisplay());
    var miSpan = document.getElementById('formulagraphic');
    M.parseMath(miSpan);

    var primero = $('#graphic-list-expresions').find('div:first-child');
    if (primero) {
        rellenaFiltros(primero);
    }
}

//--------------------------------------------------------------------
//Funcion para mostrar todas los filtros de una expresion
//--------------------------------------------------------------------
function rellenaFiltros(objeto) {
    var id = objeto.data('idexpresion');
    $('mtext').attr('class', 'negrita');
    $('mtext span').each(function (index) {
        if ($(this).data('id') == id) {
            $(this).parent().prev().attr('class', 'color-terciario-foreground negrita');
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
    var dataString = "{idindicator:" + objeto.data('idindicator') + "}";
    proxy.getDimensions(dataString, $(this).displayDimensions, $(this).hideDimensions);

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
            $('<div>').appendTo('#filter-list').kpieditfilter({ data: item,
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
    dataString = "{idexpresion:" + objeto.data('idexpresion') + "}";
    proxy.getFilters(dataString, $(this).displayFilters, $(this).errorFilters);
}

/******************************************************************************************************
**  Comprueba si se han cumplido los pre-requisitos para insertar una fórmula                     **
*******************************************************************************************************/
function validarFormula() {
    var inputNombre = $('#txtNombreFormula');
    if (inputNombre.val()) {
        inputNombre.removeClass('with-error');
    }
    else {
        inputNombre.addClass('with-error');
        inputNombre.next('.mensaje-error').fadeIn().delay(2000).fadeOut();
        return false;
    }
    return true;
}

//---------------------------------------------------------------------
//Lee el widget actual e inicializa todos los botones del gráfico
//---------------------------------------------------------------------
function muestraGrafica(idwidget) {
    //Mostramos la zona de la gráfica
    $('#no-grafico-inside').hide();
    $('#grafico-inside').show();

    //Dibujamos la clase activa actual
    $('ul.widgetsmenuitem li').each(function (index) {
        var hijo = $(this).find('a');
        if (hijo.attr('idwidget') == idwidget)
            $(this).addClass('active');
        else
            $(this).removeClass('active');
    });

    formulaObject.setIdWidget(idwidget);
    widgetObject.setIdWidget(idwidget);

    $.fn.returnOK = function (response) {
        $('#graphic').data('idwidget', response.idwidget);
        $('#txtTituloEdit').val(response.titulo);
        $('#txtDateRange').val(response.fecha_inicio + ' - ' + response.fecha_fin);
        if (response.fecha_inicio) {
            prv = (new Date(response.fecha_inicio)).getTime();
            graphic.fechaini = response.fecha_inicio;
        }
        else {
            prv = -1;
            graphic.fechaini = undefined;
        }

        if (response.fecha_fin) {
            cur = (new Date(response.fecha_fin)).getTime();
            graphic.fechafin = response.fecha_fin;
        }
        else {
            cur = -1;
            graphic.fechafin = undefined;
        }
        var tipografica;
        if (response.grafico == 'B') { tipografica = $('#btnBarChart'); }
        else if (response.grafico == 'L') { tipografica = $('#btnLineChart'); }
        else if (response.grafico == 'A') { tipografica = $('#btnAreaChart'); }
        else if (response.grafico == 'H') { tipografica = $('#btnHistogram'); }
        else if (response.grafico == 'Q') { tipografica = $('#btnPieChart'); }
        else if (response.grafico == 'T') { tipografica = $('#btnTable'); }
        if (tipografica) {
            tipografica[0].checked = true;
            tipografica.button("refresh");
            graphic.tipografico = response.grafico;
        }
        else
            graphic.tipografico = undefined;

        var tipotiempo;
        if (response.dimension == 'D') { tipotiempo = $('#btnDia'); }
        else if (response.dimension == 's') { tipotiempo = $('#btnSemana'); }
        else if (response.dimension == 'Q') { tipotiempo = $('#btnQuincena'); }
        else if (response.dimension == 'M') { tipotiempo = $('#btnMes'); }
        else if (response.dimension == 'T') { tipotiempo = $('#btnTrimestre'); }
        else if (response.dimension == 'S') { tipotiempo = $('#btnSemestre'); }
        else if (response.dimension == 'A') { tipotiempo = $('#btnAnual'); }
        if (tipotiempo) {
            tipotiempo[0].checked = true;
            tipotiempo.button("refresh");
            graphic.dimension = response.dimension;
        }
        else
            graphic.dimension = undefined

                

        //        $('#time-slider').dateRangeSlider({
        //            bounds: {
        //                min: new Date(graphic.fechaini),
        //                max: new Date(graphic.fechafin)
        //            }
        //        });
        //        $('#time-slider').dateRangeSlider("values", new Date(graphic.fechaini), new Date(graphic.fechafin));

        if (response.idwidget) {
            graphic.idwidget = response.idwidget;
        }
        else
            graphic.idwidget = undefined;

        rellenaFormulas(response.idwidget);
        pintaGrafica();
    };
    $.fn.returnFALSE = function (response) {
        $('#grafico-inside').hide();
        $('#no-grafico-inside').show();
    };

    widgetObject.select($(this).returnOK, $(this).returnFALSE);
}

//--------------------------------------------------------------------
//Funcion para eliminar una gráfica cuyo id se envia por parámetro
//--------------------------------------------------------------------
function eliminaGrafica(id, objeto) {
    $.fn.returnOK = function (response) {
        objeto.remove();
    };
    $.fn.returnNO = function (response) {
        alert('Error: eliminaGrafica');
    };

    widgetObject.setIdWidget(id);
    widgetObject.remove($(this).returnOK, $(this).returnNO);
}

//--------------------------------------------------------------------
//Funcion para pintar la gráfica en función de los parámetros definidos
//--------------------------------------------------------------------
function pintaGrafica() {
    if (!graphic.esvalido()) {
        return;
    }

    ShowProgressAnimation();

    if (graphic.tipografico == 'Q') {
        GetPercentData();
    } else if (graphic.tipografico == 'B') {
        GetPercentData();
    } else if (graphic.tipografico == 'L') {
        GetAbsoluteData();
    } else if (graphic.tipografico == 'A') {
        GetAbsoluteData();
    } else if (graphic.tipografico == 'T') {
        GetAbsoluteData();
    } else if (graphic.tipografico == 'H') {
        GetAbsoluteData();
    }

    //Mostramos el resumen
    mostrarResumen();

    //Muestra la barra temporal inferior
    mostrarTimeSlider();

    //mostramos la gráfica con los resultados
    drawChart(null, null);

    CloseProgressAnimation();
}

function GetPercentData() {
    var options = {
        type: 'POST',
        url: '/WebServices.aspx/PercentChart',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: "{pnIdWidget:" + graphic.idwidget + ", psTipo:'" + graphic.tipografico + "', pdDesde:'" + graphic.fechaini + "', pdHasta:'" + graphic.fechafin + "'}",
        async: false,
        success: function (result) {
            if (result.d) {
                datos = new google.visualization.DataTable(JSON.parse(result.d));
            }
            else
                alert('Error: GetPercentData');
        },
        error: function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }
    }
    $.ajax(options);
}
function GetAbsoluteData() {
    var options = {
        type: 'POST',
        url: '/WebServices.aspx/TimeChart',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: "{pnIdWidget:" + graphic.idwidget + ", pDimension:'" + graphic.dimension + "', psTipo:'" + graphic.tipografico + "', pdDesde:'" + graphic.fechaini + "', pdHasta:'" + graphic.fechafin + "'}",
        async: false,
        success: function (result) {
            if (result.d) {
                datos = new google.visualization.DataTable(JSON.parse(result.d));   
            }
            else
                alert('GetAbsoluteData: malo');
        },
        error: function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }
    }
    $.ajax(options);
}

function drawChart(posIni, posFin) {
    $('#graphic-formula').hide();
    $('#graphic-expresions').hide();

    if (datos.getNumberOfColumns() == 1) {
        $('svg').hide();
        return;
    }

    var i = 0;
    var j = 0;
    //creamos la tabla temporal o vista
    var myView = new google.visualization.DataView(datos);

    if ((posIni) && (posFin)) {
        myView.setRows(posIni, posFin);
    }

    graphic.colors.length = 0;
    graphic.hiddencols.length = 0;
    graphic.hiddenrows.length = 0;

    $("div.formula-item").each(function (index) {
        if ($(this).children('input:checkbox').is(':checked')) {
            graphic.colors[i] = '#' + $(this).children('span.categoria-small').data('color');
            i++;
        } else {
            graphic.hiddencols[j] = index + 1;
            graphic.hiddenrows[j] = index;
            j++;
        }
    });
    if (graphic.tipografico == 'Q') {
        opciones = {
            'width': '100%',
            'height': graphic.altura,
            'fontSize': 11,
            'colors': graphic.colors,
            'legend': { 'position': 'bottom' },
            'chartArea': { 'width': '85%', 'height': '70%' },
            'is3D': true,
            'animation':{
                'duration': 1000,
                'easing': 'out'
            }
        };
        graphic.chart = new google.visualization.PieChart(document.getElementById('graphic-chart'));
        myView.hideRows(graphic.hiddenrows);
    } else if (graphic.tipografico == 'B') {
        opciones = {
            'width': '100%',
            'height': graphic.altura,
            'fontSize': 11,
            'colors': graphic.colors,
            'legend': { 'position': 'bottom' },
            'isStacked': 'true',
            'chartArea': { 'width': '85%', 'height': '70%' }
        };
        graphic.chart = new google.visualization.BarChart(document.getElementById('graphic-chart'));
        myView.hideRows(graphic.hiddenrows);
    } else if (graphic.tipografico == 'A') {
        opciones = {
            'width': '100%',
            'height': graphic.altura,
            'fontSize': 11,
            'colors': graphic.colors,
            'legend': { 'position': 'bottom' },
            'pointSize': 5,
            'chartArea': { 'width': '85%', 'height': '70%' }
        };
        graphic.chart = new google.visualization.AreaChart(document.getElementById('graphic-chart'));
        myView.hideColumns(graphic.hiddencols);
    } else if (graphic.tipografico == 'L') {
        opciones = {
            'width': '100%',
            'height': graphic.altura,
            'fontSize': 11,
            'colors': graphic.colors,
            'legend': { 'position': 'bottom' },
            'chartArea': { 'width': '85%', 'height': '70%' },
            'pointSize': 5,
            'animation':{
                'duration': 1000,
                'easing': 'in'
            }
        };
        graphic.chart = new google.visualization.LineChart(document.getElementById('graphic-chart'));
        myView.hideColumns(graphic.hiddencols);
    } else if (graphic.tipografico == 'H') {
        opciones = {
            'width': '100%',
            'height': graphic.altura,
            'fontSize': 11,
            'colors': graphic.colors,
            'legend': { 'position': 'bottom' },
            'chartArea': { 'width': '85%', 'height': '70%' }
        };
        graphic.chart = new google.visualization.ColumnChart(document.getElementById('graphic-chart'));
        myView.hideColumns(graphic.hiddencols);
    } else if (graphic.tipografico == 'T') {
        opciones = {
            'page': 'enable',
            'pageSize': 18
        };
        graphic.chart = new google.visualization.Table(document.getElementById('graphic-chart'));
        myView.hideColumns(graphic.hiddencols);
    }
    graphic.chart.draw(myView, opciones);
}

//Funcion para mostrar el resumen de los datos
function mostrarResumen() {
    $.fn.returnOK = function (response) {
        $('input.userPercent').val(response.usuariosP).trigger('change');
        $('div.userFiltered').text(response.usuariosV);
        $('div.userGlobal').text(response.usuariosAll);
        $('input.datasetPercent').val(response.datasetsP).trigger('change');
        $('div.datasetFiltered').text(response.datasetsV);
        $('div.datasetGlobal').text(response.datasetsAll);
        $('input.dataPercent').val(response.datosP).trigger('change');
        $('div.dataFiltered').text(response.datosV);
        $('div.dataGlobal').text(response.datosAll);
    };
    $.fn.returnFALSE = function (response) {
        alert('Error: mostrarResumen');
    };

    widgetObject.getResumen(graphic, $(this).returnOK, $(this).returnFALSE);
}

//Funcion para mostrar la barra horizontal de tiempo
function mostrarTimeSlider() {

    var myTimes = [];
    for (var pos = 0; pos < datos.getNumberOfRows(); pos++) {
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

