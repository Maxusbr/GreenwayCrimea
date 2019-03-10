; (function (ng) {
    'use strict';

    var ModalChangeOrderStatusesCtrl = function ($uibModalInstance, $http, lastStatisticsService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            $http.get('orders/getorderstatuses').then(function (response) {
                ctrl.statuses = response.data;
                if (response.data != null && response.data.length > 0) {
                    ctrl.status = response.data[0];
                }
            });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.changeStatus = function () {

            $http.post('orders/changestatus',
                        ng.extend(ctrl.$resolve.params || {}, {
                            newOrderStatusId: ctrl.status.value, statusBasis: (ctrl.basis != null ? ctrl.basis : '')
                        })).then(function (response) {

                            lastStatisticsService.getLastStatistics();
                            $uibModalInstance.close('changedStatus');
                        });
        }
    };

    ModalChangeOrderStatusesCtrl.$inject = ['$uibModalInstance', '$http', 'lastStatisticsService'];

    ng.module('uiModal')
        .controller('ModalChangeOrderStatusesCtrl', ModalChangeOrderStatusesCtrl);

})(window.angular);