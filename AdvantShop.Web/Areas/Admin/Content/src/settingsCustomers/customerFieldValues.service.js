; (function (ng) {
    'use strict';

    var customerFieldValuesService = function ($http) {
        var service = this;

        service.getCustomerFieldValue = function (id) {
            return $http.post('customerFieldValues/get', { id: id, rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        }

        service.deleteCustomerFieldValue = function (id) {
            return $http.post('customerFieldValues/delete', { id: id }).then(function (response) {
                return response.data;
            });
        }

        service.addOrUpdateCustomerFieldValue = function (add, params) {
            var url = add === true ? 'customerFieldValues/add' : 'customerFieldValues/update';
            return $http.post(url, params).then(function (response) {
                return response.data;
            });
        }
    };

    customerFieldValuesService.$inject = ['$http'];

    ng.module('settingsCustomers')
        .service('customerFieldValuesService', customerFieldValuesService);

})(window.angular);