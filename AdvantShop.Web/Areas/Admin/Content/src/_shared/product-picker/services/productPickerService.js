; (function (ng) {
    'use strict';

    var productPickerService = function ($http) {
        var service = this;

        service.geProductsByCategory = function (url, categoryId) {
            return $http.get(url, { params: { categoryId: categoryId } }).then(function (response) {
                return response.data;
            });
        };
    };

    ng.module('productPicker')
        .service('productPickerService', productPickerService);

    productPickerService.$inject = ['$http'];

})(window.angular);