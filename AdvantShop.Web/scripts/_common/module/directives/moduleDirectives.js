; (function (ng) {
    'use strict';

    ng.module('module')
      .directive('module', ['$sce', 'moduleService', function ($sce, moduleService) {
          return {
              restrict: 'A',
              scope: {
                  key: '@'
              },
              controller: 'ModuleCtrl',
              controllerAs: 'module',
              bindToController: true,
              replace: true,
              transclude: true,
              template: '<div data-ng-bind-html="module.content"></div>',
              link: function (scope, element, attrs, ctrl, transclude) {
                  ctrl.content = $sce.trustAsHtml(ng.element('<div />').html(transclude()).html());
                  moduleService.add(ctrl.key, ctrl);
              }
          }
      }]);
})(window.angular);