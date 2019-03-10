; (function (ng) {
    'use strict';


    var productsCarouselService = function ($http) {
        var service = this;

        service.getData = function (ids, title, type, visibleItems) {
            return $http.get('catalog/productsbyIds', { params: { ids: ids, title: title, type: type, visibleItems: visibleItems, enabledCarousel: true } })
                .then(function (response) {
                    return response.data;
                });
        };
    };

    ng.module('productsCarousel')
      .service('productsCarouselService', productsCarouselService);

    productsCarouselService.$inject = ['$http'];

})(window.angular);