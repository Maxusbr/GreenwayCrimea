; (function (ng) {
    'use strict';

    var CustomersCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, $q, SweetAlert, $http, customerFieldsService) {

        var ctrl = this,
            columnDefs = [];

        ctrl.gridOptions = {};

        ctrl.customersInit = function (enableManagers) {

            columnDefs = [
                        {
                            name: 'Name',
                            displayName: 'Покупатель',
                            filter: {
                                placeholder: 'Имя',
                                type: uiGridConstants.filter.INPUT,
                                name: 'Name',
                            },
                            cellTemplate: '<div class="ui-grid-cell-contents"><a ng-href="customers/edit/{{row.entity.CustomerId}}">{{COL_FIELD}}</a></div>',
                        },
                        {
                            name: 'Phone',
                            displayName: 'Телефон',
                            filter: {
                                placeholder: 'Телефон',
                                type: uiGridConstants.filter.INPUT,
                                name: 'Phone',
                            }
                        },
                        {
                            name: 'Email',
                            displayName: 'Email',
                            filter: {
                                placeholder: 'Email',
                                type: uiGridConstants.filter.INPUT,
                                name: 'Email',
                            }
                        },
                        {
                            name: 'OrdersCount',
                            displayName: 'Кол-во оплаченных заказов',
                            width: 100,
                            type: 'number',
                            filter: {
                                placeholder: 'Кол-во оплаченных заказов',
                                type: 'range',
                                term: {
                                    from: 0,
                                    to: 0
                                },
                                rangeOptions: {
                                    from: {
                                        name: 'OrdersCountFrom'
                                    },
                                    to: {
                                        name: 'OrdersCountTo'
                                    },
                                }
                            },
                        },
                        {
                            name: 'LastOrderNumber',
                            displayName: 'Последний заказ',
                            cellTemplate: '<div class="ui-grid-cell-contents js-grid-not-clicked" data-ng-show="row.entity.LastOrderNumber != null"> <a href="orders/edit/{{row.entity.LastOrderId}}" class="link"># {{row.entity.LastOrderNumber}}</a> </div>',
                            width: 100,
                            filter: {
                                placeholder: 'Последний заказ',
                                type: uiGridConstants.filter.INPUT,
                                name: 'LastOrderNumber',
                            }
                        },
                        {
                            name: 'OrdersSum',
                            displayName: 'Сумма заказов',
                            type: 'number',
                            width: 100,
                            filter: {
                                placeholder: 'Сумма заказов',
                                type: 'range',
                                term: {
                                    from: 0,
                                    to: 0
                                },
                                rangeOptions: {
                                    from: {
                                        name: 'OrderSumFrom'
                                    },
                                    to: {
                                        name: 'OrderSumTo'
                                    },
                                }
                            },
                        },
                        {
                            name: 'RegistrationDateTimeFormatted',
                            displayName: 'Дата регистрации',
                            width: 110,
                            filter: {
                                placeholder: 'Дата регистрации',
                                type: 'datetime',
                                term: {
                                    from: (new Date()).setMonth((new Date()).getMonth() - 1),
                                    to: new Date()
                                },
                                datetimeOptions: {
                                    from: {
                                        name: 'RegistrationDateTimeFrom'
                                    },
                                    to: {
                                        name: 'RegistrationDateTimeTo'
                                    }
                                }
                            }
                        }
            ];

            if (enableManagers) {
                columnDefs.push(
                    {
                        name: 'ManagerName',
                        displayName: 'Менеджер',
                        width: 110,
                        filter: {
                            placeholder: 'Менеджер',
                            type: uiGridConstants.filter.INPUT,
                            name: 'ManagerName',
                        }
                    });
            }

            columnDefs.push(
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 60,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div class="js-grid-not-clicked"><a ng-href="customers/edit/{{row.entity.CustomerId}}" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a>' +
                        '<ui-grid-custom-delete data-ng-show="{{row.entity.CanBeDeleted}}" url="customers/deletecustomer" params="{\'CustomerId\': row.entity.CustomerId}"></ui-grid-custom-delete></div></div>'
                });

            columnDefs.push(
                {
                    name: '_noopColumnLocation',
                    visible: false,
                    filter: {
                        placeholder: 'Местоположение',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Location',
                    }
                });


            ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
                columnDefs: columnDefs,
                uiGridCustom: {
                    rowUrl: 'customers/edit/{{row.entity.CustomerId}}',
                    selectionOptions: [
                        {
                            text: 'Удалить выделенные',
                            url: 'customers/deletecustomers',
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
        }

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };

        ctrl.gridOnFilterInit = function (filter) {
            ctrl.gridFilter = filter;
            customerFieldsService.getFilterColumns().then(function (columns) {
                Array.prototype.push.apply(ctrl.gridOptions.columnDefs, columns);
                ctrl.gridFilter.updateColumns();
            });
        };
    };

    CustomersCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', '$q', 'SweetAlert', '$http', 'customerFieldsService'];


    ng.module('customers', ['uiGridCustom', 'urlHelper'])
      .controller('CustomersCtrl', CustomersCtrl);

})(window.angular);