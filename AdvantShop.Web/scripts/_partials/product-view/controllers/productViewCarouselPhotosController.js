; (function (ng) {
    'use strict';

    var ProductViewCarouselPhotosCtrl = function () {
        var ctrl = this;

        ctrl.carouselInit = function (carousel) {
            ctrl.carousel = carousel;
        };
    };

    ng.module('productView')
      .controller('ProductViewCarouselPhotosCtrl', ProductViewCarouselPhotosCtrl);

    ProductViewCarouselPhotosCtrl.$inject = [];
    
})(window.angular);