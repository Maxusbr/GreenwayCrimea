; (function (ng) {
    'use strict';

    var ModalAddTaskCtrl = function ($uibModalInstance, $filter, Upload, toaster, tasksService, $window, lastStatisticsService) {
        var ctrl = this;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.redirectToTasks = params.redirectToTasks || false;
            if (params.bindTo != null) {
                switch (params.bindTo.type.toLowerCase()) {
                    case 'order':
                        ctrl.orderId = params.bindTo.objId;
                        break;
                    case 'lead':
                        ctrl.leadId = params.bindTo.objId;
                        break;
                }
            }

            tasksService.getFormData().then(function (data) {
                if (data != null) {
                    ctrl.managers = data.managers;
                    ctrl.assignedManagerId = '';
                    ctrl.taskGroups = data.taskGroups;
                    ctrl.priorities = data.priorities;
                    ctrl.filesHelpText = data.filesHelpText;
                    ctrl.taskGroupId = params.taskGroupId || data.defaultTaskGroupId || '';
                    if (ctrl.priorities.length > 1) {
                        ctrl.priority = ctrl.priorities[1].value;
                    } else if (ctrl.priorities.length > 0) {
                        ctrl.priority = ctrl.priorities[0].value;
                    }
                }

                if (ctrl.$resolve != null && ctrl.$resolve.userData != null) {
                    ctrl.assignedManagerId = ctrl.$resolve.userData.assignedManager;
                }

            });

            ctrl.name = "";
            //ctrl.dueDate = new Date();
            ctrl.attachments = [];
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.addAttachments = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {

            ctrl.loadingFiles = true;
            if (($event.type === 'change' || $event.type === 'drop') && $files != null && $files.length > 0) {
                for (var i in ctrl.attachments) {
                    if ($filter('filter')($files, { name: ctrl.attachments[i].name }, true)[0] != null) {
                        toaster.pop('error', 'Ошибка', ctrl.attachments[i].name + ": файл уже существует");
                        $files = $filter('filter')($files, function (file) { return file.name !== ctrl.attachments[i].name; });
                        ctrl.loadingFiles = false;
                        return;
                    }
                }
                Upload.upload({
                    url: 'tasks/validateAttachments',
                    data: {},
                    file: $files,
                }).then(function (response) {
                    var data = response.data;
                    for (var i in response.data) {
                        if (data[i].Result === true) {
                            ctrl.attachments.push($filter('filter')($files, { name: data[i].Attachment.FileName }, true)[0]);
                        }
                        else {
                            toaster.pop('error', 'Ошибка', (data[i].Attachment != null ? data[i].Attachment.FileName + ": " : "") + data[i].Error);
                        }
                    }
                    ctrl.loadingFiles = false;
                });
            } else if ($invalidFiles.length > 0) {
                toaster.pop('error', 'Ошибка при загрузке', 'Файл не соответствует требованиям');
                ctrl.loadingFiles = false;
            }
            else {
                ctrl.loadingFiles = false;
            }
        };

        ctrl.deleteAttachment = function (name) {
            ctrl.attachments = $filter('filter')(ctrl.attachments, function (file) { return file.name !== name; });
        }

        ctrl.addTask = function () {

            if (ctrl.name == "") return;

            Upload.upload({
                url: 'tasks/addTask',
                data: {
                    name: ctrl.name,
                    assignedManagerId: ctrl.assignedManagerId,
                    dueDate: ctrl.dueDate,
                    description: ctrl.description,
                    taskGroupId: ctrl.taskGroupId,
                    priority: ctrl.priority,
                    orderId: ctrl.orderId,
                    leadId: ctrl.leadId
                },
                file: ctrl.attachments,
            }).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', 'Задача добавлена');

                    if (ctrl.redirectToTasks) {
                        $window.location.assign('tasks');
                    }

                    $uibModalInstance.close();

                    lastStatisticsService.getLastStatistics();
                } else {
                    toaster.pop('error', 'Ошибка', data.Error);
                }
            });
        };
    };

    ModalAddTaskCtrl.$inject = ['$uibModalInstance', '$filter', 'Upload', 'toaster', 'tasksService', '$window', 'lastStatisticsService'];

    ng.module('uiModal')
        .controller('ModalAddTaskCtrl', ModalAddTaskCtrl);

})(window.angular);