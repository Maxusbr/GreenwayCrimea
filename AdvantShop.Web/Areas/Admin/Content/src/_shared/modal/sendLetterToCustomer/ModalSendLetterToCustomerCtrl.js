; (function (ng) {
    'use strict';

    var ModalSendLetterToCustomerCtrl = function ($uibModalInstance, $http, $window, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve.params;
            ctrl.customerId = params.customerId;
            ctrl.email = params.email;
            ctrl.firstName = params.firstName;
            ctrl.lastName = params.lastName;
            ctrl.patronymic = params.patronymic;

            ctrl.getLetterFormat();
        };
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };


        ctrl.getLetterFormat = function() {
            var params = {
                customerId: ctrl.customerId,
                email: ctrl.email,
                firstName: ctrl.firstName,
                lastName: ctrl.lastName,
                patronymic: ctrl.patronymic
            };

            $http.get('customers/getLetterToCustomer', { params: params }).then(function (response) {
                var data = response.data;
                ctrl.subject = data.subject;
                ctrl.text = data.text;
            });
        }

        ctrl.send = function () {

            var params = {
                customerId: ctrl.customerId,
                email: ctrl.email,
                subject: ctrl.subject,
                text: ctrl.text
            };
            
            $http.post('customers/sendLetterToCustomer', params).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', 'Письмо успешно отослано');
                    $uibModalInstance.close();
                } else {
                    toaster.pop('error', '', data.errors);
                }
            });
        }
    };

    ModalSendLetterToCustomerCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'toaster'];

    ng.module('uiModal')
        .controller('ModalSendLetterToCustomerCtrl', ModalSendLetterToCustomerCtrl);

})(window.angular);