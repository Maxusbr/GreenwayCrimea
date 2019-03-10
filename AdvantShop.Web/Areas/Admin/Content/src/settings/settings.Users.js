; (function (ng) {
    'use strict';

    var SettingsUsersCtrl = function ($uibModal, $http, $q, uiGridConstants, uiGridCustomConfig, SweetAlert, toaster) {

        var ctrl = this;
        ctrl.gridUsersInited = false;

        // #region Users
        var columnDefsUsers = [
            {
                name: 'PhotoSrc',
                headerCellClass: 'ui-grid-custom-header-cell-center',
                displayName: 'Фото',
                cellTemplate:
                    '<div class="ui-grid-cell-contents"><span class="ui-grid-custom-flex-center ui-grid-custom-link-for-img">' +
                        '<img class="ui-grid-custom-col-img" ng-src="{{row.entity.PhotoSrc}}"></span></div>',
                width: 80,
                enableSorting: false,
                filter: {
                    placeholder: 'Фото',
                    type: uiGridConstants.filter.SELECT,
                    name: 'HasPhoto',
                    selectOptions: [{ label: 'С фото', value: true }, { label: 'Без фото', value: false }]
                }
            },
            {
                name: 'FullName',
                displayName: 'ФИО',
                cellTemplate: '<div class="ui-grid-cell-contents"><span class="link">{{COL_FIELD}}</span></div>',
                filter: {
                    placeholder: 'ФИО',
                    type: uiGridConstants.filter.INPUT,
                    name: 'FullName',
                }
            },
            {
                name: 'Email',
                displayName: 'Email',
                cellTemplate: '<div class="ui-grid-cell-contents"><span class="link">{{COL_FIELD}}</span></div>',
                filter: {
                    placeholder: 'Email',
                    type: uiGridConstants.filter.INPUT,
                    name: 'Email',
                }
            },
            {
                name: 'DepartmentName',
                displayName: 'Отдел'
            },
            {
                name: '_noopColumnDepartments',
                visible: false,
                filter: {
                    placeholder: 'Отдел',
                    type: uiGridConstants.filter.SELECT,
                    name: 'DepartmentId',
                    fetch: 'departments/getDepartmentsSelectOptions'
                }
            },
            {
                name: 'Roles',
                displayName: 'Роль',
                enableSorting: false
                //filter: {
                //    placeholder: 'Отдел',
                //    type: uiGridConstants.filter.INPUT,
                //    name: 'Role',
                //}
            },
            {
                name: 'Enabled',
                displayName: 'Активен',
                enableCellEdit: false,
                cellTemplate: '<ui-grid-custom-switch row="row" class="js-grid-not-clicked"></ui-grid-custom-switch>',
                width: 90,
                filter: {
                    placeholder: 'Активность',
                    name: 'Enabled',
                    type: uiGridConstants.filter.SELECT,
                    selectOptions: [{ label: 'Активные', value: true }, { label: 'Неактивные', value: false }]
                }
            },
            {
                name: '_serviceColumn',
                displayName: '',
                width: 80,
                cellTemplate:
                    '<div class="ui-grid-cell-contents js-grid-not-clicked"><div>' +
                    '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil" ng-click="grid.appScope.$ctrl.gridExtendCtrl.loadUser(row.entity.CustomerId)"></a> ' +
                    '<a href="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.deleteUser(row.entity.CanBeDeleted, row.entity.CustomerId, row.entity.CantDeleteMessage)" ' +
                               'class="ui-grid-custom-service-icon fa fa-times link-invert"></a> ' +
                               //'ng-class="(!row.entity.CanBeDeleted ? \'ui-grid-custom-service-icon fa fa-times link-disabled\' : \'ui-grid-custom-service-icon fa fa-times link-invert\')"></a> ' +
                    '</div></div>'
            }
        ];

        ctrl.gridUsersOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefsUsers,
            uiGridCustom: {
                rowClick: function ($event, row) {
                    ctrl.loadUser(row.entity.CustomerId);
                },
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'users/deleteusers',
                        field: 'CustomerId',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    }
                ]
            }
        });

        ctrl.gridUsersOnInit = function (grid) {
            ctrl.gridUsers = grid;
            ctrl.getSaasDataInformation();
        };

        ctrl.changeManagersPageState = function () {
            $http.post('users/changeManagersPageState', { state: ctrl.showManagersPage }).then(function (response) {
                toaster.pop('success', '', 'Изменения сохранены');
            });
        };

        ctrl.changeEnableManagersModuleState = function () {
            $http.post('users/changeEnableManagersModuleState', { state: ctrl.enableManagersModule }).then(function (response) {
                toaster.pop('success', '', 'Изменения сохранены');
            });
        };

        ctrl.loadUser = function (id) {
            $uibModal.open({
                bindToController: true,
                controller: 'ModalAddEditUserCtrl',
                controllerAs: 'ctrl',
                templateUrl: '../areas/admin/content/src/settings/modal/addEditUser/AddEditUser.html',
                resolve: {
                    params: {
                        customerId: id
                    }
                }
            }).result.then(function (result) {
                ctrl.gridUsers.fetchData();
                return result;
            }, function (result) {
                ctrl.getSaasDataInformation();
                ctrl.gridUsers.fetchData();
                return result;
            });
        };

        ctrl.deleteUser = function (canBeDeleted, customerId, message) {
            if (canBeDeleted) {
                SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                    if (result === true) {
                        $http.post('users/deleteuser', { customerId: customerId }).then(function (response) {
                            var data = response.data;
                            if (data != null && data.result === false) {
                                data.errors.forEach(function (error) {
                                    toaster.error('Ошибка', error);
                                });
                            }
                            ctrl.gridUsers.fetchData();
                            ctrl.getSaasDataInformation();
                        });
                    }
                });
            } else {
                SweetAlert.error(message, { title: "Удаление невозможно" });
            }
        };
        // #endregion


        // #region Departments
        var columnDefsDepartments = [
            {
                name: 'Name',
                displayName: 'Название',
                cellTemplate: '<div class="ui-grid-cell-contents"><span class="link">{{COL_FIELD}}</span></div>'
            },
            {
                name: 'Sort',
                displayName: 'Порядок сортировки',
                type: 'number',
                width: 150,
                enableCellEdit: true
            },
            {
                name: 'Enabled',
                displayName: 'Активен',
                enableCellEdit: false,
                cellTemplate: '<ui-grid-custom-switch row="row" class="js-grid-not-clicked"></ui-grid-custom-switch>',
                width: 90,
                filter: {
                    placeholder: 'Активность',
                    name: 'Enabled',
                    type: uiGridConstants.filter.SELECT,
                    selectOptions: [{ label: 'Активные', value: true }, { label: 'Неактивные', value: false }]
                }
            },
            {
                name: '_serviceColumn',
                displayName: '',
                width: 80,
                cellTemplate:
                    '<div class="ui-grid-cell-contents js-grid-not-clicked"><div>' +
                    '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil" ng-click="grid.appScope.$ctrl.gridExtendCtrl.loadDepartment(row.entity.DepartmentId)"></a> ' +
                    '<ui-grid-custom-delete url="departments/deletedepartment" params="{\'departmentId\': row.entity.DepartmentId}"></ui-grid-custom-delete>' +
                    '</div></div>'
            }
        ];

        ctrl.gridDepartmentsOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefsDepartments,
            uiGridCustom: {
                rowClick: function ($event, row) {
                    ctrl.loadDepartment(row.entity.DepartmentId);
                },
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'departments/deletedepartments',
                        field: 'DepartmentId',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    }
                ]
            }
        });

        ctrl.gridDepartmentsOnInit = function (grid) {
            ctrl.gridDepartments = grid;
        };

        ctrl.loadDepartment = function (id) {
            $uibModal.open({
                bindToController: true,
                controller: 'ModalAddEditDepartmentCtrl',
                controllerAs: 'ctrl',
                templateUrl: '../areas/admin/content/src/settings/modal/addEditDepartment/AddEditDepartment.html',
                resolve: {
                    departmentId: function () {
                        return id;
                    }
                }
            }).result.then(function (result) {
                ctrl.gridDepartments.fetchData();
                return result;
            }, function (result) {
                return result;
            });
        };
        // #endregion


        // #region ManagerRoles
        var columnDefsManagerRoles = [
            {
                name: 'Name',
                displayName: 'Название',
                cellTemplate: '<div class="ui-grid-cell-contents"><span class="link">{{COL_FIELD}}</span></div>',
                filter: {
                    placeholder: 'Название',
                    type: uiGridConstants.filter.INPUT,
                    name: 'Name',
                }
            },
            {
                name: 'SortOrder',
                displayName: 'Порядок',
                type: 'number',
                width: 150,
                enableCellEdit: true
            },
            {
                name: '_serviceColumn',
                displayName: '',
                width: 80,
                cellTemplate:
                    '<div class="ui-grid-cell-contents js-grid-not-clicked"><div>' +
                    '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil" ng-click="grid.appScope.$ctrl.gridExtendCtrl.loadManagerRole(row.entity.Id)"></a> ' +
                    '<ui-grid-custom-delete url="managerRoles/deleteManagerRole" params="{\'Id\': row.entity.Id}"></ui-grid-custom-delete>' +
                    '</div></div>'
            }
        ];

        ctrl.gridManagerRolesOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefsManagerRoles,
            uiGridCustom: {
                rowClick: function ($event, row) {
                    ctrl.loadManagerRole(row.entity.Id);
                },
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'managerRoles/deleteManagerRoles',
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

        ctrl.gridManagerRolesOnInit = function (grid) {
            ctrl.gridManagerRoles = grid;
        };

        ctrl.loadManagerRole = function (id) {
            $uibModal.open({
                bindToController: true,
                controller: 'ModalAddEditManagerRoleCtrl',
                controllerAs: 'ctrl',
                templateUrl: '../areas/admin/content/src/settings/modal/addEditManagerRole/AddEditManagerRole.html',
                resolve: {
                    id: function () {
                        return id;
                    }
                }
            }).result.then(function (result) {
                ctrl.gridManagerRoles.fetchData();
                return result;
            }, function (result) {
                return result;
            });
        };
        // #endregion
        
        ctrl.getSaasDataInformation = function () {
            $http.post('users/GetSaasDataInformation', {}).then(function (response) {
                var data = response.data;
                if (data != null && data.result === false) {
                    toaster.pop("error", "Ошибка", data.error);
                }

                ctrl.managersLimitation = data.obj.ManagersLimitation;
                ctrl.managersLimit = data.obj.ManagersLimit;

                ctrl.employeesCount = data.obj.EmployeesCount;
                ctrl.employeesLimit = data.obj.EmployeesLimit;
                ctrl.enableEmployees = data.obj.EnableEmployees;

                ctrl.showManagersPage = data.obj.ShowManagersPage;
                ctrl.enableManagersModule = data.obj.EnableManagersModule;
                ctrl.gridUsersInited = true;
            });
        };
    };

    SettingsUsersCtrl.$inject = ['$uibModal', '$http', '$q', 'uiGridConstants', 'uiGridCustomConfig', 'SweetAlert', 'toaster'];

    ng.module('settingsUsers', ['ngFileUpload'])
      .controller('SettingsUsersCtrl', SettingsUsersCtrl);

})(window.angular);