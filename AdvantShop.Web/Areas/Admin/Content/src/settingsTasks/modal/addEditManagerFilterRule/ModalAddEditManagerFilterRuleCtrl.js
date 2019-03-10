; (function (ng) {
    'use strict';

    /*
        filterType: 0 - Any, 1 - MostFree, 2 - Specific, 3 - FromBizObject
    */

    var ModalAddEditManagerFilterRuleCtrl = function ($http, $filter, $uibModalInstance, SweetAlert, toaster) {
        var ctrl = this;
        ctrl.formInited = false;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            if (params.event != null) {
                ctrl.eventType = params.event.type;
            }
            ctrl.mode = params.rule != null ? "edit" : "add";
            ctrl.filterType = {};

            ctrl.getFormData().then(function () {
                if (ctrl.mode == "add") {

                } else {
                    ctrl.filterType.label = params.rule.FilterTypeName;
                    ctrl.filterType.value = ctrl.filterType.type = params.rule.FilterType;
                    ctrl.city = params.rule.City;
                    if (params.rule.FilterType == 2) {
                        ctrl.filterType.value = params.rule.CustomerId;
                    }
                    if (params.rule.ManagerRoleId != null) {
                        ctrl.managerRole = {
                            value: params.rule.ManagerRoleId,
                            label: params.rule.ManagerRoleName
                        }
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
                FilterType: ctrl.filterType.type,
                FilterTypeName: ctrl.filterType.label
            };
            if (ctrl.filterType.type == 2) {
                result.CustomerId = ctrl.filterType.value;
                result.CustomerName = ctrl.filterType.label;
            }
            else {
                result.City = ctrl.city;
                if (ctrl.managerRole != null) {
                    result.ManagerRoleId = ctrl.managerRole.value;
                    result.ManagerRoleName = ctrl.managerRole.label;
                }
            }

            $uibModalInstance.close(result);
        }

        ctrl.getFormData = function () {
            return $http.get('bizprocessrules/getManagerFilterRuleFormData', { params: { eventType: ctrl.eventType, rnd: Math.random() } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.filterTypes = data.managerFilterTypes;
                    ctrl.managerRoles = data.managerRoles;
                }
            });
        }
    };

    ModalAddEditManagerFilterRuleCtrl.$inject = ['$http', '$filter', '$uibModalInstance', 'SweetAlert', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditManagerFilterRuleCtrl', ModalAddEditManagerFilterRuleCtrl);

})(window.angular);