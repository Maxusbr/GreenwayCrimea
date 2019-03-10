// LOADER
$(document).ready(function () {
    "use strict";

    setTimeout(function () {
        $('body').addClass('loaded')
    }, 1000);



// OwlCarousels


    $('.lightbox').magnificPopup({
        type: 'image'
    });


    var owl = $("#shoots");

    owl.owlCarousel({

        itemsCustom: [
               [0, 1],
               [450, 1],
               [600, 1],
               [700, 1],
               [1000, 3],
               [1200, 3],
               [1400, 3],
               [1600, 3]
             ],
        navigation: false,
        pagination: true,
        autoPlay: 3000, // set slide intervals here
        stopOnHover: true

    });

    // Used for main slider/carousel

    $("#main-carousel").owlCarousel({

        navigation: false, // Show next and prev buttons
        pagination: false,
        slideSpeed: 400,
        autoPlay: 6000, // set slide intervals here
        singleItem: true,
        autoHeight: true, // use same size images to change this in to false
        stopOnHover: true

        // "singleItem:true" is a shortcut for:
        // items : 1,
        // itemsDesktop : false,
        // itemsDesktopSmall : false,
        // itemsTablet: false,
        // itemsMobile : false

    });

    // Seeting for quote carousel

    $("#quote-carousel").owlCarousel({

        navigation: false, // Show next and prev buttons
        pagination: false,
        slideSpeed: 300,
        paginationSpeed: 400,
        autoPlay: 5000, // set slide intervals here
        singleItem: true,
        autoHeight: true, // use same size images to change this in to false
        transitionStyle: "fade",
        stopOnHover: true

        // "singleItem:true" is a shortcut for:
        // items : 1,
        // itemsDesktop : false,
        // itemsDesktopSmall : false,
        // itemsTablet: false,
        // itemsMobile : false

    });


    /**    add image o the elemenet via data atribute and class="tile-item" or class="bg-image"
     *  example used in breadcrumbs
     *****************************************************/
    $(".tile-item, .bg-image").css('background', function () {
        var bg = ('url(' + $(this).data("image-src") + ') no-repeat center center');
        return bg;
    });

    // cover data image
    $(".tile-item, .bg-image").css("background-size", "cover");


});



//jQuery for page scrolling feature - requires jQuery Easing plugin
$(function () {
    $('.page-scroll a').bind('click', function (event) {
        var $anchor = $(this);
        $('html, body').stop().animate({
            scrollTop: $($anchor.attr('href')).offset().top
        }, 1500, 'easeInOutExpo');
        event.preventDefault();
    });
});



// jQuery to collapse the navbar on scroll
$(window).scroll(function () {
    //if ($(".navbar").offset().top > 50) {
    //    $(".navbar-fixed-top").addClass("top-nav-collapse");
    //} else {
    //    $(".navbar-fixed-top").removeClass("top-nav-collapse");
    //}
});


/**    Activate tooltips
 *************************************************** **/

jQuery('[data-toggle~="tooltip"]').tooltip({
    container: 'body'
});

/**    Activate popovers
 *************************************************** **/

jQuery('[data-toggle~="popover"]').popover({
    container: 'body'
});


/**    Contact form

 *************************************************** **/



jQuery('#contactform').submit(function () {

    var action = $(this).attr('action');

    jQuery("#message").slideUp(750, function () {
        jQuery('#message').hide();

        jQuery.post(action, {
            name: $('#name').val(),
            email: $('#email').val(),
            phone: $('#phone').val(),
            comments: $('#comments').val()
        },
                    function (data) {
            document.getElementById('message').innerHTML = data;
            $('#message').slideDown('slow');
            if (data.match('success') != null) $('#contactform').slideUp('slow');

        }
                   );

    });

    return false;

});

/**    Change text on contact button when open

 *************************************************** **/



$('.btn-contact').click(function () {

    var $this = $(this);



    $this.toggleClass('Open contact form');



    if ($this.hasClass('Open contact form')) {

        $this.text('Close me if you done');

    } else {

        $this.text('Open contact form');

    }

});
