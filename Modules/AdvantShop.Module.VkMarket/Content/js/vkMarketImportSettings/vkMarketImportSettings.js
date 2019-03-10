; (function (ng) {
    'use strict';

    var vkMarketImportSettingsCtrl = function ($http, toaster, vkMarketService) {
        var ctrl = this;

        ctrl.$onInit = function () {

        };

        ctrl.import = function () {
            vkMarketService.import(ctrl.settings).then(function (data) {
               if (data.result === true) {
                   toaster.pop('success', '', 'Импорт товаров начался');

                   setTimeout(function () { ctrl.getImportProgress(); }, 500);
               } else {
                   data.errors.forEach(function(e) {
                       toaster.pop('error', '', e);
                   });
               }
            });
        }

        ctrl.getImportProgress = function() {
            vkMarketService.getImportProgress().then(function (data) {
                ctrl.Total = data.Total;
                ctrl.Current = data.Current;
                ctrl.Percent = ctrl.Total > 0 ? parseInt(100 / ctrl.Total * ctrl.Current) : 0;

                if (ctrl.Current === ctrl.Total && ctrl.Total > 0) {
                    toaster.pop('success', '', 'Импорт товаров закончен');
                    setTimeout(function () { window.location.reload(); }, 500);
                } else {
                    setTimeout(function () { ctrl.getImportProgress(); }, 500);
                }
            });
        }
    };

    vkMarketImportSettingsCtrl.$inject = ['$http', 'toaster', 'vkMarketService'];

    ng.module('vkMarketImportSettings', [])
        .controller('vkMarketImportSettingsCtrl', vkMarketImportSettingsCtrl)
        .component('vkMarketImportSettings', {
            templateUrl: '../modules/vkmarket/content/js/vkMarketImportSettings/vkMarketImportSettings.html',
            controller: 'vkMarketImportSettingsCtrl',
            bindings: {
                onUpdate: '&'
            }
        });

})(window.angular);