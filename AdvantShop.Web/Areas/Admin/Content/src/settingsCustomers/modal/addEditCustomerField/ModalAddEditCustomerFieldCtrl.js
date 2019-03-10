; (function (ng) {
    'use strict';

    var ModalAddEditCustomerFieldCtrl = function ($uibModalInstance, $filter, $timeout, customerFieldsService, toaster) {
        var ctrl = this;
        ctrl.formInited = false;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.id = params.id != null ? params.id : 0;
            ctrl.mode = ctrl.id != 0 ? "edit" : "add";

            customerFieldsService.getFormData().then(function (data) {
                if (data != null) {
                    ctrl.fieldTypes = data.fieldTypes;
                    ctrl.fieldValues = [];
                    if (ctrl.mode == "add") {
                        ctrl.fieldType = ctrl.fieldTypes.length > 0 ? ctrl.fieldTypes[1] : ctrl.fieldTypes[0];
                        ctrl.sortOrder = 0;
                        ctrl.enabled = true;
                        ctrl.fieldValues.push({});
                        ctrl.formInited = true;
                    } else {
                        ctrl.getCustomerField(ctrl.id);
                    }
                }
            });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
        
        ctrl.getCustomerField = function (id) {
            customerFieldsService.getCustomerField(id).then(function (data) {
                if (data != null) {
                    ctrl.name = data.Name;
                    ctrl.fieldType = $filter('filter')(ctrl.fieldTypes, { value: data.FieldType }, true)[0];
                    ctrl.sortOrder = data.SortOrder;
                    ctrl.required = data.Required;
                    ctrl.showInClient = data.ShowInClient;
                    ctrl.enabled = data.Enabled;
                    ctrl.fieldValues = data.FieldValues;
                }
                ctrl.fieldValues.push({});
                ctrl.form.$setPristine();
                ctrl.formInited = true;
            });
        }

        ctrl.save = function () {

            ctrl.btnSleep = true;

            var params = {
                id: ctrl.id,
                name: ctrl.name,
                fieldType: ctrl.fieldType.value,
                sortOrder: ctrl.sortOrder,
                required: ctrl.required,
                showInClient: ctrl.showInClient,
                enabled: ctrl.enabled,
                fieldValues: ctrl.fieldValues
            };

            customerFieldsService.addOrUpdateCustomerField(ctrl.mode == "add", params).then(function (data) {
                if (data.result == true) {
                    toaster.pop("success", "", ctrl.mode == "add" ? "Поле добавлено" : "Изменения сохранены");
                    $uibModalInstance.close('saveCustomerField');
                } else {
                    toaster.pop("error", "Ошибка", "Ошибка при " + (ctrl.mode == "add" ? "создании" : "редактировании"));
                    ctrl.btnSleep = false;
                }
            });
        }

        ctrl.sortableFieldValues = {
            orderChanged: function (event) {
                ctrl.form.modified = true;
            }
        };

        ctrl.addFieldValue = function (fieldValue, value) {
            ctrl.focusOnValue = false;
            $timeout(function () {
                ctrl.focusOnValue = true;
            }, 0);
            if (value == null || value == '') {
                return;
            }
            fieldValue.Id = 0;
            fieldValue.Value = value;
            ctrl.fieldValues.push({});
        }

        ctrl.deleteFieldValue = function (fieldValue) {
            ctrl.fieldValues = $filter('filter')(ctrl.fieldValues, function (value) { return value !== fieldValue; });
        }
    };

    ModalAddEditCustomerFieldCtrl.$inject = ['$uibModalInstance', '$filter', '$timeout', 'customerFieldsService', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditCustomerFieldCtrl', ModalAddEditCustomerFieldCtrl);

})(window.angular);