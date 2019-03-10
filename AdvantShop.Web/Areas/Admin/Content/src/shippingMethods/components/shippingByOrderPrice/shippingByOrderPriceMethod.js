; (function (ng) {
    'use strict';

    var ShippingByOrderPriceMethodCtrl = function ($http, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            
            ctrl.ranges = [];
            
            if (ctrl.priceRanges != null && ctrl.priceRanges !== '') {
                ctrl.ranges = ctrl.priceRanges.split(';')
                    .map(function (x) {
                        var arr = x.split('=');
                        return { orderPrice: parseFloat(arr[0]), shippingPrice: parseFloat(arr[1]) };
                    });

                ctrl.ranges.sort(compare);
            }
        };

        ctrl.addRange = function() {

            // todo: use globalization for parseFloat
            ctrl.ranges.push({ orderPrice: ctrl.orderPrice, shippingPrice: ctrl.shippingPrice });
            ctrl.ranges.sort(compare);

            ctrl.updatePriceRanges();

            ctrl.orderPrice = null;
            ctrl.shippingPrice = null;
        }

        ctrl.deleteRange = function (index) {

            ctrl.ranges.splice(index, 1);
            ctrl.updatePriceRanges();
        }

        ctrl.updatePriceRanges = function() {
            ctrl.priceRanges = ctrl.ranges.map(function (x) { return x.orderPrice + "=" + x.shippingPrice }).join(';');
            ctrl.update = true;
        }

        function compare(a, b) {
            if (a.orderPrice < b.orderPrice)
                return -1;
            if (a.orderPrice > b.orderPrice)
                return 1;
            return 0;
        }
    };

    ShippingByOrderPriceMethodCtrl.$inject = ['$http', 'toaster'];

    ng.module('shippingMethod')
        .controller('ShippingByOrderPriceMethodCtrl', ShippingByOrderPriceMethodCtrl)
        .component('shippingByOrderPriceMethod', {
            templateUrl: '../areas/admin/content/src/shippingMethods/components/shippingByOrderPrice/templates/shippingByOrderPriceMethod.html',
            controller: 'ShippingByOrderPriceMethodCtrl',
            bindings: {
                onInit: '&',
                methodId: '@',
                priceRanges: '@',
                currencyLabel: '@'
            }
        });

})(window.angular);