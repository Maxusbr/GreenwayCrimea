; (function (ng) {
    'use strict';

    var CustomerOrdersCtrl = function ($http, uiGridCustomConfig) {
        var ctrl = this;

        ctrl.gridOrdersOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: [
                {
                    name: 'OrderNumber',
                    displayName: '№ Заказа',
                    enableCellEdit: false,
                    enableSorting: false,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"> ' +
                            '<div class="m-l-xs"><a href="orders/edit/{{row.entity.OrderId}}" target="_blank">{{row.entity.OrderNumber}}</a></div> ' +
                        '</div>',
                },
                {
                    name: 'Status',
                    displayName: 'Статус',
                    enableCellEdit: false,
                    enableSorting: false,
                },
                {
                    name: 'Payed',
                    displayName: 'Оплачен',
                    enableCellEdit: false,
                    enableSorting: false,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"> ' +
                            '{{row.entity.Payed ? "Да" : "Нет"}}' +
                        '</div>',
                },
                /*{
                    name: 'ArchivedPaymentName',
                    displayName: 'Оплата',
                    enableCellEdit: false,
                    enableSorting: false,
                },
                {
                    name: 'ShippingMethodName',
                    displayName: 'Доставка',
                    enableCellEdit: false,
                    enableSorting: false,
                },*/
                {
                    name: 'Sum',
                    displayName: 'Сумма',
                    enableCellEdit: false,
                    enableSorting: false,
                },
                {
                    name: 'OrderDate',
                    displayName: 'Дата',
                    enableCellEdit: false,
                    enableSorting: false,
                },
                {
                    name: 'ManagerName',
                    displayName: 'Менеджер',
                    enableCellEdit: false,
                    enableSorting: false,
                }
            ],
        });


    };

    CustomerOrdersCtrl.$inject = ['$http', 'uiGridCustomConfig'];

    ng.module('customerOrders', ['uiGridCustom'])
        .controller('CustomerOrdersCtrl', CustomerOrdersCtrl)
        .component('customerOrders', {
            templateUrl: '../areas/admin/content/src/customer/components/customerOrders/customerOrders.html',
            controller: CustomerOrdersCtrl,
            bindings: {
                customerId: '@'
            }
      });

})(window.angular);