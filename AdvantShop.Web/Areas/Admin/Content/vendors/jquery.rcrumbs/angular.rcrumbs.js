; (function (ng) {
    'use strict';

    var windowLoaded = false;

    window.addEventListener('load', function load() {
        windowLoaded = true;
    });

    ng.module('rcrumbs', [])
      .directive('rcrumbs', function () {
          return {
              restrict: 'A',
              scope: {
                  ellipsis: '<?',
                  windowResize: '<?',
                  nbUncollapsableCrumbs: '<?',
                  nbFixedCrumbs: '<?',
                  animation: '<?',
                  //preCrumbsListDisplay: '&',
                  //preCrumbDisplay: '&',
                  //postCrumbsListDisplay: '&',
                  //postCrumbDisplay: '&'
              },
              link: function (scope, element, attrs, ctrl) {

                  if (windowLoaded === false) {
                      window.addEventListener('load', function load() {

                          window.removeEventListener('load', load);

                          element.rcrumbs({
                              ellipsis: scope.ellipsis,
                              windowResize: scope.windowResize,
                              nbUncollapsableCrumbs: scope.nbUncollapsableCrumbs,
                              nbFixedCrumbs: scope.nbFixedCrumbs,
                              animation: scope.animation
                          });
                      });
                  } else {
                      element.rcrumbs({
                          ellipsis: scope.ellipsis,
                          windowResize: scope.windowResize,
                          nbUncollapsableCrumbs: scope.nbUncollapsableCrumbs,
                          nbFixedCrumbs: scope.nbFixedCrumbs,
                          animation: scope.animation
                      });
                  }

              }
          }
      });

})(window.angular);