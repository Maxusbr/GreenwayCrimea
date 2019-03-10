; (function (ng) {
    'use strict';

    var ImportCtrl = function ($q, $location, $window, $interval, urlHelper, importService, SweetAlert, toaster, Upload) {

        var ctrl = this;

        ctrl.init = function () {

        };

        ctrl.uploadFiles = function () {
            importService.UploadProductFiles().then(function (response) {

            });
        };

        //ctrl.isStartExport = false;

        ctrl.firstProductFields = [];
        ctrl.firstObjectFields = [];
        ctrl.headers = [];
        ctrl.allProductFields = [];

        ctrl.allFields = [];

        ctrl.selectedProductFields = [];
        ctrl.selectedCategoryFields = [];
        ctrl.selectedCustomerFields = [];


        ctrl.showFields = false;

        ctrl.getFiledsFromCsvFile = function () {
            importService.getFiledsFromCsvFile(ctrl.ColumnSeparator, ctrl.PropertySeparator, ctrl.PropertyValueSeparator, ctrl.Encoding, ctrl.HaveHeader).then(function (response) {
                if (response.result) {
                    ctrl.firstProductFields = response.obj.firstProduct;
                    ctrl.headers = response.obj.headers;
                    ctrl.allProductFields = response.obj.allFields;
                    ctrl.showFields = true;
                }
                else {
                    ctrl.firstProductFields = [];
                    ctrl.firstObjectFields = [];
                    ctrl.headers = [];
                    ctrl.allProductFields = [];

                    toaster.pop('error', response.errors[0]);
                }
            });
        };

        ctrl.getFiledsFromCategoriesCsvFile = function () {
            importService.getFiledsFromCategoriesCsvFile(ctrl.ColumnSeparator, ctrl.Encoding, ctrl.HaveHeader).then(function (response) {
                if (response.result) {
                    ctrl.firstObjectFields = response.obj.firstCategory;
                    ctrl.headers = response.obj.headers;
                    ctrl.allProductFields = response.obj.allFields;
                    ctrl.showFields = true;                   
                }
                else {
                    ctrl.firstProductFields = [];
                    ctrl.firstObjectFields = [];
                    ctrl.headers = [];
                    ctrl.allProductFields = [];

                    toaster.pop('error', response.errors[0]);
                }
            });
        };

        ctrl.getFiledsFromCustomersCsvFile = function () {
            importService.getFiledsFromCustomersCsvFile(ctrl.ColumnSeparator, ctrl.Encoding, ctrl.HaveHeader).then(function (response) {
                ctrl.firstObjectFields = response.firstCustomer;
                ctrl.headers = response.headers;
                ctrl.allFields = response.allFields;
                ctrl.showFields = true;
            });
        };


        ctrl.startProductsImport = function () {
            importService.startProductsImport(ctrl.selectedProductFields, ctrl.ColumnSeparator, ctrl.PropertySeparator, ctrl.PropertyValueSeparator, ctrl.Encoding, ctrl.HaveHeader, ctrl.DisableProducts).then(function (response) {
                if (response) {
                    ctrl.isStartExport = true;
                }
            });
        };

        ctrl.startCategoriesImport = function () {
            importService.startCategoriesImport(ctrl.selectedCategoryFields, ctrl.ColumnSeparator, ctrl.Encoding, ctrl.HaveHeader).then(function (response) {
                if (response) {
                    ctrl.isStartExport = true;
                }
            });
        };

        ctrl.startCustomersImport = function () {
            importService.startCustomersImport(ctrl.selectedCustomerFields, ctrl.ColumnSeparator, ctrl.Encoding, ctrl.HaveHeader).then(function (response) {
                if (response) {
                    ctrl.isStartExport = true;
                }
            });
        };

        ctrl.isSaas = false;
        ctrl.productsCount = 0;
        ctrl.tariffProductsCount = 0;

        ctrl.initProgress = function () {
            ctrl.getSaasDataInformation();

            if (isSaas) {
                ctrl.stop = $interval(function () {
                    ctrl.getSaasDataInformation();
                }, 500);
            }
        };

        ctrl.abortImport = function () {
            importService.abortImport().then(function (response) {
                ctrl.IsRun = false;
            });
        };

        ctrl.getSaasDataInformation = function () {
            importService.getSaasBlockInformation().then(function (response) {
                ctrl.productsCount = response.productsCount;
                ctrl.tariffProductsCount = response.productsInTariff;
                ctrl.isSaas = response.isSaas;
            });
        };

        ctrl.getLogFile = function () {
            importService.getLogFile().then(function (response) {
                return response;
            });
        };


        ctrl.getExampleCustomersFile = function () {
            importService.getExampleCustomersFile(ctrl.ColumnSeparator, ctrl.Encoding).then(function (response) {
                return response;
            });
        };

        ctrl.changeNewFile = function() {
            ctrl.isStartExport = false;
            ctrl.showFields = false;
        };

    };

    ImportCtrl.$inject = ['$q', '$location', '$window', '$interval', 'urlHelper', 'importService', 'SweetAlert', 'toaster', 'Upload', ];

    ng.module('import', ['urlHelper'])
      .controller('ImportCtrl', ImportCtrl);

})(window.angular);