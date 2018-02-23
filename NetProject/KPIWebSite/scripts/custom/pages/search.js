var isPreviousEventComplete = true,
    isDataAvailable = true;

/***********************************************************************************************************
** Función para mostrar la lista de indicadores                                                           **
***********************************************************************************************************/
function mostrarIndicadores() {
    $.fn.displayIndicators = function (response) {
        isPreviousEventComplete = true;
        $('#divloadingMessage').addClass('hidden');
        $('#listaIndicadores').indicatorItem({
            datos: response,
            insertar: function (event, data) {
                // Prevents the default action to be triggered. 
                event.preventDefault();

                //Añadimos el indicador a la biblioteca
                addIndicatorUser(data.indicadorid, data.nombre);
            }
        });
    };
    $.fn.hideIndicators = function (response) {
        alert('K.O');
        $('#divloadingMessage').addClass('hidden');
    };
    $('#divloadingMessage').removeClass('hidden');
    indicatorObject.setTitulo($('#txtBuscarIndicador').val());
    indicatorObject.populate($(this).displayIndicators, $(this).hideIndicators);

    indicatorObject.count();

    var nTotal = indicatorObject.getTotalItems();
    var numElements = $("#listaIndicadores div.search").length;
    if (numElements == 0) {
        $('.search-count').html('There are no available data').addClass('search-no-count');
        $('#PanelResultadosBusqueda').removeClass('hide');
        isDataAvailable = false;
    }
    else if (numElements >= nTotal) {
        $('.search-count').html('Showing 1-' + numElements + ' of ' + nTotal + ' results').removeClass('search-no-count');
        $('#PanelResultadosBusqueda').addClass('hide');
        isDataAvailable = false;
    }
    else {
        $('.search-count').html('Showing 1-' + numElements + ' of ' + nTotal + ' results').removeClass('search-no-count');
        $('#PanelResultadosBusqueda').addClass('hide');
        isDataAvailable = true;
    }
}

/******************************************************************************************************
**  Función para añadir un indicador a la biblioteca del usuario actual. Lanza un cuadro de diálogo  **
**  modal para preguntarle si desea insertarlo también en alguno de los gráficos disponibles         **
*******************************************************************************************************/
function addIndicatorUser(indicatorid, nombre) {
    if (indicatorid >= 0) {
        $('#txtNombre').val(nombre);
        $('#cbIncluirEnGrafico').attr('indicatorid', indicatorid);
        $.fn.returnOK = function (response) {
            //Eliminamos el enlace para añadirlo a nuestra biblioteca
            $('a.add-indicator').filter(function () {
                return $(this).data("indicadorid") == indicatorid;
            }).remove();

            var numUsers = $('span.num-users').filter(function () {
                return $(this).data("indicadorid") == indicatorid;
            });
            var Num = parseInt(numUsers.text());
            numUsers.text(Num + 1);

            $('#frmAddIndicador').modal('show');
        };
        $.fn.returnNO = function (response) {
            alert('Algo salió mal');
        };
        indicatorObject.setIdIndicator(indicatorid);
        indicatorObject.addIndicatorUser($(this).returnOK, $(this).returnNO);
    }
}

/***********************************************************************************************************
**  Función para refrescar el combo de selección de gráficos en función del cuadro de mando seleccionado  **
***********************************************************************************************************/
function refreshcmbWidget(id) {
    if (!id)
        return false;

    $.fn.returnOK = function (response) {
        $("#cmbWidget").removeAttr("disabled");
        $('#cmbWidget').kpicombo({ datos: response });
    };
    $.fn.returnNO = function (response) {
        $("#cmbWidget").attr("disabled", "disabled");
        $('#cmbWidget').kpicombo({ datos: null });
    };
    widgetObject.setIdDashboard(id);
    widgetObject.populate($(this).returnOK, $(this).returnNO);
}

/***********************************************************************************************************
**  Función para marcar la categoría actualmente seleccionada y desactivar el resto para que solo pueda   **
**  haber una activada al mismo tiempo                                                                    **
***********************************************************************************************************/
function pintaCategorias(categoryid) {
    //Ocultamos todos
    $('a.lisubcategory').addClass('hide');
    $('a.licategory').removeClass('active');

    //Mostramos los de la categoria actual
    $('a.lisubcategory').filter(function () {
        return $(this).data("categoryid") == categoryid;
    }).removeClass('hide');
    $('a.licategory').filter(function () {
        return $(this).data("categoryid") == categoryid;
    }).addClass('active');
}

/***********************************************************************************************************
**  I. Función para rellenar la lista con más indicadores a medida que el usuario hace scroll hacia abajo **
***********************************************************************************************************/
function AddMoreContent() {
    if (isPreviousEventComplete && isDataAvailable) {

        isPreviousEventComplete = false;

        var pagina = indicatorObject.getCurrentPage();
        indicatorObject.setCurrentPage(pagina + 1);
        mostrarIndicadores();
    }
}

/***********************************************************************************************************
**  II. Función para rellenar la lista con más indicadores a medida que el usuario hace scroll hacia abajo**
***********************************************************************************************************/
$(window).scroll(function () {
    if ($(document).height() <= $(window).scrollTop() + $(window).height()) {
        AddMoreContent();
    }
});

$(document).ready(function () {

    $("a[elem='buscador']").css("color", "#759dae;");
    indicatorObject.setOrderBy($('#lnkOrderAtributoByFecha').data('orderby'));

    /*Nube de tags*/
    $.fn.tagcloud.defaults = {
        size: { start: 11, end: 24, unit: 'pt' },
        color: { start: '#759dae', end: '#415360' }
    };
    $('#whatever a').tagcloud();

    //Nos aseguramos que al presionar ENTER/RETURN en los respectivos buscadores
    $('#txtBuscarIndicador').keypress(function (event) {
        if (event.which == 13) {

            //Limpiamos la lista
            $('div.search').remove();

            //Inicializamos la búsqueda
            indicatorObject.setCurrentPage(1);
            isDataAvailable = true;

            //Rellenamos los indicadores
            mostrarIndicadores();
        }
    });
    //Botón de búsqueda
    $('#btnBuscar').on('click', function (e) {
        // Prevents the default action to be triggered. 
        e.preventDefault();

        //Limpiamos la lista
        $('div.search').remove();

        //Inicializamos la búsqueda
        indicatorObject.setCurrentPage(1);
        isDataAvailable = true;

        //Rellenamos los indicadores
        mostrarIndicadores();
    });
    //Funciones de ordenación
    $('.attribute-order-by').on('click', function (e) {
        // Prevents the default action to be triggered. 
        e.preventDefault();

        // Establecemos el orden
        indicatorObject.setOrderBy($(this).data('orderby'));

        //Inicializamos la búsqueda
        indicatorObject.setCurrentPage(1);
        isDataAvailable = true;

        //Limpiamos la lista
        $('div.search').remove();

        //Rellenamos los indicadores
        mostrarIndicadores();
    });

    //Filtro por categoría o subcategoría 
    $('a.lisubcategory').addClass('hide');
    $('a.licategory').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        //Mostramos las subcategorias
        pintaCategorias($(this).data('categoryid'));

        //Establecemos la categoría
        indicatorObject.setCategoryId($(this).data('categoryid'));
        indicatorObject.setSubcategoryId(-1);

        //Inicializamos la búsqueda
        indicatorObject.setCurrentPage(1);
        isDataAvailable = true;

        //Limpiamos la lista
        $('div.search').remove();

        $('.search-category').html('Selected category : ' + $(this).children('span.nombre').text());

        mostrarIndicadores();
    });
    $('a.lisubcategory').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        //Establecemos la subcategoría
        indicatorObject.setSubcategoryId($(this).data('subcategoryid'));

        //Inicializamos la búsqueda
        indicatorObject.setCurrentPage(1);
        isDataAvailable = true;

        //Limpiamos la lista
        $('div.search').remove();

        $('.search-category').html('Selected subcategory : ' + $(this).children('span.nombre').text());

        mostrarIndicadores();
    });

    //Botón para insertar el indicador en una gráfica
    $('#btnTerminar').click(function () {
        if ($('#cbIncluirEnGrafico').prop('checked')) {
            var indicatorid = $('#cbIncluirEnGrafico').attr('indicatorid');
            var idwidget = $("#cmbWidget").val();    
            if (idwidget && indicatorid) {
                formulaObject.setIdWidget(idwidget);
                formulaObject.setNombre($('#txtNombre').val());
                formulaObject.setOriginal('@[[-1:' + indicatorid + ':' + $('#cmbAggFuncion').val() + ':' + $('#txtNombre').val() + ']]');
                formulaObject.setColor('EB2250');

                $.fn.returnOK = function (response) {
                    $('#frmAddIndicador').modal('hide');
                };
                $.fn.returnNO = function (response) {
                    alert('Algo salió mal');
                    $('#frmAddIndicador').modal('hide');
                };
                formulaObject.insert($(this).returnOK, $(this).returnNO);
            }
        }
        $('#frmAddIndicador').modal('hide');
    });

    //Comportamientos internos del formulario modal
    $("#cbIncluirEnGrafico").click(function (event) {
        // Prevents the default action to be triggered. 
        //event.preventDefault();

        if ($(this).is(":checked")) {
            $('#panelIncluirEnGrafico').removeClass("hide");
            $("#txtNombre").removeAttr("disabled");
            $("#cmbAggFuncion").removeAttr("disabled");
            $("#cmbDashboard").removeAttr("disabled");
            $("#cmbWidget").removeAttr("disabled");
        } else {
            $('#panelIncluirEnGrafico').addClass("hide");
            $("#txtNombre").attr("disabled", "disabled");
            $("#cmbAggFuncion").attr("disabled", "disabled");
            $("#cmbDashboard").attr("disabled", "disabled");
            $("#cmbWidget").attr("disabled", "disabled");
        }
    });

    $('a.add-indicator').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        //Añadimos el indicador a la biblioteca
        addIndicatorUser($(this).data('indicadorid'), $(this).data('nombre'));
    });

    $('#frmAddIndicador').on('shown.bs.modal', function () {
        $.fn.returnDashboardOK = function (response) {
            $('#cmbDashboard').kpicombo({ datos: response });
            var thisvalue = $('#cmbDashboard').find("option:selected").val();
            refreshcmbWidget(thisvalue);
            $('#cbIncluirEnGrafico').prop('checked', false);
            $("#txtNombre").attr("disabled", "disabled");
            $("#cmbAggFuncion").attr("disabled", "disabled");
            $("#cmbDashboard").attr("disabled", "disabled");
            $("#cmbWidget").attr("disabled", "disabled");
            $('#cmbDashboard').on('change', function (e) {
                var optionSelected = $("option:selected", this);
                var valueSelected = this.value;
                refreshcmbWidget(valueSelected);
            });
        };
        dashboardObject.populate($(this).returnDashboardOK, null);

        $.fn.returnFunctionOK = function (response) {
            $('#cmbAggFuncion').kpicombo({ datos: response });
        };
        functionObject.populate($(this).returnFunctionOK, null);
    });
});