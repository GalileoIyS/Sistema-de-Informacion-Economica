/******************************************************************************************************
**  Comprueba si se han cumplido los pre-requisitos para actualizar el indicador                     **
*******************************************************************************************************/
function InsertaCategoria() {
    var inputNombre = $('#txtCategoria');
    if (inputNombre.val()) {
        inputNombre.removeClass('with-error');
    }
    else {
        inputNombre.addClass('with-error');
        inputNombre.next('.mensaje-error').fadeIn().delay(2000).fadeOut();
        return false;
    }
}
