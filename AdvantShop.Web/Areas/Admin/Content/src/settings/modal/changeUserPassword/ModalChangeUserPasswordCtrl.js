; (function (ng) {
    'use strict';

    var ModalChangeUserPasswordCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        ctrl.formInited = false;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve.params;
            if (params.customerId == null) {
                ctrl.close();
                return;
            }
            ctrl.customerId = params.customerId;
            ctrl.editcurrent = params.editcurrent;
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.changePassword = function () {

            var params = {
                customerId: ctrl.customerId,
                password: ctrl.password,
                passwordConfirm: ctrl.passwordConfirm,
                rnd: Math.random()
            };

            var url = ctrl.editcurrent === true ? "account/changePasswordJson" : "users/changePassword";
            $http.post(url, params).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop("success", "", "Пароль изменен");
                    $uibModalInstance.close('changePassword');
                } else {
                    toaster.error('Ошибка', data.errors[0]);
                }
            });
        }
    };

    ModalChangeUserPasswordCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalChangeUserPasswordCtrl', ModalChangeUserPasswordCtrl);

})(window.angular);