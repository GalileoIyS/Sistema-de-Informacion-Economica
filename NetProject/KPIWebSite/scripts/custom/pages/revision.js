function validaDatos() {
    var isValid = true;

    var TitleText = $('#txtTituloValue').val();
    if (!TitleText || TitleText.length <= 0) {
        if (isValid == true) {
            $('#txtTituloValue').closest('.form-group').addClass('has-error');
            $('#txtTituloValue').focus();
        }
        isValid = false;
    } else {
        $('#txtTituloValue').closest('.form-group').removeClass('has-error');
    }

    var UnitText = $('#txtUnidadValue').val();
    if (!UnitText || UnitText.length <= 0) {
        if (isValid == true) {
            $('#txtUnidadValue').closest('.form-group').addClass('has-error');
            $('#txtUnidadValue').focus();
        }
        isValid = false;
    } else {
        $('#txtUnidadValue').closest('.form-group').removeClass('has-error');
    }

    var SymbolText = $('#txtSimboloValue').val();
    if (!SymbolText || SymbolText.length <= 0) {
        if (isValid == true) {
            $('#txtSimboloValue').closest('.form-group').addClass('has-error');
            $('#txtSimboloValue').focus();
        }
        isValid = false;
    } else {
        $('#txtSimboloValue').closest('.form-group').removeClass('has-error');
    }

    var ResumeText = $('#txtResumenValue').val();
    if (!ResumeText || ResumeText.length <= 0) {
        if (isValid == true) {
            $('#txtResumenValue').closest('.form-group').addClass('has-error');
            $('#txtResumenValue').focus();
        }
        isValid = false;
    } else {
        $('#txtResumenValue').closest('.form-group').removeClass('has-error');
    }

    return isValid;
}

function compruebaTitulo()
{
    if ($('#lbTitle').text() != $('#txtTituloValue').val())
        $('#txtTituloValue').closest('.form-group').addClass('has-warning');
    else
        $('#txtTituloValue').closest('.form-group').removeClass('has-warning');
}
function compruebaFuncion() {
    if ($('#lbAgregacion').text() != $('#cmbFuncionAgregadaValue option:selected').text())
        $('#cmbFuncionAgregadaValue').closest('.form-group').addClass('has-warning');
    else
        $('#cmbFuncionAgregadaValue').closest('.form-group').removeClass('has-warning');
}
function compruebaUnidad() {
    if ($('#lbUnidad').text() != $('#txtUnidadValue').val())
        $('#txtUnidadValue').closest('.form-group').addClass('has-warning');
    else
        $('#txtUnidadValue').closest('.form-group').removeClass('has-warning');
}
function compruebaSimbolo() {
    if ($('#lbSimbolo').text() != $('#txtSimboloValue').val())
        $('#txtSimboloValue').closest('.form-group').addClass('has-warning');
    else
        $('#txtSimboloValue').closest('.form-group').removeClass('has-warning');
}
function compruebaResumen() {
    if ($('#txtResumen').text() != $('#txtResumenValue').val())
        $('#txtResumenValue').closest('.form-group').addClass('has-warning');
    else
        $('#txtResumenValue').closest('.form-group').removeClass('has-warning');
}


$(document).ready(function () {
    //Titulo
    compruebaTitulo();
    $('#txtTituloValue').change(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        compruebaTitulo();
    });

    //Funcion de agregacion
    compruebaFuncion();
    $('#cmbFuncionAgregadaValue').change(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        compruebaFuncion();
    });

    //Unidad
    compruebaUnidad();
    $('#txtUnidadValue').change(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        compruebaUnidad();
    });

    //Simbolo
    compruebaSimbolo();
    $('#txtSimboloValue').change(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        compruebaSimbolo();
    });

    //Resumen
    compruebaResumen();
    $('#txtResumenValue').change(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        compruebaResumen();
    });
});