; (function (ng) {
    'use strict';

    var ModalAddEditManagerRoleCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        ctrl.formInited = false;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.id = params.id != null ? params.id : 0;
            ctrl.mode = ctrl.id != 0 ? "edit" : "add";

            if (ctrl.mode == "add") {
                ctrl.sortOrder = 0;
                ctrl.formInited = true;
            } else {
                ctrl.getManagerRole(ctrl.id);
            }
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
        
        ctrl.getManagerRole = function (id) {
            $http.get('managerRoles/getManagerRole', { params: { id: id, rnd: Math.random() } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.name = data.Name;
                    ctrl.sortOrder = data.SortOrder;
                }
                ctrl.addEditManagerRoleForm.$setPristine();
                ctrl.formInited = true;
            });
        }

        ctrl.save = function () {

            ctrl.btnSleep = true;

            var params = {
                id: ctrl.id,
                name: ctrl.name,
                sortOrder: ctrl.sortOrder,
                rnd: Math.random()
            };

            var url = ctrl.mode == "add" ? 'managerRoles/addManagerRole' : 'managerRoles/updateManagerRole';

            $http.post(url, params).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop("success", "", ctrl.mode == "add" ? "Роль добавлена" : "Изменения сохранены");
                    $uibModalInstance.close('saveManagerRole');
                } else {
                    toaster.pop("error", "Ошибка", "Ошибка при " + (ctrl.mode == "add" ? "создании" : "редактировании"));
                    ctrl.btnSleep = false;
                }
            });
        }
    };

    ModalAddEditManagerRoleCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditManagerRoleCtrl', ModalAddEditManagerRoleCtrl);

})(window.angular);