; (function (ng) {
    'use strict';

    var productService = function ($http) {
        var service = this;

        service.getColors = function () {
            return $http.get('product/getcolors').then(function (response) {
                return response.data;
            });
        };

        service.getSizes = function () {
            return $http.get('product/getsizes').then(function (response) {
                return response.data;
            });
        };

        service.getOffer = function (offerId) {
            return $http.get('product/getoffer', { params: { offerId: offerId } }).then(function (response) {
                return response.data;
            });
        };

        service.getAvailableArtNo = function (productId) {
            return $http.get('product/getavailableartno', { params: { productId: productId } }).then(function (response) {
                return response.data;
            });
        };

        service.addOffer = function (params) {
            return $http.post('product/addoffer', params).then(function (response) {
                return response.data;
            });
        };

        service.updateOffer = function (params) {
            return $http.post('product/updateoffer', params).then(function (response) {
                return response.data;
            });
        };
    };

    productService.$inject = ['$http'];

    ng.module('product')
      .service('productService', productService);

})(window.angular);