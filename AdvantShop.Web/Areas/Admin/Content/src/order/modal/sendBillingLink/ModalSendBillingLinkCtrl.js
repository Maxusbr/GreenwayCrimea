; (function (ng) {
    'use strict';

    var ModalSendBillingLinkCtrl = function ($uibModalInstance, $http, $window, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve.params;
            ctrl.orderId = params.orderId;

            $http.get('orders/getBillingLinkMailTemplate', {params: { orderId: ctrl.orderId}}).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    ctrl.subject = data.subject;
                    ctrl.text = data.text;
                } else {
                    data.errors.forEach(function(error) {
                        ctrl.error = error;
                    });
                }
            });
        };
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };


        ctrl.send = function () {

            if (ctrl.subject == null || ctrl.text == null) {
                toaster.pop('error', '', 'Заполните обязательные поля');
                return;
            }

            var params = {
                orderId: ctrl.orderId,
                subject: ctrl.subject,
                text: ctrl.text
            };

            $http.post('orders/sendBillingLink', params).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', 'Письмо успешно отослано');
                    $uibModalInstance.close();
                } else {
                    data.errors.forEach(function (error) {
                        toaster.pop('error', '', error);
                    });
                }
            });
        }
    };

    ModalSendBillingLinkCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'toaster'];

    ng.module('uiModal')
        .controller('ModalSendBillingLinkCtrl', ModalSendBillingLinkCtrl);

})(window.angular);