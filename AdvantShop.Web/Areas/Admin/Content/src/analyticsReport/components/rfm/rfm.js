; (function (ng) {
    'use strict';

    var RfmCtrl = function ($http) {
        var ctrl = this;

        ctrl.$onInit = function () {
            if (ctrl.onInit != null) {
                ctrl.onInit({ rfm: ctrl });
            }
        }

        ctrl.recalc = function (dateFrom, dateTo) {
            ctrl.fetch(dateFrom, dateTo);
        }

        ctrl.fetch = function (dateFrom, dateTo) {
            ctrl.dateFrom = dateFrom;
            ctrl.dateTo = dateTo;

            $http.get("analytics/getRfm", { params: { dateFrom: dateFrom, dateTo: dateTo }}).then(function (result) {
                ctrl.Data = result.data;

                ctrl.Rm = ctrl.Data.Rm;
                ctrl.Rf = ctrl.Data.Rf;
            });
        }

        ctrl.showRm = function (i, j) {
            var url = 'analytics/analyticsFilter?type=rfm&group=r_m_' + (i + 1) + "_" + (j + 1) + "&from=" + ctrl.dateFrom + "&to=" + ctrl.dateTo;
            var win = window.open(url, '_blank');
            win.focus();
        }

        ctrl.showRf = function (i, j) {
            var url = 'analytics/analyticsFilter?type=rfm&group=r_f_' + (i + 1) + "_" + (j + 1) + "&from=" + ctrl.dateFrom + "&to=" + ctrl.dateTo;
            var win = window.open(url, '_blank');
            win.focus();
        }

        ctrl.range = function (min, max) {
            var input = [];
            for (var i = min; i <= max; i += 1) {
                input.push(i);
            }
            return input;
        }

        ctrl.rangeNg = function (max, min) {
            var input = [];
            for (var i = max; i >= min; i -= 1) {
                input.push(i);
            }
            return input;
        }
    };

    RfmCtrl.$inject = ['$http'];

    ng.module('analyticsReport')
        .controller('RfmCtrl', RfmCtrl)
        .component('rfm', {
            templateUrl: '../areas/admin/content/src/analyticsReport/components/rfm/rfm.html',
            controller: RfmCtrl,
            bindings: {
                onInit: '&'
            }
      });

})(window.angular);