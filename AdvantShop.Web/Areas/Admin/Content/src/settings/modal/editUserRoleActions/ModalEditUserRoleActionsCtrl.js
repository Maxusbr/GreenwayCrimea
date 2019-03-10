; (function (ng) {
    'use strict';

    var ModalEditUserRoleActionsCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        ctrl.formInited = false;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve.params;
            ctrl.customerId = params.customerId;
            ctrl.roleActions = params.roleActionKeys
        };
        
        ctrl.setSelectionAll = function (selected) {
            angular.forEach(ctrl.roleActions, function (roleAction, key) {
                roleAction.Enabled = selected;        
            });
        }

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.apply = function () {            
            $uibModalInstance.close({ roleActionKeys: ctrl.roleActions });            
        }

    };

    ModalEditUserRoleActionsCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalEditUserRoleActionsCtrl', ModalEditUserRoleActionsCtrl);

})(window.angular);