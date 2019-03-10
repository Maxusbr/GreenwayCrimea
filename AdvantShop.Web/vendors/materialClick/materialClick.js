///////////////////////////////////////////////////////
///////////////////////////////////////////////////////
///////////////////////////////////////////////////////
// Made my Andrew Vaganov <prostoandrei91@gmail.com> //
///////////////// Use it and have fun /////////////////
///////////////////////////////////////////////////////
///////////////////////////////////////////////////////

function materialClickInit() {
    //fix for tap highlighting on touchdevice
    document.addEventListener("touchstart", function () { }, true);

    //make material click effects
    var parent, ink, d, x, y;
    $("body").on("click", ".inked", function (e) {
        parent = $(this);
        e.stopPropagation();
        //create .ink element if it doesn't exist
        if (parent.find(".ink").length == 0)
            parent.prepend("<span class='ink'></span>");

        ink = parent.find(".ink");
        //incase double click
        ink.removeClass("animate");

        //size of .ink
        if (!ink.height() && !ink.width()) {
            //take parent width and height to make something like a circle
            d = Math.max(parent.outerWidth() / 2, parent.outerHeight());
            ink.css({ height: d, width: d });
        }

        //take click coordinates
        x = e.pageX - parent.offset().left - ink.width() / 2;
        y = e.pageY - parent.offset().top - ink.height() / 2;

        //set the position and add class .animate
        ink.css({ top: y + 'px', left: x + 'px' }).addClass("animate");
    })
};