; (function (ng) {
    'use strict';

    var RecalcCtrl = function ($http) {
        var ctrl = this;
        
        ctrl.recalc = function() {
            $http.post('catalog/recalculateproductscount').then(function (response) {
                location.reload();
            });
        }
    };

    RecalcCtrl.$inject = ['$http'];

    ng.module('recalc', [])
        .controller('RecalcCtrl', RecalcCtrl)
        .component('recalcTrigger', {
          template: '<a href="" data-ng-click="$ctrl.recalc()" ng-transclude></a>',
          controller: RecalcCtrl,
          transclude: true
      });

})(window.angular);