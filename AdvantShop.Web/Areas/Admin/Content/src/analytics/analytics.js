; (function (ng) {
    'use strict';

    var AnalyticsCtrl = function ($q, $location, $window, $interval, urlHelper, analyticsService, SweetAlert, toaster) {

        var ctrl = this;

        ctrl.exportOrdersSettings = {};
        ctrl.exportCustomersSettings = {};
        ctrl.exportProductsSettings = {};
        

        ctrl.isStartExport = false;

        ctrl.init = function () {
        
        };
        
        ctrl.exportOrders = function () {
            analyticsService.exportOrders(ctrl.exportOrdersSettings).then(function (response) {
                if (response) {
                    ctrl.isStartExport = true;                    
                }              
            });
        };

        ctrl.exportCustomers = function () {
            analyticsService.exportCustomers(ctrl.exportCustomersSettings).then(function (response) {
                if (response) {
                    ctrl.isStartExport = true;
                }
            });
        };
        ctrl.exportProducts = function () {            
            analyticsService.exportProducts(ctrl.exportProductsSettings).then(function (response) {
                if (response) {
                    ctrl.isStartExport = true;
                }
            });
        };

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

                analyticsService.getCommonStatistic().then(function (response) {
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

                ctrl.exportProductsSettings.selectedCategories = selected;
                
                toaster.pop('success', 'Категория добавлена в выгрузку');
            },

            deselect_node: function (event, data) {

                var tree = ng.element(event.target).jstree(true);
                var selected = tree.get_selected(false);
                ctrl.exportProductsSettings.selectedCategories = selected;
                
                toaster.pop('success', 'Категория удалена из выгрузки');
            },
        };
    };

    AnalyticsCtrl.$inject = ['$q', '$location', '$window', '$interval', 'urlHelper', 'analyticsService', 'SweetAlert', 'toaster'];

    ng.module('analytics', ['urlHelper'])
      .controller('AnalyticsCtrl', AnalyticsCtrl);

})(window.angular);