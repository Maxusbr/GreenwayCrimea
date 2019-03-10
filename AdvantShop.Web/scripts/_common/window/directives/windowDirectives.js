; (function (ng) {
    'use strict';

    ng.module('windowExt')
      .directive('windowResize', ['windowService', function (windowService) {
          return {
              restrict: 'A',
              scope: {
                  windowResize: '&'
              },
              controller: 'WindowCtrl',
              controllerAs: 'windowCtrl',
              bindToController: true,
              link: function (scope, element, attrs, ctrl) {
                  windowService.addCallback("resize", ctrl.windowResize);
              }
          }
      }]);

    ng.module('windowExt')
  .directive('windowScroll', ['windowService', function (windowService) {
      return {
          restrict: 'A',
          scope: {
              windowScroll: '&'
          },
          controller: 'WindowCtrl',
          controllerAs: 'windowCtrl',
          bindToController: true,
          link: function (scope, element, attrs, ctrl) {
              windowService.addCallback("scroll", ctrl.windowScroll);
          }
      }
  }]);

    ng.module('windowExt')
  .directive('windowClick', ['windowService', function (windowService) {
      return {
          restrict: 'A',
          scope: {
              windowClick: '&'
          },
          controller: 'WindowCtrl',
          controllerAs: 'windowCtrl',
          bindToController: true,
          link: function (scope, element, attrs, ctrl) {
              windowService.addCallback("click", ctrl.windowClick);
              windowService.addCallback("touchstart", ctrl.windowClick);
          }
      }
  }]);

    ng.module('windowExt')
    .directive('windowPrint', ['windowService', function (windowService) {
        return {
            restrict: 'A',
            controller: 'WindowCtrl',
            controllerAs: 'windowCtrl',
            bindToController: true,
            link: function (scope, element, attrs, ctrl) {

                var func = function () {
                    windowService.open(attrs.windowPrint, attrs.windowPrintName, attrs.windowPrintParams);
                }

                windowService.addCallBack("click", func);
                windowService.addCallBack("touchend", func);
            }
        }
    }]);

})(angular);