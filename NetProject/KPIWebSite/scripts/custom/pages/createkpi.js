/******************************************************************************************************
**  Comprueba si se han cumplido los pre-requisitos para insertar una nueva característica           **
*******************************************************************************************************/
function validarAtributo() {
    var isValid = true;

    var NameText = $('#txtNombreAttribute').val();
    if (!NameText && NameText.length <= 0) {
        if (isValid == true) {
            $('#txtNombreAttribute').closest('.form-group').addClass('has-error');
            $('#txtNombreAttribute').focus();
        }
        isValid = false;
    } else {
        $('#txtNombreAttribute').closest('.form-group').removeClass('has-error');
    }

    var DescriptionText = $('#txtDescripcionAttribute').val();
    if (!DescriptionText && DescriptionText.length <= 0) {
        if (isValid == true) {
            $('#txtDescripcionAttribute').closest('.form-group').addClass('has-error');
            $('#txtDescripcionAttribute').focus();
        }
        isValid = false;
    } else {
        $('#txtDescripcionAttribute').closest('.form-group').removeClass('has-error');
    }

    return isValid;
}

/******************************************************************************************************
**  Funciones para comprobar la validez de los datos introducidos en los diferentes pasos            **
*******************************************************************************************************/
function validateSteps(stepnumber) {
    var isStepValid = true;
    if (stepnumber == 1) {
        return validateStep1();
    }
    return isStepValid;
}
function validateStep1() {
    var isValid = true;

    var titleText = $('#txtTitulo').val();
    if (!titleText && titleText.length <= 0) {
        if (isValid == true) {
            $('#txtTitulo').closest('.form-group').addClass('has-error');
            $('#txtTitulo').focus();
        }
        isValid = false;
    } else {
        $('#txtTitulo').closest('.form-group').removeClass('has-error');
    }

    var ResumeText = $('#txtResumen').val();
    if (!ResumeText && ResumeText.length <= 0) {
        if (isValid == true) {
            $('#txtResumen').closest('.form-group').addClass('has-error');
            $('#txtResumen').focus();
        }
        isValid = false;
    } else {
        $('#txtResumen').closest('.form-group').removeClass('has-error');
    }

    var UnitText = $('#txtUnidad').val();
    if (!UnitText && UnitText.length <= 0) {
        if (isValid == true) {
            $('#txtUnidad').closest('.form-group').addClass('has-error');
            $('#txtUnidad').focus();
        }
        isValid = false;
    } else {
        $('#txtUnidad').closest('.form-group').removeClass('has-error');
    }

    var SymbolText = $('#txtSimbolo').val();
    if (!SymbolText && SymbolText.length <= 0) {
        if (isValid == true) {
            $('#txtSimbolo').closest('.form-group').addClass('has-error');
            $('#txtSimbolo').focus();
        }
        isValid = false;
    } else {
        $('#txtSimbolo').closest('.form-group').removeClass('has-error');
    }

    return isValid;
}

/******************************************************************************************************
**  Otras funciones específicas del funcionamiento del asistente                                     **
*******************************************************************************************************/
function onFinishCallback() {
    $.fn.returnOK = function (response) {
        $('#hdnIndicatorID').val(response);
        $('#frmImportarExcel').modal('show');
    };
    $.fn.returnNO = function (response) {
        alert('Algo salió mal');
    };

    var atributos = [];
    $("#lstAtributos option").each(function () {
        var elemAtributo = new Object();
        elemAtributo.nombre = $(this).text();
        elemAtributo.value = $(this).val();
        atributos.push(elemAtributo);
    });

    indicatorObject.setTitulo($('#txtTitulo').val().escapeSpecialChars());
    indicatorObject.setResumen($('#txtResumen').val().escapeSpecialChars());
    indicatorObject.setUnidad($('#txtUnidad').val().escapeSpecialChars());
    indicatorObject.setSimbolo($('#txtSimbolo').val().escapeSpecialChars());
    indicatorObject.setFuncion($('#cmbFuncionAgregada').val());
    indicatorObject.insert(JSON.stringify(atributos), $(this).returnOK, $(this).returnNO);
}

$(document).ready(function () {

    /******************************************************************************************************
    **  Ejecutamos el asistente de creación de un nuevo indicador                                        **
    *******************************************************************************************************/
    $('#CreateWizard').wizard();
    $('#CreateWizard').on('change', function (e, data) {
        return validateSteps(data.step)
    });
    $('#CreateWizard').on('finished', function (e, data) {
        onFinishCallback();
    });
 
    $("#btnAddAttribute").click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        $('#txtNombreAttribute').val('');
        $('#txtDescripcionAttribute').val('');
        $('#frmEditAttribute').modal('show');
    });
    $("#btnDelAttribute").click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        $("#lstAtributos option:selected").remove();
    });
    $("#btnGuardarAttribute").click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        if (validarAtributo()) {
            $('#lstAtributos').append($("<option></option>")
                            .attr("value", $('#txtDescripcionAttribute').val())
                            .text($('#txtNombreAttribute').val()));

            $('#frmEditAttribute').modal("hide");
        }
        return false;
    });

    $('#btnCargarSi').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        $('#cargaDeDatos').removeClass('hidden');
        return false;
    });

    $('#btnImportarTabla').click(function (event) {
        __doPostBack('btnImportarTabla', 'OnClick');
    });
    $('#btnImportarExcel').click(function (event) {
        __doPostBack('btnImportarExcel', 'OnClick');
    });
    $('#btnImportarCsv').click(function (event) {
        __doPostBack('btnImportarCsv', 'OnClick');
    });
    $('#btnImportarJson').click(function (event) {
        __doPostBack('btnImportarJson', 'OnClick');
    });
    $('#btnImportarXML').click(function (event) {
        __doPostBack('btnImportarXML', 'OnClick');
    });
    $('#btnCargarNo').click(function () {
        __doPostBack('btnAceptarNo', 'OnClick');
    });

    /*Establecemos el foco en el titulo*/
    $('#txtTitulo').focus();
});