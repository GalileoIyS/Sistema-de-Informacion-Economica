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
    if ($.inArray(ext, ['xml']) == -1) {
        alert('Solo se aceptan ficheros tipo XML');
    }
    else {
        $('#uploadingMessage').removeClass('hidden');
        $.ajaxFileUpload({
            url: '/AjaxFileUploader.ashx',
            secureuri: false,
            fileElementId: 'fileToUpload',
            dataType: 'json',
            data: { name: 'logan', id: 'id' },
            success: function (data, status) {
                $('#uploadingMessage').addClass('hidden');
                if (typeof (data.error) != 'undefined') {
                    if (data.error != '') {
                        alert(data.error);
                    } else {
                        $('#uploadOkMessage').removeClass('hidden');
                        $('.NFI-button').removeClass('with-error');
                        $('#messageFileExists').removeClass('alert-danger').addClass('alert-info');
                    }
                }
                else
                    alert('??');
            },
            error: function (data, status, e) {
                alert(e);
            }
        });
    }

    return false;
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
        isValid = false;
    }
    if (!isValid) {
        $('.NFI-button').addClass('with-error');
        $('#messageFileExists').removeClass('alert-info').addClass('alert-danger');
    }
    else {
        $('.NFI-button').removeClass('with-error');
        $('#messageFileExists').removeClass('alert-danger').addClass('alert-info');
    }
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

function populateSteps(stepnumber) {
    // populate step 1
    if (stepnumber == 1) {
        populateStep1();
    }
    // populate step 2
    if (stepnumber == 2) {
        populateStep2();
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
                    autoWrapRow: true,
                    stretchH: 'all',
                    fillHandle: false,
                    afterLoadData: function () {
                        arrayColumnNames = $("#TablaDatos").handsontable('getColHeader');
                        initColumnNames();
                    },
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
        $console.text('Save error');
    };
    importObject.setFileName($('#fileToUpload').val());
    importObject.readXml($(this).returnOK, $(this).returnNO);
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
    importObject.setDateFormat($('#cmbFormatInfo').val());
    importObject.setNombre($('#txtNombreImport').val());
    importObject.setDescripcion($('#txtDescripcionImport').val());
    importObject.writeXml(columnsObject.serialize(), attributeObject.serialize(), $(this).returnOK, $(this).returnNO);
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

    //Activamos la subida de ficheros mediante AJAX
    $("#fileToUpload").change(function () {
        ajaxFileUpload();
    });

    $("input[type=file]").nicefileinput({
        label: 'Browse...'
    });

});