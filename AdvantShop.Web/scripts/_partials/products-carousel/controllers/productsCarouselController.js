; (function (ng) {

    'use strict';

    var ProductsCarouselCtrl = function ($scope, $compile, $element, productsCarouselService) {
        var ctrl = this;

        ctrl.generate = function (ids, title, type, visibleItems) {
            productsCarouselService.getData(ids, title, type, visibleItems)
                 .then(function (result) {
                     $element.empty();
                     $element.append(result);
                     $compile($element.contents())($scope);
                 });
        };

        ctrl.update = function () {
            ctrl.generate(ctrl.ids, ctrl.title, ctrl.type, ctrl.visibleItems);
        };

        ctrl.update();
    };

    ng.module('productsCarousel')
      .controller('ProductsCarouselCtrl', ProductsCarouselCtrl);

    ProductsCarouselCtrl.$inject = ['$scope', '$compile', '$element', 'productsCarouselService']

})(window.angular);