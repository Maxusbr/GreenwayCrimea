; (function (ng) {
    'use strict';

    var ModalExportOptionsCtrl = function ($uibModalInstance, $http, $window, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve;
            ctrl.productId = params.productId != null ? params.productId : 0;

            ctrl.getExportOptions();
        };
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.getExportOptions = function () {
            $http.get('product/getExportOptions', { params: { productId: ctrl.productId } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.SalesNote = data.SalesNote;
                    ctrl.Gtin = data.Gtin;
                    ctrl.GoogleProductCategory = data.GoogleProductCategory;
                    ctrl.YandexMarketCategory = data.YandexMarketCategory;
                    ctrl.YandexTypePrefix = data.YandexTypePrefix;
                    ctrl.YandexModel = data.YandexModel;
                    ctrl.Adult = data.Adult;
                    ctrl.ManufacturerWarranty = data.ManufacturerWarranty;
                    ctrl.YandexSizeUnit = data.YandexSizeUnit;
                    ctrl.Fee = data.Fee;
                    ctrl.Cbid = data.Cbid;
                    ctrl.YandexName = data.YandexName;
                }
            });
        }

        ctrl.save = function () {

            var params = {
                productId: ctrl.productId,
                SalesNote: ctrl.SalesNote,
                Gtin: ctrl.Gtin,
                GoogleProductCategory: ctrl.GoogleProductCategory,
                YandexMarketCategory: ctrl.YandexMarketCategory,
                YandexTypePrefix: ctrl.YandexTypePrefix,
                YandexModel: ctrl.YandexModel,
                Adult: ctrl.Adult,
                ManufacturerWarranty: ctrl.ManufacturerWarranty,
                YandexSizeUnit: ctrl.YandexSizeUnit,
                Fee: ctrl.Fee,
                Cbid: ctrl.Cbid,
                YandexName: ctrl.YandexName
            };
            
            $http.post('product/saveExportOptions', params).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop("success", "Изменения сохранены");
                    $uibModalInstance.close('saveExportOptions');
                } else {
                    toaster.pop("error", "Ошибка", "Ошибка при сохранении");
                }
            });
        }
    };

    ModalExportOptionsCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'toaster'];

    ng.module('uiModal')
        .controller('ModalExportOptionsCtrl', ModalExportOptionsCtrl);

})(window.angular);