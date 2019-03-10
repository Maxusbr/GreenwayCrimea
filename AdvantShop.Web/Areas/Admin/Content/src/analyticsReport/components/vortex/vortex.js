; (function (ng) {
    'use strict';

    var VortexCtrl = function ($http) {
        var ctrl = this;

        ctrl.$onInit = function() {

            if (ctrl.onInit != null) {
                ctrl.onInit({ vortex: ctrl });
            }
        }

        ctrl.recalc = function (dateFrom, dateTo) {
            ctrl.fetch(dateFrom, dateTo);
        }

        ctrl.fetch = function (dateFrom, dateTo) {
            $http.get("analytics/getVortex", {params: { dateFrom: dateFrom, dateTo: dateTo }}).then(function(result) {
                ctrl.vortex = result.data;
            });
        }
    };

    VortexCtrl.$inject = ['$http'];

    ng.module('analyticsReport')
        .controller('VortexCtrl', VortexCtrl)
        .component('vortex', {
            templateUrl: '../areas/admin/content/src/analyticsReport/components/vortex/vortex.html',
            controller: VortexCtrl,
            bindings: {
                onInit: '&'
            }
      });

})(window.angular);