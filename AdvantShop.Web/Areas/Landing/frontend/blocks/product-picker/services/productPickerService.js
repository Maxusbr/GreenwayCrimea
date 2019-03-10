; (function (ng) {
    'use strict';

    var productPickerService = function ($http) {
        var service = this;

        service.geProductsByCategory = function (categoryId) {
            return $http.get('landinginplace/ProductsForGrid', { params: { categoryId: categoryId } }).then(function (response) {
                return response.data;
            });
        };
    };

    ng.module('productPicker')
        .service('productPickerService', productPickerService);

    productPickerService.$inject = ['$http'];

})(window.angular);