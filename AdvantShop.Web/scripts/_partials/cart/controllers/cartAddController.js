; (function (ng) {
    'use strict';

    var CartAddCtrl = function ($q, $timeout, $window, cartConfig, cartService, moduleService, popoverService) {

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
                        $(document).trigger("cart.add", [offerId, productId, amount, attributesXml, result[0].cartId, event.target]);

                        moduleService.update(['minicartmessage', 'fullcartmessage']).then(ctrl.popoverModule);

                        return result;
                    }
                })
            } else {
                deferNoop.resolve(null);
                return deferNoop.promise;
            }
        };

        ctrl.popoverModule = function (content) {
            if (moduleService.getModule('minicartmessage') != null && content[0].trim().length > 0) {

                $timeout(function () {
                    popoverService.getPopoverScope('popoverCartToolbar').then(function (popoverScope) {

                        cartToolbar = cartToolbar || document.getElementById('cartToolbar');

                        popoverScope.active(cartToolbar);

                        popoverScope.updatePosition(cartToolbar);

                        if (timerPopoverHide != null) {
                            $timeout.cancel(timerPopoverHide);
                        }

                        timerPopoverHide = $timeout(function () {
                            popoverScope.deactive();
                        }, 5000);
                    });
                }, 0)
            }
        };
    };

    ng.module('cart')
      .controller('CartAddCtrl', CartAddCtrl);

    CartAddCtrl.$inject = ['$q', '$timeout', '$window', 'cartConfig', 'cartService', 'moduleService', 'popoverService'];

})(window.angular);