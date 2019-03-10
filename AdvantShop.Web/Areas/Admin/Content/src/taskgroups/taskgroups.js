; (function (ng) {
    'use strict';

    var TaskGroupsCtrl = function ($q, $location, $window, $http, $uibModal, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, SweetAlert) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'Name',
                    displayName: 'Проект',
                    cellTemplate: '<div class="ui-grid-cell-contents"><a href=\'projects/{{row.entity.Id}}\' class="link">{{COL_FIELD}}</span></div>'
                },
                {
                    name: 'SortOrder',
                    displayName: 'Порядок',
                    type: 'number',
                    enableCellEdit: true,
                    width: 150,
                    filter: {
                        placeholder: 'Сортировка',
                        type: 'range',
                        rangeOptions: {
                            from: {
                                name: 'SortingFrom'
                            },
                            to: {
                                name: 'SortingTo'
                            }
                        }
                    },
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 80,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents js-grid-not-clicked"><div>' +
                        '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil" ng-click="grid.appScope.$ctrl.gridExtendCtrl.loadTaskGroup(row.entity.Id)"></a> ' +
                        '<a href="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.deleteTaskGroup(row.entity.CanBeDeleted, row.entity.Id)" ' +
                                   'ng-class="(!row.entity.CanBeDeleted ? \'ui-grid-custom-service-icon fa fa-times link-disabled\' : \'ui-grid-custom-service-icon fa fa-times link-invert\')"></a> ' +
                        '</div></div>'
                }
            ];

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                rowUrl: 'projects/{{row.entity.Id}}',
                //rowClick: function ($event, row) {
                //    ctrl.loadTaskGroup(row.entity.Id);
                //},
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'taskgroups/deletetaskgroups',
                        field: 'Id',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    }
                ]
            }
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };

        ctrl.loadTaskGroup = function (id) {
            $uibModal.open({
                bindToController: true,
                controller: 'ModalAddEditTaskGroupCtrl',
                controllerAs: 'ctrl',
                templateUrl: '../areas/admin/content/src/taskgroups/modal/addEditTaskGroup.html',
                resolve: {
                    id: function () {
                        return id;
                    }
                }
            }).result.then(function (result) {
                ctrl.grid.fetchData();
                return result;
            }, function (result) {
                return result;
            });
        };

        ctrl.deleteTaskGroup = function (canBeDeleted, id) {
            if (canBeDeleted) {
                SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                    if (result === true) {
                        $http.post('taskgroups/deletetaskgroup', { Id: id }).then(function (response) {
                            ctrl.grid.fetchData();
                        });
                    }
                });
            } else {
                SweetAlert.alert("В проекте есть задачи", { title: "Удаление невозможно" });
            }
        };

    };

    TaskGroupsCtrl.$inject = ['$q', '$location', '$window', '$http', '$uibModal', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'SweetAlert'];


    ng.module('taskgroups', ['uiGridCustom', 'urlHelper'])
      .controller('TaskGroupsCtrl', TaskGroupsCtrl);

})(window.angular);