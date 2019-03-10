; (function (ng) {
    'use strict';

    var OrderStatusHistoryCtrl = function ($http, uiGridCustomConfig) {
        var ctrl = this;

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: [
                {
                    name: 'Date',
                    displayName: 'Дата',
                    enableCellEdit: false,
                    enableSorting: false,
                },
                {
                    name: 'PreviousStatus',
                    displayName: 'Предыдущий статус',
                    enableCellEdit: false,
                    enableSorting: false,
                },
                {
                    name: 'NewStatus',
                    displayName: 'Новый статус',
                    enableCellEdit: false,
                    enableSorting: false,
                },
                {
                    name: 'CustomerName',
                    displayName: 'Пользователь сменивший статус',
                    enableCellEdit: false,
                    enableSorting: false,
                },
                {
                    name: 'Basis',
                    displayName: 'Основание',
                    enableCellEdit: false,
                    enableSorting: false,
                }
            ],
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.statuses = grid;
        };
    };

    OrderStatusHistoryCtrl.$inject = ['$http', 'uiGridCustomConfig'];

    ng.module('orderStatusHistory', ['uiGridCustom'])
        .controller('OrderStatusHistoryCtrl', OrderStatusHistoryCtrl)
        .component('orderStatusHistory', {
            templateUrl: '../areas/admin/content/src/order/components/orderStatusHistory/orderStatusHistory.html',
            controller: OrderStatusHistoryCtrl,
            bindings: {
                orderId: '<?',
            }
      });

})(window.angular);