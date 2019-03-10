; (function (ng) {
    'use strict';

    var ModalAddEditOfferCtrl = function ($uibModalInstance, $window, toaster, productService, $q) {
        var ctrl = this;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve;
            ctrl.OfferId = params.OfferId != null ? params.OfferId : 0;
            ctrl.ProductId = params.ProductId != null ? params.ProductId : 0;
            ctrl.mode = ctrl.OfferId != 0 ? "edit" : "add";

            ctrl.getColors()
                .then(ctrl.getSizes)
                .then(function () {

                    if (ctrl.mode == 'add') {

                        ctrl.getAvailableArtNo().then(function (data) {
                            ctrl.ArtNo = data;
                        });

                        ctrl.BasePrice = 0;
                        ctrl.SupplyPrice = 0;
                        ctrl.Amount = 1;
                        ctrl.ColorId = ctrl.Colors[0];
                        ctrl.SizeId = ctrl.Sizes[0];

                    } else {
                        ctrl.getOffer();
                    }
                });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.getOffer = function () {
            return productService.getOffer(ctrl.OfferId).then(function (data) {
                if (data != null) {
                    ctrl.OfferId = data.OfferId;
                    ctrl.ProductId = data.ProductId;
                    ctrl.ArtNo = data.ArtNo;
                    ctrl.BasePrice = data.BasePrice;
                    ctrl.SupplyPrice = data.SupplyPrice;
                    ctrl.Amount = data.Amount;
                    ctrl.ColorId = ctrl.Colors.filter(function (x) { return x.value == data.ColorId; })[0];
                    ctrl.SizeId = ctrl.Sizes.filter(function (x) { return x.value == data.SizeId; })[0];
                    ctrl.Main = data.Main;
                }

                return data;
            });
        }

        ctrl.getColors = function () {
            return productService.getColors().then(function (result) {
                ctrl.Colors = result;
                ctrl.Colors.unshift({ value: null, label: '––––' });

                return result;
            });
        };

        ctrl.getSizes = function () {
            return productService.getSizes().then(function (result) {
                ctrl.Sizes = result;
                ctrl.Sizes.unshift({ value: null, label: '––––' });

                return result;
            });
        };

        ctrl.save = function () {

            var params = {
                OfferId: ctrl.OfferId,
                ProductId: ctrl.ProductId,
                ArtNo: ctrl.ArtNo,
                BasePrice: ctrl.BasePrice,
                SupplyPrice: ctrl.SupplyPrice,
                Amount: ctrl.Amount,
                ColorId: ctrl.ColorId.value,
                SizeId: ctrl.SizeId.value,
                Main: ctrl.Main,
            };

            productService[ctrl.mode == 'add' ? 'addOffer' : 'updateOffer'](params).then(function (data) {
                if (data.result == true) {
                    toaster.pop('success', '', 'Изменения сохранены');
                    $uibModalInstance.close('saveOffer');
                } else {
                    data.errors.forEach(function(error) {
                        toaster.pop('error', 'Ошибка', error);
                    });
                }
            });
        };

        ctrl.getAvailableArtNo = function () {
            return productService.getAvailableArtNo(ctrl.ProductId);
        }
    };

    ModalAddEditOfferCtrl.$inject = ['$uibModalInstance', '$window', 'toaster', 'productService', '$q'];

    ng.module('uiModal')
        .controller('ModalAddEditOfferCtrl', ModalAddEditOfferCtrl);

})(window.angular);