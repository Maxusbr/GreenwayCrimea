; (function (ng) {
    'use strict';

    var PointDeliveryMethodCtrl = function ($http, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {

            ctrl.pointsArr = [];

            if (ctrl.points != null && ctrl.points.length > 0) {
                ctrl.pointsArr = ctrl.points.split(';')
                    .map(function (x, index) {
                        return { pointAddress: x };
                    });
            }
        };

        ctrl.addPoint = function (pointAddress) {

            ctrl.pointsArr.push({ pointAddress: pointAddress });

            ctrl.updatePoints();

            ctrl.pointAddress = '';
        }

        ctrl.deletePoint = function (item) {

            var index = ctrl.pointsArr.indexOf(item);

            if (index !== -1) {
                ctrl.pointsArr.splice(index, 1);
                ctrl.updatePoints();
            }
        }

        ctrl.updatePoints = function () {
            ctrl.points = ctrl.pointsArr.map(function (x) { return x.pointAddress; }).join(';');
            ctrl.update = true;
        }

        ctrl.sortableOptions = {
            containment: "#pointDeliverySortingContainer",
            scrollableContainer: '#pointDeliverySortingContainer',
            containerPositioning: 'relative',
            accept: function (sourceItemHandleScope, destSortableScope) {
                return sourceItemHandleScope.itemScope.sortableScope.$id === destSortableScope.$id;
            },
            orderChanged: function (event) {
                ctrl.updatePoints();
            }
        };
    };

    PointDeliveryMethodCtrl.$inject = ['$http', 'toaster'];

    ng.module('shippingMethod')
        .controller('PointDeliveryMethodCtrl', PointDeliveryMethodCtrl)
        .component('pointDelivery', {
            templateUrl: '../areas/admin/content/src/shippingMethods/components/pointDelivery/templates/pointDelivery.html',
            controller: 'PointDeliveryMethodCtrl',
            bindings: {
                onInit: '&',
                methodId: '@',
                points: '@'
            }
        });

})(window.angular);