; (function (ng) {
    'use strict';

    var productPickerService = function ($http) {
        var service = this;

        service.getCategories = function (categoryId) {
            return $http.get('landingpageadmin/categoriestree', { categoryId: categoryId }).then(function (response) {
                return response.data;
            });
        };
    };

    ng.module('productPicker')
        .service('productPickerService', productPickerService);

    productPickerService.$inject = ['$http'];

})(window.angular);