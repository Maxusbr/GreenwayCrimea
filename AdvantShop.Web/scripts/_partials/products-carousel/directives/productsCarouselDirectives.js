; (function (ng) {
    'use strict';

    ng.module('productsCarousel')
      .directive('productsCarousel', function () {
          return {
              restrict: 'A',
              scope: {
                  ids: '@',
                  title: '@',
                  type: '@',
                  visibleItems: '@'
              },
              controller: 'ProductsCarouselCtrl',
              controllerAs: 'productsCarousel',
              bindToController: true
          };
      });

})(window.angular);