$(function () {

    // Tracking events
    // Trigger example: $(document).trigger("add_to_cart");
    $(document).on("add_to_cart", function (e, url) {
        try {
            var path = url.indexOf("products/") != -1 ? url.replace("products/", "") : window.location.pathname.replace("products/", "").replace("/", "");

            // virtual page view '/addtocart/some-product-name'
            if (typeof ga != 'undefined') {
                ga('send', 'pageview', '/addtocart/' + path);
                ga('send', 'event', "Advantshop_events", "addToCart", document.URL);
            }

            if (typeof dataLayer != 'undefined') {
                dataLayer.push({ 'event': 'addToCart' });
            }
        } catch (err) { }
    })
        .on("buy_one_click_pre", function () {
            try {
                if (typeof ga != 'undefined') {
                    ga('send', 'event', "Advantshop_events", "buyOneClickForm", document.URL);
                }

                if (typeof dataLayer != 'undefined') {
                    dataLayer.push({ 'event': 'buyOneClickForm' });
                }

            } catch (err) { }
        })
        .on("buy_one_click_confirm", function () {
            try {
                if (typeof ga != 'undefined') {
                    ga('send', 'event', "Advantshop_events", "buyOneClickConfirm", document.URL);
                }

                if (typeof dataLayer != 'undefined') {
                    dataLayer.push({ 'event': 'buyOneClickConfirm' });
                }

            } catch (err) { }
        })
        .on("compare.add", function () {
            try {
                if (typeof ga != 'undefined') {
                    ga('send', 'event', "Advantshop_events", "addToCompare", document.URL);
                }

                if (typeof dataLayer != 'undefined') {
                    dataLayer.push({ 'event': 'addToCompare' });
                }

            } catch (err) { }
        })
        .on("add_to_wishlist", function () {
            try {
                if (typeof ga != 'undefined') {
                    ga('send', 'event', "Advantshop_events", "addToWishlist", document.URL);
                }

                if (typeof dataLayer != 'undefined') {
                    dataLayer.push({ 'event': 'addToWishlist' });
                }
            } catch (err) { }
        })
        .on("send_feedback", function () {
            try {
                if (typeof ga != 'undefined') {
                    ga('send', 'event', "Advantshop_events", "sendFeedback", document.URL);
                }

                if (typeof dataLayer != 'undefined') {
                    dataLayer.push({ 'event': 'sendFeedback' });
                }
            } catch (err) { }
        })
        .on("add_response", function () {
            try {
                if (typeof ga != 'undefined') {
                    ga('send', 'event', "Advantshop_events", "addResponse", document.URL);
                }

                if (typeof dataLayer != 'undefined') {
                    dataLayer.push({ 'event': 'addResponse' });
                }
            } catch (err) { }
        })
        .on("module_callback", function () {
            try {
                if (typeof ga != 'undefined') {
                    ga('send', 'event', "Advantshop_events", "getCallBack", document.URL);
                }

                if (typeof dataLayer != 'undefined') {
                    dataLayer.push({ 'event': 'getCallBack' });
                }

            } catch (err) { }

        });
});