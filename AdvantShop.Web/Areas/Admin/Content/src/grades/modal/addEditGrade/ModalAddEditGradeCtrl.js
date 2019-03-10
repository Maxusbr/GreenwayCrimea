; (function (ng) {
    'use strict';

    var ModalAddEditGradeCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        ctrl.formInited = false;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.id = params.id;
            ctrl.mode = ctrl.id != null ? "edit" : "add";

            if (ctrl.mode == "add") {
                ctrl.sortOrder = 0;
                ctrl.formInited = true;
            } else {
                ctrl.getGrade(ctrl.id);
            }
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
        
        ctrl.getGrade = function (id) {
            $http.get('grades/get', { params: { id: id, rnd: Math.random() } }).then(function (response) {
                var data = response.data.obj;
                if (data != null) {
                    ctrl.name = data.Name;
                    ctrl.bonusPercent = data.BonusPercent;
                    ctrl.purchaseBarrier = data.PurchaseBarrier;
                    ctrl.sortOrder = data.SortOrder;
                } else {
                    ctrl.close();
                }
                ctrl.addEditGradeForm.$setPristine();
                ctrl.formInited = true;
            });
        }

        ctrl.save = function () {
            ctrl.btnSleep = true;
            var params = {
                id: ctrl.id,
                name: ctrl.name,
                bonusPercent: ctrl.bonusPercent,
                purchaseBarrier: ctrl.purchaseBarrier,
                sortOrder: ctrl.sortOrder,
                rnd: Math.random()
            };

            var url = ctrl.mode == "add" ? 'grades/add' : 'grades/edit';

            $http.post(url, params).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop("success", "", ctrl.mode == "add" ? "Грейд добавлен" : "Изменения сохранены");
                    $uibModalInstance.close('saveGrade');
                } else {
                    toaster.pop("error", "Ошибка", "Ошибка при " + (ctrl.mode == "add" ? "добавлении" : "редактировании"));
                    ctrl.btnSleep = false;
                }
            });
        }
    };

    ModalAddEditGradeCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditGradeCtrl', ModalAddEditGradeCtrl);

})(window.angular);