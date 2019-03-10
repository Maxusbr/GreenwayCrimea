; (function (ng) {
    'use strict';

    ng.module('countdown')
      .directive('countdown', function () {
          return {
              restrict: 'A',
              scope: {
                  endTime: '='
              },
              replace: true,
              controller: 'CountdownCtrl',
              controllerAs: 'countdown',
              bindToController: true,
              templateUrl: function (element, attrs) {
                  return attrs.templateUrl || 'frontend/blocks/countdown/templates/countdown.html'
              },
              link: function (scope, element, attrs, ctrl) {
                  ctrl.init(ctrl.endTime);
              }
          }
      });
})(window.angular);