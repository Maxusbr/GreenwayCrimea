; (function (ng) {
    'use strict';

    ng.module('sizesViewer')
      .directive('sizesViewer', function () {
          return {
              restrict: 'A',
              replace: true,
              templateUrl: '/scripts/_partials/sizes-viewer/templates/sizes.html',
              controller: 'SizesViewerCtrl',
              controllerAs: 'sizesViewer',
              bindToController: true,
              scope: {
                  sizes: '&',
                  sizeSelected: '=?',
                  initSizes: '&',
                  changeSize: '&',
              }
          }
      });

})(window.angular);