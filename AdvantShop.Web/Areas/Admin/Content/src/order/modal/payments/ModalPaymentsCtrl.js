; (function (ng) {
    'use strict';

    var ModalPaymentsCtrl = function ($uibModalInstance, $window, toaster, $q, $http) {
        var ctrl = this;

        ctrl.paymentLoading = true;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve.order;

            ctrl.orderId = params.orderId;

            ctrl.contact = {
                Country: params.country,
                Region: params.region,
                City: params.city //City - с большой потому что используется в scripts\_partials\shipping\extend\yandexdelivery\yandexdelivery.js
            };

            ctrl.getPayments();
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.getPayments = function() {
            $http.get('orders/getPayments', { params: ng.extend(ctrl.contact, { orderId: ctrl.orderId }) }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.payments = data.payments;
                }
            }).finally(function () {
                ctrl.paymentLoading = false;
            });
        }

        ctrl.changePayment = function (payment) {
            ctrl.selectPayment = payment;
        };

        ctrl.save = function() {
            
            if (ctrl.selectPayment == null) {
                toaster.pop('error', '', 'Выберите метод оплаты');
                return;
            }

            $http.post('orders/savePayment', ng.extend(ctrl.contact, { orderId: ctrl.orderId, payment: ctrl.selectPayment })).then(function (response) {
                $uibModalInstance.close({ payment: ctrl.selectPayment });
            });
        }

    };

    ModalPaymentsCtrl.$inject = ['$uibModalInstance', '$window', 'toaster', '$q', '$http'];

    ng.module('uiModal')
        .controller('ModalPaymentsCtrl', ModalPaymentsCtrl);

})(window.angular);