; (function (ng) {
    'use strict';

    var TasksCtrl = function (
        $cookies,
        $location,
        $q,
        $rootScope,
        $uibModal,
        $window,
        adminWebNotificationsEvents,
        adminWebNotificationsService,
        lastStatisticsService,
        SweetAlert,
        tasksService,
        toaster,
        uiGridConstants,
        uiGridCustomConfig,
        uiGridCustomParamsConfig,
        uiGridCustomService,
        urlHelper) {

        var ctrl = this;

        ctrl.init = function (useKanban, onlyMy, prefilter, taskGroupId) {
            ctrl.useKanban = useKanban;
            ctrl.prefilter = prefilter || '';
            ctrl.taskGroupId = taskGroupId;

            ctrl.gridParams = {};
            if (!ctrl.useKanban)
                ctrl.gridParams.filterby = prefilter;
            if (taskGroupId)
                ctrl.gridParams.TaskGroupId = taskGroupId;
            if (onlyMy) {
                ctrl.onlyMy = ctrl.gridParams.OnlyMy = true;
            }

            ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
                enableExpandAll: false,
                columnDefs: ctrl.getColumnDefs(useKanban, onlyMy, prefilter, taskGroupId),
                uiGridCustom: {
                    rowClick: function ($event, row) {
                        ctrl.loadTask(row.entity.Id);
                    },
                    groupByField: ctrl.taskGroupId == null ? 'TaskGroupName' : null,
                    selectionOptions: [
                        {
                            text: 'Удалить выделенные',
                            url: 'tasks/deletetasks',
                            field: 'Id',
                            before: function () {
                                return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                    return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                                });
                            }
                        },
                        {
                            template:
                                '<ui-modal-trigger data-controller="\'ModalChangeTaskStatusesCtrl\'" controller-as="ctrl" data-resolve=\"{params:$ctrl.getSelectedParams(\'Id\'), canAccept:' + (ctrl.prefilter == 'completed') + '}\" ' +
                                    'template-url="../areas/admin/content/src/tasks/modal/changeTaskStatuses/changeTaskStatuses.html" ' +
                                    'data-on-close="$ctrl.gridOnAction()">' +
                                    'Изменить статус выделенным</ui-modal-trigger>'
                        },
                        {
                            text: 'Пометить просмотренными',
                            url: 'tasks/markviewed',
                            field: 'Id'
                        },
                        {
                            text: 'Пометить непросмотренными',
                            url: 'tasks/marknotviewed',
                            field: 'Id'
                        }
                    ],
                    rowClasses: function (row) {
                        var classes = '';
                        if (!row.entity.Viewed || row.entity.NewCommentsCount > 0)
                            classes += 'ui-grid-custom-row-bold ';
                        //if (row.entity.Overdue)
                        //    classes += 'ui-grid-custom-row-red ';
                        //if (row.entity.InProgress)
                        //    classes += 'ui-grid-custom-row-blue ';
                        if (row.entity.Completed)
                            classes += 'ui-grid-custom-row-linethrough ';
                        return classes;
                    }
                }
            });
        }

        ctrl.getColumnDefs = function (useKanban, onlyMy, prefilter, taskGroupId) {

            // visible columns
            var columnDefs = [
                {
                    name: 'Viewed',
                    displayName: '',
                    width: 20,
                    cellTemplate: '<div class="ui-grid-cell-contents"><span ng-if="!row.entity.Viewed" class="fa fa-circle text-warning" title="Не просмотрена"> </span></div>',
                },
                {
                    name: 'Id',
                    displayName: '№',
                    width: 50
                },
                {
                    name: 'Name',
                    displayName: 'Задача',
                    cellTemplate: '<div class="ui-grid-cell-contents"><span class="link">{{COL_FIELD}}</span> <span ng-if="row.entity.NewCommentsCount > 0" class="badge badge-pink" title="Количество новых комментариев">{{row.entity.NewCommentsCount}}</span></div>',
                },
                {
                    name: 'PriorityFormatted',
                    displayName: 'Приоритет',
                    width: 110,
                },
                {
                    name: 'DueDateFormatted',
                    displayName: 'Крайний срок',
                    width: 130,
                    cellTemplate: '<div class="ui-grid-cell-contents" ng-class="{\'ui-grid-cell-red\': row.entity.Overdue}">{{COL_FIELD}}</div>'
                },
                {
                    name: 'StatusFormatted',
                    displayName: 'Статус',
                    width: 110,
                    cellTemplate: '<div class="ui-grid-cell-contents" ng-class="{\'ui-grid-cell-blue\': row.entity.InProgress}">{{COL_FIELD}}</div>',
                },
                {
                    name: 'AssignedName',
                    displayName: 'Исполнитель',
                    cellTemplate:
                        '<div class="ui-grid-cell-contents js-grid-not-clicked">' +
                        '<sidebar-user-trigger customer-id="row.entity.AssignedCustomerId" ng-if="row.entity.AssignedCustomerId != null" class="ui-grid-cell-contents">' +
                        '<div class="ui-grid-cell-avatar" ng-if="row.entity.AssignedCustomerAvatarSrc != null"><img ng-src="{{row.entity.AssignedCustomerAvatarSrc}}"/></div>' +
                        '<a href="">{{COL_FIELD}}</a>' +
                        '</sidebar-user-trigger>' +
                        '</div>',
                    width: 200,
                },
                {
                    name: 'AppointedName',
                    displayName: 'Постановщик',
                    cellTemplate:
                        '<div class="ui-grid-cell-contents js-grid-not-clicked">' +
                        '<sidebar-user-trigger customer-id="row.entity.AppointedCustomerId" ng-if="row.entity.AppointedCustomerId != null" class="ui-grid-cell-contents">' +
                        '<div class="ui-grid-cell-avatar" ng-if="row.entity.AppointedCustomerAvatarSrc != null"><img ng-src="{{row.entity.AppointedCustomerAvatarSrc}}"/></div>' +
                        '<a href="">{{COL_FIELD}}</a>' +
                        '</sidebar-user-trigger>' +
                        '</div>',
                    width: 200,
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 65,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents ui-grid-custom-ignore-row-style js-grid-not-clicked"><div>' +
                        '<ui-modal-trigger controller="\'ModalEditTaskCtrl\'" controller-as="ctrl" on-dismiss="grid.appScope.$ctrl.gridExtendCtrl.modalDismiss()" on-close="grid.appScope.$ctrl.gridExtendCtrl.modalClose()" size="lg" backdrop="static" \
                                            template-url="../areas/admin/content/src/tasks/modal/editTask/editTask.html" resolve="{\'id\': row.entity.Id}" modal-id="{{row.entity.Id}}"> \
                            <a href="javascript:void(0);" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a> \
                        </ui-modal-trigger>' +
                        '<ui-grid-custom-delete url="tasks/deletetask" params="{\'Id\': row.entity.Id}"></ui-grid-custom-delete></div></div>' //  ng-show="row.entity.CanDelete"
                }];

            // filters
            if (taskGroupId == null) {
                columnDefs.push({
                    name: '_noopColumnTaskGroups',
                    visible: false,
                    filter: {
                        placeholder: 'Проект',
                        type: uiGridConstants.filter.SELECT,
                        name: 'TaskGroupId',
                        fetch: 'taskgroups/getTaskGroupsSelectOptions'
                    }
                });
            }
            columnDefs.push.apply(columnDefs,
                    [{
                        name: '_noopColumnDateCreated',
                        visible: false,
                        filter: {
                            placeholder: 'Создана',
                            type: 'datetime',
                            term: {
                                from: (new Date()).setMonth((new Date()).getMonth() - 1),
                                to: new Date()
                            },
                            datetimeOptions: {
                                from: { name: 'DateCreatedFrom' },
                                to: { name: 'DateCreatedTo' }
                            }
                        }
                    },
                    {
                        name: '_noopColumnPriorities',
                        visible: false,
                        filter: {
                            placeholder: 'Приоритет',
                            type: uiGridConstants.filter.SELECT,
                            name: 'Priority',
                            fetch: 'tasks/getTaskPrioritiesSelectOptions'
                        }
                    },
                    {
                        name: '_noopColumnDueDateFormatted',
                        visible: false,
                        filter: {
                            placeholder: 'Крайний срок',
                            type: 'datetime',
                            term: {
                                from: (new Date()).setMonth((new Date()).getMonth() - 1),
                                to: new Date()
                            },
                            datetimeOptions: {
                                from: { name: 'DueDateFrom' },
                                to: { name: 'DueDateTo' }
                            }
                        }
                    },
                    {
                        name: '_noopColumnViewed',
                        visible: false,
                        filter: {
                            placeholder: 'Просмотрена',
                            name: 'Viewed',
                            type: uiGridConstants.filter.SELECT,
                            selectOptions: [{ label: 'Да', value: 'true' }, { label: 'Нет', value: 'false' }]
                        }
                    }]);

            if (!onlyMy && prefilter != 'assignedtome') {
                columnDefs.push({
                    name: '_noopColumnAssigned',
                    visible: false,
                    filter: {
                        placeholder: 'Исполнитель',
                        type: uiGridConstants.filter.SELECT,
                        name: 'AssignedManagerId',
                        fetch: 'managers/getManagersSelectOptions'
                    }
                });
            }
            if (prefilter != 'appointedbyme') {
                columnDefs.push({
                    name: '_noopColumnAppointed',
                    visible: false,
                    filter: {
                        placeholder: 'Постановщик',
                        type: uiGridConstants.filter.SELECT,
                        name: 'AppointedManagerId',
                        fetch: 'managers/getManagersSelectOptions'
                    }
                });
            }
            if (!useKanban && prefilter != 'completed' && prefilter != 'accepted') {
                columnDefs.push({
                    name: '_noopColumnStatuses',
                    visible: false,
                    filter: {
                        placeholder: 'Статус',
                        type: uiGridConstants.filter.SELECT,
                        name: 'Status',
                        fetch: prefilter == '' || prefilter == 'none' || prefilter == 'assignedtome'
                            ? 'tasks/getNotCompletedTaskStatusesSelectOptions'
                            : 'tasks/getTaskStatusesSelectOptions'
                    }
                });
            }
            //if (prefilter == 'completed') {
            //    ctrl.gridOptions.uiGridCustom.selectionOptions.push({
            //        text: 'Принять выделенные',
            //        url: 'tasks/accepttasks',
            //        field: 'Id'
            //    });
            //}

            return columnDefs;
        }

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };

        ctrl.fetchData = function (ignoreHistory) {
            if (!ctrl.useKanban) {
                ctrl.grid.fetchData(ignoreHistory);
            } else {
                ctrl.kanban.fetchData();
            }
        };

        ctrl.refresh = function (reload) {
            if (reload) {
                ctrl.reload();
            } else {
                ctrl.fetchData();
            }
        }

        ctrl.reload = function (query) {
            var loc = $window.location.href.split('#')[0];
            $window.location.href = loc.split('?')[0] + (query || '');
        };

        ctrl.modalDismiss = ctrl.modalClose = function () {
            ctrl.fetchData();
            $location.search('modal', null);
            $location.search('modalShow', null);
        };

        ctrl.changeView = function (view) {
            ctrl.setCookie('tasks_viewmode', view);
            ctrl.reload(view == 'grid' && ctrl.onlyMy ? '?filterby=assignedtome' : '');
        }

        ctrl.toggleMy = function (onlyMy) {
            ctrl.gridParams.OnlyMy = onlyMy;
            ctrl.setCookie('tasks_mykanban', onlyMy);

            ctrl.gridOptions.columnDefs.length = 0;
            ctrl.gridOptions.columnDefs.push.apply(ctrl.gridOptions.columnDefs, ctrl.getColumnDefs(ctrl.useKanban, onlyMy, ctrl.prefilter, ctrl.taskGroupId));

            ctrl.kanbanFilter.updateColumns();
            ctrl.fetchData();
        }

        ctrl.setCookie = function (name, value) {
            var date = new Date();
            date.setFullYear(date.getFullYear() + 1)
            $cookies.put(name, value, { expires: date });
        }

        ctrl.loadTask = function (id) {
            $uibModal.open({
                animation: false,
                bindToController: true,
                controller: 'ModalEditTaskCtrl',
                controllerAs: 'ctrl',
                templateUrl: '../areas/admin/content/src/tasks/modal/editTask/editTask.html',
                resolve: {
                    id: function () {
                        return id;
                    }
                },
                size: 'lg',
                backdrop: 'static'
            }).result.then(function (result) {
                ctrl.modalClose();
                return result;
            }, function (result) {
                ctrl.modalDismiss();
                return result;
            });
        }

        ctrl.newTask = function () {
            $uibModal.open({
                animation: false,
                bindToController: true,
                controller: 'ModalAddTaskCtrl',
                controllerAs: 'ctrl',
                templateUrl: '../areas/admin/content/src/_shared/modal/addTask/addTask.html',
                size: 'lg',
                backdrop: 'static'
            }).result.then(function (result) {
                ctrl.refresh(!ctrl.useKanban);
                return result;
            }, function (result) {
                $location.search('modalNewTask', null);
                return result;
            });
        }


        /************ Kanban  +  filter **************/

        ctrl.sortableOptions = {
            containment: '#kanban',
            containerPositioning: 'relative',
            additionalPlaceholderClass: 'kanban__placeholder',
            itemMoved: function (event) {
                var status;
                switch (event.dest.sortableScope.$parent.column.Id) {
                    case 'New':
                        status = 'Open';
                        break;
                    case 'InProgress':
                        status = 'InProgress';
                        break;
                    case 'Done':
                        status = 'Completed';
                        break;
                    //case 'Accepted':
                    //    tasksService.completeTask(event.source.itemScope.modelValue.Id).then(function (data) {
                    //        if (data != null && data.result === true) {
                    //            lastStatisticsService.getLastStatistics();
                    //        }
                    //    });
                    //    break;
                    default:
                        throw ('no status to change');
                }
                if (status) {
                    tasksService.changeTaskStatus(event.source.itemScope.modelValue.Id, status).then(function (response) {
                        toaster.pop('success', 'Статус изменен');
                    });
                }
                event.source.sortableScope.$parent.column.TotalCardsCount -= 1;
                event.dest.sortableScope.$parent.column.TotalCardsCount += 1;

                ctrl.onOrderChanged(event);
            },
            orderChanged: function (event) {
                ctrl.onOrderChanged(event, true);
            }
        };

        ctrl.onOrderChanged = function (event, showMessage) {
            var taskId = event.source.itemScope.card.Id,
                prev = event.dest.sortableScope.modelValue[event.dest.index - 1],
                next = event.dest.sortableScope.modelValue[event.dest.index + 1];
            tasksService.changeSorting(taskId, prev != null ? prev.Id : null, next != null ? next.Id : null).then(function (data) {
                if (showMessage && data != null && data.result === true) {
                    toaster.pop('success', '', 'Изменения сохранены');
                }
            });
        };

        ctrl.kanbanOnInit = function (kanban) {
            ctrl.kanban = kanban;
        };

        ctrl.kanbanOnFilterInit = function (filter) {
            ctrl.kanbanFilter = filter;
        };

        ctrl.$onInit = function () {

            var locationSearch = $location.search();

            if (locationSearch != null) {

                if (locationSearch.modal != null) {
                    ctrl.loadTask(locationSearch.modal);
                } else if (locationSearch.modalNewTask) {
                    ctrl.newTask();
                }
            }

            $rootScope.$on('$locationChangeSuccess', function (event, newUrl, oldUrl) {
                if (newUrl !== oldUrl && $location.search() != null && $location.search().modal != null && $location.search().modalShow != null) {
                    ctrl.loadTask($location.search().modal);
                }
            });

            adminWebNotificationsService.addListener(adminWebNotificationsEvents.updateTasks, function () { ctrl.fetchData(true); });
        };

    };

    TasksCtrl.$inject = [
        '$cookies',
        '$location',
        '$q',
        '$rootScope',
        '$uibModal',
        '$window',
        'adminWebNotificationsEvents',
        'adminWebNotificationsService',
        'lastStatisticsService',
        'SweetAlert',
        'tasksService',
        'toaster',
        'uiGridConstants',
        'uiGridCustomConfig',
        'uiGridCustomParamsConfig',
        'uiGridCustomService',
        'urlHelper'];

    ng.module('tasks', ['uiGridCustom', 'adminComments', 'urlHelper'])
      .controller('TasksCtrl', TasksCtrl);

})(window.angular);