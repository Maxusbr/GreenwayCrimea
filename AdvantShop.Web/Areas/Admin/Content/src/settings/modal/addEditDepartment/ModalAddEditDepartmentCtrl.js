; (function (ng) {
    'use strict';

    var ModalAddEditDepartmentCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        ctrl.formInited = false;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.departmentId = params.departmentId != null ? params.departmentId : 0;
            ctrl.mode = ctrl.departmentId != 0 ? "edit" : "add";

            if (ctrl.mode == "add") {
                ctrl.sort = 0;
                ctrl.enabled = true;
                ctrl.formInited = true;
            } else {
                ctrl.getDepartment(ctrl.departmentId);
            }
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
        
        ctrl.getDepartment = function (departmentId) {
            $http.get('departments/getDepartment', { params: { departmentId: departmentId, rnd: Math.random() } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.name = data.Name;
                    ctrl.sort = data.Sort;
                    ctrl.enabled = data.Enabled;
                }
                ctrl.addEditDepartmentForm.$setPristine();
                ctrl.formInited = true;
            });
        }

        ctrl.save = function () {

            ctrl.btnSleep = true;

            var params = {
                departmentId: ctrl.departmentId,
                name: ctrl.name,
                sort: ctrl.sort,
                enabled: ctrl.enabled,
                rnd: Math.random()
            };

            var url = ctrl.mode == "add" ? 'departments/addDepartment' : 'departments/updateDepartment';

            $http.post(url, params).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop("success", "", ctrl.mode == "add" ? "Отдел добавлен" : "Изменения сохранены");
                    $uibModalInstance.close('saveDepartment');
                } else {
                    toaster.pop("error", "Ошибка", "Ошибка при " + ctrl.mode == "add" ? "создании" : "редактировании");
                    ctrl.btnSleep = false;
                }
            });
        }
    };

    ModalAddEditDepartmentCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditDepartmentCtrl', ModalAddEditDepartmentCtrl);

})(window.angular);