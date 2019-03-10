; (function (ng) {
    'use strict';

    var ShippingByWeightLimitCtrl = function ($http, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            
            ctrl.ranges = [];

            if (ctrl.weightLimit != null && ctrl.weightLimit !== '') {
                ctrl.ranges = JSON.parse(ctrl.weightLimit);
                ctrl.ranges.sort(compare);
            }
        };

        ctrl.addRange = function() {

            // todo: use globalization for parseFloat
            ctrl.ranges.push({ Amount: ctrl.Amount, PerUnit: ctrl.PerUnit, Price: ctrl.Price });
            ctrl.ranges.sort(compare);

            ctrl.updateWeightLimit();

            ctrl.Amount = null;
            ctrl.Price = null;
            ctrl.PerUnit = false;
        }

        ctrl.deleteRange = function (index) {

            ctrl.ranges.splice(index, 1);
            ctrl.updateWeightLimit();
        }

        ctrl.updateWeightLimit = function() {
            ctrl.weightLimit = JSON.stringify(ctrl.ranges);
            ctrl.update = true;
        }

        function compare(a, b) {
            if (a.Amount < b.Amount)
                return -1;
            if (a.Amount > b.Amount)
                return 1;
            return 0;
        }
    };

    ShippingByWeightLimitCtrl.$inject = ['$http', 'toaster'];

    ng.module('shippingMethod')
        .controller('ShippingByWeightLimitCtrl', ShippingByWeightLimitCtrl)
        .component('shippingByWeightLimit', {
            templateUrl: '../areas/admin/content/src/shippingMethods/components/shippingByRangeWeightAndDistance/templates/shippingByWeightLimit.html',
            controller: 'ShippingByWeightLimitCtrl',
            bindings: {
                onInit: '&',
                methodId: '@',
                weightLimit: '@',
                currencyLabel: '@'
            }
        });

})(window.angular);