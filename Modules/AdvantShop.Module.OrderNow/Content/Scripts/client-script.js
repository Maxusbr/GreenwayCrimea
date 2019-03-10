$(document).ready(function () {
    var artno = $('.details-sku').children('.details-param-value').text();
    var url = window.location.pathname;
        $.ajax({
            url: "ONClient/Index",
            data: { artno, url },
            success: function (data) {
                    var ShowAt = data.substring(0, 4);
                    var message = data.substring(4);
                    if (ShowAt == "rate") {
                        $(".details-rating").prepend("<div id =\"ONclientWrap\" class=\"ONwrap\"></div>");
                    }
                    if (ShowAt == "prce") {
                        $(".details-payment").parent().append("<div id =\"ONclientWrap\" class=\"ONwrap\"></div>");
                    }
                    if (ShowAt == "ship") {
                        $(".details-aside").append("<div id =\"ONclientWrap\" class=\"ONwrap\"></div>");
                        $("#ONclientWrap").css("margin-left", "1em");
                        $("#ONclientWrap").css("margin-right", "1em");
                    }
                    if (ShowAt == "chck") {
                        setTimeout(function () { 
                            checkoutLoader(message)
                    }, 1000);
                    }
                    if (ShowAt == "cart") {
                        setTimeout(function () {
                            cartLoader(message);
                        }, 1000);
                    }
                    if (ShowAt == "clrt") {
                        $(".details-rating").parent().prepend("<div id =\"ONclientWrap\" class=\"ONwrap\"></div>");
                    }
                    if (ShowAt == "clpr") {
                        $(".details-payment").append("<div id =\"ONclientWrap\" class=\"ONwrap\"></div>");
                    }
                    if (ShowAt == "clsh") {
                        $(".details-aside").append("<div id =\"ONclientWrap\" class=\"ONwrap\"></div>");
                    }
                    $("#ONclientWrap").html(message);
            }
        })

        $('.col-xs-6').on('click', '.sizes-viewer-item-selected', function () {
            var artno = $('.details-sku').children('.details-param-value').text();
            $.ajax({
                url: "ONClient/Index",
                data: { artno, url },
                success: function (data) {
                    var message = data.substring(4);
                    $("#ONclientWrap").html(message);
                }
            })
        })
        setInterval(function () {
            var artno = $('.details-sku').children('.details-param-value').text();
            $.ajax({
                url: "ONClient/Index",
                data: { artno, url },
                success: function (data) {
                    var message = data.substring(4);
                    $("#ONclientWrap").html(message);
                }
            })
        }, 60 * 1000);
})

function checkoutLoader(message) {
    var done = false;
    var element = $(".checkout-cart");
    if (element.length > 0) {
        done = true;
        $(".checkout-cart").append("<div id =\"ONclientWrap\" class=\"ONwrap\"></div>");
        $("#ONclientWrap").html(message);
    }
    if (done == false) setTimeout(function () { checkoutLoader(message); }, 1000);
}

function cartLoader(message) {
    var done = false;
    var element = $(".cart-full-buttons");
    if (element.length > 0)
    {
        done = true;
        $(".cart-full-buttons").append("<div id =\"ONclientWrap\" class=\"ONwrap\"></div>");
        $("#ONclientWrap").html(message);
    }
    if (done == false) setTimeout(function () { cartLoader(message); }, 1000);
}