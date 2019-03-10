; (function (ng) {
    'use strict';

    ng.module('demo')
      .directive('demoModal', function () {
          return {
              restrict: 'A',
              scope: {
                  demoModalUrl: '@',
                  demoModalId: '@'
              },
              controller: 'DemoCtrl',
              controllerAs: 'demoModal',
              bindToController: true,
              templateUrl: '/scripts/_partials/demo/templates/demoModal.html'
          };
      });

})(window.angular);