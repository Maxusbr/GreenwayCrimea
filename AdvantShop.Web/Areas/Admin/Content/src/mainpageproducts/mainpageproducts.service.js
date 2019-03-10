; (function (ng) {
    'use strict';

    var mainpageproductsService = function ($http) {
        var service = this;

        service.addProducts = function (params) {
            return $http.post('mainpageproducts/addproducts', params).then(function (response) {
                return response.data;
            });
        };
    };

    mainpageproductsService.$inject = ['$http'];

    ng.module('mainpageproducts')
        .service('mainpageproductsService', mainpageproductsService);

})(window.angular);