; (function (ng) {
    'use strict';

    var AbcxyzAnalysisCtrl = function ($http) {
        var ctrl = this;

        ctrl.$onInit = function () {
            if (ctrl.onInit != null) {
                ctrl.onInit({ abcxyz: ctrl });
            }
        }

        ctrl.recalc = function (dateFrom, dateTo) {
            ctrl.from = dateFrom;
            ctrl.to = dateTo;

            ctrl.fetch();
        }

        ctrl.fetch = function () {
            $http.get("analytics/getAbcxyzAnalysis", { params: { dateFrom: ctrl.from, dateTo: ctrl.to } }).then(function (result) {
                ctrl.Data = result.data;
            });
        }

        ctrl.showProducts = function (group) {
            var url = 'analytics/analyticsFilter?from=' + moment(ctrl.from, 'DD.MM.YYYY').format('DD.MM.YYYY') +
                                                 '&to=' + moment(ctrl.to, 'DD.MM.YYYY').format('DD.MM.YYYY') +
                                                 '&group=' + group +
                                                 '&type=abcxyz';
            var win = window.open(url, '_blank');
            win.focus();
        }
    };

    AbcxyzAnalysisCtrl.$inject = ['$http'];

    ng.module('analyticsReport')
        .controller('AbcxyzAnalysisCtrl', AbcxyzAnalysisCtrl)
        .component('abcxyzAnalysis', {
            templateUrl: '../areas/admin/content/src/analyticsReport/components/abcxyzAnalysis/abcxyzAnalysis.html',
            controller: AbcxyzAnalysisCtrl,
            bindings: {
                onInit: '&'
            }
      });

})(window.angular);