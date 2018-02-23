var dimensiones = [];

/******************************************************************************************************
**  Proceso para subir ficheros desde código cliente a través de Ajax                                **
*******************************************************************************************************/
function ajaxFileUpload() {
    var dataFile = $('#fileToUpload').val();
    if (!dataFile) {
        $('.NFI-button').addClass('with-error');
        return false;
    }
    var ext = dataFile.split('.').pop().toLowerCase();
    if ($.inArray(ext, ['csv']) == -1) {
        alert('Solo se aceptan ficheros tipo csv');
        return false;
    }
    else {
        $('#uploadingMessage').removeClass('hidden');
        $.ajaxFileUpload({
            url: '/AjaxFileUploader.ashx',
            secureuri: false,
            fileElementId: 'fileToUpload',
            dataType: 'json',
            async: false,
            data: { name: 'logan', id: 'id' },
            success: function (data, status) {
                $('#uploadingMessage').addClass('hidden');
                if (typeof (data.error) != 'undefined') {
                    if (data.error != '') {
                        alert(data.error);
                        return false;
                    } else {
                        $('#uploadOkMessage').removeClass('hidden');
                        $('.NFI-button').removeClass('with-error');
                        $('#messageFileExists').removeClass('alert-danger').addClass('alert-info');
                        
                        $.fn.separatorOK = function (result) {
                            if (result) {
                                $('#txtDelimiterCharacter').val(result.d);
                            }
                        }
                        $.fn.separatorCANCEL = function (response) {
                            alert('Algo salió mal');
                        };
                        importObject.setFileName($('#fileToUpload').val());
                        importObject.readCsvSeparator($(this).separatorOK, $(this).separatorCANCEL);
                    }
                }
                else {
                    alert('Algo salió mal');
                    return false;
                }
            },
            error: function (data, status, e) {
                alert('Algo salió mal');
                return false;
            }
        });
    }
}

/******************************************************************************************************
**  Funciones específicas para construir la tabla de datos                                           **
*******************************************************************************************************/
function getColumnNames() {
    var isOk = true;
    $.fn.displayDimensions = function (response) {
        dimensiones = response;
    };
    $.fn.hideDimensions = function (response) {
        isOk = false;
    };
    var dataString = "{idindicator:" + importObject.getIdindicator() + "}";
    proxy.getImportDimensions(dataString, $(this).displayDimensions, $(this).hideDimensions);

    return isOk;
}
function initColumnNames() {
    var count = $("#TablaDatos").handsontable('countCols');
    for (i = 0; i < count; i++) {
        columnsObject.add(i, '-- none --', -1);
    }
    $("#TablaDatos").handsontable({
        colHeaders: columnsObject.getNames()
    });
}
function buildMenu(data) {
    var menu = $('<ul></ul>').addClass('changeDimensionCol');

    $.each(data, function (index, elem) {
        menu.append($('<li></li>').data('attributeId', elem.id).text(elem.value));
    });
    return menu;
}
function buildButton() {
    return $('<button></button>').addClass('changeType').html('\u25BC');
}
function changeColumnValue(i, liObject, instance) {
    liObject.parent().hide();
    columnsObject.update(i, liObject.text(), liObject.data('attributeId'));

    $("#TablaDatos").handsontable({
        colHeaders: columnsObject.getNames()
    });
}

/******************************************************************************************************
**  Funciones para comprobar la validez de los datos introducidos en los diferentes pasos            **
*******************************************************************************************************/
function validateSteps(stepnumberout, stepnumberin) {
    var isStepValid = true;
    // validate step 1
    if ((stepnumberout == 1) && (stepnumberin == 2)) {
        isStepValid = validateStep1();
    }
    // validate step 2
    if ((stepnumberout == 2) && (stepnumberin == 3)) {
        isStepValid = validateStep2();
    }
    return isStepValid;
}
function validateStep1() {
    var isValid = true;
    var dataFile = $('#fileToUpload').val();
    if (!dataFile) {
        $('.NFI-button').addClass('with-error');
        $('#messageFileExists').removeClass('alert-info').addClass('alert-danger');
        isValid = false;
    }
    else
    {
        $('.NFI-button').removeClass('with-error');
        $('#messageFileExists').removeClass('alert-danger').addClass('alert-info');
    }
    var delimiterChar = $('#txtDelimiterCharacter').val();
    if (!delimiterChar) {
        isValid = false;
        $('#txtDelimiterCharacter').parents('.form-group').addClass('has-error');
    }
    else
        $('#txtDelimiterCharacter').parents('.form-group').removeClass('has-error');

    return isValid;
}
function validateStep2() {
    var isValid = true;

    if (!columnsObject.hasAttributeId(-2)) {
        isValid = false;
    }
    if (!columnsObject.hasAttributeId(-3)) {
        isValid = false;
    }
    if (!isValid)
        $('#messageAttributeExists').removeClass('alert-info').addClass('alert-danger');
    else
        $('#messageAttributeExists').removeClass('alert-danger').addClass('alert-info');

    return isValid;
}

/******************************************************************************************************
**  Funciones para rellenar los diferentes pasos del asistente a medida que avanzamos en él          **
*******************************************************************************************************/
function populateSteps(stepnumber) {
    // populate step 1
    if (stepnumber == 1) {
        populateStep1();
    }
    // populate step 2
    if (stepnumber == 2) {
        populateStep2();
    }
    // populate step 3
    if (stepnumber == 3) {
        populateStep3();
    }
}
function populateStep1() {

}
function populateStep2() {
    $.fn.returnOK = function (result) {
        if (result) {
            if (getColumnNames()) {
                var ht = $('#TablaDatos').handsontable('getInstance');
                if (ht) { ht.destroy(); }

                $("#TablaDatos").handsontable({
                    data: result,
                    colHeaders: true,
                    rowHeaders: true,
                    currentRowClassName: 'currentRow',
                    currentColClassName: 'currentCol',
                    manualColumnResize: true,

                    afterLoadData: function () {
                        arrayColumnNames = $("#TablaDatos").handsontable('getColHeader');
                        initColumnNames();
                    }
                    ,
                    afterGetColHeader: function (col, TH) {
                        var instance = this;
                        var menu = buildMenu(dimensiones);

                        var $button = buildButton();
                        $button.click(function (e) {

                            e.preventDefault();
                            e.stopImmediatePropagation();

                            menu.slideToggle('fast');

                            menu.position({
                                my: 'left top',
                                at: 'left bottom',
                                of: $button,
                                within: instance.rootElement
                            });
                        });

                        menu.hide();
                        menu.on('click', 'li', function () {
                            changeColumnValue(col, $(this), instance);
                        });

                        TH.firstChild.appendChild($button[0]);
                        TH.appendChild(menu[0]);
                    }
                });

                $('.wtHolder').css('overflow', 'auto');
            }
        }
    };
    $.fn.returnNO = function (response) {
        alert('Algo salió mal');
    };
    importObject.setFileName($('#fileToUpload').val());
    importObject.readCsvData($(this).returnOK, $(this).returnNO);
}
function populateStep3() {
    $.fn.returnOK = function (result) {
        if (result) {
            var comboDateFormat = $('#cmbValidDateFormat');
            if (result.length > 0) {
                comboDateFormat.empty();
                $.each(result, function (i, item) {
                    var opcion = $('<option />').text(item);
                    opcion.appendTo(comboDateFormat);
                });
                $('#rbValidDateFormat').prop('disabled', false);
                $('#rbValidDateFormat').trigger("click");
            }
            else {
                var opcion = $('<option />').text('--unrecognized datetime format--');
                opcion.appendTo(comboDateFormat);
                $('#rbCustomDateFormat').trigger("click");
                $('#rbValidDateFormat').prop('disabled', true);
            }
        }
    };
    $.fn.returnNO = function (response) {
        $console.text('Save error');
    };
    importObject.setFileName($('#fileToUpload').val());
    importObject.readCsvDate(columnsObject.getColumnId(-2), $(this).returnOK, $(this).returnNO);
}

/******************************************************************************************************
**  Otras funciones específicas del funcionamiento del asistente                                     **
*******************************************************************************************************/
function onFinishCallback() {
    $.fn.returnOK = function (result) {
        $('#frmImportEnd').modal('show');
    };
    $.fn.returnNO = function (response) {
        alert('Algo salió mal');
    };

    attributeObject.reset();
    $('input.attribute').each(function (index) {
        if ($(this).val()) {
            attributeObject.add($(this).data('dimensionid'), $(this).val());
        };
    });
    importObject.setModo($('input[name=rbmodo]:checked').val());
    if ($('#rbCustomDateFormat').is(':checked'))
        importObject.setDateFormat($('#txtCustomDateFormat').val());
    else
        importObject.setDateFormat($('#cmbValidDateFormat').val());
    importObject.setNombre($('#txtNombreImport').val());
    importObject.setDescripcion($('#txtDescripcionImport').val());
    importObject.writeCsv(columnsObject.serialize(), attributeObject.serialize(), $(this).returnOK, $(this).returnNO);
}

$(document).ready(function () {

    importObject.setIdindicator(document.getElementById('hdnIndicatorID').value);

    /******************************************************************************************************
    **  Ejecutamos el asistente de creación de un nuevo indicador                                        **
    *******************************************************************************************************/
    $('#ImportWizard').wizard();
    $('#ImportWizard').on('change', function (e, data) {
        if (data.direction === 'next') {
            if (validateSteps(data.step, data.step + 1)) {
                populateSteps(data.step + 1);
                return true;
            }
            else
                return false;
        }
        else {
            if (validateSteps(data.step, data.step - 1)) {
                populateSteps(data.step - 1);
                return true;
            }
            else return false;
        }
    });
    $('#ImportWizard').on('finished', function (e, data) {
        onFinishCallback();
    });

    //Controlamos el cambio de formato del campo tipo Fecha
    $('#rbCustomDateFormat').click(function (event) {
        $('#txtCustomDateFormat').prop('disabled', false);
        $('#cmbValidDateFormat').prop('disabled', true);
    });
    $('#rbValidDateFormat').click(function (event) {
        $('#txtCustomDateFormat').val('');
        $('#txtCustomDateFormat').prop('disabled', true);
        $('#cmbValidDateFormat').prop('disabled', false);
    });

    //Activamos la subida de ficheros mediante AJAX
    $("#fileToUpload").change(function () {
        ajaxFileUpload();
    });

    //Mejoramos el estilo del boton de subida
    $("input[type=file]").nicefileinput({
        label: 'Browse...'
    });

});