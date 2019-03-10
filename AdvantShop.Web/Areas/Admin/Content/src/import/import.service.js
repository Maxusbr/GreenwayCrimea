; (function (ng) {
    'use strict';

    var importService = function ($http) {
        var service = this;

        service.UploadProductFiles = function (exportFeedId, categories) {
            return $http.post('import/uploadproductfiles').then(function (response) {
                return response.data;
            });
        };

        service.getFiledsFromCsvFile = function (columnSeparator, propertySeparator, propertyValueSeparator, encoding, haveHeader) {
            return $http.post('import/GetFiledsFromCsvFile', {
                'columnSeparator': columnSeparator,
                'propertySeparator': propertySeparator,
                'propertyValueSeparator': propertyValueSeparator,
                'encoding': encoding,
                'haveHeader': haveHeader
            }).then(function (response) {
                return response.data;
            });
        };

        service.getFiledsFromCategoriesCsvFile = function (columnSeparator, encoding, haveHeader) {
            return $http.post('import/GetFiledsFromCategoriesCsvFile', {
                'columnSeparator': columnSeparator,
                'encoding': encoding,
                'haveHeader': haveHeader
            }).then(function (response) {
                return response.data;
            });
        };

        service.getFiledsFromCustomersCsvFile = function (columnSeparator, encoding, haveHeader) {
            return $http.post('import/GetFiledsFromCustomersCsvFile', {
                'columnSeparator': columnSeparator,
                'encoding': encoding,
                'haveHeader': haveHeader
            }).then(function (response) {
                return response.data;
            });
        };

        service.startProductsImport = function (selectedProductFields, columnSeparator, propertySeparator, propertyValueSeparator, encoding, haveHeader, disableProducts) {
            return $http.post('import/StartProductsImport', {
                'selectedProductFields': selectedProductFields,
                'columnSeparator': columnSeparator,
                'propertySeparator': propertySeparator,
                'propertyValueSeparator': propertyValueSeparator,
                'encoding': encoding,
                'haveHeader': haveHeader,
                'disableProducts': disableProducts
            }).then(function (response) {
                return response.data;
            });
        };

        service.startCategoriesImport = function (selectedCategoryFields, columnSeparator, encoding, haveHeader) {
            return $http.post('import/StartCategoriesImport', {
                'selectedCategoryFields': selectedCategoryFields,
                'columnSeparator': columnSeparator,
                'encoding': encoding,
                'haveHeader': haveHeader
            }).then(function (response) {
                return response.data;
            });
        };

        service.startCustomersImport = function (selectedCustomerFields, columnSeparator, encoding, haveHeader) {
            return $http.post('import/StartCustomersImport', {
                'selectedCustomerFields': selectedCustomerFields,
                'columnSeparator': columnSeparator,
                'encoding': encoding,
                'haveHeader': haveHeader
            }).then(function (response) {
                return response.data;
            });
        };

        service.getSaasBlockInformation = function () {
            return $http.post('import/GetSaasBlockInformation').then(function (response) {
                return response.data;
            });
        };

        service.abortImport = function () {
            return $http.post('ExportImportCommon/InterruptProcess').then(function (response) {
                return response.data;
            });
        };

        service.getCommonStatistic = function () {
            return $http.post('ExportImportCommon/GetCommonStatistic').then(function (response) {
                return response.data;
            });
        };

        service.getLogFile = function () {
            $http({
                url: 'ExportImportCommon/GetLogFile',
                method: 'POST',
                params: {},
                headers: {
                    'Content-type': 'application/txt',
                },
                responseType: 'arraybuffer'
            }).success(function (data, status, headers, config) {
                var file = new Blob([data], {
                    type: 'application/txt'
                });
                //trick to download store a file having its URL
                var fileURL = URL.createObjectURL(file);
                var a = document.createElement('a');
                a.href = fileURL;
                a.target = '_blank';
                a.download = 'errlog.txt';
                document.body.appendChild(a);
                a.click();
            }).error(function (data, status, headers, config) {

            });
        };

        service.getExampleCustomersFile = function (columnSeparator, encoding) {
            $http({
                url: 'import/getExampleCustomersFile',
                method: 'POST',
                params: {
                    columnSeparator: columnSeparator,
                    encoding: encoding
                },
                headers: {
                    'Content-type': 'application/txt',
                },
                responseType: 'arraybuffer'
            }).success(function (data, status, headers, config) {
                var file = new Blob([data], {
                    type: 'application/txt'
                });
                //trick to download store a file having its URL
                var fileURL = URL.createObjectURL(file);
                var a = document.createElement('a');
                a.href = fileURL;
                a.target = '_blank';
                a.download = 'exampleCustomersFile.csv';
                document.body.appendChild(a);
                a.click();
            }).error(function (data, status, headers, config) {

            });
        };

    };

    importService.$inject = ['$http'];

    ng.module('import')
        .service('importService', importService);

})(window.angular);