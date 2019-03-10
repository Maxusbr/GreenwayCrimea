; (function (ng) {
    'use strict';

    var LeadsCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, $q, SweetAlert, $http, $uibModal, toaster, adminWebNotificationsService, adminWebNotificationsEvents) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'Id',
                    displayName: 'Номер',
                    enableCellEdit: false,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><a ng-href="leads/edit/{{row.entity.Id}}">{{COL_FIELD}}</a></div>',
                    width: 90,
                },
                {
                    name: 'DealStatusName',
                    displayName: 'Этап сделки',
                    enableCellEdit: false,
                    width: 170,
                },
                {
                    name: 'FullName',
                    displayName: 'Контакт',
                    enableCellEdit: false,
                    filter: {
                        placeholder: 'Контакт',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Name',
                    }
                },
                {
                    name: 'ManagerName',
                    displayName: 'Менеджер',
                    enableCellEdit: false,
                    filter: {
                        placeholder: 'Менеджер',
                        type: uiGridConstants.filter.SELECT,
                        name: 'ManagerCustomerId',
                        fetch: 'leads/getmanagers'
                    }
                },
                {
                    name: 'ProductsCount',
                    displayName: 'Товаров',
                    enableCellEdit: false,
                    width: 90,
                },
                {
                    name: 'Sum',
                    displayName: 'Бюджет',
                    enableCellEdit: false,
                    filter: {
                        placeholder: 'Бюджет',
                        type: 'range',
                        rangeOptions: {
                            from: {
                                name: 'SumFrom'
                            },
                            to: {
                                name: 'SumTo'
                            }
                        }
                    },
                    width: 100,
                },
                {
                    name: 'CreatedDateFormatted',
                    displayName: 'Дата создания',
                    enableCellEdit: false,
                    filter: {
                        placeholder: 'Дата создания',
                        type: 'datetime',
                        term: {
                            from: (new Date()).setMonth((new Date()).getMonth() - 1),
                            to: new Date()
                        },
                        datetimeOptions: {
                            from: {
                                name: 'CreatedDateFrom'
                            },
                            to: {
                                name: 'CreatedDateTo'
                            }
                        }
                    },
                    width: 150,
                },
                {
                    name: '_noopColumnSources',
                    visible: false,
                    filter: {
                        placeholder: 'Источник лида',
                        type: uiGridConstants.filter.SELECT,
                        name: 'OrderSourceId',
                        fetch: 'leads/getordersources'
                    }
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 60,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<a ng-href="leads/edit/{{row.entity.Id}}" class="ui-grid-custom-service-icon fa fa-pencil"></a>' +
                            '<ui-grid-custom-delete url="leads/deleteLeads" params="{\'Ids\': row.entity.Id}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];

        ctrl.leadsParam = { 'DealStatusId': null };

        ctrl.changeParam = function (statusId) {
            ctrl.leadsParam['DealStatusId'] = statusId;
            ctrl.grid.setParams(ctrl.leadsParam);
            ctrl.grid.fetchData();
        };

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                rowUrl: 'leads/edit/{{row.entity.Id}}',
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'leads/deleteLeads',
                        field: 'Id',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    },
                ]
            }
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };

        ctrl.gridUpdate = function () {
            ctrl.grid.fetchData(true);
        };

        ctrl.changeBuyInOneClickCreateOrder = function () {
            $http.post('leads/changeBuyInOneClickCreateOrder').then(function (response) {
                toaster.pop('success', '', 'Изменения сохранены');
                window.location.reload();
            });
        }

        ctrl.$onInit = function () {
            adminWebNotificationsService.addListener(adminWebNotificationsEvents.updateLeads, function () { ctrl.gridUpdate(); });
        };
    };

    LeadsCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', '$q', 'SweetAlert', '$http', '$uibModal', 'toaster', 'adminWebNotificationsService', 'adminWebNotificationsEvents'];


    ng.module('leads', ['uiGridCustom', 'urlHelper'])
      .controller('LeadsCtrl', LeadsCtrl);

})(window.angular);