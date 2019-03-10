; (function (ng) {
    'use strict';

    var CartMiniListCtrl = function ($element, $timeout, $window, $scope, cartService, cartConfig, domService) {

        var ctrl = this,
            timer;

        ctrl.isPopup = ctrl.isPopup != null ? ctrl.isPopup() : true;
        ctrl.showEmptyCart = ctrl.showEmptyCart != null ? ctrl.showEmptyCart() : true;

        ctrl.isVisibleCart = ctrl.isPopup === true ? false : true;
        ctrl.isCartMiniFixed = false;

        ctrl.cartOpen = function (startTime) {

            if (startTime == null) {
                startTime = true;
            }

            ctrl.isVisibleCart = true;

            ctrl.saveStartPosition();

            if (startTime === true) {
                ctrl.startTimerClose();
            }

        };

        ctrl.saveStartPosition = function () {
            $timeout(function () {

                var offset = $element[0].children[0].getBoundingClientRect();

                ctrl.staticPosition = {
                    top: offset.top,
                    left: offset.left,
                    width: offset.width
                };

                //if (ctrl.staticPosition.top < 0) {
                ctrl.staticPosition.top += $window.pageYOffset;
                //}

                ctrl.checkFixed();
            }, 100);
        };

        ctrl.cartClose = function () {

            ctrl.isCartMiniFixed = false;

            ctrl.isVisibleCart = false;

            ctrl.clearTimerClose();
        };

        ctrl.cartToggle = function (startTime) {
            ctrl.isVisibleCart === true ? ctrl.cartClose() : ctrl.cartOpen(startTime);
        }

        ctrl.cartMouseEnter = function () {
            ctrl.clearTimerClose();
        };

        ctrl.cartMouseLeave = function (event) {
            ctrl.startTimerClose();
        };

        ctrl.checkVisibleCart = function () {
            return ctrl.isVisibleCart === true && (ctrl.showEmptyCart === false ? ctrl.cartData.TotalItems > 0 : true);
        };

        ctrl.windowScroll = function (event) {

            if (ctrl.checkVisibleCart() === true) {
                $scope.$apply(ctrl.checkFixed);
            }
        };

        ctrl.startTimerClose = function () {
            timer = $timeout(function () { ctrl.cartClose(); }, cartConfig.cartMini.delayHide);
        };

        ctrl.clearTimerClose = function () {
            if (timer != null) {
                $timeout.cancel(timer);
            }
        };

        ctrl.checkFixed = function () {
            ctrl.isCartMiniFixed = $window.pageYOffset > ctrl.staticPosition.top;
        };

        ctrl.clickOut = function (event) {
            var parentCart = domService.closest(event.target, '[data-cart-mini]');

            if (parentCart == null && ctrl.checkVisibleCart() === true) {
                $scope.$apply(function () {
                    ctrl.cartClose();
                });
            }
        };

        ctrl.updateAmount = function (value, itemId) {

            var item = {
                Key: itemId,
                Value: value
            };

            cartService.updateAmount([item]);
        };

        ctrl.remove = function (shoppingCartItemId) {
            cartService.removeItem(shoppingCartItemId).then(function (result) {
                $(document).trigger("cart.remove", result.offerId);
            });
        };

        cartService.addCallback(cartConfig.callbackNames.add, ctrl.cartOpen, 'cartMiniList');
    };

    ng.module('cart')
      .controller('CartMiniListCtrl', CartMiniListCtrl);

    CartMiniListCtrl.$inject = ['$element', '$timeout', '$window', '$scope', 'cartService', 'cartConfig', 'domService'];

})(angular);