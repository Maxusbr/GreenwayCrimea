; (function (ng) {
    'use strict';

    var TelephonyCtrl = function ($http) {
        var ctrl = this;

        ctrl.$onInit = function () {

            ctrl.groupFormatString = 'dd';
            
            if (ctrl.onInit != null) {
                ctrl.onInit({ telephony: ctrl });
            }
        }

        ctrl.recalc = function (dateFrom, dateTo) {
            ctrl.dateFrom = dateFrom;
            ctrl.dateTo = dateTo;

            ctrl.fetchCallsIn();
            ctrl.fetchCallsMissed();
            ctrl.fetchCallsOut();
            ctrl.fetchCallsAvgtime();
        }

        ctrl.changeGroup = function (groupFormatString) {
            ctrl.groupFormatString = groupFormatString;
            ctrl.recalc(ctrl.dateFrom, ctrl.dateTo, ctrl.paid, ctrl.orderStatus);
        }

        ctrl.fetchCallsIn = function () {
            $http.get("analytics/getTelephony", { params: { type: "in", dateFrom: ctrl.dateFrom, dateTo: ctrl.dateTo, groupFormatString: ctrl.groupFormatString } }).then(function (result) {
                ctrl.IncomingCalls = result.data;
            });
        }

        ctrl.fetchCallsMissed = function () {
            $http.get("analytics/getTelephony", { params: { type: "missed", dateFrom: ctrl.dateFrom, dateTo: ctrl.dateTo, groupFormatString: ctrl.groupFormatString } }).then(function (result) {
                ctrl.MissedCalls = result.data;
            });
        }

        ctrl.fetchCallsOut = function () {
            $http.get("analytics/getTelephony", { params: { type: "out", dateFrom: ctrl.dateFrom, dateTo: ctrl.dateTo, groupFormatString: ctrl.groupFormatString } }).then(function (result) {
                ctrl.OutgoingCalls = result.data;
            });
        }

        ctrl.fetchCallsAvgtime = function () {
            $http.get("analytics/getTelephony", { params: { type: "avgtime", dateFrom: ctrl.dateFrom, dateTo: ctrl.dateTo, groupFormatString: ctrl.groupFormatString } }).then(function (result) {
                ctrl.AvgDuration = result.data;
            });
        }
    };

    TelephonyCtrl.$inject = ['$http'];

    ng.module('analyticsReport')
        .controller('TelephonyCtrl', TelephonyCtrl)
        .component('telephony', {
            templateUrl: '../areas/admin/content/src/analyticsReport/components/telephony/telephony.html',
            controller: TelephonyCtrl,
            bindings: {
                onInit: '&'
            }
      });

})(window.angular);