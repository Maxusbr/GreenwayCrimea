; (function (ng) {
    'use strict';

    var CartCountCtrl = function ($filter, cartService) {

        var ctrl = this;

        cartService.getData().then(function (data) {
            ctrl.cartData = data;
        });

        ctrl.getValue = function () {

            var result;

            if (ctrl.cartData == null) {
                result = ctrl.startValue;
            } else {
                switch (ctrl.type) {
                    case 'count':
                        result = ctrl.cartData.Count
                        break;
                    default:
                        result = $filter('number')(ctrl.cartData.TotalItems);
                }
            }
            return result;
        };

    };

    ng.module('cart')
      .controller('CartCountCtrl', CartCountCtrl);

    CartCountCtrl.$inject = ['$filter', 'cartService'];

})(angular);