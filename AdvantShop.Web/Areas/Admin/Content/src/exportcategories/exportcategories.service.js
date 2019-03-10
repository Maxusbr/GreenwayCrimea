; (function (ng) {
    'use strict';

    var exportCategoriesService = function ($http) {
        var service = this;

        service.getCommonStatistic = function () {
            return $http.post('ExportImportCommon/GetCommonStatistic').then(function (response) {
                return response.data;
            });
        };

        service.interruptProcess = function () {
            return $http.post('ExportImportCommon/InterruptProcess').then(function (response) {
                return response.data;
            });
        };
    };

    exportCategoriesService.$inject = ['$http'];

    ng.module('exportCategories')
        .service('exportCategoriesService', exportCategoriesService);

})(window.angular);