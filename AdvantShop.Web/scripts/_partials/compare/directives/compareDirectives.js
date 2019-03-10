; (function (ng) {
    'use strict';

    ng.module('compare')
      .directive('compareControl', function () {
          return {
              restrict: 'A',
              scope: true,
              controller: 'CompareCtrl',
              controllerAs: 'compare',
              bindToController: true,
              link: function (scope, element, attrs, ctrl) {
                  ctrl.dirty = true;
              }
          };
      });


    ng.module('compare')
      .directive('compareCount', function () {
          return {
              restrict: 'A',
              scope: true,
              controller: 'CompareCountCtrl',
              controllerAs: 'compareCount',
              bindToController: true,
              link: function (scope, element, attrs, ctrl) {
                  ctrl.countObj.count = parseInt(attrs.startCount, 10);
              }
          };
      });

    ng.module('compare')
  .directive('compareRemove', ['compareService', function (compareService) {
      return {
          restrict: 'A',
          scope: true,
          link: function (scope, element, attrs) {
              element.on('click', function (event) {
                  event.preventDefault();
                  compareService.remove(attrs.compareRemove);
              });
          }
      };
  }]);

})(window.angular);