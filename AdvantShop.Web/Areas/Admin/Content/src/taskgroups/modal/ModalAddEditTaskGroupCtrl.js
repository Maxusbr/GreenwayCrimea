; (function (ng) {
    'use strict';

    var ModalAddEditTaskGroupCtrl = function ($uibModalInstance, $http, $window, toaster) {
        var ctrl = this;
        ctrl.formInited = false;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve;
            ctrl.id = params.id != null ? params.id : 0;
            ctrl.goToProjectPage = params.goToProjectPage;
            ctrl.mode = ctrl.id != 0 ? "edit" : "add";

            if (ctrl.mode == "add") {
                ctrl.name = "";
                ctrl.sortOrder = 0;
                ctrl.formInited = true;
            } else {
                ctrl.getTaskGroup(ctrl.id);
            }
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.getTaskGroup = function (id) {
            $http.get('taskgroups/getTaskGroup', { params: { id: id, rnd: Math.random() } }).then(function (response) {
                var data = response.data.obj;
                if (data != null) {
                    ctrl.name = data.Name;
                    ctrl.sortOrder = data.SortOrder;
                }
                ctrl.formAddEditTaskGroup.$setPristine();
                ctrl.formInited = true;
            });
        }

        ctrl.save = function () {
            var params = {
                id: ctrl.id,
                name: ctrl.name,
                sortOrder: ctrl.sortOrder,
                rnd: Math.random()
            };

            var url = ctrl.mode == "add" ? 'taskgroups/addTaskGroup' : 'taskgroups/updateTaskGroup';

            $http.post(url, params).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop("success", "", ctrl.mode == "add" ? "Проект добавлен" : "Изменения сохранены");
                    if (ctrl.goToProjectPage === true) {
                        $uibModalInstance.close(data.obj);
                        $window.location.assign('projects/' + data.obj);
                    } else {
                        $uibModalInstance.close(data.obj);
                    }
                } else {
                    toaster.pop("error", "Ошибка", "Ошибка при " + ctrl.mode == "add" ? "создании" : "редактировании");
                }
            });
        }
    };

    ModalAddEditTaskGroupCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditTaskGroupCtrl', ModalAddEditTaskGroupCtrl);

})(window.angular);