/******************************************************************************************************
**  Variables locales de la página                                                                   **
**    Ejercicio:-1
**    Semestre: -2
**    Trimestre: -3
**    Mes: -4
**    Quincena: -5
**    Semana: -6
**    Dia: -7
**    Libre: -8
*******************************************************************************************************/
var dynamicColumns = [];

/******************************************************************************************************
**  Funciones específicas para construir la tabla de datos                                           **
*******************************************************************************************************/
function columnasReset() {
    dynamicColumns = [];
    columnsObject.reset();
}
function columnasEjercicio(col) {
    var colEjercicio = new Object();
    colEjercicio.data = col;
    colEjercicio.title = "Year";
    colEjercicio.type = "numeric";
    dynamicColumns.push(colEjercicio);
}
function columnasSemestre(col) {
    var colSemestre = new Object();
    colSemestre.data = col;
    colSemestre.title = "Semester";
    colSemestre.type = "autocomplete";
    colSemestre.source = ['1', '2'];
    colSemestre.strict = true;
    colSemestre.allowInvalid = false;
    colSemestre.filter = false;
    dynamicColumns.push(colSemestre);
}
function columnasTrimestre(col) {
    var colTrimestre = new Object();
    colTrimestre.data = col;
    colTrimestre.title = "Quarter";
    colTrimestre.type = "autocomplete";
    colTrimestre.source = ['1', '2', '3', '4'];
    colTrimestre.strict = true;
    colTrimestre.allowInvalid = false;
    dynamicColumns.push(colTrimestre);
}
function columnasMeses(col) {
    var colMes = new Object();
    colMes.data = col;
    colMes.title = "Month";
    colMes.type = "autocomplete";
    colMes.source = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12'];
    colMes.strict = true;
    colMes.allowInvalid = false;
    dynamicColumns.push(colMes);
}
function columnasQuincenas(col) {
    var colQuincena = new Object();
    colQuincena.data = col;
    colQuincena.title = "Fortnight";
    colQuincena.type = "autocomplete";
    colQuincena.source = ['1', '2'];
    colQuincena.strict = true;
    colQuincena.allowInvalid = false;
    dynamicColumns.push(colQuincena);
}
function columnasSemanas(col) {
    var colSemana = new Object();
    colSemana.data = col;
    colSemana.title = "Week";
    colSemana.type = "autocomplete";
    colSemana.source = ['1', '2', '3', '4'];
    colSemana.strict = true;
    colSemana.allowInvalid = false;
    dynamicColumns.push(colSemana);
}
function columnasDiario(col) {
    var colDia = new Object();
    colDia.data = col;
    colDia.title = "Day";
    colDia.type = "numeric";
    dynamicColumns.push(colDia);
}
function columnasLibre(col) {
    var colLibre = new Object();
    colLibre.data = col;
    colLibre.title = "Date";
    colLibre.type = "date";
    colLibre.dateFormat = "dd/mm/yy";
    dynamicColumns.push(colLibre);
}
function columnasAtributo(id, col) {

    var colAtributo = new Object();
    dimensionObject.setDimensionId(id);
    dimensionObject.select();
    colAtributo.data = col;
    colAtributo.title = dimensionObject.getNombre();

    $.fn.returnOK = function (result) {
        var elementos = [];
        $.each(result, function (index, elem) {
            elementos.push(elem.codigo);
        });
        colAtributo.source = elementos;
    };
    $.fn.returnNO = function (response) {
        alert('Algo salió mal');
    };
    dimensionvaluesObject.setDimensionId(id);
    dimensionvaluesObject.find('', $(this).returnOK, $(this).returnNO);

    colAtributo.type = "autocomplete";
    colAtributo.strict = false;
    colAtributo.allowInvalid = true;

    dynamicColumns.push(colAtributo);
    columnsObject.add(col, colAtributo.title, parseInt(id));
}
function columnasValor(col) {
    var colValor = new Object();
    colValor.data = col;
    colValor.title = "Value";
    colValor.type = "numeric";
    colValor.format = "0,0.00";
    dynamicColumns.push(colValor);
    columnsObject.add(col, "valor", -3);
}
function construyeTablaDinamica() {
    $("#TablaDatos").handsontable({
        minRows: 12,
        minSpareRows: 1,
        rowHeaders: true,
        colHeaders: ['Year', '€/hab'],
        currentRowClassName: 'currentRow',
        currentColClassName: 'currentCol',
        autoWrapRow: true,
        stretchH: 'last',
        manualColumnResize: true,
        columns: dynamicColumns
    });
}

/******************************************************************************************************
**  Funciones para comprobar la validez de los datos introducidos en los diferentes pasos            **
*******************************************************************************************************/
function validateSteps(stepnumberout, stepnumberin) {
    var isStepValid = true;
    // validate step 2
    if ((stepnumberout == 2) && (stepnumberin == 3)) {
        isStepValid = validateStep2();
    }
    return isStepValid;
}

/******************************************************************************************************
**  Funciones para rellenar los diferentes pasos del asistente a medida que avanzamos en él          **
*******************************************************************************************************/
function populateTable() {
    var TimeLine = $('#cmbTemporal').val();
    var currentCol;

    columnasReset();
    switch (TimeLine) {
        case 'A':
            columnasEjercicio(0);
            currentCol = 1;
            break;
        case 'S':
            columnasEjercicio(0);
            columnasSemestre(1);
            currentCol = 2;
            break;
        case 'T':
            columnasEjercicio(0);
            columnasTrimestre(1);
            currentCol = 2;
            break;
        case 'M':
            columnasEjercicio(0);
            columnasMeses(1);
            currentCol = 2;
            break;
        case 'Q':
            columnasEjercicio(0);
            columnasMeses(1);
            columnasQuincenas(2);
            currentCol = 3;
            break;
        case 's':
            columnasEjercicio(0);
            columnasMeses(1);
            columnasSemanas(2);
            currentCol = 3;
            break;
        case 'D':
            columnasEjercicio(0);
            columnasMeses(1);
            columnasDiario(2);
            currentCol = 3;
            break;
        default:
            columnasLibre(0);
            currentCol = 1;
    }

    $('#lstAtributos :selected').each(function (i, selected) {
        columnasAtributo($(selected).val(), currentCol);
        currentCol = currentCol + 1;
    });

    columnasValor(currentCol);
    construyeTablaDinamica();
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
    var handsontable = $("#TablaDatos").data('handsontable');
    importObject.setModo($('input[name=rbmodo]:checked').val());
    importObject.setNombre($('#txtNombreImport').val());
    importObject.setDescripcion($('#txtDescripcionImport').val());
    importObject.writeTable(columnsObject.serialize(), JSON.stringify(handsontable.getData()), $('#cmbTemporal').val(), $(this).returnOK, $(this).returnNO);
}

$(document).ready(function () {

    importObject.setIdindicator(document.getElementById('hdnIndicatorID').value);

    /******************************************************************************************************
    **  Ejecutamos el asistente de creación de un nuevo indicador                                        **
    *******************************************************************************************************/
    $('#ImportWizard').wizard();
    $('#ImportWizard').on('change', function (e, data) {
        if ((data.step === 1) && (data.direction === 'next')) {
            populateTable();
        }
    });
    $('#ImportWizard').on('finished', function (e, data) {
        onFinishCallback();
    });

    $('#btnSelTodos').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        $('#lstAtributos option').prop('selected', true);
    });

    $('#btnSelNinguno').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        $('#lstAtributos option').prop('selected', false);
    });
});