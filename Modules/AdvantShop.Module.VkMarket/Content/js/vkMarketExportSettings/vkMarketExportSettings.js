; (function (ng) {
    'use strict';

    var vkMarketExportSettingsCtrl = function ($http, toaster, vkMarketService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.getSettings();
            ctrl.reportsStart = false;
        };

        ctrl.getSettings = function () {
            vkMarketService.getExportSettings().then(function (data) {
                ctrl.YesNo = [{ label: 'Нет', value: false }, { label: 'Да', value: true }];
                ctrl.settings = data;
            });
        }

        ctrl.save = function() {
            return vkMarketService.saveExportSettings(ctrl.settings).then(function (data) {
               if (data.result === true) {
                   toaster.pop('success', '', 'Настройки сохранены');
               } else {
                   toaster.pop('error', '', 'Ошибка при сохранении настроек');
               }
            });
        }
        
        ctrl.deleteGroup = function() {
            vkMarketService.deleteGroup().then(ctrl.onUpdate);
        }
        
        ctrl.export = function () {
            ctrl.save().then(function() {
                vkMarketService.export().then(function (data) {
                    if (data.result === true) {
                        toaster.pop('success', '', 'Начался перенос товаров в ВКонтакте. Длительность переноса зависит от кол-ва товаров и фотографий.');
                        ctrl.settings.ExportIsRun = true;

                        ctrl.getExportProgress();

                        if (!ctrl.reportsStart) {
                            ctrl.getReports();
                            ctrl.reportsStart = true;
                        }
                    } else {
                        data.errors.forEach(function (e) {
                            toaster.pop('error', '', e);
                        });
                    }
                });
            });
        }

        ctrl.getReports = function() {
            setTimeout(function() {
                vkMarketService.getReports().then(function (data) {
                    ctrl.settings.Reports = data.reports;
                    ctrl.getReports();
                });
            }, 3000);
        }

        ctrl.getExportProgress = function () {
            vkMarketService.getExportProgress().then(function (data) {
                ctrl.Total = data.Total;
                ctrl.Current = data.Current;
                ctrl.Percent = ctrl.Total > 0 ? parseInt(100 / ctrl.Total * ctrl.Current) : 0;

                if (ctrl.Current === ctrl.Total && ctrl.Total > 0) {
                    toaster.pop('success', '', 'Экспорт закончен');
                } else {
                    setTimeout(function () { ctrl.getExportProgress(); }, 500);
                }
            });
        }

        ctrl.deleteAllProducts = function() {
            vkMarketService.deleteAllProducts().then(function (data) {
                toaster.pop('success', '', 'Удаление началось');
            });
        }
    };

    vkMarketExportSettingsCtrl.$inject = ['$http', 'toaster', 'vkMarketService'];

    ng.module('vkMarketExportSettings', [])
        .controller('vkMarketExportSettingsCtrl', vkMarketExportSettingsCtrl)
        .component('vkMarketExportSettings', {
            templateUrl: '../modules/vkmarket/content/js/vkMarketExportSettings/vkMarketExportSettings.html',
            controller: 'vkMarketExportSettingsCtrl',
            bindings: {
                onUpdate: '&'
            }
        });


})(window.angular);