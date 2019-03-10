; (function (ng) {
    'use strict';

    var HomeCtrl = function (uiGridCustomConfig) {

        var ctrl = this,
            columnDefs = [
                {
                    name: '№',
                    displayName: '№ заказа',
                    cellTemplate: '<div class="ui-grid-cell-contents"><a ng-href="orders/edit/{{row.entity.OrderId}}">{{row.entity.Number}}</a></div>',
                    width: 110,
                },
                {
                    name: 'StatusName',
                    displayName: 'Статус',
                    cellTemplate: '<div class="ui-grid-cell-contents"><i class="fa fa-circle" style="color:#{{row.entity.StatusColor}}"></i>&nbsp;{{COL_FIELD}}</div>',
                },
                {
                    name: 'CustomerName',
                    displayName: 'Заказчик',
                },
                {
                    name: 'OrderDate',
                    displayName: 'Дата',
                },
                {
                    name: 'Sum',
                    displayName: 'Сумма',
                },
            ];

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            enableSorting: false,
            columnDefs: columnDefs,
            uiGridCustom: {
                rowUrl: 'orders/edit/{{row.entity.OrderId}}'
            }
        });

        ctrl.gridOptionsMy = ng.extend({}, uiGridCustomConfig, {
            enableSorting: false,
            columnDefs: columnDefs,
            uiGridCustom: {
                rowUrl: 'orders/edit/{{row.entity.OrderId}}'
            }
        });

        ctrl.gridOptionsNotMy = ng.extend({}, uiGridCustomConfig, {
            enableSorting: false,
            columnDefs: columnDefs,
            uiGridCustom: {
                rowUrl: 'orders/edit/{{row.entity.OrderId}}'
            }
        });
    };
    
    HomeCtrl.$inject = ['uiGridCustomConfig'];

    ng.module('home', ['uiGridCustom'])
      .controller('HomeCtrl', HomeCtrl);

})(window.angular);