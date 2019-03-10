; (function (ng) {
    'use strict';

    var exportfeedsService = function ($http) {
        var service = this;

        service.addCategoriesToExport = function (exportFeedId, categories) {
            return $http.post('exportfeeds/AddCategoriesToExport', {
                'exportFeedId': exportFeedId,
                'categories': categories
            }).then(function (response) {
                return response.data;
            });
        };

        service.saveExportFeedFields = function (exportFeedId, exportFeedFields) {
            return $http.post('exportfeeds/SaveExportFeedFields', {
                'exportFeedId': exportFeedId,
                'exportFeedFields': exportFeedFields
            }).then(function (response) {
                return response.data.result;
            });
        };

        service.getCommonStatistic = function () {
            return $http.post('ExportImportCommon/GetCommonStatistic').then(function (response) {
                return response.data;
            });
        };

        service.deleteExport = function (exportFeedId) {
            return $http.post('exportfeeds/DeleteExport', { 'exportFeedId': exportFeedId }).then(function (response) {
                return response.data;
            });
        };

        service.saveExportFeedSettings = function (exportFeedId, exportFeedName, exportFeedDescription, commonSettings, advancedSettings) {
            return $http.post('exportfeeds/SaveExportFeedSettings', {
                'exportFeedId': exportFeedId,
                'exportFeedName': exportFeedName,
                'exportFeedDescription': exportFeedDescription,
                'commonSettings': commonSettings,
                'advancedSettings': advancedSettings
            }).then(function (response) {
                return response.data;
            });
        };

    };

    exportfeedsService.$inject = ['$http'];

    ng.module('exportfeeds')
        .service('exportfeedsService', exportfeedsService);

})(window.angular);