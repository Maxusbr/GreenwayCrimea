; (function (ng) {
    'use strict';

    var SettingsCustomersCtrl = function ($uibModal, $q, $location, uiGridConstants, uiGridCustomConfig, SweetAlert) {

        var ctrl = this;

        ctrl.editValues = function (field) {
            ctrl.field = field;
            // при перезагрузке страницы из редактирования значений в урле остается gridCustomerFieldValues
            $location.search('gridCustomerFieldValues', null);
        };

        ctrl.back = function () {
            ctrl.field = null;
            ctrl.gridCustomerFieldValues.clearParams();
        };

        // #region CustomerFields
        var columnDefsCustomerFields = [
            {
                name: 'Name',
                displayName: 'Наименование',
                cellTemplate: '<div class="ui-grid-cell-contents"><span class="link">{{COL_FIELD}}</span></div>',
                filter: {
                    placeholder: 'Наименование',
                    type: uiGridConstants.filter.INPUT,
                    name: 'Name',
                }
            },
            {
                name: 'FieldTypeFormatted',
                displayName: 'Тип',
                width: 150,
                filter: {
                    placeholder: 'Тип',
                    type: uiGridConstants.filter.SELECT,
                    name: 'FieldType',
                    fetch: 'customerFields/getCustomerFieldTypes'
                }
            },
            {
                name: 'HasValues',
                displayName: 'Настройки поля',
                cellTemplate: '<div class="ui-grid-cell-contents"><span class="link" ng-if="row.entity.HasValues" ng-click="grid.appScope.$ctrl.gridExtendCtrl.editValues(row.entity)">Список значений</span></div>',
                width: 140,
                enableSorting: false
            },
            {
                name: 'Required',
                displayName: 'Обязательное',
                enableCellEdit: false,
                cellTemplate: '<ui-grid-custom-switch row="row" field-name="Required" class="js-grid-not-clicked"></ui-grid-custom-switch>',
                width: 100,
                filter: {
                    placeholder: 'Обязательное',
                    name: 'Required',
                    type: uiGridConstants.filter.SELECT,
                    selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                }
            },
            {
                name: 'SortOrder',
                displayName: 'Порядок сортировки',
                type: 'number',
                width: 100,
                enableCellEdit: true
            },
            {
                name: 'ShowInClient',
                displayName: 'Запрашивать у покупателя',
                enableCellEdit: false,
                cellTemplate: '<ui-grid-custom-switch row="row" field-name="ShowInClient" class="js-grid-not-clicked"></ui-grid-custom-switch>',
                width: 110,
                filter: {
                    placeholder: 'Запрашивать у покупателя',
                    name: 'ShowInClient',
                    type: uiGridConstants.filter.SELECT,
                    selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                }
            },
            {
                name: 'Enabled',
                displayName: 'Активно',
                enableCellEdit: false,
                cellTemplate: '<ui-grid-custom-switch row="row" field-name="Enabled" class="js-grid-not-clicked"></ui-grid-custom-switch>',
                width: 70,
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
                    '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil" ng-click="grid.appScope.$ctrl.gridExtendCtrl.loadCustomerField(row.entity.Id)"></a> ' +
                    '<ui-grid-custom-delete url="customerFields/delete" params="{\'Id\': row.entity.Id}"></ui-grid-custom-delete>' +
                    '</div></div>'
            }
        ];

        ctrl.gridCustomerFieldsOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefsCustomerFields,
            uiGridCustom: {
                rowClick: function ($event, row) {
                    ctrl.loadCustomerField(row.entity.Id);
                },
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'customerFields/deleteItems',
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

        ctrl.gridCustomerFieldsOnInit = function (grid) {
            ctrl.gridCustomerFields = grid;
        };

        ctrl.loadCustomerField = function (id) {
            $uibModal.open({
                bindToController: true,
                controller: 'ModalAddEditCustomerFieldCtrl',
                controllerAs: 'ctrl',
                templateUrl: '../areas/admin/content/src/settingsCustomers/modal/addEditCustomerField/AddEditCustomerField.html',
                resolve: {
                    id: function () {
                        return id;
                    }
                }
            }).result.then(function (result) {
                ctrl.gridCustomerFields.fetchData();
                return result;
            }, function (result) {
                return result;
            });
        };
        // #endregion

        // #region CustomerFieldValues
        var columnDefsCustomerFieldValues = [
            {
                name: 'Value',
                displayName: 'Значение',
                cellTemplate: '<div class="ui-grid-cell-contents"><span class="link">{{COL_FIELD}}</span></div>',
                filter: {
                    placeholder: 'Значение',
                    type: uiGridConstants.filter.INPUT,
                    name: 'Value',
                }
            },
            {
                name: 'SortOrder',
                displayName: 'Порядок сортировки',
                type: 'number',
                width: 100,
                enableCellEdit: true
            },
            {
                name: '_serviceColumn',
                displayName: '',
                width: 80,
                cellTemplate:
                    '<div class="ui-grid-cell-contents js-grid-not-clicked"><div>' +
                    '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil" ng-click="grid.appScope.$ctrl.gridExtendCtrl.loadCustomerFieldValue(row.entity.Id)"></a> ' +
                    '<ui-grid-custom-delete url="customerFieldValues/delete" params="{\'Id\': row.entity.Id}"></ui-grid-custom-delete>' +
                    '</div></div>'
            }
        ];

        ctrl.gridCustomerFieldValuesOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefsCustomerFieldValues,
            uiGridCustom: {
                rowClick: function ($event, row) {
                    ctrl.loadCustomerFieldValue(row.entity.Id);
                },
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'customerFieldValues/deleteItems',
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

        ctrl.gridCustomerFieldValuesOnInit = function (grid) {
            ctrl.gridCustomerFieldValues = grid;
        };

        ctrl.loadCustomerFieldValue = function (id) {
            $uibModal.open({
                bindToController: true,
                controller: 'ModalAddEditCustomerFieldValueCtrl',
                controllerAs: 'ctrl',
                templateUrl: '../areas/admin/content/src/settingsCustomers/modal/addEditCustomerFieldValue/AddEditCustomerFieldValue.html',
                resolve: {
                    id: function () {
                        return id;
                    }
                }
            }).result.then(function (result) {
                ctrl.gridCustomerFieldValues.fetchData();
                return result;
            }, function (result) {
                return result;
            });
        };
        // #endregion
    };

    SettingsCustomersCtrl.$inject = ['$uibModal', '$q', '$location', 'uiGridConstants', 'uiGridCustomConfig', 'SweetAlert'];

    ng.module('settingsCustomers', ['as.sortable', 'vkAuth'])
      .controller('SettingsCustomersCtrl', SettingsCustomersCtrl);

})(window.angular);