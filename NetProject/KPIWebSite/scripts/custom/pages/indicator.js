var jcrop;

/******************************************************************************************************
**  Dibuja una lista con los amigos que tienen este indicador en su biblioteca                       **
*******************************************************************************************************/
function fillFriends(txtFilter) {
    var filtro = txtFilter;
    $.fn.displayFriends = function (response) {
        $("#listadoFriends").shortlist({
            datos: response,
            onClick: function (event, data) {
                // Prevents the default action to be triggered. 
                event.preventDefault();

                //Mostramos información del usuario
                showUserInfo(data);
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
    friendObject.getFriendsList(indicatorObject.getIndicatorId(), $(this).displayFriends, $(this).hideFriends);
}

/******************************************************************************************************
**  Dibuja una lista con las fórmulas que tienen este indicador en su interior                       **
*******************************************************************************************************/
function fillFormulas(txtFilter) {
    var filtro = txtFilter;
    $.fn.displayFormulas = function (response) {
        $("#listadoFormulas").shortlist({
            datos: response,
            onClick: function (event, data) {
                // Prevents the default action to be triggered. 
                event.preventDefault();

                //Mostramos la fórmula del indicador
                muestraFormula(data);
            },
            onEnter: function (event, data) {
                fillFormulas(data);
            },
            txtValue: filtro,
            txtBadge: 'Number of users'
        });
    };
    $.fn.hideFormulas = function (response) {
        alert('Algo salió mal');
    };
    formulaObject.setFilter(filtro);
    formulaObject.getFormulasIndicator(indicatorObject.getIndicatorId(), $(this).displayFormulas, $(this).hideFormulas);
}

/******************************************************************************************************
**  Dibuja la gráfica lineal con la media de los valores introducidos por la gente y mi propia media **
*******************************************************************************************************/
function drawLineChart() {
    var id = indicatorObject.getIndicatorId();
    if (id) {
        $.fn.returnOK = function (response) {
            if (response) {
                var options = {
                    'width': '100%',
                    'height': '350',
                    'pointSize': '5',
                    'colors': ['#415360', '#EB2250'],
                    'legend': {
                        'position': 'bottom'
                    },
                    'chartArea': { 'width': '85%', 'height': '80%' }
                };
                var data = new google.visualization.DataTable(response);
                var chart = new google.visualization.LineChart(document.getElementById('chart_div'));
                chart.draw(data, options);
            }
        }
        indicatorObject.getLineChart($(this).returnOK, null);
    }
}

/******************************************************************************************************
**  Dibuja la gráfica de barras con el número de valores introducidos por año                        **
*******************************************************************************************************/
function drawBarChart() {
    var id = indicatorObject.getIndicatorId();
    if (id) {
        $.fn.returnOK = function (response) {
            if (response) {
                var options = {
                    'width': '100%',
                    'height': '250',
                    'legend': 'none',
                    'isStacked': 'true',
                    'colors': ['#90CCD2', '#148EA4', '#E20943', '#F2BC00'],
                    'chartArea': { 'width': '75%', 'height': '80%' }
                };
                var data = new google.visualization.DataTable(response);
                var chart = new google.visualization.BarChart(document.getElementById('data_div'));
                chart.draw(data, options);
            }
        }
        indicatorObject.getBarChart($(this).returnOK, null);
    }
}

/******************************************************************************************************
**  Dibuja la gráfica de los últimos valores introducidos para cada uno de los datasets              **
*******************************************************************************************************/
function drawLastChart(id) {
    if (id) {
        $.fn.returnOK = function (response) {
            if (response) {
                var valores = '';
                for (var i = 0; i < response.length; i++) {
                    if (i == 0)
                        valores = response[i].valor;
                    else
                        valores = valores + ',' + response[i].valor;
                }
                var dataValues = $('<span>').attr('class', 'line').text(valores);
                var lastColumn = $('<div>').addClass('col-md-2').append(dataValues);

                $('div.lidatasets').filter(function () {
                    return $(this).data('datasetid') == datasetObject.getDatasetId()
                }).append(lastColumn);
            }
        };
        datasetObject.setDatasetId(id);
        datasetObject.getLastChart($(this).returnOK, null);
    }
}

/******************************************************************************************************
**  Comprueba si se han cumplido los pre-requisitos para actualizar el indicador                     **
*******************************************************************************************************/
function validarIndicador() {
    var isValid = true;

    var inputNombre = $('#txtTituloValue').val();
    if (!inputNombre || inputNombre.length <= 0) {
        $('#txtTituloValue').closest('.form-group').addClass('has-error');
        $('#txtTituloValue').focus();
        isValid = false;
    } else {
        $('#txtTituloValue').closest('.form-group').removeClass('has-error');
    }

    var inputUnidad = $('#txtUnidadValue').val();
    if (!inputUnidad || inputUnidad.length <= 0) {
        $('#txtUnidadValue').closest('.form-group').addClass('has-error');
        $('#txtUnidadValue').focus();
        isValid = false;
    } else {
        $('#txtUnidadValue').closest('.form-group').removeClass('has-error');
    }

    var inputSimbolo = $('#txtSimboloValue').val();
    if (!inputSimbolo || inputSimbolo.length <= 0) {
        $('#txtSimboloValue').closest('.form-group').addClass('has-error');
        $('#txtSimboloValue').focus();
        isValid = false;
    } else {
        $('#txtSimboloValue').closest('.form-group').removeClass('has-error');
    }

    var inputResumen = $('#txtResumenValue').val();
    if (!inputResumen || inputResumen.length <= 0) {
        $('#txtResumenValue').closest('.form-group').addClass('has-error');
        $('#txtResumenValue').focus();
        isValid = false;
    } else {
        $('#txtResumenValue').closest('.form-group').removeClass('has-error');
    }

    return isValid;
}

/******************************************************************************************************
**  Comprueba si se han cumplido los pre-requisitos para insertar un nuevo dataset                   **
*******************************************************************************************************/
function validarDataSet() {
    var inputNombre = $('#txtNombreDataSet');
    if (inputNombre.val()) {
        inputNombre.closest('.form-group-sm').removeClass('has-error');
    }
    else {
        inputNombre.closest('.form-group-sm').addClass('has-error');
        return false;
    }

    var inputDimension = $('#cmbTemporal');
    if (inputDimension.val()) {
        inputDimension.closest('.form-group-sm').removeClass('has-error');
    }
    else {
        inputDimension.closest('.form-group-sm').addClass('has-error');
        return false;
    }
    return true;
}

/******************************************************************************************************
**  Comprueba si se han cumplido los pre-requisitos para insertar una nueva característica           **
*******************************************************************************************************/
function validarAtributo() {
    var inputNombre = $('#txtNombreDimension');
    if (inputNombre.val()) {
        inputNombre.closest('.form-group-sm').removeClass('has-error');
    }
    else {
        inputNombre.closest('.form-group-sm').addClass('has-error');
        return false;
    }

    var inputDescripcion = $('#txtDescripcionDimension');
    if (inputDescripcion.val()) {
        inputDescripcion.closest('.form-group-sm').removeClass('has-error');
    }
    else {
        inputDescripcion.closest('.form-group-sm').addClass('has-error');
        return false;
    }
    return true;
}

/******************************************************************************************************
**  Inserta un nuevo atributo                                                                        **
*******************************************************************************************************/
function InsertaAtributo() {
    if (validarAtributo()) {
        $.fn.returnOK = function (response) {
            if (response > 0) {
                mostrarPanelNewDimension();
                mostrarAtributos();
            }
        };
        $.fn.returnNO = function (response) {
            alert('Algo salió mal');
        };
        dimensionObject.setIndicatorId(indicatorObject.getIndicatorId());
        dimensionObject.setNombre($('#txtNombreDimension').val().escapeSpecialChars());
        dimensionObject.setDescripcion($('#txtDescripcionDimension').val().escapeSpecialChars());
        dimensionObject.insert($(this).returnOK, $(this).returnNO);
    }
    return false;
}

/******************************************************************************************************
**  Inserta un nuevo dataset                                                                         **
*******************************************************************************************************/
function InsertaDataset() {
    if (validarDataSet()) {
        $.fn.returnOK = function (response) {
            if (response > 0) {
                mostrarPanelNewDataSet();
                mostrarDatasets();
            }
        };
        $.fn.returnNO = function (response) {
            alert('Algo salió mal');
        };
        datasetObject.setIndicatorId(indicatorObject.getIndicatorId());
        datasetObject.setNombre($('#txtNombreDataSet').val().escapeSpecialChars());
        datasetObject.setDimension($('#cmbTemporal').val());
        datasetObject.insert($(this).returnOK, $(this).returnNO);
    }
    return false;
}

/******************************************************************************************************
**  Inserta un nuevo comentario                                                                      **
*******************************************************************************************************/
function InsertaComentario(texto, padreid) {
    if (texto) {
        $.fn.returnOK = function (response) {
            $('#txtEditComment').val('');
            var commentlist = $('#listaDeComentarios');
            var newComment = $('<div>').commentItem({
                data: response,
                onchange: function (event, data) {
                    alert('hola');
                }
            });
            commentlist.prepend(newComment);
        };
        $.fn.returnNO = function (response) {
            alert('Algo salió mal');
        };
        commentObject.setIndicatorId(indicatorObject.getIndicatorId());
        commentObject.setComment(texto);
        commentObject.setPadreId(padreid);
        commentObject.insert($(this).returnOK, $(this).returnNO);
    }
}

/******************************************************************************************************
**  Inserta una nueva respuesta                                                                      **
*******************************************************************************************************/
function InsertaRespuesta(texto, padreid) {
    if (texto) {
        $.fn.returnOK = function (response) {
            $('.txtNewReply').val('');
            var replylist = $('div.chat-body').filter(function () {
                return $(this).data("commentid") == response.padreid;
            }).children('ul:first');
            var newReply = $('<li>').replyItem({
                data: response,
                onchange: function (event, data) {
                    alert('hola');
                }
            });
            replylist.append(newReply);
        };
        $.fn.returnNO = function (response) {
            alert('Algo salió mal');
        };
        commentObject.setIndicatorId(indicatorObject.getIndicatorId());
        commentObject.setComment(texto);
        commentObject.setPadreId(padreid);
        commentObject.insert($(this).returnOK, $(this).returnNO);
    }
}

/******************************************************************************************************
**  Elimina el dataset especificado                                                                  **
*******************************************************************************************************/
function EliminarDataset(id) {
    if ((id) && (confirm('Are you sure you want to delete this dataset?'))) {
        $.fn.returnOK = function (response) {
            if (response.d > 0) {
                var elem = $('div.lidatasets').filter(function () {
                    return $(this).data('datasetid') == id
                });
                elem.fadeOut(1000, function () { elem.remove(); });
            }
        };
        $.fn.returnNO = function (response) {
            alert('Algo salió mal');
        };
        datasetObject.setDatasetId(id);
        datasetObject.remove($(this).returnOK, $(this).returnNO);
    }
}

/******************************************************************************************************
**  Elimina la dimension especificada                                                                **
*******************************************************************************************************/
function EliminarDimension(object) {
    var id = object.data('dimensionid');
    if ((id) && (confirm('Are you sure you want to delete this attribute?'))) {
        $.fn.returnOK = function (response) {
            if (response.d > 0) {
                var liElement = object.closest('div.lidimensions');
                liElement.fadeOut('slow');
            }
        };
        $.fn.returnNO = function (response) {
            alert('Algo salió mal');
        };
        dimensionObject.setDimensionId(id);
        dimensionObject.remove($(this).returnOK, $(this).returnNO);
    }
}

/***********************************************************************************************************
**  Recorre la lista y elimina todas aquellas revisiones seleccionadas                                   **
***********************************************************************************************************/
function EliminaRevisionSeleccionados() {
    if (confirm('Are you sure you want to delete all the selected revisions?')) {
        $('#listaRevisions input:checkbox:checked').each(function () {
            var elemActual = $(this);
            $.fn.returnOK = function (response) {
                if (response.d > 0) {
                    var liElement = elemActual.closest('div.lirevision');
                    liElement.remove();
                    $("#btnDelSelRevision").addClass('hidden');
                }
            };
            $.fn.returnNO = function (response) {
                alert('Algo salió mal');
            };
            revisionObject.setRevisionId(elemActual.data('revisionid'));
            revisionObject.remove($(this).returnOK, $(this).returnNO);
        });

        //mostrarRevisions();

    }
    return true;
}

/******************************************************************************************************
**  Recorre la lista y elimina todos aquellos datasets seleccionados                                 **
*******************************************************************************************************/
function EliminaDatasetsSeleccionados() {
    if (confirm('Are you sure you want to delete the selected datasets?')) {
        $('#listaDatasets input:checkbox:checked').each(function () {
            var elemActual = $(this);
            $.fn.returnOK = function (response) {
                if (response.d > 0) {
                    var liElement = elemActual.closest('div.lidatasets');
                    liElement.fadeOut('slow');
                    $("#btnDelSelDataSet").addClass('hidden');
                }
            };
            $.fn.returnNO = function (response) {
                alert('Algo salió mal');
            };
            datasetObject.setDatasetId(elemActual.data('datasetid'));
            datasetObject.remove($(this).returnOK, $(this).returnNO);
        });

        mostrarDatasets();
    }
}

/***********************************************************************************************************
**  Recorre la lista y elimina todas aquellas dimensiones seleccionados                                   **
***********************************************************************************************************/
function EliminaDimensionsSeleccionados() {
    if (confirm('Are you sure you want to delete the selected attributes?')) {
        $('#listaDimensions input:checkbox:checked').each(function () {
            var elemActual = $(this);
            $.fn.returnOK = function (response) {
                if (response.d > 0) {
                    var liElement = elemActual.closest('div.lidimensions');
                    liElement.remove();
                    $("#btnDelSelDimension").addClass('hidden');
                }
            };
            $.fn.returnNO = function (response) {
                alert('Algo salió mal');
            };
            dimensionObject.setDimensionId(elemActual.data('dimensionid'));
            dimensionObject.remove($(this).returnOK, $(this).returnNO);
        });

        mostrarAtributos();

    }
    return true;
}

/***********************************************************************************************************
**  Recorre la lista y elimina todos aquellas importaciones seleccionados                                 **
***********************************************************************************************************/
function EliminaImportsSeleccionados() {
    if (confirm('Are you sure you want to delete the imported data?')) {
        $('#listaImports input:checkbox:checked').each(function () {
            var elemActual = $(this);
            $.fn.returnOK = function (response) {
                if (response.d > 0) {
                    var liElement = elemActual.closest('div.liimports');
                    liElement.fadeOut('slow');
                    $("#btnDelSelImport").addClass('hidden');
                }
            };
            $.fn.returnNO = function (response) {
                alert('Algo salió mal');
            };
            importObject.setImportId(elemActual.data('importid'));
            importObject.remove($(this).returnOK, $(this).returnNO);
        });

        mostrarAtributos();
        mostrarDatasets();
        mostrarImports();
    }
}

/***********************************************************************************************************
**  Elimina todas las revisiones del indicador                                                           **
***********************************************************************************************************/
function EliminaRevisionTodos() {
    if (confirm('Are you sure you want to delete all the revisions?')) {
        $('div.lirevision').each(function () {
            var elemActual = $(this);
            $.fn.returnOK = function (response) {
                if (response.d > 0) {
                    elemActual.fadeOut('slow');
                    $("#btnDelSelRevision").addClass('hidden');
                }
            };
            $.fn.returnNO = function (response) {
                alert('Algo salió mal');
            };
            revisionObject.setRevisionId(elemActual.data('revisionid'));
            revisionObject.remove($(this).returnOK, $(this).returnNO);
        });

        //mostrarRevisions();
    }
}

/***********************************************************************************************************
**  Elimina todos los datasets del indicador                                                              **
***********************************************************************************************************/
function EliminaDatasetsTodos() {
    if (confirm('Are you sure you want to delete all the datasets?')) {
        $('div.lidatasets').each(function () {
            var elemActual = $(this);
            $.fn.returnOK = function (response) {
                if (response.d > 0) {
                    elemActual.fadeOut('slow');
                    $("#btnDelSelDataSet").addClass('hidden');
                }
            };
            $.fn.returnNO = function (response) {
                alert('Algo salió mal');
            };
            datasetObject.setDatasetId(elemActual.data('datasetid'));
            datasetObject.remove($(this).returnOK, $(this).returnNO);
        });

        mostrarDatasets();
    }
}

/***********************************************************************************************************
**  Elimina todas las dimensiones del indicador                                                           **
***********************************************************************************************************/
function EliminaDimensionsTodos() {
    if (confirm('Are you sure you want to delete all the attributes?')) {
        $('div.lidimensions').each(function () {
            var elemActual = $(this);
            $.fn.returnOK = function (response) {
                if (response.d > 0) {
                    elemActual.fadeOut('slow');
                    $("#btnDelSelDimension").addClass('hidden');
                }
            };
            $.fn.returnNO = function (response) {
                alert('Algo salió mal');
            };
            dimensionObject.setDimensionId(elemActual.data('dimensionid'));
            dimensionObject.remove($(this).returnOK, $(this).returnNO);
        });

        mostrarAtributos();
    }
}

/***********************************************************************************************************
**  Elimina todas las importaciones de datos del indicador                                                **
***********************************************************************************************************/
function EliminaImportsTodos() {
    if (confirm('Are you sure you want to delete all the import data?')) {
        $('div.liimports').each(function () {
            var elemActual = $(this);
            $.fn.returnOK = function (response) {
                if (response.d > 0) {
                    elemActual.fadeOut('slow');
                    $("#btnDelSelImport").addClass('hidden');
                }
            };
            $.fn.returnNO = function (response) {
                alert('Algo salió mal');
            };

            importObject.setImportId(elemActual.data('importid'));
            importObject.remove($(this).returnOK, $(this).returnNO);
        });

        mostrarAtributos();
        mostrarDatasets();
        mostrarImports();
    }
}

/******************************************************************************************************
**  Exporta los datos del dataset especificado                                                       **
*******************************************************************************************************/
function ExportarDataset(id) {
    $.fn.returnOK = function (response) {
        var resultado = ConvertToCSV(response);
        downloadCSV(resultado, 'dataset');
    };
    $.fn.returnNO = function (response) {
        alert('Algo salió mal');
    };
    datasetObject.setDatasetId(id);
    datasetObject.exportData($(this).returnOK, $(this).returnNO);
}

/***********************************************************************************************************
**  Muestra la ventana modal para enseñar los datos específicos de la fórmula seleccionada                **
***********************************************************************************************************/
function muestraFormula(formulaid) {
    //Mostramos la fórmula
    formulaObject.setIdFormula(formulaid);
    formulaObject.select(null);
    $('#lbNombreDeFormula').text(formulaObject.getNombre());
    $('#lbFechaDeFormula').text(formulaObject.getFecha());
    $('#formulagraphic').text(formulaObject.getDisplay());
    var miSpan = document.getElementById('formulagraphic');
    M.parseMath(miSpan);

    //Mostramos los Dashboards
    $.fn.returnDashboardOK = function (response) {
        $('#cmbDashboard').kpicombo({ datos: response });
        var thisvalue = $('#cmbDashboard').find("option:selected").val();
        refreshcmbWidget(thisvalue);
        $('#panelIncluirEnGrafico').addClass("hidden");
        $('#cbIncluirEnGrafico').prop('checked', false).attr('formulaid', formulaid);
        $("#txtNombre").attr("disabled", "disabled");
        $("#cmbDashboard").attr("disabled", "disabled");
        $("#cmbWidget").attr("disabled", "disabled");
        $("#btnCopiarFormula").addClass("hidden");
        $('#cmbDashboard').on('change', function (e) {
            var optionSelected = $("option:selected", this);
            var valueSelected = this.value;
            refreshcmbWidget(valueSelected);
        });
    };
    dashboardObject.populate($(this).returnDashboardOK, null);

    $('#frmShowFormula').modal('show');
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
** Función para mostrar u ocultar el boton eliminar las revisiones seleccionadas según el número de estos**
***********************************************************************************************************/
function revisionOnChange() {
    if ($('div.search.lirevision input:checkbox:checked').length == 0) {
        $("#btnDelSelRevision").addClass('hidden');
    } else {
        $("#btnDelSelRevision").removeClass('hidden');
    }
}

/***********************************************************************************************************
** Función para mostrar u ocultar el boton eliminar las dimensiones seleccionadas según el número de estos**
***********************************************************************************************************/
function dimensionOnChange() {
    if ($('div.search.lidimensions input:checkbox:checked').length == 0) {
        $("#btnDelSelDimension").addClass('hidden');
    } else {
        $("#btnDelSelDimension").removeClass('hidden');
    }
}

/***********************************************************************************************************
**  Función para mostrar u ocultar el boton eliminar los datasets seleccionados según el número de estos  **
***********************************************************************************************************/
function datasetOnChange() {
    if ($('div.search.lidatasets input:checkbox:checked').length == 0) {
        $("#btnDelSelDataSet").addClass('hidden');
    } else {
        $("#btnDelSelDataSet").removeClass('hidden');
    }
}

/***********************************************************************************************************
**  Función para mostrar u ocultar el boton eliminar los imports seleccionados según el número de estos   **
***********************************************************************************************************/
function importOnChange() {
    if ($('div.search.liimports input:checkbox:checked').length == 0) {
        $("#btnDelSelImport").addClass('hidden');
    } else {
        $("#btnDelSelImport").removeClass('hidden');
    }
}

/***********************************************************************************************************
** Función para mostrar u ocultar el panel para insertar un nuevo dataset                                 **
***********************************************************************************************************/
function mostrarPanelNewDataSet() {
    if ($('#PanelNewDataSet').hasClass('hidden')) {
        $('#PanelNewDataSet').removeClass('hidden');
        $('#btnNewDataSet').val('Close');
        $('#txtNombreDataSet').val('');
        $('#txtNombreDataSet').focus();
    }
    else {
        $('#PanelNewDataSet').addClass('hidden');
        $('#btnNewDataSet').val('New');
    }
    return false;
}

/***********************************************************************************************************
** Función para mostrar u ocultar el panel para insertar una nueva característica                         **
***********************************************************************************************************/
function mostrarPanelNewDimension() {
    if ($('#PanelNewDimension').hasClass('hidden')) {
        $('#PanelNewDimension').removeClass('hidden');
        $('#btnNewDimension').val('Close');
        $('#txtNombreDimension').val('');
        $('#txtNombreDimension').focus();
        $('#txtDescripcionDimension').val('');
    }
    else {
        $('#PanelNewDimension').addClass('hidden');
        $('#btnNewDimension').val('New');
    }
    return false;
}

/***********************************************************************************************************
** Función para mostrar las estadísitcas de uso de este indicador                                         **
***********************************************************************************************************/
function mostrarEstadisticas() {
    $.fn.returnOK = function (response) {
        $('span.numUsers').text(response.usuariosAll);
        $('span.numDatasets').text(response.datasetsAll);
        $('span.numDatas').text(response.datosAll);
    };
    $.fn.returnFALSE = function (response) {
        alert('Algo salió mal');
    };

    indicatorObject.getResumen($(this).returnOK, $(this).returnFALSE);
}

/***********************************************************************************************************
** Función para mostrar la lista de usuarios                                                              **
***********************************************************************************************************/
function mostrarUsuarios() {
    $.fn.displayUsers = function (response) {
        $('#listaUsuarios').userItem({
            datos: response,
            view: function (event, data) {
                showUserInfo(data.userid);
            }
        });
    };
    $.fn.hideUsers = function (response) {
        alert('K.O');
    };
    userObject.setNombre($('#txtBuscarUsers').val());
    userObject.populate($(this).displayUsers, $(this).hideUsers);
}

/***********************************************************************************************************
** Función para mostrar la lista de atributos                                                              **
***********************************************************************************************************/
function mostrarAtributos() {
    $.fn.displayAttributes = function (response) {
        $('#listaDimensions').attributeItem({
            datos: response,
            onchange: function (event, data) {
                dimensionOnChange();
            }
        });
    };
    $.fn.hideAttributes = function (response) {
        alert('K.O');
    };
    dimensionObject.setNombre($('#txtBuscarDimension').val());
    dimensionObject.populate($(this).displayAttributes, $(this).hideAttributes);

    dimensionObject.count();
    $('#countAttributes').text(dimensionObject.getTotalItems());
}

/***********************************************************************************************************
** Función para mostrar la lista de datasets                                                              **
***********************************************************************************************************/
function mostrarDatasets() {
    $.fn.displayDatasets = function (response) {
        $('#listaDatasets').datasetItem({
            datos: response,
            editar: function (event, data) {
                var url = document.URL,
                    shortUrl = url.substring(0, url.lastIndexOf("/"));
                window.location.replace(shortUrl + '/registrado/datosbydataset.aspx?dataset=' + data.datasetid);
            },
            eliminar: function (event, data) {
                EliminarDataset(data.datasetid);
            },
            exportar: function (event, data) {
                ExportarDataset(data.datasetid);
            },
            onchange: function (event, data) {
                datasetOnChange();
            }
        });

        $('div.lidatasets').each(function (index) {
            drawLastChart($(this).data('datasetid'));
        });

        $(".line").peity("line", {
            colour: "#90CCD2",
            strokeColour: "#415360",
            strokeWidth: 1,
            delimiter: ",",
            height: 80,
            width: 80
        });
    };
    $.fn.hideDatasets = function (response) {
        alert('K.O');
    };
    datasetObject.setNombre($('#txtBuscarDataset').val());
    datasetObject.populate($(this).displayDatasets, $(this).hideDatasets);

    datasetObject.count();
    $('#countDatasets').text(datasetObject.getTotalItems());
}

/***********************************************************************************************************
** Función para mostrar la lista de importaciones                                                         **
***********************************************************************************************************/
function mostrarImports() {
    $.fn.displayImports = function (response) {
        $('#listaImports').importItem({
            datos: response,
            onchange: function (event, data) {
                importOnChange();
            }
        });
    };
    $.fn.hideImports = function (response) {
        alert('K.O');
    };

    importObject.setNombre($('#txtBuscarImportacion').val());
    importObject.populate($(this).displayImports, $(this).hideImports);

    importObject.count();
    $('#countImports').text(importObject.getTotalItems());
}

/***********************************************************************************************************
** Función para mostrar una preview de la imagen recortada                                                **
***********************************************************************************************************/
function updatePreview(c) {
    if (parseInt(c.w) > 0) {
        // Show image preview
        var imageObj = $("#uploadImage")[0];
        var canvas = $("#finishImage")[0];
        canvas.width = canvas.height = 150;
        var context = canvas.getContext('2d');
        context.drawImage(imageObj, c.x, c.y, c.w, c.h, 0, 0, canvas.width, canvas.height);
        $('#image_output').attr('src', canvas.toDataURL());
    }
};

/***********************************************************************************************************
** Función para resetera las coordenadas de la imagen tras un recorte                                     **
***********************************************************************************************************/
function crop_reset() {
    //Reset coordinates of thumbnail preview container
    $('#uploadImage').replaceWith('<img id="uploadImage" alt="Upload an image"/>');
}

/**********************************************************************************************************
***********************************************************************************************************
**                              CÓDIGO DE INICIALIZACIÓN DE LA PAGINA                                    **
***********************************************************************************************************
***********************************************************************************************************/
$(document).ready(function () {
    var lastpage = getLastPageName();
    if (lastpage === 'datosbydataset.aspx') {
        $('li.datasetsItem-class a').click();
    }

    //Obtenemos el ID del código de servidor
    var id = document.getElementById('hdnIndicatorID').value;
    if (id) {
        indicatorObject.setIdIndicator(id);
        revisionObject.setIndicatorId(id);
        userObject.setIndicatorId(id);
        dimensionObject.setIndicatorId(id);
        datasetObject.setIndicatorId(id);
        importObject.setIdindicator(id);
    }

    //Nos aseguramos que al presionar ENTER/RETURN en los respectivos buscadores
    $('#txtBuscarUsers').keypress(function (event) {
        if (event.which == 13) {
            mostrarUsuarios();
            return false;
        }
    });
    $('#txtBuscarDimension').keypress(function (event) {
        if (event.which == 13) {
            $('#dataPagerAttributes').jqPagination('option', 'current_page', 1);
            return false;
        }
    });
    $('#txtBuscarDataset').keypress(function (event) {
        if (event.which == 13) {
            $('#dataPagerDatasets').jqPagination('option', 'current_page', 1);
            return false;
        }
    });
    $('#txtBuscarImportacion').keypress(function (event) {
        if (event.which == 13) {
            $('#dataPagerImports').jqPagination('option', 'current_page', 1);
            return false;
        }
    });
    $('#txtSearchFormulas').keypress(function (event) {
        if (event.which == 13) {
            fillFormulas($(this).val());

            //Prevents the default action to be triggered. 
            event.preventDefault();
        }
    });
    $('#txtSearchFriends').keypress(function (event) {
        if (event.which == 13) {
            fillFriends($(this).val());

            //Prevents the default action to be triggered. 
            event.preventDefault();
        }
    });

    //Funciones de ordenación
    dimensionObject.setOrderBy($('#lnkOrderAtributoByFecha').data('orderby'));
    $('.attribute-order-by').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        //Establecemos el orden
        dimensionObject.setOrderBy($(this).data('orderby'));

        //Rellenamos los datasets
        mostrarAtributos();
        return false;
    });
    datasetObject.setOrderBy($('#lnkOrderDatasetByFecha').data('orderby'));
    $('.dataset-order-by').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        //Establecemos el orden
        datasetObject.setOrderBy($(this).data('orderby'));

        //Rellenamos los datasets
        mostrarDatasets();
        return false;
    });
    importObject.setOrderBy($('#lnkOrderImportByFecha').data('orderby'));
    $('.import-order-by').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        //Establecemos el orden
        importObject.setOrderBy($(this).data('orderby'));

        //Rellenamos los datasets
        mostrarImports();
        return false;
    });

    //Código para votar los indicadores
    $('.target-out').raty({
        path: 'images/rating/',
        numberMax: 5,
        score: function () {
            return $(this).attr('data-score');
        },
        hints: ['very bad', 'bad', 'regular', 'good', 'excelent'],
        targetText: '',
        click: function (score) {
            $.fn.returnOK = function (response) {
                if (response.d > 0) {
                    alert('Thanks for your feedback');
                    $('.target-out').raty('score', response.d);
                }
            };
            $.fn.returnNO = function (response) {
                alert('Algo salió mal');
            };
            indicatorObject.vote(score, $(this).returnOK, $(this).returnNO);
        }
    });

    //Funciones relacionadas con la visualizacion/eliminacion del indicador
    $('.btn-edit-indicator').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        $('#edit-mode').removeClass("hidden");
        $('#preview-mode').addClass("hidden");
        $('.btn-edit-indicator').addClass("hidden");
        $('.btn-cancel-indicator').removeClass("hidden");
    });
    $('.btn-cancel-indicator').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        $('#edit-mode').addClass("hidden");
        $('#preview-mode').removeClass("hidden");
        $('.btn-edit-indicator').removeClass("hidden");
        $('.btn-cancel-indicator').addClass("hidden");
    });
    $('#btnViewIndicador').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        $('#edit-mode').addClass("hidden");
        $('#preview-mode').removeClass("hidden");
        $('.btn-edit-indicator').removeClass("hidden");
        $('.btn-cancel-indicator').addClass("hidden");
    });

    $('#btnEliminarIndicador').on('click', function (e) {
        return confirm('¿Are you sure you want to delete this indicator out of your library and its associated data?');
    });
    $('#lnkRefreshGraph').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        drawLineChart();
    });
    $('#lnkRefreshSatistics').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        mostrarEstadisticas();
    });

    //Funciones relacionadas con la selección/previsualización/carga de la imagen
    $('#btnChangePhoto').click(function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        if (jcrop) {
            $("#uploadImage").Jcrop();
        }
        $('#frmShowImage').modal('show');
    });
    $('#fileImageUpload').on('change', function (e) {
        var file = this.files[0];
        var imageType = /image.*/;

        if (file.type.match(imageType)) {
            var reader = new FileReader();
            $('#ImageDisplayArea').fadeIn();
            reader.onload = (function (theFile) {
                return function (e) {
                    if (jcrop) {
                        jcrop.destroy();
                    }
                    crop_reset();
                    $("#uploadImage").attr('src', e.target.result);
                    $("#uploadImage").Jcrop({
                        setSelect: [20, 160, 280, 270],
                        onChange: updatePreview,
                        onSelect: updatePreview,
                        bgColor: 'black',
                        bgOpacity: .4,
                        boxWidth: 550,
                        aspectRatio: 1
                    }, function () {
                        //Save the jCrop instance locally
                        jcrop = this;

                        $(".jcrop-holder").not(":last").remove();
                    });
                    $('#ImageDisplayButtons').removeClass('hidden');
                };
            })(file);
            reader.readAsDataURL(file);
        } else {
            $('#ImageDisplayArea').innerHTML = "Formato de imagen no soportado";
        }
    });
    $('#btnUploadImage').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        var canvas = $("#finishImage")[0];
        var pic1 = canvas.toDataURL("image/jpg");
        var pic = pic1.replace(/^data:image\/(png|jpg);base64,/, "")

        $.fn.returnOK = function (response) {
            if (response.d) {
                indicatorObject.setImageUrl(response.d);
                $('#imgIndicador').attr('src', response.d);
                $('#frmShowImage').modal('hide');
            }
            else
                $('#ImageDisplayArea').innerHTML = "Lo sentimos, no ha sido posible actualizar la imagen en el servidor. Por favor, inténtelo de nuevo.";
        };
        $.fn.returnNO = function (response) {
            $('#ImageDisplayArea').innerHTML = "Lo sentimos, no ha sido posible actualizar la imagen en el servidor. Por favor, inténtelo de nuevo.";
        };
        indicatorObject.changeImage(pic, $(this).returnOK, $(this).returnNO);
    });
    $('#btnCancelImage').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        $('#frmShowImage').modal('hide');
    });

    //Funciones relacionadas con la eliminación de revisiones
    $('#btnDelRevision').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        EliminaRevisionTodos();
    });
    $('#btnDelSelRevision').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        EliminaRevisionSeleccionados();
    });

    //Funciones relacionadas con la creación/eliminación de atributos
    $('#btnNewDimension').click(function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        mostrarPanelNewDimension();
    });
    $('#btnDelDimension').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        EliminaDimensionsTodos();
    });
    $('#btnDelSelDimension').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        EliminaDimensionsSeleccionados();
    });

    //Funciones relacionadas con la creación/eliminación de conjuntos de datos
    $('#btnNewDataSet').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        mostrarPanelNewDataSet();
    });
    $('#btnDelAllDataSet').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        EliminaDatasetsTodos();
    });
    $('#btnDelSelDataSet').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        EliminaDatasetsSeleccionados();
    });
    $('.lnkDeleteCurrentDataset').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        var id = $(this).data('datasetid');
        EliminarDataset(id);
    });
    $('.lnkExportCurrentDataset').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        var id = $(this).data('datasetid');
        ExportarDataset(id);
    });

    //Funciones relacionadas con la creación/eliminación de importaciones
    $('#btnDelAllImports').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        EliminaImportsTodos();
    });
    $('#btnDelSelImport').on('click', function (e) {
        //Prevents the default action to be triggered. 
        e.preventDefault();

        EliminaImportsSeleccionados();
    });

    //Código para la edición de etiquetas asociadas
    $(".tag-prefix").tagsInput({
        'width': '100%',
        'height': '150px',
        'defaultText': '(add tag)',
        'minChars': 0,
        'maxChars': 0,
        'placeholderColor': '#666666',
        onAddTag: function (value) {
            var id = indicatorObject.getIndicatorId();
            if (id) {
                $.fn.returnOK = function (response) {
                    if (response > 0) {
                        var Num = parseInt($("#lbNumEtiquetasEditable").text());
                        $("#lbNumEtiquetasEditable").text(Num + 1);
                        $(".tagsinput").css('background-color', '#86C59C').animate({ 'background-color': '#ffffff' }, 1000);
                    }
                    else
                        $(".tagsinput").css('background-color', '#F9A7A7').animate({ 'background-color': '#ffffff' }, 1000);
                };
                $.fn.returnNO = function (response) {
                    $(".tagsinput").css('background-color', '#F9A7A7').animate({ 'background-color': '#ffffff' }, 1000);
                };
                indicatorObject.addTag(value, $(this).returnOK, $(this).returnNO);
            }
        },
        onRemoveTag: function (value) {
            var id = indicatorObject.getIndicatorId();
            if (id) {
                $.fn.returnOK = function (response) {
                    if (response.d > 0) {
                        var Num = parseInt($("#lbNumEtiquetasEditable").text());
                        $("#lbNumEtiquetasEditable").text(Num - 1);
                        $(".tagsinput").css('background-color', '#86C59C').animate({ 'background-color': '#ffffff' }, 1000);
                    }
                    else
                        $(".tagsinput").css('background-color', '#F9A7A7').animate({ 'background-color': '#ffffff' }, 1000);
                };
                $.fn.returnNO = function (response) {
                    $(".tagsinput").css('background-color', '#F9A7A7').animate({ 'background-color': '#ffffff' }, 1000);
                };
                indicatorObject.delTag(value, $(this).returnOK, $(this).returnNO);
            }
        }
    });

    $('#lnkPostComment').on('click', function (e) {
        // Prevents the default action to be triggered. 
        e.preventDefault();

        var commentText = $('#txtEditComment').val();

        InsertaComentario(commentText, -1);
    });
    //Nos aseguramos que al presionar ENTER/RETURN en los respectivos buscadores
    $('.txtNewReply').keypress(function (event) {
        if (event.which == 13) {

            var replyText = $(this).val();
            var padreid = $(this).parents('.chat-body').data('commentid');

            InsertaRespuesta(replyText, padreid);

            //Prevents the default action to be triggered. 
            event.preventDefault();
        }
    });

    $('#cbIsAnonymnous').addClass('onoffswitch-checkbox');
    $('#cbIsAnonymnous').on('click', function () {
        $('span.savingIsAnonymous').removeClass('hidden');
        if ($(this).is(':checked')) {
            $.fn.returnSOK = function (response) {
                $('span.savingIsAnonymous').addClass('hidden');
            };
            $.fn.returnSNO = function (response) {
                alert('Algo salió mal');
                $('span.savingIsAnonymous').addClass('hidden');
            };
            indicatorObject.changeIndicatorUser('S', $(this).returnSOK, $(this).returnSNO);
        }
        else {
            $.fn.returnNOK = function (response) {
                $('span.savingIsAnonymous').addClass('hidden');
            };
            $.fn.returnNNO = function (response) {
                alert('Algo salió mal');
                $('span.savingIsAnonymous').addClass('hidden');
            };
            indicatorObject.changeIndicatorUser('N', $(this).returnNOK, $(this).returnNNO);
        }
    });

    //Checkbox para la selección de todas las revisiones
    $('#select_all_revisions').change(function () {
        var checkboxes = $("#listaRevisions").find(':checkbox');
        if ($(this).prop('checked')) {
            checkboxes.prop('checked', true);
            $("#btnDelSelRevision").removeClass('hidden');
        } else {
            checkboxes.prop('checked', false);
            $("#btnDelSelRevision").addClass('hidden');
        }
    });
    //Checkbox para la selección de todos los datasets
    $('#select_all_datasets').change(function () {
        var checkboxes = $("#listaDatasets").find(':checkbox');
        if ($(this).prop('checked')) {
            checkboxes.prop('checked', true);
            $("#btnDelSelDataSet").removeClass('hidden');
        } else {
            checkboxes.prop('checked', false);
            $("#btnDelSelDataSet").addClass('hidden');
        }
    });
    //Checkbox para la selección de todas las características
    $('#select_all_dimensions').change(function () {
        var checkboxes = $("#listaDimensions").find(':checkbox');
        if ($(this).prop('checked')) {
            checkboxes.prop('checked', true);
            $("#btnDelSelDimension").removeClass('hidden');
        } else {
            checkboxes.prop('checked', false);
            $("#btnDelSelDimension").addClass('hidden');
        }
    });
    //Checkbox para la selección de todas las importaciones
    $('#select_all_imports').change(function () {
        var checkboxes = $("#listaImports").find(':checkbox');
        if ($(this).prop('checked')) {
            checkboxes.prop('checked', true);
            $("#btnDelSelImport").removeClass('hidden');
        } else {
            checkboxes.prop('checked', false);
            $("#btnDelSelImport").addClass('hidden');
        }
    });

    $("#cbIncluirEnGrafico").click(function (event) {
        if ($(this).is(":checked")) {
            $('#panelIncluirEnGrafico').removeClass("hidden");
            $("#txtNombre").removeAttr("disabled");
            $("#cmbDashboard").removeAttr("disabled");
            $("#cmbWidget").removeAttr("disabled");
            $("#btnCopiarFormula").removeClass("hidden");
        } else {
            $('#panelIncluirEnGrafico').addClass("hidden");
            $("#txtNombre").attr("disabled", "disabled");
            $("#cmbDashboard").attr("disabled", "disabled");
            $("#cmbWidget").attr("disabled", "disabled");
            $("#btnCopiarFormula").addClass("hidden");
        }
    });

    $('a.show-formula').click(function (event) {
        //Prevents the default action to be triggered. 
        event.preventDefault();

        //Mostramos la fórmula del indicador
        muestraFormula($(this).data('id'));
    });

    $("#btnCopiarFormula").click(function (event) {
        //Prevents the default action to be triggered. 
        event.preventDefault();

        if ($('#cbIncluirEnGrafico').prop('checked')) {
            var idwidget = $("#cmbWidget").val();
            var idformula = $('#cbIncluirEnGrafico').attr('formulaid');
            if (idwidget && idformula) {
                formulaObject.setIdFormula(idformula);
                formulaObject.setIdWidget(idwidget);
                formulaObject.setNombre($('#txtNombre').val());
                $.fn.returnOK = function (response) {
                    $('#frmShowFormula').modal("hidden");
                };
                $.fn.returnNO = function (response) {
                    alert('Algo salió mal');
                    $('#frmShowFormula').modal("hidden");
                };
                formulaObject.copy($(this).returnOK, $(this).returnNO);
            }
        }
    });

    //Mostramos la paginación de las revisiones
    revisionObject.count();
    $('#countRevisions').text(revisionObject.getTotalItems());
    $('#dataPagerRevisions').jqPagination({
        max_page: revisionObject.getTotalPages(),
        page_string: 'Page {current_page} of {max_page}',
        paged: function (page) {
            revisionObject.setCurrentPage(page);
            //mostrarAtributos();
        }
    });

    //Mostramos la paginación de los usuarios
    userObject.count();
    $('#countOtherSources').text(userObject.getTotalItems());

    //Mostramos la paginación de los atributos
    dimensionObject.count();
    $('#countAttributes').text(dimensionObject.getTotalItems());
    $('#dataPagerAttributes').jqPagination({
        max_page: dimensionObject.getTotalPages(),
        page_string: 'Page {current_page} of {max_page}',
        paged: function (page) {
            dimensionObject.setCurrentPage(page);
            mostrarAtributos();
        }
    });

    //Mostramos la paginación de los datasets
    datasetObject.count();
    $('#countDatasets').text(datasetObject.getTotalItems());
    $('#dataPagerDatasets').jqPagination({
        max_page: datasetObject.getTotalPages(),
        page_string: 'Page {current_page} of {max_page}',
        paged: function (page) {
            datasetObject.setCurrentPage(page);
            mostrarDatasets();
        }
    });

    //Mostramos un listado con las importaciones
    importObject.count();
    $('#countImports').text(importObject.getTotalItems());
    $('#dataPagerImports').jqPagination({
        max_page: importObject.getTotalPages(),
        page_string: 'Page {current_page} of {max_page}',
        paged: function (page) {
            importObject.setCurrentPage(page);
            mostrarImports();
        }
    });

    //Mostramos por cada dataset, una gráfica de los últimos resultados
    $('div.lidatasets').each(function (index) {
        drawLastChart($(this).data('datasetid'));
    });

    $(".line").peity("line", {
        colour: "#90CCD2",
        strokeColour: "#415360",
        strokeWidth: 1,
        delimiter: ",",
        height: 80,
        width: 80
    });

    //Mejoramos el estilo de la subida de imágenes
    $("input[type=file]").nicefileinput({
        label: 'Browse...'
    });

    ////Mostramos la gráfica lineal por ejercicios
    drawLineChart();
    ////Mostramos la gráfica de barras por ejercicios
    drawBarChart();
    ////Mostramos el cuadro de estadísitcas
    mostrarEstadisticas();
});