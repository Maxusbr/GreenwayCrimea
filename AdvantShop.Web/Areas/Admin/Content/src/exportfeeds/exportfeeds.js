; (function (ng) {
    'use strict';

    var ExportFeedsCtrl = function ($q, $location, $window, $interval, urlHelper, exportfeedsService, SweetAlert, toaster, uiGridCustomConfig) {

        var ctrl = this;

        ctrl.exportFeedId = 0;

        ctrl.exportFeedFields = [];

        ctrl.defaultFields = [];

        ctrl.CommonSettings = {};

        ctrl.AdvancedSettings = {};

        ctrl.init = function (exportFeedId) {
            ctrl.exportFeedId = exportFeedId;
            ctrl.CommonSettings.Name = 'test';
        };

        ctrl.initExportFields = function (defaultFields) {
            ctrl.defaultFields = defaultFields;
        };

        ctrl.exportAllProductsClick = function () {
            ctrl.exportAllProducts = true;
            toaster.pop('success', 'Выбрана выгрузка всего каталога');
        };

        ctrl.startExport = function () {
            var success = true;
            var messsage = "";

            if (ctrl.gridGlobalDeliveryOptions != null && ctrl.gridGlobalDeliveryOptions.data != null) { // ctrl.showGridGlobalDeliveryCosts
                ctrl.AdvancedSettings.GlobalDeliveryCost = JSON.stringify(ctrl.gridGlobalDeliveryOptions.data);
            }

            if (ctrl.LocalDeliveryOption != null) {
                ctrl.AdvancedSettings.LocalDeliveryOption =
                    JSON.stringify({
                        Days: ctrl.LocalDeliveryOption.Days,
                        OrderBefore: ctrl.LocalDeliveryOption.OrderBefore
                    });
            }

            exportfeedsService.saveExportFeedSettings(ctrl.exportFeedId, ctrl.CommonSettings.Name, ctrl.CommonSettings.Description, ctrl.CommonSettings, JSON.stringify(ctrl.AdvancedSettings)).then(function (response) {
                if (!response) {
                    messsage = " Ошибка при сохранении настроек.";
                    success = false;
                }
                if (ctrl.exportFeedFields.length > 0) {
                    exportfeedsService.saveExportFeedFields(ctrl.exportFeedId, ctrl.exportFeedFields).then(function (response) {
                        if (!response) {
                            messsage = " Ошибка при сохранении полей выгрузки.";
                            success = false;
                        }

                        if (success) {
                            $window.location.assign('exportfeeds/export/' + ctrl.exportFeedId);
                        }
                        else {
                            toaster.pop('error', messsage);
                        }
                        return success;
                    });
                }
                else {
                    $window.location.assign('exportfeeds/export/' + ctrl.exportFeedId);
                    return success;
                }
            });
        };

        ctrl.saveExportFeed = function () {
            var success = true;
            var messsage = "";
            if (ctrl.gridGlobalDeliveryOptions != null && ctrl.gridGlobalDeliveryOptions.data != null) { // ctrl.showGridGlobalDeliveryCosts
                ctrl.AdvancedSettings.GlobalDeliveryCost = JSON.stringify(ctrl.gridGlobalDeliveryOptions.data);
            }

            if (ctrl.LocalDeliveryOption != null) {
                ctrl.AdvancedSettings.LocalDeliveryOption =
                    JSON.stringify({
                        Days: ctrl.LocalDeliveryOption.Days,
                        OrderBefore: ctrl.LocalDeliveryOption.OrderBefore
                    });
            }

            exportfeedsService.saveExportFeedSettings(ctrl.exportFeedId, ctrl.CommonSettings.Name, ctrl.CommonSettings.Description, ctrl.CommonSettings, JSON.stringify(ctrl.AdvancedSettings)).then(function (response) {
                if (!response) {
                    messsage = " Ошибка при сохранении настроек.";
                    success = false;
                }
                if (ctrl.exportFeedFields.length > 0) {
                    exportfeedsService.saveExportFeedFields(ctrl.exportFeedId, ctrl.exportFeedFields).then(function (response) {
                        if (!response) {
                            messsage = " Ошибка при сохранении полей выгрузки.";
                            success = false;
                        }

                        if (success) {
                            toaster.pop('success', '', 'Изменения сохранены');
                        }
                        else {
                            toaster.pop('error', '', messsage);
                        }

                        //ctrl.exportFeedForm.modified = false;

                        ctrl.exportFeedForm.$setPristine();
                        return success;
                    });
                }
                else {
                    toaster.pop('success', '', 'Изменения сохранены');
                    ctrl.exportFeedForm.$setPristine();
                    //ctrl.exportFeedForm.modified = false;
                    return success;
                }
            });
        };


        ctrl.saveExportFeedFields = function () {
            exportfeedsService.saveExportFeedFields(ctrl.exportFeedId, ctrl.exportFeedFields).then(function (response) {
                if (response) {
                    toaster.pop('success', '', 'Изменения сохранены');
                }
                else {
                    toaster.pop('error', '', 'Изменения не сохранены');
                }
            });
        };

        ctrl.setNoneExportFeedFields = function () {
            for (var i = 0; i < ctrl.exportFeedFields.length; i++) {
                ctrl.exportFeedFields[i] = 'None';
            }
        }

        ctrl.setDefaultExportFeedFields = function () {
            for (var i = 0; i < ctrl.defaultFields.length; i++) {
                ctrl.exportFeedFields[i] = ctrl.defaultFields[i];
            }
        }

        ctrl.deleteExport = function () {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result) {
                    exportfeedsService.deleteExport(ctrl.exportFeedId).then(function (response) {
                        if (response) {
                            $window.location.assign('exportfeeds');
                        }
                        else {
                            toaster.pop('error', '', 'Не удалось удалить выгрузку');
                        }
                    });
                }
            });
        }

        ctrl.progressValue = 0;
        ctrl.progressTotal = 0;
        ctrl.progressPercent = 0;
        ctrl.progressCurrentProcess = "";
        ctrl.progressCurrentProcessName = "";
        ctrl.IsRun = true;
        ctrl.FileName = "";

        ctrl.stop = 0;

        ctrl.initProgress = function () {

            ctrl.stop = $interval(function () {

                exportfeedsService.getCommonStatistic().then(function (response) {
                    ctrl.IsRun = response.IsRun;
                    if (!response.IsRun) {
                        $interval.cancel(ctrl.stop);
                        ctrl.FileName = response.FileName.indexOf('?') != -1 ? response.FileName : response.FileName + "?rnd=" + Math.random();
                    }
                    ctrl.progressTotal = response.Total;
                    ctrl.progressValue = response.Processed;
                    ctrl.progressCurrentProcess = response.CurrentProcess;
                    ctrl.progressCurrentProcessName = response.CurrentProcessName;
                });

            }, 100);
        }

        ctrl.treeCallbacks = {
            select_node: function (event, data) {

                var tree = ng.element(event.target).jstree(true);
                var selected = tree.get_selected(false);
                exportfeedsService.addCategoriesToExport(ctrl.exportFeedId, selected).then(function (response) { });

                toaster.pop('success', '', 'Категория добавлена в выгрузку');
            },

            deselect_node: function (event, data) {

                var tree = ng.element(event.target).jstree(true);
                var selected = tree.get_selected(false);
                exportfeedsService.addCategoriesToExport(ctrl.exportFeedId, selected).then(function (response) { });

                toaster.pop('success', '', 'Категория удалена из выгрузки');
            },
        };

        ctrl.saveExportFeedSettings = function () {            
            exportfeedsService.saveExportFeedSettings(ctrl.exportFeedId, ctrl.CommonSettings, JSON.stringify(ctrl.AdvancedSettings)).then(function (response) {
                if (response) {
                    toaster.pop('success', '', 'Изменения сохранены');
                }
                else {
                    toaster.pop('error', '', 'Изменения не сохранены');
                }
            });
        };

        //ctrl.gridGlobalDeliveryOptions = {};

        ctrl.$onInit = function () {
            ctrl.showGridGlobalDeliveryCosts = true;
        }

        ctrl.gridGlobalDeliveryOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: [
                {
                    name: 'Cost',
                    displayName: 'Стоимость'
                },
                {
                    name: 'Days',
                    displayName: 'Срок доставки (Примеры: "0","1","1-3")'
                },
                {
                    name: 'OrderBefore',
                    displayName: 'Время, до которого нужно успеть заказать, чтобы сроки доставки не сдвинулись на один день вперед'
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 40,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.deleteGlobalOptionCost(row.entity)" class="ui-grid-custom-service-icon fa fa-times link-invert"></a></div>'
                }
            ],
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.gridGlobalDeliveryCosts = grid;            
            ctrl.showGridGlobalDeliveryCosts = grid.gridOptions.data != null && grid.gridOptions.data.length > 0;
        };

        ctrl.gridOnFetch = function (grid) {
            ctrl.showGridGlobalDeliveryCosts = grid.gridOptions.data != null && grid.gridOptions.data.length > 0;
        }

        ctrl.setGridOptionsData = function (data)
        {
            ctrl.gridGlobalDeliveryOptions.data = data;
        }

        ctrl.deleteGlobalOptionCost = function(row)
        {
            var indexDelete = ctrl.gridGlobalDeliveryOptions.data.indexOf(row);
            ctrl.gridGlobalDeliveryOptions.data.splice(indexDelete, 1);
            //delete ctrl.gridGlobalDeliveryOptions.data[indexDelete];

            ctrl.exportFeedForm.modified = true;
        }

        ctrl.yandexAddGlobalDeliveryCost = function (result)
        {
            ctrl.gridGlobalDeliveryOptions.data.push(result);
            ctrl.exportFeedForm.modified = true;
        }
    };

    ExportFeedsCtrl.$inject = ['$q', '$location', '$window', '$interval', 'urlHelper', 'exportfeedsService', 'SweetAlert', 'toaster', 'uiGridCustomConfig'];

    ng.module('exportfeeds', ['urlHelper'])
      .controller('ExportFeedsCtrl', ExportFeedsCtrl);

})(window.angular);