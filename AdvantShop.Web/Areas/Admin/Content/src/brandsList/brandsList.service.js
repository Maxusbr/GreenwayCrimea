; (function (ng) {
    'use strict';

    var brandsListService = function ($http) {
        var service = this;

        service.getBrands = function (params) {
            return $http.get('brands/getbrands', { params: params }).then(function (response) {
                return response.data;
            });
        };
        
        service.deleteBrands = function (brandIds) {
            return $http.post('brands/deletebrands', { brandIds: brandIds }).then(function (response) {
                return response.data;
            });
        };     
    };

    brandsListService.$inject = ['$http'];

    ng.module('brandsList')
        .service('brandsListService', brandsListService);

})(window.angular);