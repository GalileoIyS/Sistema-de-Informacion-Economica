$(window).scroll(function () {
    var topOfWindow = $(window).scrollTop();

    var imagePos1 = $('#imgColLeft').offset().top;
    var imagePos2 = $('#imgColMed').offset().top;
    var imagePos3 = $('#imgColRight').offset().top;

    if (imagePos1 < topOfWindow + 600) {
        $('#imgColLeft').addClass("slideRight");
    }
    if (imagePos2 < topOfWindow + 600) {
        $('#imgColMed').addClass("fadeIn");
    }
    if (imagePos3 < topOfWindow + 600) {
        $('#imgColRight').addClass("slideLeft");
    }

    $('.animate-title').each(function () {
        var imagePos = $(this).offset().top;

        var topOfWindow = $(window).scrollTop();
        if (imagePos < topOfWindow + 400) {
            $(this).addClass("hatch");
        }
    });
    $('.timeline-image').each(function () {
        var imagePos = $(this).offset().top;

        var topOfWindow = $(window).scrollTop();
        if (imagePos < topOfWindow + 400) {
            $(this).addClass("fadeIn");
        }
    });
    $('.timeline-panel').each(function () {
        var imagePos = $(this).offset().top;

        var topOfWindow = $(window).scrollTop();
        if (imagePos < topOfWindow + 400) {
            $(this).addClass("expandUp");
        }
    });
});