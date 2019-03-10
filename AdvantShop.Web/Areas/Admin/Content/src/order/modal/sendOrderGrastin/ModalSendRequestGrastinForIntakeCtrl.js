; (function (ng) {
    'use strict';

    var ModalSendRequestGrastinForIntakeCtrl = function ($uibModalInstance, $window, toaster, $q, $http, $translate) {
        var ctrl = this;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve.obj;
            ctrl.orderId = params.orderId;
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.send = function () {
            var params = {
                orderId: ctrl.orderId,
                regionId: ctrl.regionId,
                time: ctrl.time,
                volume: ctrl.volume,
            };

            $http.post('grastin/sendrequestforintake', params).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', $translate.instant('Admin.Js.Order.ApplicationSuccessfullySent'));
                    $uibModalInstance.close();
                } else {
                    ctrl.btnLoading = false;
                    data.errors.forEach(function (error) {
                        toaster.pop('error', '', error);
                    });
                }
            });
        }

    };

    ModalSendRequestGrastinForIntakeCtrl.$inject = ['$uibModalInstance', '$window', 'toaster', '$q', '$http', '$translate'];

    ng.module('uiModal')
        .controller('ModalSendRequestGrastinForIntakeCtrl', ModalSendRequestGrastinForIntakeCtrl);

})(window.angular);