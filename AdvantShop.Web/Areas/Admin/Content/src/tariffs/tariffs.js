; (function (ng) {
    'use strict';

    var TariffsCtrl = function (lastStatisticsService) {
        var ctrl = this;
        window.addEventListener('message', function (e) {
            if (e.data === "reCountIndicatorsAcademy") {
                lastStatisticsService.getLastStatistics();
            }
        });
    };

    TariffsCtrl.$inject = ['lastStatisticsService'];

    ng.module('tariffs', []).controller('TariffsCtrl', TariffsCtrl);

})(window.angular);