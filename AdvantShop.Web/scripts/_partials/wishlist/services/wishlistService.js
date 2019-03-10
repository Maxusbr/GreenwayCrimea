; (function (ng) {

    'use strict';

    var wishlistService = function ($http) {
        var service = this,
            countObj = {};

        service.add = function (offerId) {
            return $http.post('wishlist/wishlistadd', { offerId: offerId, rnd: Math.random() }).then(function (response) {

                countObj.count = response.data.Count;
                $(document).trigger("add_to_wishlist");

                return response.data;
            });
        };

        service.remove = function (offerId) {
            return $http.post('wishlist/wishlistremove', { offerId: offerId, rnd: Math.random() }).then(function (response) {

                countObj.count = response.data.Count;

                return response.data;
            });
        };

        service.getCountObj = function () {
            return countObj;
        };

        service.getStatus = function (offerId) {
            return $http.get('/wishlist/getstatus', { params: { offerId: offerId, rnd: Math.random() } }).then(function (response) {
                return response.data;
            });
        };
    };

    ng.module('wishlist')
      .service('wishlistService', wishlistService);

    wishlistService.$inject = ['$http'];

})(window.angular);