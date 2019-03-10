; (function (ng) {
    'use strict';

    var ModalSendSmsCtrl = function ($uibModalInstance, $filter, $http, toaster, $q) {
        var ctrl = this;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve.params;
            ctrl.customerId = params.customerId != null ? params.customerId : '';
            ctrl.phone = params.phone != null ? params.phone.replace(new RegExp("\\D", "g"), "") : '';

            ctrl.check();
        };

        ctrl.check = function () {
            $http.get('../smsNotifications/CheckSmsConfiguration').then(function (response) {
                var data = response.data;
                ctrl.error = data.result != true ? data.error : null;
            });
        }

        ctrl.send = function() {
            
            $http.post('../smsNotifications/sendSms', { customerId: ctrl.customerId, phone: ctrl.phone, text: ctrl.text })
                .then(function (response) {

                    var data = response.data;
                    if (data.result === true) {
                        toaster.pop('success', '', 'Смс отправлено');
                    } else {
                        toaster.pop('error', '', data.error);
                    }

                    $uibModalInstance.close();
                });
        }

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    ModalSendSmsCtrl.$inject = ['$uibModalInstance', '$filter', '$http', 'toaster', '$q'];

    ng.module('uiModal')
        .controller('ModalSendSmsCtrl', ModalSendSmsCtrl);

})(window.angular);