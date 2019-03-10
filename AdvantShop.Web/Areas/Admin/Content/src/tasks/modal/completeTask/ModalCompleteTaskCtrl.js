; (function (ng) {
    'use strict';

    var ModalCompleteTaskCtrl = function ($uibModalInstance, tasksService, lastStatisticsService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.task = ctrl.$resolve.task;
            if (ctrl.task == null) {
                ctrl.close();
                return;
            }
            if (ctrl.task.orderId != null) {
                tasksService.getOrderStatuses().then(function (data) {
                    ctrl.orderStatuses = data;
                });
            } else if (ctrl.task.leadId != null) {
                tasksService.getDealStatuses().then(function (data) {
                    ctrl.dealStatuses = data;
                });
            }
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.completeTask = function () {
            ctrl.btnSleep = true;
            tasksService.completeTask(ctrl.task.id, ctrl.taskResult, ctrl.orderStatusId, ctrl.dealStatusId).then(function (data) {
                if (data != null && data.result === true) {
                    $uibModalInstance.close(true);
                    lastStatisticsService.getLastStatistics();
                } else {
                    $uibModalInstance.close(false);
                }
                ctrl.btnSleep = false;
            });

        }
    };

    ModalCompleteTaskCtrl.$inject = ['$uibModalInstance', 'tasksService', 'lastStatisticsService'];

    ng.module('uiModal')
        .controller('ModalCompleteTaskCtrl', ModalCompleteTaskCtrl);

})(window.angular);