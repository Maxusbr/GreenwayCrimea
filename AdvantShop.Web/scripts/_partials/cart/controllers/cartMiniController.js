; (function (ng) {
    'use strict';

    var CartMiniCtrl = function (cartService) {

        var ctrl = this;

        ctrl.cartData = {};

        cartService.getData().then(function (data) {
            ctrl.cartData = data;
        });

        ctrl.addMinicartList = function (miniCartList) {
            ctrl.list = miniCartList;
        };

        ctrl.triggerClick = function (event) {
            if (event != null) {
                event.preventDefault();
            }

            if (ctrl.cartData.TotalItems > 0) {
                ctrl.list.cartToggle(true);
            }
        };
    };

    ng.module('cart')
      .controller('CartMiniCtrl', CartMiniCtrl);

    CartMiniCtrl.$inject = ['cartService'];

})(angular);