; (function (ng) {
    'use strict';

    var ModalAddEditFilterRuleCtrl = function ($http, $filter, $uibModalInstance, SweetAlert, toaster) {
        var ctrl = this;
        ctrl.formInited = false;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            if (params.event != null) {
                ctrl.eventType = params.event.type;
            }
            ctrl.mode = params.rule != null ? "edit" : "add";

            ctrl.getFormData().then(function () {
                if (ctrl.mode == "add") {
                    ctrl.resetComparers();
                } else {
                    ctrl.field = $filter('filter')(ctrl.fields, function(item) {
                        return item.type == params.rule.FieldType && (params.rule.FieldComparer.FieldObjId == null || item.objId == params.rule.FieldComparer.FieldObjId)
                    })[0];
                    ctrl.setParamValues(ctrl.field.type);
                    ctrl.fieldComparer = {
                        Type: params.rule.FieldComparer.Type,
                    };
                    switch (ctrl.fieldComparer.Type) {
                        case 1:
                        case 4:
                            ctrl.fieldComparer.Value = params.rule.FieldComparer.Value;
                            break;
                        case 2:
                            ctrl.fieldComparer.From = params.rule.FieldComparer.From;
                            ctrl.fieldComparer.To = params.rule.FieldComparer.To;
                            ctrl.fieldComparer.DateFrom = params.rule.FieldComparer.DateFromString;
                            ctrl.fieldComparer.DateTo = params.rule.FieldComparer.DateToString;
                            break;
                        case 3:
                            ctrl.fieldComparer.Flag = params.rule.FieldComparer.Flag;
                            break;
                    }
                    ctrl.addEditRuleForm.$setPristine();
                }
                ctrl.formInited = true;
            });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.apply = function () {
            var result = {
                FieldType: ctrl.field.type,
                FieldName: ctrl.field.name,
                FieldComparer: {
                    Type: ctrl.fieldComparer.Type,
                    FieldObjId: ctrl.field.objId 
                }
            };
            switch (ctrl.fieldComparer.Type) {
                case 1: // equal
                case 4: // contains
                    result.FieldComparer.Value = ctrl.fieldComparer.Value;
                    if (!ctrl.paramValuesEmpty()) {
                        var selected = $filter('filter')(ctrl.compareValues, function (item) { return item.value === ctrl.fieldComparer.Value; })[0];
                        result.FieldValueObjectName = selected != null ? selected.label : null;
                        if (ctrl.field.objId == null) {
                            result.FieldComparer.ValueObjId = ctrl.fieldComparer.Value;
                        }
                    }
                    break;
                case 2: // range
                    if (ctrl.field.fieldType == 'date') {
                        result.FieldComparer.DateFrom = result.FieldComparer.DateFromString = ctrl.fieldComparer.DateFrom;
                        result.FieldComparer.DateTo = result.FieldComparer.DateToString = ctrl.fieldComparer.DateTo;
                    } else {
                        result.FieldComparer.From = ctrl.fieldComparer.From;
                        result.FieldComparer.To = ctrl.fieldComparer.To;
                    }
                    break;
                case 3: // flag
                    result.FieldComparer.Flag = ctrl.fieldComparer.Flag;
                    break;
            }
            $uibModalInstance.close(result);
        }

        ctrl.resetComparers = function () {
            ctrl.fieldComparer = { Type: 1 };
            ctrl.compareValues = null;
        }

        ctrl.paramValuesEmpty = function () {
            return ctrl.compareValues == null || ctrl.compareValues.length == 0;
        }

        ctrl.setParamValues = function (field) {
            ctrl.resetComparers();
            if (field != null) {
                if (field.fieldType == 'checkbox') {
                    ctrl.fieldComparer.Type = 3;
                    ctrl.fieldComparer.Flag = true;
                } else if ((ctrl.eventType == 'LeadCreated' || ctrl.eventType == 'LeadStatusChanged') && field.type == 14) { // Contains пока только для описания лида
                    ctrl.fieldComparer.Type = 4;
                } else {
                    $http.get('bizprocessrules/getFilterRuleParamValues', { params: { eventType: ctrl.eventType, fieldType: ctrl.field.type, fieldObjId: ctrl.field.objId, rnd: Math.random() } }).then(function (response) {
                        var data = response.data;
                        ctrl.compareValues = data != null ? data.values : null;
                    });
                }
            }
        }

        ctrl.getFormData = function () {
            return $http.get('bizprocessrules/getFilterRuleFormData', { params: { eventType: ctrl.eventType, rnd: Math.random() } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.fields = data.fields;
                }
            });
        }
    };

    ModalAddEditFilterRuleCtrl.$inject = ['$http', '$filter', '$uibModalInstance', 'SweetAlert', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditFilterRuleCtrl', ModalAddEditFilterRuleCtrl);

})(window.angular);