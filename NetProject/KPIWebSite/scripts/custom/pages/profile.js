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

function changePassword() {
    $('#PanelErrorPassword').addClass('hidden');

    var inputOldPwd = $('#txtOldUserPassword');
    if (inputOldPwd.val()) {
        inputOldPwd.closest('.form-group').removeClass('has-error');
    }
    else {
        inputOldPwd.closest('.form-group').addClass('has-error');
        return false;
    }

    var inputNewPwd = $('#txtNewUserPassword');
    if (inputNewPwd.val()) {
        inputNewPwd.closest('.form-group').removeClass('has-error');
    }
    else {
        inputNewPwd.closest('.form-group').addClass('has-error');
        return false;
    }

    var inputRepeatPwd = $('#txtNewUserPasswordRepeated');
    if (inputRepeatPwd.val()) {
        inputRepeatPwd.closest('.form-group').removeClass('has-error');
    }
    else {
        inputRepeatPwd.closest('.form-group').addClass('has-error');
        return false;
    }

    if (inputRepeatPwd.val() != inputNewPwd.val()) {
        inputNewPwd.closest('.form-group').addClass('has-error');
        inputRepeatPwd.closest('.form-group').addClass('has-error');
        return false;
    }

    $.fn.returnOK = function (response) {
        if (response < 0)
        {
            $('#PanelErrorPassword').removeClass('hidden');
            inputOldPwd.closest('.form-group').addClass('has-error').focus();
        }
        else
            $('#frmChangePassword').modal('hide');
    };
    $.fn.returnNO = function (response) {
        alert('Algo salió mal');
        $('#frmChangePassword').modal('hide');
    };
    userObject.changePassword(inputOldPwd.val(), inputNewPwd.val(), $(this).returnOK, $(this).returnNO);
}
/***********************************************************************************************************
** Función para resetera las coordenadas de la imagen tras un recorte                                     **
***********************************************************************************************************/
function crop_reset() {
    //Reset coordinates of thumbnail preview container
    $('#uploadImage').replaceWith('<img id="uploadImage" alt="Upload an image"/>');
}

$(document).ready(function () {
    $('.friends-list > li > a').tooltip({ placement: 'top' });

    var miSpan = $('.display-formula');
    M.parseMath(miSpan);

    $('#btnChangeProfile').click(function (e) {
        // Prevents the default action to be triggered. 
        e.preventDefault();

        $('#view-profile-data').addClass('hidden');
        $('#edit-profile-data').removeClass('hidden');
    });
    $('#btnChangePwd').click(function (e) {
        // Prevents the default action to be triggered. 
        e.preventDefault();

        changePassword();
    });
    $('#btnChangePhoto').click(function (e) {
        // Prevents the default action to be triggered. 
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
            $('#imagePerfilUsuario').attr('src', pic1);
            $('#frmShowImage').modal('hide');
        };
        $.fn.returnNO = function (response) {
            $('#ImageDisplayArea').innerHTML = "Lo sentimos, no ha sido posible actualizar la imagen en el servidor. Por favor, inténtelo de nuevo.";
        };
        userObject.changeImage(pic, $(this).returnOK, $(this).returnNO);
    });
    $('#btnCancelImage').click(function (e) {
        // Prevents the default action to be triggered. 
        e.preventDefault();

        $('#frmShowImage').modal('hide');
    });

    //Mejoramos el estilo de la subida de imágenes
    $("input[type=file]").nicefileinput({
        label: 'Browse...'
    });

    //Boton para eliminar una amistad
    $('.lnkRemoveFriend').click(function (e) {
        // Prevents the default action to be triggered. 
        e.preventDefault();

        $.fn.returnOK = function (response) {
            if (response.d >= 0) {
                $('div.elemFriendship').filter(function () {
                    return $(this).data("userid") == iduser;
                }).remove();
                //Actualizamos el contador en la pestaña
                var num = parseInt($('#numTabFriends').text()) - 1;
                $('#numTabFriends').text(num);
            }
        };
        $.fn.returnNO = function (response) {
            alert('Algo salió mal');
        };
        var iduser = $(this).data('fromuserid');
        friendObject.setToUserId(iduser);
        friendObject.remove($(this).returnOK, $(this).returnNO);
    });
    //Boton para eliminar un comentario
    $('.lnkRemoveComment').click(function (e) {
        // Prevents the default action to be triggered. 
        e.preventDefault();

        $.fn.returnOK = function (response) {
            if (response.d >= 0) {
                $('div.elemComment').filter(function () {
                    return $(this).data("commentid") == idcomment;
                }).remove();
                //Actualizamos el contador en la pestaña
                var num = parseInt($('#numTabComments').text()) - 1;
                $('#numTabComments').text(num);
            }
        };
        $.fn.returnNO = function (response) {
            alert('Algo salió mal');
        };
        var idcomment = $(this).data('commentid');
        commentObject.setCommentId(idcomment);
        commentObject.remove($(this).returnOK, $(this).returnNO);
    });
    //Boton para eliminar un indicador
    $('.lnkRemoveIndicator').click(function (e) {
        // Prevents the default action to be triggered. 
        e.preventDefault();

        $.fn.returnOK = function (response) {
            if (response.d >= 0) {
                $('div.elemIndicator').filter(function () {
                    return $(this).data("indicatorid") == idindicator;
                }).remove();
                //Actualizamos el contador en la pestaña
                var num = parseInt($('#numTabIndicators').text()) - 1;
                $('#numTabIndicators').text(num);
            }
        };
        $.fn.returnNO = function (response) {
            alert('Algo salió mal');
        };
        var idindicator = $(this).data('indicatorid');
        indicatorObject.setIdIndicator(idindicator);
        indicatorObject.remove($(this).returnOK, $(this).returnNO);
    });
    //Boton para eliminar un dashboard
    $('.lnkRemoveDashboard').click(function (e) {
        // Prevents the default action to be triggered. 
        e.preventDefault();

        $.fn.returnOK = function (response) {
            if (response.d >= 0) {
                $('div.elemDashboard').filter(function () {
                    return $(this).data("dashboardid") == iddashboard;
                }).remove();
                //Actualizamos el contador en la pestaña
                var num = parseInt($('#numTabDashboards').text()) - 1;
                $('#numTabDashboards').text(num);
            }
        };
        $.fn.returnNO = function (response) {
            alert('Algo salió mal');
        };
        var iddashboard = $(this).data('dashboardid');
        dashboardObject.setIdDashboard(iddashboard);
        dashboardObject.remove($(this).returnOK, $(this).returnNO);
    });
});