; (function (ng) {
    'use strict';

    ng.module('inplace')
      .directive('inplaceStart', ['$compile', '$rootScope', function ($compile, $rootScope) {
          return {
              restrict: 'A',
              scope: {},
              link: function (scope, element, attrs, ctrl) {
                  var objs = document.querySelectorAll('[data-inplace-rich], [data-inplace-modal], [data-inplace-image], [data-inplace-autocomplete], [data-inplace-properties-new], [data-inplace-image], [data-inplace-price], [data-inplace-price-panel], [data-inplace-switch]');
                  if (objs != null && objs.length > 0) {
                      $compile(objs)($rootScope);
                  }
              }
          };
      }]);

    ng.module('inplace')
    .directive('inplaceSwitch', [function ($compile) {
        return {
            restrict: 'A',
            scope: true,
            controller: 'InplaceSwitchCtrl',
            controllerAs: 'inplaceSwitch',
            bindToController: true
        };
    }]);


    ng.module('inplace')
    .directive('inplaceProgress', function () {
        return {
            restrict: 'A',
            scope: {},
            controller: 'InplaceProgressCtrl',
            controllerAs: 'inplaceProgress',
            bindToController: true,
            replace: true,
            template: '<div class="inplace-progress icon-spinner-before icon-animate-spin-before" data-ng-if="inplaceProgress.state.show === true"></div>'
        };
    });

})(window.angular);