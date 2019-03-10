; (function (ng) {
    'use strict';

    var OrdersCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, urlHelper, $q, SweetAlert, lastStatisticsService, adminWebNotificationsService, adminWebNotificationsEvents) {

        var ctrl = this;

        ctrl.init = function (showManagers) {

            adminWebNotificationsService.addListener(adminWebNotificationsEvents.updateOrders, function () { ctrl.gridUpdate(); });

            ctrl.showManagers = showManagers;

            var columnDefs = [
                {
                    name: '_noopColumnNumber',
                    visible: false,
                    filter: {
                        placeholder: 'Номер заказа',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Number',
                    }
                },
                {
                    name: '_noopColumnStatuses',
                    visible: false,
                    filter: {
                        placeholder: 'Статус',
                        type: uiGridConstants.filter.SELECT,
                        name: 'OrderStatusId',
                        fetch: 'orders/getorderstatuses'
                    }
                },
                {
                    name: '_noopColumnSum',
                    visible: false,
                    filter: {
                        placeholder: 'Стоимость',
                        type: 'range',
                        rangeOptions: {
                            from: {
                                name: 'PriceFrom'
                            },
                            to: {
                                name: 'PriceTo'
                            },
                        }
                    }
                },
                {
                    name: '_noopColumnIsPaid',
                    visible: false,
                    filter: {
                        placeholder: 'Оплата',
                        type: uiGridConstants.filter.SELECT,
                        name: 'IsPaid',
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
                },
                {
                    name: '_noopColumnName',
                    visible: false,
                    filter: {
                        placeholder: 'ФИО покупателя',
                        type: uiGridConstants.filter.INPUT,
                        name: 'BuyerName'
                    }
                },
                {
                    name: '_noopColumnPhone',
                    visible: false,
                    filter: {
                        placeholder: 'Телефон покупателя',
                        type: uiGridConstants.filter.INPUT,
                        name: 'BuyerPhone'
                    }
                },
                {
                    name: '_noopColumnEmail',
                    visible: false,
                    filter: {
                        placeholder: 'Email покупателя',
                        type: uiGridConstants.filter.INPUT,
                        name: 'BuyerEmail'
                    }
                },
                {
                    name: '_noopColumnCity',
                    visible: false,
                    filter: {
                        placeholder: 'Город покупателя',
                        type: uiGridConstants.filter.INPUT,
                        name: 'BuyerCity'
                    }
                },
            ];

            if (ctrl.showManagers) {
                columnDefs.push({
                    name: '_noopColumnManager',
                    visible: false,
                    enabled: ctrl.showManagers,
                    filter: {
                        placeholder: 'Менеджер',
                        type: uiGridConstants.filter.SELECT,
                        name: 'ManagerCustomerId',
                        fetch: 'orders/getmanagers'
                    }
                });
            }

            columnDefs = columnDefs.concat([
                {
                    name: '_noopColumnShippings',
                    visible: false,
                    filter: {
                        placeholder: 'Метод доставки',
                        type: uiGridConstants.filter.INPUT,
                        name: 'ShippingMethod',
                        //fetch: 'orders/getordershippingmethods'
                    }
                },
                {
                    name: '_noopColumnPayments',
                    visible: false,
                    filter: {
                        placeholder: 'Метод оплаты',
                        type: uiGridConstants.filter.SELECT,
                        name: 'PaymentMethod',
                        fetch: 'orders/getorderpaymentmethods'
                    }
                },
                {
                    name: '_noopColumnSources',
                    visible: false,
                    filter: {
                        placeholder: 'Источник заказа',
                        type: uiGridConstants.filter.SELECT,
                        name: 'OrderSourceId',
                        fetch: 'orders/getordersources'
                    }
                },
                {
                    name: 'Number',
                    displayName: 'Номер',
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><a ng-href="orders/edit/{{row.entity.OrderId}}">{{COL_FIELD}}</a></div>',
                    width: 90
                },
                {
                    name: 'StatusName',
                    displayName: 'Статус',
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><i class="fa fa-circle m-r-xs" style="color:#{{row.entity.Color}}"></i> {{row.entity.StatusName}}</div>',
                },
                {
                    name: 'BuyerName',
                    displayName: 'Покупатель',
                },
                {
                    name: 'ManagerName',
                    displayName: 'Менеджер',
                    visible: ctrl.showManagers,
                },
                {
                    name: 'IsPaid',
                    displayName: 'Оплата',
                    //cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity.IsPaid ? "Да" : "Нет"}}</div>',
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><label class="adv-checkbox-label">' +
                            '<input type="checkbox" disabled ng-model="row.entity.IsPaid" class="adv-checkbox-input control-checkbox" data-e2e="switchOnOffSelect" />' +
                            '<span class="adv-checkbox-emul" data-e2e="switchOnOffInput"></span>' +
                            '</label></div>',
                    width: 65
                },
                {
                    name: 'SumFormatted',
                    displayName: 'Сумма',
                    width: 135,
                },
                {
                    name: 'OrderDateFormatted',
                    displayName: 'Дата и время',
                    width: 140,
                    filter: {
                        placeholder: 'Дата и время',
                        type: 'datetime',
                        term: {
                            from: (new Date()).setMonth((new Date()).getMonth() - 1),
                            to: new Date()
                        },
                        datetimeOptions: {
                            from: {
                                name: 'OrderDateFrom'
                            },
                            to: {
                                name: 'OrderDateTo'
                            }
                        }
                    }
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 60,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div class="js-grid-not-clicked">' +
                            '<a ng-href="orders/edit/{{row.entity.OrderId}}" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a>' +
                            '<ui-grid-custom-delete url="orders/deleteorder" params="{\'OrderId\': row.entity.OrderId }"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ]);


            ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
                columnDefs: columnDefs,
                uiGridCustom: {
                    rowUrl: 'orders/edit/{{row.entity.OrderId}}',
                    selectionOptions: [
                        {
                            text: 'Удалить выделенные',
                            url: 'orders/deleteorders',
                            field: 'OrderId',
                            before: function() {
                                return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                    return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                                });
                            },
                            after: function () {
                                lastStatisticsService.getLastStatistics();
                            },
                        },
                        {
                            template:
                                '<ui-modal-trigger data-controller="\'ModalChangeOrderStatusesCtrl\'" controller-as="ctrl" data-resolve=\"{params:$ctrl.getSelectedParams(\'OrderId\')}\" ' +
                                    'template-url="../areas/admin/content/src/orders/modal/ChangeOrderStatuses.html" ' +
                                    'data-on-close="$ctrl.gridOnAction()">' +
                                    'Изменить статус выделенным</ui-modal-trigger>'
                        },
                        {
                            text: 'Пометить оплаченным',
                            url: 'orders/markpaid',
                            field: 'OrderId'
                        },
                        {
                            text: 'Пометить неоплаченым',
                            url: 'orders/marknotpaid',
                            field: 'OrderId'
                        }
                    ]
                }
            });
        }

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };

        ctrl.gridUpdate = function(){
            ctrl.grid.fetchData(true);
        };
    };

    OrdersCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'urlHelper', '$q', 'SweetAlert', 'lastStatisticsService', 'adminWebNotificationsService', 'adminWebNotificationsEvents'];


    ng.module('orders', ['uiGridCustom', 'urlHelper'])
      .controller('OrdersCtrl', OrdersCtrl);

})(window.angular);