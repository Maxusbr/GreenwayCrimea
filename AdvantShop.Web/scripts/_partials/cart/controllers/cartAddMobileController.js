; (function (ng) {
    'use strict';

    var CartAddCtrl = function ($q, $window, cartConfig, cartService) {

        var ctrl = this,
            cartToolbar,
            timerPopoverHide;

        ctrl.addItem = function (event, offerId, productId, amount, attributesXml, payment, href) {

            event.preventDefault();

            var isValid = ctrl.cartAddValid(),
                deferNoop = $q.defer();

            if (isValid === true || isValid == null) {
                return cartService.addItem(offerId, productId, amount, attributesXml, payment).then(function (result) {

                    if (result[0].status == 'redirect') {
                        $window.location.assign(href);
                    } else {
                        $(document).trigger("add_to_cart", href);
                        $(document).trigger("cart.add", [offerId, productId, amount, attributesXml]);
                        $window.location.assign(href);
                        return result;
                    }
                });
            } else {
                deferNoop.resolve(null);
                return deferNoop.promise;
            }
        };
    };

    ng.module('cart')
        .controller('CartAddCtrl', CartAddCtrl);

    CartAddCtrl.$inject = ['$q', '$window', 'cartConfig', 'cartService'];

})(window.angular);