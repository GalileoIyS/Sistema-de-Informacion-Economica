/******************************************************************************************************
**  Obtener los parámetros de una llamada                                                            **
*******************************************************************************************************/
function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

/******************************************************************************************************
**  Obtener el nombre de la anterior página consultada                                               **
*******************************************************************************************************/
function getLastPageName() {
    return document.referrer.split('/').slice(-1)[0].split('?')[0];
}

/******************************************************************************************************
**  Limpiamos las cadenas Json de caracteres especiales                                              **
*******************************************************************************************************/
String.prototype.escapeSpecialChars = function () {
    return this.replace(/\\n/g, "\\n")
               .replace(/\\'/g, "\\'")
               .replace(/'/g, "\\'")
               .replace(/\\"/g, '\\"')
               .replace(/\\&/g, "\\&")
               .replace(/\\r/g, "\\r")
               .replace(/\\t/g, "\\t")
               .replace(/\\b/g, "\\b")
               .replace(/\\f/g, "\\f");
};

/******************************************************************************************************
**  Obtiene todas las solicitudes de amistad y/o mensajes                                            **
*******************************************************************************************************/
function displayCountFriends() {
    var num = localStorage.getItem('numFriendRequest');
    $('.activity-count').html(num);
    $('a.btnViewFriends > span').html(num);
    if (num == 0) {
        $('.activity-count').removeClass('bg-color-red');
    }
    else {
        $('.activity-count').addClass('bg-color-red');
    }
}
function reloadFriends()
{  
    $.fn.displayFriends = function (response) {
        localStorage.setItem('numFriendRequest', response);
        displayCountFriends();
    };
    $.fn.hideFriends = function (response) {
        localStorage.setItem('numFriendRequest', 0);
        displayCountFriends();
    };
    friendObject.setAceptado('N');
    friendObject.count($(this).displayFriends, $(this).hideFriends);
}
function refreshFriends(myuserid, userid)
{
    var currentFriend = $('li.friend-item').filter(function () {
        return ($(this).data("myuserid") == myuserid && $(this).data("userid") == userid);
    }).remove();
    var i = $('.ajax-notifications').friendship('countFriends');
    localStorage.setItem('numFriendRequest', i);
    $('a.btnViewFriends > span').html(i);
    displayCountFriends();
}
function acceptFriendships(iduser) {
    friendObject.setFromUserId(iduser);
    friendObject.accept(null, null);
}
function insertFriendships(iduser) {
    friendObject.setToUserId(iduser);
    friendObject.insert(null, null);
}
function cancelFriendships(iduser)
{
    friendObject.setToUserId(iduser);
    friendObject.remove(null, null);
}
function getFriendships(tab) {
    $.fn.displayFriends = function (response) {
        $('.activity-last-updated').livestamp();
        $('.ajax-notifications').friendship({
            datos: response,
            accept: function (event, data) {
                acceptFriendships(data.userid);  
                refreshFriends(data.myuserid, data.userid);
            },
            reject: function (event, data) {
                cancelFriendships(data.userid);
                refreshFriends(data.myuserid, data.userid);
            },
            cancel: function (event, data) {
                cancelFriendships(data.userid);
                refreshFriends(data.myuserid, data.userid);
            }
        });
    };
    $.fn.hideFriends = function (response) {
        alert('K.O');
    };
    friendObject.setAceptado('N');
    friendObject.populate($(this).displayFriends, $(this).hideFriends);

    if (tab === 0) {
        $('.ajax-notifications').friendship('showFriends');
        var i = $('.ajax-notifications').friendship('countFriends');
        localStorage.setItem('numFriendRequest', i);
        displayCountFriends();
    }
    else {
        $('.ajax-notifications').friendship('showMessages');
        var i = $('.ajax-notifications').friendship('countMessages');
        $('a.btnViewMessages > span').html(i);
    }
}

/******************************************************************************************************
**  Muestra el formulario modal con información de un usuario                                        **
*******************************************************************************************************/
function showUserInfo(iduser)
{
    $.fn.displayUser = function (response) {
        $('#modalUserImage').attr("src", response.imageurl);
        $('#modal-user-name').text(response.apellidos + ', ' + response.nombre);
        $('#modal-user-resume').text(response.resumen);
        $('#modal-user-shared').text(response.shared);
        $('#modal-user-formula').text(response.formulas);
        $('#modal-user-friends').text(response.friends);
        $('#frmUserData-btnYes').data('userid', response.userid).empty();
        $('#frmUserData-btnNo').data('userid', response.userid).empty();

        var items = [];
        if (response.indicadores.length > 0) {
            $.each(response.indicadores, function (index, value) {
                items.push('<li><a href="/indicator.aspx?indicatorid=' + value.indicatorid + '"><img src="' + value.imageurl + '" alt="' + value.titulo + '"></li>');
            });
        }
        $('#modal-user-indicators').html(items.join(''));

        if (response.situacion === 0) {
            $("#frmUserData-btnYes").removeClass('hidden');
            $("#frmUserData-btnNo").addClass('hidden');
            $("<i class='fa fa-chain'></i>").append("&nbsp;Add friendship").appendTo("#frmUserData-btnYes");
        }
        else if (response.situacion === 1) {
            $("#frmUserData-btnYes").removeClass('hidden');
            $("#frmUserData-btnNo").removeClass('hidden');
            $("<i class='fa fa-chain'></i>").append("&nbsp;Accept").appendTo("#frmUserData-btnYes");
            $("<i class='fa fa-chain-broken'></i>").append("&nbsp;Reject").appendTo("#frmUserData-btnNo");
        }
        else if (response.situacion === 2) {
            $("#frmUserData-btnYes").addClass('hidden');
            $("#frmUserData-btnNo").removeClass('hidden');
            $("<i class='fa fa-chain-broken'></i>").append("&nbsp;Cancel friendship").appendTo("#frmUserData-btnNo");
        }

        $('#frmUserData').modal('show');
    };
    $.fn.errorUser = function (response) {
        alert('Algo salio mal');
    };
    userObject.setUserId(iduser);
    userObject.select($(this).displayUser, $(this).errorUser);
}

/******************************************************************************************************
**  Convierte una tabla devuelta en JSON en datos tipo CSV                                           **
*******************************************************************************************************/
function ConvertToCSV(objArray) {
    var array = typeof objArray != 'object' ? JSON.parse(objArray) : objArray;
    var str = '';

    for (var i = 0; i < array.length; i++) {
        var line = '';
        for (var index in array[i]) {
            if (line != '') line += ','

            line += array[i][index];
        }
        str += line + '\r\n';
    }
    return str;
}

/******************************************************************************************************
**  Descarga el contenido de un CSV                                                                  **
*******************************************************************************************************/
function downloadCSV(csv_out, titulo) {
    var blob = new Blob([csv_out], { type: 'text/csv;charset=utf-8' });
    var url = window.URL || window.webkitURL;
    var link = document.createElementNS("http://www.w3.org/1999/xhtml", "a");
    link.href = url.createObjectURL(blob);
    link.download = titulo + '.csv';

    var event = document.createEvent("MouseEvents");
    event.initEvent("click", true, false);
    link.dispatchEvent(event);
}

/******************************************************************************************************
**  Funcion para hacer obligatorio aceptar los términos el servicio                                  **
*******************************************************************************************************/
function AcceptTermsCheckBoxValidation(oSrouce, args) {
    var myCheckBox = $('#cbAcceptTerms');
    if (myCheckBox.is(':checked')) {
        args.IsValid = true;
    }
    else {
        args.IsValid = false;
    }
}

/******************************************************************************************************
**  Modificar el tamaño del menu con el scroll                                                       **
*******************************************************************************************************/
$(window).scroll(function () {
    if ($(".navbar").offset().top > 50) {
        $(".navbar-fixed-top").addClass("top-nav-collapse");
    } else {
        $(".navbar-fixed-top").removeClass("top-nav-collapse");
    }
});

$(document).ready(function () {

    $('#btnFacebookLogin').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        FB.login(function (response) {
            if (response.status === 'connected') {
                // Logged into your app and Facebook.
                FB.api('/me', function (response) {
                    $.fn.displaylogin = function (response) {
                        var url = document.URL,
                            shortUrl = url.substring(0, url.lastIndexOf("/"));
                        window.location.replace(shortUrl + '/registrado/profile.aspx?username=' + response.d);
                    };
                    $.fn.errorlogin = function (response) {
                        alert('Algo salio mal');
                    };

                    userObject.setUserId(response.id);
                    userObject.setNombre(response.first_name);
                    userObject.setApellidos(response.last_name);
                    userObject.setEmail(response.email);
                    userObject.loginUsingFacebook($(this).displaylogin, $(this).errorlogin);
                });
            } else if (response.status === 'not_authorized') {
                // The person is logged into Facebook, but not your app.
            } else {
                // The person is not logged into Facebook, so we're not sure if
                // they are logged into this app or not.
            }
        }, {scope: 'public_profile,email'});
    });

    //Botón para mostrar el panel para darse de alta
    $('#showPanelSignUp').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        $('#PanelLogIn').addClass("hidden");
        $('#PanelSignUp').removeClass("hidden");
    });
    //Botón para mostrar el panel de registro
    $('#showPanelLogIn').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        $('#PanelLogIn').removeClass("hidden");
        $('#PanelSignUp').addClass("hidden");
    });

    //Funciones para recuperar la contraseña perdida
    $('#toogleToRecovery').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        $("#PanelRecoveryPassword").toggle();
    });
    $('#RecoveryButton').click(function () {
        var email = $('#txtPasswordRecovery').val();

        if ((!email) || (email.length == 0)) {
            $('#txtPasswordRecovery').parent().addClass('has-error');
            return;
        }

        var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
        if (!pattern.test(email)) {
            $('#txtPasswordRecovery').parent().addClass('has-warning');
            return;
        }

        $.fn.funcOK = function (result) {
            $('#sendRecoveryEmailMessage').addClass('hidden');
            if (result >= 0) {
                $.SmartMessageBox({
                    title: '<i class="fa fa-envelope" style="color:#7FD87D"></i>&nbsp;Your new password has been successfully sent',
                    content: "Warning: if you find you've not received the email, please be sure to check your spam folder, just incase it's got stuck.",
                    buttons: '[OK]'
                });
            }
            else {
                $('#txtPasswordRecovery').parent().addClass('has-error');
            }
        };
        $.fn.funcFALSE = function (response) {
            $('#sendRecoveryEmailMessage').addClass('hidden');
            $('#txtPasswordRecovery').parent().addClass('has-error');
        };

        $('#sendRecoveryEmailMessage').removeClass('hidden');
        var dataString = "{email:'" + email + "'}";
        proxy.getNewPassword(dataString, $(this).funcOK, $(this).funcFALSE);
    });

    //Funciones para seguir o dejar de seguir a un usuario
    $('.show-modal-info-user').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        var id = $(this).data('userid');
        if (id) {
            showUserInfo(id);
        }
    });
    $('#frmUserData-btnYes').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();
    
        insertFriendships($(this).data('userid'));
        $('#frmUserData').modal('hide');
    });
    $('#frmUserData-btnNo').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        cancelFriendships($(this).data('userid'));
        $('#frmUserData').modal('hide');
    });

    $('.activity-dropdown').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        var $this = $(this);

        if (!$this.next('.ajax-dropdown').is(':visible')) {
            $this.next('.ajax-dropdown').fadeIn(150);
            $this.addClass('active');
            getFriendships(0);
            
        } else {
            $this.next('.ajax-dropdown').fadeOut(150);
            $this.removeClass('active');
        }

    });
    $('.btnViewFriends').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        getFriendships(0);
    });
    $('.btnViewMessages').click(function (event) {
        // Prevents the default action to be triggered. 
        event.preventDefault();

        getFriendships(1);
    });

    //Procedimiento para consultar las peticiones de amistad
    var minutes = 5;
    var now = new Date().getTime();
    var setupRefreshTime = localStorage.getItem('setupRefreshTime');
    if (setupRefreshTime == null) {
        localStorage.setItem('setupRefreshTime', now);
        reloadFriends();
    }
    else {
        if (now - setupRefreshTime > minutes * 60 * 1000) {
            localStorage.clear();
            localStorage.setItem('setupRefreshTime', now);
            reloadFriends();
        }
        else {
            displayCountFriends();
        }
    }

});