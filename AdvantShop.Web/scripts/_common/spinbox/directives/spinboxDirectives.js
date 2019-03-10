; (function (ng) {
    'use strict';

    ng.module('spinbox')
      .directive('spinbox', ['$filter', function ($filter) {
          return {
              restrict: 'A',
              scope: {
                  value: '=',
                  proxy: '=?',
                  min: '=?',
                  max: '=?',
                  step: '=?',
                  updateFn: '&'
              },
              replace: true,
              bindToController: true,
              templateUrl: (window.baseUrl != null ? window.baseUrl : '') + 'scripts/_common/spinbox/templates/spinbox.html',
              controller: 'SpinboxCtrl',
              controllerAs: 'spinbox'
          }
      }]);
})(window.angular);