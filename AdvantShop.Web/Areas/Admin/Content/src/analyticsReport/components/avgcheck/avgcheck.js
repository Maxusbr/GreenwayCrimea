; (function (ng) {
    'use strict';

    var AvgcheckCtrl = function ($http) {
        var ctrl = this;

        ctrl.$onInit = function () {

            ctrl.groupFormatString = 'dd';
            
            if (ctrl.onInit != null) {
                ctrl.onInit({ avgcheck: ctrl });
            }
        }

        ctrl.recalc = function (dateFrom, dateTo, paid, orderStatus) {

            ctrl.dateFrom = dateFrom;
            ctrl.dateTo = dateTo;
            ctrl.paid = paid;
            ctrl.orderStatus = orderStatus;

            ctrl.fetchAvgCheck();
            ctrl.fetchByCity();
        }

        ctrl.changeGroup = function (groupFormatString) {
            ctrl.groupFormatString = groupFormatString;
            ctrl.recalc(ctrl.dateFrom, ctrl.dateTo, ctrl.paid, ctrl.orderStatus);
        }

        ctrl.fetchAvgCheck = function() {
            $http.get("analytics/getAvgCheck", { params: { type: "avg", dateFrom: ctrl.dateFrom, dateTo: ctrl.dateTo, paid: ctrl.paid, orderStatus: ctrl.orderStatus, groupFormatString: ctrl.groupFormatString } }).then(function (result) {
                ctrl.AvgData = result.data;
            });
        }

        ctrl.fetchByCity = function () {
            $http.get("analytics/getAvgCheck", { params: { type: "city", dateFrom: ctrl.dateFrom, dateTo: ctrl.dateTo, paid: ctrl.paid, orderStatus: ctrl.orderStatus } }).then(function (result) {
                ctrl.AvgCityData = result.data;
            });
        }

    };

    AvgcheckCtrl.$inject = ['$http'];

    ng.module('analyticsReport')
        .controller('AvgcheckCtrl', AvgcheckCtrl)
        .component('avgcheck', {
            templateUrl: '../areas/admin/content/src/analyticsReport/components/avgcheck/avgcheck.html',
            controller: AvgcheckCtrl,
            bindings: {
                onInit: '&'
            }
      });

})(window.angular);