; (function (ng) {
    'use strict';

    ng.module('builder')
      .directive('builderTrigger', ['$compile', function ($compile) {
          return {
              restrict: 'EA',
              scope: {},
              controller: 'BuilderCtrl',
              controllerAs: 'builder',
              bindToController: true,
              link: function (scope, element, attrs, ctrl) {
                  element.on('click', function (event) {
                      event.preventDefault();
                      ctrl.showDialog(ctrl);
                      scope.$apply();
                  });
              }
          };
      }]);

    ng.module('builder')
    .directive('builderStylesheet', ['builderService', 'builderTypes', function (builderService) {
        return {
            restrict: 'A',
            scope: {},
            link: function (scope, element, attrs) {
                builderService.memoryStylesheet(attrs.builderType, element);
            }
        };
    }]);

})(window.angular);