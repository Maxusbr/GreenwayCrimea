; (function () {

    'use strict';

    window.addEventListener('load', function load() {

        window.removeEventListener('load', load);

        $(document).on("cart.add", function () {
            sendCart();
        });

        $(document).on("cart.update", function () {
            sendCart();
        });

        $(document).on("cart.clear", function () {
            sendCart();
        });

        $(document).on("cart.remove", function () {
            sendCart();
        });

        function sendCart() {
            try {
                $.ajax({
                    dataType: "json",
                    cache: false,
                    url: "convead",
                    success: function (data) {
                        if (data != null && data.length > 0) {
                            convead('event', 'update_cart', {
                                items: data
                            });
                        }
                    }
                });
            } catch (e) { console.log(e); }
        }
    });
})();