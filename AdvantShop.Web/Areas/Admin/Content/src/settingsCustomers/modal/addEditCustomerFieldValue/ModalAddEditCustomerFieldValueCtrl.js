; (function (ng) {
    'use strict';

    var ModalAddEditCustomerFieldValueCtrl = function ($uibModalInstance, customerFieldValuesService, toaster) {
        var ctrl = this;
        ctrl.formInited = false;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.id = params.id != null ? params.id : 0;
            ctrl.fieldId = params.fieldId;
            ctrl.mode = ctrl.id != 0 ? "edit" : "add";

            if (ctrl.mode == "add") {
                ctrl.sortOrder = 0;
                ctrl.formInited = true;
            } else {
                ctrl.getCustomerFieldValue(ctrl.id);
            }
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
        
        ctrl.getCustomerFieldValue = function (id) {
            customerFieldValuesService.getCustomerFieldValue(id).then(function (data) {
                if (data != null) {
                    ctrl.fieldId = data.CustomerFieldId;
                    ctrl.value = data.Value;
                    ctrl.sortOrder = data.SortOrder;
                }
                ctrl.form.$setPristine();
                ctrl.formInited = true;
            });
        }

        ctrl.save = function () {

            ctrl.btnSleep = true;

            var params = {
                id: ctrl.id,
                customerFieldId: ctrl.fieldId,
                value: ctrl.value,
                sortOrder: ctrl.sortOrder
            };

            customerFieldValuesService.addOrUpdateCustomerFieldValue(ctrl.mode == "add", params).then(function (data) {
                if (data.result == true) {
                    toaster.pop("success", "", ctrl.mode == "add" ? "Значение добавлено" : "Изменения сохранены");
                    $uibModalInstance.close('saveCustomerFieldValue');
                } else {
                    toaster.pop("error", "Ошибка", "Ошибка при " + ctrl.mode == "add" ? "добавлении" : "редактировании");
                    ctrl.btnSleep = false;
                }
            });
        }
    };

    ModalAddEditCustomerFieldValueCtrl.$inject = ['$uibModalInstance', 'customerFieldValuesService', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditCustomerFieldValueCtrl', ModalAddEditCustomerFieldValueCtrl);

})(window.angular);