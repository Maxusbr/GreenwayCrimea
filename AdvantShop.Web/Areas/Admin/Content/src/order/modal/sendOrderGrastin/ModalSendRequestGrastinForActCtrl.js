; (function (ng) {
    'use strict';

    var ModalSendRequestGrastinForActCtrl = function ($uibModalInstance, $window, toaster, $q, $http) {
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
                contractId: ctrl.contractId,
                seats: ctrl.seats,
            };

            $http.post('grastin/sendrequestforact', params).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    $uibModalInstance.close();
                    window.location = "grastin/getact?filename=" + encodeURIComponent(data.obj.FileName);
                } else {
                    ctrl.btnLoading = false;
                    data.errors.forEach(function (error) {
                        toaster.pop('error', '', error);
                    });
                }
            });
        }

    };

    ModalSendRequestGrastinForActCtrl.$inject = ['$uibModalInstance', '$window', 'toaster', '$q', '$http'];

    ng.module('uiModal')
        .controller('ModalSendRequestGrastinForActCtrl', ModalSendRequestGrastinForActCtrl);

})(window.angular);