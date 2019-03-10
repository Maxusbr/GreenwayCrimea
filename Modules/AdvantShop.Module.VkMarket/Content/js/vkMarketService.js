; (function (ng) {
    'use strict';

    var vkMarketService = function ($http) {
        var service = this;
        
        service.getAuthSettings = function () {
            return $http.get('../vkMarketSettings/getAuthSettings').then(function (response) {
                return response.data;
            });
        }

        service.saveAuth = function (params) {
            return $http.post('../vkMarketSettings/saveAuth', params).then(function (response) {
                return params;
            });
        }

        service.getGroups = function () {
            return $http.get('../vkMarketSettings/getGroups').then(function (response) {
                return response.data;
            });
        }

        service.saveGroup = function (params) {
            return $http.post('../vkMarketSettings/saveGroup', params).then(function (response) {
                return response.data != null ? response.data.obj : null;
            });
        }
        
        service.deleteGroup = function () {
            return $http.post('../vkMarketSettings/deleteGroup').then(function (response) {
                return response.data;
            });
        }


        service.getExportSettings = function () {
            return $http.get('../vkMarketSettings/getExportSettings').then(function (response) {
                return response.data;
            });
        }

        service.saveExportSettings = function (params) {
            return $http.post('../vkMarketSettings/saveExportSettings', params).then(function (response) {
                return response.data;
            });
        }

        service.export = function () {
            return $http.post('../vkMarketSettings/export').then(function (response) {
                return response.data;
            });
        }

        service.getExportProgress = function () {
            return $http.get('../vkMarketSettings/getExportProgress').then(function (response) {
                return response.data;
            });
        }

        service.import = function () {
            return $http.post('../vkMarketSettings/import').then(function (response) {
                return response.data;
            });
        }
        
        service.getImportProgress = function () {
            return $http.get('../vkMarketSettings/getImportProgress').then(function (response) {
                return response.data;
            });
        }

        service.getReports = function () {
            return $http.get('../vkMarketSettings/getReports').then(function (response) {
                return response.data;
            });
        }

        service.deleteAllProducts = function () {
            return $http.post('../vkMarketSettings/deleteAllProducts').then(function (response) {
                return response.data;
            });
        }
    };

    vkMarketService.$inject = ['$http'];

    ng.module('module')
        .service('vkMarketService', vkMarketService);

})(window.angular);