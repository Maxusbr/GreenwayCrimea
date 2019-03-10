; (function (ng) {
    'use strict';

    var vkAuthService = function ($http) {
        var service = this;
        
        service.getVkSettings = function () {
            return $http.get('vk/getVkSettings').then(function (response) {
                return response.data;
            });
        }

        service.saveAuthVkUser = function (params) {
            return $http.post('vk/saveAuthVkUser', params).then(function (response) {
                return params;
            });
        }

        service.getGroups = function () {
            return $http.post('vk/getVkGroups').then(function (response) {
                return response.data;
            });
        }

        service.saveAuthVkGroup = function (params) {
            return $http.post('vk/saveAuthVkGroup', params).then(function (response) {
                return response.data;
            });
        }

        service.deleteGroup = function (params) {
            return $http.post('vk/deleteGroup', params).then(function (response) {
                return response.data;
            });
        }

    };

    vkAuthService.$inject = ['$http'];

    ng.module('vkAuth')
        .service('vkAuthService', vkAuthService);

})(window.angular);