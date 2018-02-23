var jcrop;

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

/******************************************************************************************************
**  Dibuja una lista con las subcategorias que pertenecen a una categoría concreta                   **
*******************************************************************************************************/
function fillSubcategories(txtFilter) {
    var filtro = txtFilter;
    $.fn.displaySubcategories = function (response) {
        $("#listadoSubcategorias").shortlist({
            styleElem: 'show-subcategory',
            datos: response,
            onClick: function (event, data) {
                // Prevents the default action to be triggered. 
                event.preventDefault();

                $('li.show-subcategory').removeClass('selected').filter(function () {
                    return $(this).data('id') == data
                }).addClass('selected');
            },
            onEnter: function (event, data) {
                fillSubcategories(data);
            },
            txtValue: filtro
        });
    };
    $.fn.hideSubcategories = function (response) {
        alert('Algo salió mal');
    };
    subcategoryObject.setPageSize(6);
    subcategoryObject.setFilter(filtro);
    subcategoryObject.populate($(this).displaySubcategories, $(this).hideSubcategories);
}

/***********************************************************************************************************
** Función para mostrar las imagenes de flickr relacionadas con este indicador                            **
***********************************************************************************************************/
function mostrarImagenes(searchstr) {
    $("#searchedimages").empty();

    searchstr = searchstr.replace(/\s/g, ",");
    var flickerAPI = 'http://api.flickr.com/services/feeds/photos_public.gne?jsoncallback=?';
    $.getJSON(flickerAPI, {
        tags: searchstr,
        tagmode: "any",
        format: "json"
    }).done(function (data) {
        $.each(data.items, function (i, item) {
            var li = $('<li>').appendTo("#searchedimages");
            $("<img>").attr("src", item.media.m).addClass('flickr-image').appendTo(li);
            if (i >= 22) {
                return false;
            }
        });

        $("img.flickr-image").click(function () {
            $("img.flickr-image").not(this).removeClass("selected");
            $(this).addClass("selected");
        });
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
    // validate step 3
    if ((stepnumberout == 3) && (stepnumberin == 4)) {
        isStepValid = validateStep3();
    }
    return isStepValid;
}
function validateStep1() {
    var isValid = true;

    return isValid;
}
function validateStep2() {
    var isValid = true;

    if ($("img.flickr-image").hasClass('selected'))
        return true;

    if (indicatorObject.getImageUrl())
        return true;

    return false;
}
function validateStep3() {
    var isValid = true;

    if ($('#cmbCategorias').find(":selected").length == 0)
        return false;

    if (!$('#cmbCategorias').find(":selected").val())
        return false;

    if ($('#rbNuevaSubcategoria').is(':checked')) {

        //No hay error en el listado
        $('#listadoSubcategorias').find('.dropkeys-shortlist-container').removeClass('has-error');

        if ($('#txtNuevaSubcategoria').val().length == 0) {
            $('#txtNuevaSubcategoria').closest('.form-group').addClass('has-error');
            $('#txtNuevaSubcategoria').focus();
            return false;
        }
        else {
            $('#txtNuevaSubcategoria').closest('.form-group').removeClass('has-error');
        }
    }
    else {

        //No hay error en el textbox
        $('#txtNuevaSubcategoria').closest('.form-group').removeClass('has-error');

        if ($('li.show-subcategory.selected').length == 0) {
            $('#listadoSubcategorias').find('.dropkeys-shortlist-container').addClass('has-error');
            return false;
        }
        if (!$('li.show-subcategory.selected').data('id'))
        {
            $('#listadoSubcategorias').find('.dropkeys-shortlist-container').addClass('has-error');
            return false;
        }
        $('#listadoSubcategorias').find('.dropkeys-shortlist-container').removeClass('has-error');
    }

    return true;
}

/******************************************************************************************************
**  Otras funciones específicas del funcionamiento del asistente                                     **
*******************************************************************************************************/
function onFinishCallback() {
    if (validateStep3()) {
        $.fn.returnOK = function (response) {
            window.location.replace($('#HlnkIndicador').attr('href'));
        };
        $.fn.returnNO = function (response) {
            alert('Algo salió mal');
        };

        indicatorObject.setCategoryId($('#cmbCategorias').find(":selected").val());
        if ($("img.flickr-image").hasClass('selected'))
            indicatorObject.setImageUrl($("img.flickr-image").attr('src'));
        else
            indicatorObject.setImageUrl($("#imgIndicador").attr('src'));

        if ($('#rbNuevaSubcategoria').is(':checked')) {
            indicatorObject.setSubcategoryId(-1);
        }
        else {
            indicatorObject.setSubcategoryId($('li.show-subcategory.selected').data('id'));
        }
        indicatorObject.shareIndicator($('#txtNuevaSubcategoria').val(), $(this).returnOK, $(this).returnNO);
    }
}

$(document).ready(function () {

    //Obtenemos el ID del código de servidor
    var id = document.getElementById('hdnIndicatorID').value;
    if (id) {
        indicatorObject.setIdIndicator(id);
        indicatorObject.select();
    }

    /******************************************************************************************************
    **  Ejecutamos el asistente de creación de un nuevo indicador                                        **
    *******************************************************************************************************/
    $('#ShareWizard').wizard();
    $('#ShareWizard').on('change', function (e, data) {
        if (data.direction === 'next') {
            if (validateSteps(data.step, data.step + 1)) {
                return true;
            }
            else
                return false;
        }
        else {
            if (validateSteps(data.step, data.step - 1)) {
                return true;
            }
            else return false;
        }
    });
    $('#ShareWizard').on('finished', function (e, data) {
        onFinishCallback();
    });

    $('#rbNuevaSubcategoria').click(function (event) {
        $('#txtNuevaSubcategoria').prop('disabled', false);
        $('#lstSubcategorias').prop('disabled', true);
        $('li.show-subcategory').removeClass('selected');
    });
    $('#rbViejaSubcategoria').click(function (event) {
        $('#txtNuevaSubcategoria').val('');
        $('#txtNuevaSubcategoria').prop('disabled', true);
        $('#lstSubcategorias').prop('disabled', false);
    });
    $('#cmbCategorias').on('change', function (e) {
        var valueSelected = $(this).find(":selected").val();
        subcategoryObject.setIdCategory(valueSelected);
        fillSubcategories('');
    });
    $('li.show-subcategory').click(function (event) {
        //Prevents the default action to be triggered. 
        event.preventDefault();

        $('li.show-subcategory').removeClass('selected');
        $(this).addClass('selected');
    });
    $('#txtSearchSubcategorias').keypress(function (event) {
        if (event.which == 13) {
            fillSubcategories($(this).val());

            //Prevents the default action to be triggered. 
            event.preventDefault();
        }
    });

    //Nos aseguramos que al presionar ENTER/RETURN en los respectivos buscadores
    //se active la función de búsqueda
    $("#txtBuscarImagen").keypress(function (event) {
        if (event.which == 13) {
            mostrarImagenes($(this).val());

            //Prevents the default action to be triggered. 
            event.preventDefault();
        }
    });

    //Funciones relacionadas con la selección/previsualización/carga de la imagen
    $('#btnChangePhoto').click(function (e) {
        // Prevents the default action to be triggered. 
        e.preventDefault();

        if (jcrop) {
            $("#uploadImage").Jcrop();
        }
        $('#frmShowImage').modal('show');
    });
    $('#btnChangeProfile').click(function (e) {
        // Prevents the default action to be triggered. 
        e.preventDefault();

        $('#view-profile-data').addClass('hidden');
        $('#edit-profile-data').removeClass('hidden');
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
                        // Save the jCrop instance locally
                        jcrop = this;

                        $(".jcrop-holder").not(":last").remove();
                    });
                    $('#ImageDisplayButtons').removeClass('hide');
                };
            })(file);
            reader.readAsDataURL(file);
        } else {
            $('#ImageDisplayArea').innerHTML = "Formato de imagen no soportado";
        }
    });
    $('#btnUploadImage').click(function (e) {
        // Prevents the default action to be triggered. 
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
    $('#btnCancelImage').click(function (e) {
        // Prevents the default action to be triggered. 
        e.preventDefault();

        $('#frmShowImage').modal('hide');
    });
});