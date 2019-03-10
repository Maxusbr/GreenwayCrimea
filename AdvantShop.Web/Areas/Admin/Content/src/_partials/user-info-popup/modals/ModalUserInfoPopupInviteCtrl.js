; (function (ng) {
    'use strict';

    var ModalUserInfoPopupInviteCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.userList = [{
                FirstName: null,
                LastName: null,
                Email: null
            }];
        };
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.addFieldsUser = function () {
            ctrl.userList.push({
                FirstName: null,
                LastName: null,
                Email: null
            });
        };

        ctrl.removeUser = function (user) {
            var index = ctrl.userList.indexOf(user);

            if (index !== -1) {
                ctrl.userList.splice(index, 1);
            } else {
                throw Error('Not found user in invite list');
            }

        };

        ctrl.invite = function (userList) {
            ctrl.btnLoading = true;
            $http.post('users/invite', { users: ctrl.userList }).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', 'Приглашения отправлены');
                    $uibModalInstance.close();
                } else {
                    toaster.pop('error', '', data.errors);
                }
                ctrl.btnLoading = false;
            });
        };
    };

    ModalUserInfoPopupInviteCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalUserInfoPopupInviteCtrl', ModalUserInfoPopupInviteCtrl);

})(window.angular);