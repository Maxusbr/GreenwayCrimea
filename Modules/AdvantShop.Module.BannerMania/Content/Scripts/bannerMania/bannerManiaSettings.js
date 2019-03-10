; (function (ng) {
    'use strict';

    var bannerManiaSettingsCtrl = function ($http, toaster, $uibModal, SweetAlert) {
        var ctrl = this;

        ctrl.buttonResizeImagesEnabled = true;

        $http.get('../bmsettingsadmin/getsettings').then(function success(response) {
            ctrl.BannerInTopWidth = response.data.BannerInTopWidth,
            ctrl.BannerInTopHeight = response.data.BannerInTopHeight,
            ctrl.UnderDeliveryInfoWidth = response.data.UnderDeliveryInfoWidth,
            ctrl.UnderDeliveryInfoHeight = response.data.UnderDeliveryInfoHeight,
            ctrl.AboveDeliveryInfoWidth = response.data.AboveDeliveryInfoWidth,
            ctrl.AboveDeliveryInfoHeight = response.data.AboveDeliveryInfoHeight,
            ctrl.UnderFilterWidth = response.data.UnderFilterWidth,
            ctrl.UnderFilterHeight = response.data.UnderFilterHeight,
            ctrl.AboveFilterWidth = response.data.AboveFilterWidth,
            ctrl.AboveFilterHeight = response.data.AboveFilterHeight,
            ctrl.UnderMenuWidth = response.data.UnderMenuWidth,
            ctrl.UnderMenuHeight = response.data.UnderMenuHeight,
            ctrl.AboveFooterWidth = response.data.AboveFooterWidth,
            ctrl.AboveFooterHeight = response.data.AboveFooterHeight
        });

        ctrl.saveChanges = function (form) {
            if (form.$valid) {
                $http.post('../bmsettingsadmin/saveChanges',
                    {
                        BannerInTopWidth: ctrl.BannerInTopWidth,
                        BannerInTopHeight: ctrl.BannerInTopHeight,
                        UnderDeliveryInfoWidth: ctrl.UnderDeliveryInfoWidth,
                        UnderDeliveryInfoHeight: ctrl.UnderDeliveryInfoHeight,
                        AboveDeliveryInfoWidth: ctrl.AboveDeliveryInfoWidth,
                        AboveDeliveryInfoHeight: ctrl.AboveDeliveryInfoHeight,
                        UnderFilterWidth: ctrl.UnderFilterWidth,
                        UnderFilterHeight: ctrl.UnderFilterHeight,
                        AboveFilterWidth: ctrl.AboveFilterWidth,
                        AboveFilterHeight: ctrl.AboveFilterHeight,
                        UnderMenuWidth: ctrl.UnderMenuWidth,
                        UnderMenuHeight: ctrl.UnderMenuHeight,
                        AboveFooterWidth: ctrl.AboveFooterWidth,
                        AboveFooterHeight: ctrl.AboveFooterHeight
                    })
                    .then(function success(response) {
                        if (response.data.success) {
                            toaster.pop('success', '', response.data.msg);
                        } else {
                            toaster.pop('error', '', response.data.msg);
                        }
                    })
            }
            else {
                toaster.pop('error', '', 'Заполните необходимые данные');
            }
        }

        ctrl.resizeImages = function (form) {
            if (form.$valid) {
                ctrl.saveChanges(form);
            }
            else {
                toaster.pop('error', '', 'Заполните необходимые данные');
                return;
            }

            SweetAlert.confirm("Вы уверены, что хотите пережать изображения?", { title: "Пережатие изображений" }).then(function (result) {
                if (result === true) {
                    ctrl.buttonResizeImagesEnabled = false;
                    $http.post('../bmsettingsadmin/resizeImages')
                        .then(function success(response) {
                            ctrl.buttonResizeImagesEnabled = true;
                            if (response.data.success) {
                                toaster.pop('success', '', response.data.msg);
                            } else {
                                toaster.pop('error', '', response.data.msg);
                            }
                        })
                }
            });

        }
    };

    bannerManiaSettingsCtrl.$inject = ['$http', 'toaster', '$uibModal', 'SweetAlert'];

    ng.module('bannerManiaSettings', [])
        .controller('bannerManiaSettingsCtrl', bannerManiaSettingsCtrl)
        .component('bannerManiaSettings', {
            templateUrl: '../modules/BannerMania/content/Scripts/bannerMania/templates/bannerManiaSettings.html',
            controller: 'bannerManiaSettingsCtrl',
            bindings: {
                redirectUrl: '=',
            }
        });

})(window.angular);