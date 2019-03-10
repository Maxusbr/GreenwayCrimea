; (function (ng) {
    'use strict';

    ng.module('colorsViewer')
      .directive('colorsViewer', function () {
          return {
              require: ['colorsViewer', '?^carousel'],
              restrict: 'A',
              replace: true,
              templateUrl: '/scripts/_partials/colors-viewer/templates/colors.html',
              controller: 'ColorsViewerCtrl',
              controllerAs: 'colorsViewer',
              bindToController: true,
              scope: {
                  colors: '=',
                  colorSelected: '=?', 
                  startSelectedColors: '<?',
                  colorWidth: '=?',
                  colorHeight: '=?',
                  initColors: '&',
                  changeColor: '&',
                  multiselect: '<?',
                  imageType: '@'
              },
              link: function (scope, element, attrs, ctrls) {
                  var colorsViewer = ctrls[0],
                      carousel = ctrls[1];

                  if (carousel != null) {
                      carousel.load = true;
                  }
              }
          }
      });

})(window.angular);