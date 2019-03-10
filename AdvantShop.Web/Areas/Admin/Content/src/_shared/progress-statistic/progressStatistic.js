; (function (ng) {
    'use strict';

    var ProgressStatisticCtrl = function ($http) {
        var ctrl = this;

        ctrl.recalc = function () {
            $http.post('catalog/recalculateproductscount').then(function (response) {
                location.reload();
            });
        }
    };

    ProgressStatisticCtrl.$inject = ['$http'];

    ng.module('recalc', [])
        .controller('RecalcCtrl', RecalcCtrl)
        .component('recalcTrigger', {
            template: '<a href="" data-ng-click="$ctrl.recalc()" ng-transclude></a>',
            controller: RecalcCtrl,
            transclude: true
        });

})(window.angular);