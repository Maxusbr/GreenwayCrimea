document.addEventListener('DOMContentLoaded', function () {
    // Tracking events
    // Trigger example: $(document).trigger("add_to_cart");
    $(document)
        .on("registration", function (e, url) {
			trackEvent("registration");
        })
        .on("add_to_cart", function (e, url) {
            trackEvent("addToCart");
        })
        .on("buy_one_click_pre", function () {
            trackEvent("buyOneClickForm");
        })
        .on("buy_one_click_confirm", function () {
            trackEvent("buyOneClickConfirm");
        })
        .on("compare.add", function () {
            trackEvent("addToCompare");
        })
        .on("add_to_wishlist", function () {
            trackEvent("addToWishlist");
        })
        .on("send_feedback", function () {
            trackEvent("sendFeedback");
        })
        .on("send_preorder", function () {
            trackEvent("sendPreOrder");
        })
        .on("add_response", function () {
            trackEvent("addResponse");
        })
        .on("callback", function () {
            trackEvent("callBack");
        })
        .on("callback_request", function () {
            trackEvent("callBackRequest");
        })
        .on("module_callback", function () {
            trackEvent("getCallBack");
        })
        .on("subscribe.email", function () {
            trackEvent("subscribeNews");
        })
        .on("order_from_mobile", function () {
            trackEvent("orderFromMobile");
        });

    // Send event
    function trackEvent(target) {
        try {
            var counterId = $(".yacounterid").attr("data-counterId");
            var yaCounter = window["yaCounter" + counterId];
            yaCounter.reachGoal(target);
        } catch (err) {
        }
    }

    $(document).on("cart.add", function (e, offerId, productId, amount, attributesXml, cartId) {

        if (window.dataLayer == null) {
            return false;
        }

        $.ajax({
            type: 'GET',
            async: false,
            dataType: 'json',
            url: 'yametrika/getproductbyid?offerId=' + offerId + "&productId=" + productId + "&cartId=" + cartId,
            success: function (data) {
                if (data != null && data.artno != null) {
                    window.dataLayer.push({
                        "ecommerce": {
                            "add": {
                                "products": [{ "id": data.artno, "name": data.name, "price": data.price, "brand": data.brand, "category": data.category, "quantity": amount || data.amount }]
                            }
                        }
                    });
                }
            }
        });
    });


    $(document).on("cart.remove", function (e, offerId) {

        if (window.dataLayer == null) {
            return false;
        }

        $.ajax({
            type: 'GET',
            async: false,
            dataType: 'json',
            url: 'yametrika/getproductbyofferid?offerid=' + offerId,
            success: function (data) {
                if (data != null && data.artno != null) {
                    window.dataLayer.push({
                        "ecommerce": {
                            "remove": {
                                "products": [{ "id": data.artno, "name": data.name }]
                            }
                        }
                    });
                }
            }
        });
    });

});