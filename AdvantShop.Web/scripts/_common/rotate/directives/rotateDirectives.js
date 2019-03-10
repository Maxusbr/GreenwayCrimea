; (function (ng) {
    'use strict';

    ng.module('rotate')
      .directive('rotate', function () {
          return {
              restrict: 'A',
              scope: {
                  imagePath: '@',
                  totalFrames: '@',
                  endFrame: '@',
                  height: '@',
                  width: '@',
                  imgList: '@',
                  progress: '@',
                  navigation: '&',
                  responsive: '&',
                  autoplayDirection: '@', // -1 or 1
                  autoplay: '&',
                  ext: '@',
                  framerate: '@'
              },
              controller: 'RotateCtrl',
              controllerAs: 'rotate',
              bindToController: true,
              replace: true,
              template: '<div class="threesixty"><div class="spinner"><span>0%</span></div><ul class="threesixty_images"></ul></div>',
              link: function(scope, element, attrs, ctrl){
                  element.ThreeSixty(ctrl.rotateOptions);
              }
          };
      });


})(window.angular);