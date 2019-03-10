; (function (ng) {
    'use strict';

    var ModalChangeOrderStatusCtrl = function ($uibModalInstance, $window, toaster, $q, $http, lastStatisticsService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve.params;
            ctrl.orderId = params.orderId;
            ctrl.statusId = params.statusId;
            ctrl.statusName = params.statusName;
            ctrl.showNotifyEmail = false;
            ctrl.showNotifySms = false;
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancelChangeOrderStatus');
        };

        ctrl.closeNotify = function () {
            $uibModalInstance.dismiss('cancelNotifyMsg');
        }
        
        ctrl.save = function() {
            $http.post('orders/changeOrderStatus', { orderId: ctrl.orderId, statusId: ctrl.statusId, basis: ctrl.basis, rnd: Math.random() }).then(function (response) {
                if (response.data.result == true) {

                    toaster.pop('success', '', 'Статус заказа сохранен');
                    
                    lastStatisticsService.getLastStatistics();
                    
                    if (response.data.isNotifyUserEmail === true) {
                        ctrl.showNotifyEmail = true;
                    }
                    if (response.data.isNotifyUserSms === true) {
                        ctrl.showNotifySms = true;
                    }

                    if (response.data.isNotifyUserEmail === false && response.data.isNotifyUserSms === false) {
                        $uibModalInstance.close(true);
                    }

                    ctrl.showNotifyMsg = true;
                }
                else {
                    toaster.pop('error', '', 'Не удалось сменить статус, обновите страницу');
                    $uibModalInstance.close();
                }
            });
        }

        ctrl.notifyStatusChanged = function () {
            $http.post('orders/notifyStatusChanged', { orderId: ctrl.orderId }).then(function (response) {
                if (response.data.result == true) {
                    toaster.pop('success', '', 'Уведомление о изменении статуса заказа отправлено');
                    $uibModalInstance.close(true);
                }
            });
        }

    };

    ModalChangeOrderStatusCtrl.$inject = ['$uibModalInstance', '$window', 'toaster', '$q', '$http', 'lastStatisticsService'];

    ng.module('uiModal')
        .controller('ModalChangeOrderStatusCtrl', ModalChangeOrderStatusCtrl);

})(window.angular);