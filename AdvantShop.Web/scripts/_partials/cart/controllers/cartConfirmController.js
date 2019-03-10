; (function (ng) {
    'use strict';

    var CartConfirmCtrl = function (cartService) {

        var ctrl = this;

        ctrl.cartData = {};

        cartService.getData().then(function (data) {
            ctrl.cartData = data;
        });

    };

    ng.module('cart')
      .controller('CartConfirmCtrl', CartConfirmCtrl);

    CartConfirmCtrl.$inject = ['cartService'];

})(angular);