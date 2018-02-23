$(window).scroll(function () {
    var topOfWindow = $(window).scrollTop();

    var imagePos1 = $('#imgDefault1').offset().top;
    var imagePos2 = $('#imgDefault2').offset().top;
    var imagePos3 = $('#imgDefault3').offset().top;
    var imagePos4 = $('#imgDefault4').offset().top;

    if (imagePos1 < topOfWindow + 600) {
        $('#imgDefault1').addClass("slideRight");
    }
    if (imagePos2 < topOfWindow + 600) {
        $('#imgDefault2').addClass("slideLeft");
    }
    if (imagePos3 < topOfWindow + 600) {
        $('#imgDefault3').addClass("fadeIn");
    }
    if (imagePos4 < topOfWindow + 600) {
        $('#imgDefault4').addClass("fadeIn");
    }
});