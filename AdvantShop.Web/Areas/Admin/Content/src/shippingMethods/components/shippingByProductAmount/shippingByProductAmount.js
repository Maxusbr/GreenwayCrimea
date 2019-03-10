; (function (ng) {
    'use strict';

    var ShippingByProductAmountCtrl = function ($http, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {

            ctrl.ranges = [];

            if (ctrl.priceRanges != null && ctrl.priceRanges !== '') {
                ctrl.ranges = ctrl.priceRanges.split(';')
                    .map(function (x) {
                        var arr = x.split('=');
                        return { amount: parseFloat(arr[0]), price: parseFloat(arr[1]) };
                    });

                ctrl.ranges.sort(compare);
            }
        };

        ctrl.addRange = function () {

            // todo: use globalization for parseFloat
            ctrl.ranges.push({ amount: ctrl.amount, price: ctrl.price });
            ctrl.ranges.sort(compare);

            ctrl.updatePriceRanges();

            ctrl.amount = null;
            ctrl.price = null;
        }

        ctrl.deleteRange = function (index) {

            ctrl.ranges.splice(index, 1);
            ctrl.updatePriceRanges();
        }

        ctrl.updatePriceRanges = function () {
            ctrl.priceRanges = ctrl.ranges.map(function (x) { return x.amount + "=" + x.price }).join(';');
            ctrl.update = true;
        }

        function compare(a, b) {
            if (a.amount < b.amount)
                return -1;
            if (a.amount > b.amount)
                return 1;
            return 0;
        }
    };

    ShippingByProductAmountCtrl.$inject = ['$http', 'toaster'];

    ng.module('shippingMethod')
        .controller('ShippingByProductAmountCtrl', ShippingByProductAmountCtrl)
        .component('shippingByProductAmount', {
            templateUrl: '../areas/admin/content/src/shippingMethods/components/shippingByProductAmount/templates/shippingByProductAmount.html',
            controller: 'ShippingByProductAmountCtrl',
            bindings: {
                onInit: '&',
                methodId: '@',
                priceRanges: '@',
                currencyLabel: '@'
            }
        });

})(window.angular);