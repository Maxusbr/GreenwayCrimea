; (function (ng) {
    'use strict';

    var analyticsService = function ($http) {
        var service = this;

        service.exportOrders = function (settings) {
            return $http.post('analytics/exportorders', { settings: settings }).then(function (response) {
                return response.data;
            });
        };

        service.exportCustomers = function (settings) {
            return $http.post('analytics/exportcustomers', { settings: settings }).then(function (response) {
                return response.data;
            });
        };

        service.exportProducts = function (settings) {
            return $http.post('analytics/exportproducts', { settings: settings }).then(function (response) {
                return response.data;
            });
        };        

        service.getCommonStatistic = function () {
            return $http.post('ExportImportCommon/GetCommonStatistic').then(function (response) {
                return response.data;
            });
        };

    };

    analyticsService.$inject = ['$http'];

    ng.module('analytics')
        .service('analyticsService', analyticsService);

})(window.angular);