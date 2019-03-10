; (function (ng) {
    'use strict';

    var ShippingByDistanceLimitCtrl = function ($http, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            
            ctrl.ranges = [];

            if (ctrl.distanceLimit != null && ctrl.distanceLimit !== '') {
                ctrl.ranges = JSON.parse(ctrl.distanceLimit);
                ctrl.ranges.sort(compare);
            }
        };

        ctrl.addRange = function() {

            // todo: use globalization for parseFloat
            ctrl.ranges.push({ Amount: ctrl.Amount, PerUnit: ctrl.PerUnit, Price:ctrl.Price });
            ctrl.ranges.sort(compare);

            ctrl.updateDistanceLimit();

            ctrl.Amount = null;
            ctrl.Price = null;
            ctrl.PerUnit = false;
        }

        ctrl.deleteRange = function (index) {

            ctrl.ranges.splice(index, 1);
            ctrl.updateDistanceLimit();
        }

        ctrl.updateDistanceLimit = function() {
            ctrl.distanceLimit = JSON.stringify(ctrl.ranges);
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

    ShippingByDistanceLimitCtrl.$inject = ['$http', 'toaster'];

    ng.module('shippingMethod')
        .controller('ShippingByDistanceLimitCtrl', ShippingByDistanceLimitCtrl)
        .component('shippingByDistanceLimit', {
            templateUrl: '../areas/admin/content/src/shippingMethods/components/shippingByRangeWeightAndDistance/templates/shippingByDistanceLimit.html',
            controller: 'ShippingByDistanceLimitCtrl',
            bindings: {
                onInit: '&',
                methodId: '@',
                distanceLimit: '@',
                currencyLabel: '@'
            }
        });

})(window.angular);