; (function (ng) {
    'use strict';

    var TasksGridCtrl = function ($location, $q, $rootScope, $uibModal, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, SweetAlert) {
        var ctrl = this,
            columnDefs = [
                {
                    name: '_noopColumnTaskGroups',
                    visible: false,
                    filter: {
                        placeholder: 'Проект',
                        type: uiGridConstants.filter.SELECT,
                        name: 'TaskGroupId',
                        fetch: 'taskgroups/getTaskGroupsSelectOptions'
                    }
                },
                {
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
                            from: {
                                name: 'DateCreatedFrom'
                            },
                            to: {
                                name: 'DateCreatedTo'
                            }
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
                    name: '_noopColumnAssigned',
                    visible: false,
                    filter: {
                        placeholder: 'Исполнитель',
                        type: uiGridConstants.filter.SELECT,
                        name: 'AssignedManagerId',
                        fetch: 'managers/getManagersSelectOptions'
                    }
                },
                {
                    name: '_noopColumnAppointed',
                    visible: false,
                    filter: {
                        placeholder: 'Постановщик',
                        type: uiGridConstants.filter.SELECT,
                        name: 'AppointedManagerId',
                        fetch: 'managers/getManagersSelectOptions'
                    }
                },
                {
                    name: '_noopColumnStatuses',
                    visible: false,
                    filter: {
                        placeholder: 'Статус',
                        type: uiGridConstants.filter.SELECT,
                        name: 'Status',
                        fetch: 'tasks/getTaskStatusesSelectOptions'
                    }
                },
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
                //{
                //    name: 'PriorityFormatted',
                //    displayName: 'Приоритет',
                //    width: 110,
                //},
                {
                    name: 'DueDateFormatted',
                    displayName: 'Крайний срок',
                    width: 130,
                    cellTemplate: '<div class="ui-grid-cell-contents" ng-class="{\'ui-grid-cell-red\': row.entity.Overdue}">{{COL_FIELD}}</span></div>',
                    filter: {
                        placeholder: 'Крайний срок',
                        type: 'datetime',
                        term: {
                            from: (new Date()).setMonth((new Date()).getMonth() - 1),
                            to: new Date()
                        },
                        datetimeOptions: {
                            from: {
                                name: 'DueDateFrom'
                            },
                            to: {
                                name: 'DueDateTo'
                            }
                        }
                    }
                },
                {
                    name: 'StatusFormatted',
                    displayName: 'Статус',
                    width: 110,
                    cellTemplate: '<div class="ui-grid-cell-contents" ng-class="{\'ui-grid-cell-blue\': row.entity.InProgress}">{{COL_FIELD}}</span></div>',
                },
                {
                    name: 'AssignedName',
                    displayName: 'Исполнитель',
                    cellTemplate: '<div class="ui-grid-cell-contents"><div class="ui-grid-cell-avatar" ng-if="row.entity.AssignedCustomerAvatarSrc != null"><img ng-src="{{row.entity.AssignedCustomerAvatarSrc}}"/></div><div>{{COL_FIELD}}</div></div>',
                    width: 150,
                },
                {
                    name: 'AppointedName',
                    displayName: 'Постановщик',
                    cellTemplate: '<div class="ui-grid-cell-contents"><div class="ui-grid-cell-avatar" ng-if="row.entity.AppointedCustomerAvatarSrc != null"><img ng-src="{{row.entity.AppointedCustomerAvatarSrc}}"/></div><div>{{COL_FIELD}}</div></div>',
                    width: 150,
                },
                {
                    name: '_noopColumnViewed',
                    visible: false,
                    filter: {
                        placeholder: 'Просмотрена',
                        name: 'Viewed',
                        type: uiGridConstants.filter.SELECT,
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
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
                        '<ui-grid-custom-delete url="tasks/deletetask" params="{\'Id\': row.entity.Id}"></ui-grid-custom-delete></div></div>'
                },
            ];

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            enableExpandAll: false,
            columnDefs: columnDefs,
            uiGridCustom: {
                rowClick: function ($event, row) {
                    ctrl.loadTask(row.entity.Id);
                },
                groupByField: 'TaskGroupName',
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
                            '<ui-modal-trigger data-controller="\'ModalChangeTaskStatusesCtrl\'" controller-as="ctrl" data-resolve=\"{params:$ctrl.getSelectedParams(\'Id\')}\" ' +
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
                    if (row.entity.Completed)
                        classes += 'ui-grid-custom-row-linethrough ';
                    return classes;
                }
            }
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.gridTasks = grid;
        };

        ctrl.modalDismiss = ctrl.modalClose = function () {
            ctrl.gridTasks.fetchData();
            $location.search('modal', null);
            $location.search('modalShow', null);
        };

        ctrl.loadTask = function (id) {
            $uibModal.open({
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

        ctrl.$onInit = function () {
            if ($location.search() != null && $location.search().modal != null) {
                ctrl.loadTask($location.search().modal);
            }

            $rootScope.$on('$locationChangeSuccess', function (event, newUrl, oldUrl) {
                if (newUrl !== oldUrl && $location.search() != null && $location.search().modal != null && $location.search().modalShow != null) {
                    ctrl.loadTask($location.search().modal);
                }
            });
        };
    };

    TasksGridCtrl.$inject = ['$location', '$q', '$rootScope', '$uibModal', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'SweetAlert'];

    ng.module('tasksGrid', [])
        .controller('TasksGridCtrl', TasksGridCtrl);

})(window.angular);