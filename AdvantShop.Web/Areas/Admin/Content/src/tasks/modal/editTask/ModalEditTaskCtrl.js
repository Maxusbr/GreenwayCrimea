; (function (ng) {
    'use strict';

    var ModalEditTaskCtrl = function ($uibModalInstance, $location, $uibModal, SweetAlert, toaster, Upload, tasksService, $q) {
        var ctrl = this,
            deferred = $q.defer();
        ctrl.formInited = false;

        ctrl.ckeditor = {
            height: 150,
            extraPlugins: 'clicklink,codemirror,lineheight,autolink,autogrow',
            bodyClass: 'm-n textarea-padding',
            toolbar: {},
            toolbarGroups: {},
            removePlugins: 'toolbar,elementspath',
            resize_enabled: false,
            toolbar_emptyToolbar: { name: 'empty', items: [] },
            autoGrow_minHeight: 233,
            autoGrow_onStartup: true,
            on: {
                blur: function (event) {
                    //ctrl.editTaskDescription(event, false);
                    deferred.resolve();
                }
            }
        }

        ctrl.$onInit = function () {
            //$location.search('grid', null);
            $location.search('modal', ctrl.$resolve.id);

            tasksService.getFormData().then(function (data) {
                if (data != null) {
                    ctrl.managers = data.managers;
                    ctrl.taskGroups = data.taskGroups;
                    ctrl.priorities = data.priorities;
                    ctrl.filesHelpText = data.filesHelpText;
                }
                tasksService.getTask(ctrl.$resolve.id).then(function (result) {
                    if (result.result === false)
                        ctrl.close();

                    ctrl.id = result.Id;
                    ctrl.name = result.Name;
                    ctrl.dueDate = result.DueDate;
                    ctrl.description = result.Description;
                    ctrl.assignedManagerId = result.AssignedManagerId;
                    ctrl.appointedManagerId = result.AppointedManagerId;
                    ctrl.taskGroupId = result.TaskGroupId;
                    ctrl.priority = result.Priority;
                    ctrl.dateCreated = result.DateCreated;
                    ctrl.dateCreatedFormatted = result.DateCreatedFormattedFull;
                    ctrl.status = result.StatusString;
                    ctrl.accepted = result.Accepted;
                    ctrl.orderId = result.OrderId;
                    ctrl.orderNumber = result.OrderNumber;
                    ctrl.leadId = result.LeadId;
                    ctrl.reviewId = result.ReviewId;
                    ctrl.clientCustomerId = result.ClientCustomerId;
                    ctrl.clientName = result.ClientName;
                    //ctrl.canDelete = result.CanDelete;
                    ctrl.result = result.ResultFull;

                    ctrl.editTaskForm.$setPristine();
                    ctrl.formInited = true;
                });
            });

            tasksService.getTaskAttachments(ctrl.$resolve.id).then(function (result) {
                ctrl.attachments = result;
            });
        };

        ctrl.dismiss = function () {
            $location.search('modal', null);
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.close = function () {
            $location.search('modal', null);
            $uibModalInstance.close();
        };

        ctrl.changeStatus = function (status) {
            tasksService.changeTaskStatus(ctrl.id, status).then(function (response) {
                ctrl.status = response.status;
                if (ctrl.accepted)
                    ctrl.accepted = false;
                toaster.pop('success', 'Статус изменен');
            });
        }

        ctrl.completeTask = function () {
            $uibModal.open({
                bindToController: true,
                controller: 'ModalCompleteTaskCtrl',
                controllerAs: 'ctrl',
                templateUrl: '../areas/admin/content/src/tasks/modal/completeTask/completeTask.html',
                resolve: {
                    task: {
                        id: ctrl.id,
                        name: ctrl.name,
                        leadId: ctrl.leadId,
                        orderId: ctrl.orderId
                    }
                }
            }).result.then(function (result) {
                if (result === true) {
                    toaster.pop('success', 'Изменения сохранены');
                    ctrl.close();
                } else {
                    toaster.pop('error', 'Не удалось завершить задачу');
                }
                return result;
            }, function (result) {
                return result;
            });
        }

        ctrl.acceptTask = function () {
            tasksService.acceptTask(ctrl.id).then(function (response) {
                //ctrl.accepted = true;
                ctrl.close();
            });
        }

        ctrl.deleteTask = function () {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    tasksService.deleteTask(ctrl.id).then(function (response) {
                        ctrl.close();
                    });
                }
            });
        }

        ctrl.saveTask = function () {

            setTimeout(function () {  // задержка нужна для ckeditor чтоб проинициализировались данные

                var objTask = {
                    id: ctrl.id,
                    name: ctrl.name,
                    assignedManagerId: ctrl.assignedManagerId,
                    appointedManagerId: ctrl.appointedManagerId,
                    dueDate: ctrl.dueDate,
                    description: ctrl.description,
                    taskGroupId: ctrl.taskGroupId,
                    priority: ctrl.priority,
                    status: ctrl.status,
                    accepted: ctrl.accepted,
                    resultFull: ctrl.result
                };
                if (deferred.promise.$$state.status === 0) {
                    ctrl.btnSleep = true;
                    tasksService.editTask(objTask).then(function (response) {
                        ctrl.close();
                    });
                } else {
                    deferred.promise.then(function (e) {
                        ctrl.btnSleep = true;
                        tasksService.editTask(objTask).then(function (response) {
                            ctrl.close();
                        });
                    });
                }


            }, 300)


        };

        ctrl.uploadAttachment = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
            ctrl.loadingFiles = true;
            if (($event.type === 'change' || $event.type === 'drop') && $files != null && $files.length > 0) {

                Upload.upload({
                    url: 'tasks/uploadAttachments',
                    data: {
                        taskId: ctrl.$resolve.id,
                    },
                    file: $files,
                }).then(function (response) {
                    var data = response.data;
                    for (var i in response.data) {
                        if (data[i].Result === true) {
                            ctrl.attachments.push(data[i].Attachment);
                            toaster.pop('success', 'Файл "' + data[i].Attachment.FileName + '" добавлен');
                        }
                        else {
                            toaster.pop('error', 'Ошибка при загрузке', (data[i].Attachment != null ? data[i].Attachment.FileName + ": " : "") + data[i].Error);
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

        ctrl.deleteAttachment = function (id) {
            SweetAlert.confirm("Вы уверены, что хотите удалить файл?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    tasksService.deleteAttachment(id, ctrl.$resolve.id).then(function (response) {
                        tasksService.getTaskAttachments(ctrl.$resolve.id).then(function (result) {
                            ctrl.attachments = result;
                        });
                    });
                }
            });
        }

        //ctrl.editTaskName = function (edit, revert) {
        //    ctrl.editName = edit;
        //    if (edit) {
        //        ctrl.namePrev = ctrl.name;
        //    }
        //    if (revert) {
        //        ctrl.name = ctrl.namePrev;
        //    }
        //}

        //ctrl.editTaskDescription = function (e, edit, revert) {
        //    if (edit && e.target.tagName === 'A')
        //        return;
        //    ctrl.editDescription = edit;
        //    if (edit) {
        //        ctrl.descPrev = ctrl.description;
        //    }
        //    if (revert) {
        //        ctrl.description = ctrl.descPrev;
        //    }
        //}
    };

    ModalEditTaskCtrl.$inject = ['$uibModalInstance', '$location', '$uibModal', 'SweetAlert', 'toaster', 'Upload', 'tasksService', '$q'];

    ng.module('uiModal')
        .controller('ModalEditTaskCtrl', ModalEditTaskCtrl);

})(window.angular);