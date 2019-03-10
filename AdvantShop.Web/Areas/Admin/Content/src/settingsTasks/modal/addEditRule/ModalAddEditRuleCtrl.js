; (function (ng) {
    'use strict';

    var ModalAddEditRuleCtrl = function ($http, $filter, $uibModalInstance, SweetAlert, toaster) {
        var ctrl = this;
        ctrl.formInited = false;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            if (params.event != null) {
                ctrl.eventType = params.event.type;
            }
            ctrl.id = params.id;
            ctrl.mode = ctrl.id != null ? "edit" : "add";

            ctrl.getFormData().then(function () {
                if (ctrl.mode == "add") {
                    ctrl.priority = 0;
                    ctrl.showEventObject = ctrl.eventType == 'OrderStatusChanged' || ctrl.eventType == 'LeadStatusChanged';
                    ctrl.taskDueDateInterval = ctrl.intervalOrDefault(null, 'taskDueDateIntervalNotSet');
                    ctrl.taskCreateInterval = ctrl.intervalOrDefault(null, 'taskCreateIntervalNotSet');
                    ctrl.taskPriority = 1;
                    ctrl.managerFilter = {
                        Comparers: []
                    };
                    ctrl.filter = {
                        Comparers: []
                    };
                    ctrl.formInited = true;
                } else {
                    ctrl.showEventObject = false;
                    ctrl.getRule(ctrl.id);
                }
            });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.getRule = function (id) {
            $http.get('bizprocessrules/get', { params: { id: id, eventType: ctrl.eventType, rnd: Math.random() } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.eventName = data.EventName;
                    ctrl.eventObjId = data.EventObjId;
                    ctrl.taskDueDateInterval = ctrl.intervalOrDefault(data.TaskDueDateInterval, 'taskDueDateIntervalNotSet');
                    ctrl.taskCreateInterval = ctrl.intervalOrDefault(data.TaskCreateInterval, 'taskCreateIntervalNotSet');
                    ctrl.priority = data.Priority;
                    ctrl.taskName = data.TaskName;
                    ctrl.taskDescription = data.TaskDescription;
                    ctrl.taskPriority = data.TaskPriority;
                    ctrl.taskGroupId = data.TaskGroupId;
                    ctrl.managerFilter = data.ManagerFilter || { Comparers: [] };
                    ctrl.filter = data.Filter || { Comparers: [] };
                }
                ctrl.serializeManagerFilter();
                ctrl.serializeFilter();
                ctrl.addEditRuleForm.$setPristine();
                ctrl.formInited = true;
            });
        }

        ctrl.save = function () {

            ctrl.btnSleep = true;
            ctrl.serializeManagerFilter();
            ctrl.serializeFilter();
            var params = {
                id: ctrl.id,
                eventType: ctrl.eventType,
                eventObjId: ctrl.eventObject != null ? ctrl.eventObject.value : ctrl.eventObjId,
                taskDueDateIntervalSerialized: !ctrl.taskDueDateIntervalNotSet && ctrl.taskDueDateInterval.Interval != null
                    ? JSON.stringify(ctrl.taskDueDateInterval) : null,
                taskCreateIntervalSerialized: !ctrl.taskCreateIntervalNotSet && ctrl.taskCreateInterval.Interval != null
                    ? JSON.stringify(ctrl.taskCreateInterval) : null,
                priority: ctrl.priority,
                taskName: ctrl.taskName,
                taskDescription: ctrl.taskDescription,
                taskPriority: ctrl.taskPriority,
                taskGroupId: ctrl.taskGroupId,
                managerFilterSerialized: ctrl.managerFilterSerialized,
                filterSerialized: ctrl.filterSerialized,
                rnd: Math.random()
            };

            var url = ctrl.mode == "add" ? 'bizprocessrules/add' : 'bizprocessrules/update';

            $http.post(url, params).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop("success", "", ctrl.mode == "add" ? "Правило добавлено" : "Изменения сохранены");
                    $uibModalInstance.close({ eventType: ctrl.eventType });
                } else {
                    toaster.pop("error", "Ошибка", data.error);
                    ctrl.btnSleep = false;
                }
            });
        }

        ctrl.processManagerFilterRule = function (prevRule, newRule) {
            if (newRule == null) {
                ctrl.managerFilter.Comparers = $filter('filter')(ctrl.managerFilter.Comparers, function (comparer) { return comparer !== prevRule; });
            } else if (prevRule == null) {
                ctrl.managerFilter.Comparers.push(newRule);
            } else {
                var ind = ctrl.managerFilter.Comparers.indexOf(prevRule);
                if (ind != -1) {
                    ctrl.managerFilter.Comparers[ind] = newRule;
                }
            }
            ctrl.serializeManagerFilter();
        }

        ctrl.processFilterRule = function (prevRule, newRule) {
            if (newRule == null) {
                ctrl.filter.Comparers = $filter('filter')(ctrl.filter.Comparers, function (comparer) { return comparer !== prevRule; });
            } else if (prevRule == null) {
                ctrl.filter.Comparers.push(newRule);
            } else {
                var ind = ctrl.filter.Comparers.indexOf(prevRule);
                if (ind != -1) {
                    ctrl.filter.Comparers[ind] = newRule;
                }
            }
            ctrl.serializeFilter();
        }

        ctrl.serializeManagerFilter = function () {
            ctrl.managerFilterSerialized = ctrl.managerFilter != null ? JSON.stringify(ctrl.managerFilter) : null;
        }

        ctrl.serializeFilter = function () {
            ctrl.filterSerialized = ctrl.filter != null ? JSON.stringify(ctrl.filter) : null;
        }

        ctrl.intervalOrDefault = function (value, flagNotSet) {
            ctrl[flagNotSet] = value == null;
            return value || { IntervalType: ctrl.intervalTypes[0].value };
        }

        ctrl.getFormData = function () {
            return $http.get('bizprocessrules/getruleformdata', { params: { eventType: ctrl.eventType, rnd: Math.random() } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.eventName = data.eventName;
                    ctrl.eventObjects = data.eventObjects;
                    ctrl.intervalTypes = data.intervalTypes;
                    ctrl.filterTypeName = data.filterTypeName;
                    ctrl.taskPriorities = data.taskPriorities;
                    ctrl.taskGroups = data.taskGroups;
                    ctrl.availableVariables = data.availableVariables;
                    ctrl.useFilter = data.useFilter;
                }
            });
        }

        ctrl.getManagerFilterHtml = function (comparer) {
            var result;
            if (comparer.FilterType == 2)
                result = comparer.CustomerName;
            else if (comparer.FilterType == 3) {
                switch (ctrl.eventType) {
                    case 'OrderCreated':
                    case 'OrderStatusChanged':
                        result = 'Менеджер заказа';
                        break;
                    case 'LeadCreated':
                    case 'LeadStatusChanged':
                        result = 'Менеджер лида';
                        break;
                    case 'MessageReply':
                        result = 'Менеджер покупателя';
                        break;
                }
            } else {
                result = comparer.FilterTypeName;
                var rules = [];
                if (comparer.ManagerRoleId != null)
                    rules.push('роль = ' + comparer.ManagerRoleName);
                if (comparer.City)
                    rules.push('город = ' + comparer.City);
                if (rules.length > 0) {
                    result += ' (' + rules.join(', ') + ')'
                }
            }
            return result;
        }
    };

    ModalAddEditRuleCtrl.$inject = ['$http', '$filter', '$uibModalInstance', 'SweetAlert', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditRuleCtrl', ModalAddEditRuleCtrl);

})(window.angular);