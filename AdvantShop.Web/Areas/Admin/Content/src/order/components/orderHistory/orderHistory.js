; (function (ng) {
    'use strict';

    var OrderHistoryCtrl = function ($http, uiGridCustomConfig) {
        var ctrl = this;

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: [
                {
                    name: 'ModificationTimeFormatted',
                    displayName: 'Дата',
                    enableCellEdit: false,
                    enableSorting: false,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents" ng-if="grid.appScope.$ctrl.gridExtendCtrl.show(row.entity.ModificationTime, rowRenderIndex )">' +
                            '{{row.entity.ModificationTimeFormatted}}' +
                        '</div>'
                },
                {
                    name: 'Parameter',
                    displayName: 'Что изменилось',
                    enableCellEdit: false,
                    enableSorting: false,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents order-history-parameter"> ' +
                            '<div>{{row.entity.Parameter}}</div> ' +
                            '<div class="order-history-param-descr" ng-if="row.entity.ParameterDescription != null">{{row.entity.ParameterDescription}}</div>' +
                        '</div>',
                },
                {
                    name: 'OldValue',
                    displayName: 'Старое значение',
                    enableCellEdit: false,
                    enableSorting: false,
                    cellTemplate: '<div class="ui-grid-cell-contents" style="overflow-y: auto;">{{row.entity.OldValue}}</div>'
                },
                {
                    name: 'NewValue',
                    displayName: 'Новое значение',
                    enableCellEdit: false,
                    enableSorting: false,
                    cellTemplate: '<div class="ui-grid-cell-contents" style="overflow-y: auto;">{{row.entity.NewValue}}</div>'
                },
                {
                    name: 'ManagerName',
                    displayName: 'Кто менял',
                    enableCellEdit: false,
                    enableSorting: false,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents js-grid-not-clicked">' +
                            '<div ng-if="row.entity.ManagerId">' +
                                '<sidebar-user-trigger customer-id="row.entity.ManagerId" ng-if="row.entity.IsEmployee">' +
                                    '<a href="">{{row.entity.ManagerName}}</a>' +
                                '</sidebar-user-trigger>' +
                                '<a href="customers/edit/{{row.entity.ManagerId}}" ng-if="!row.entity.IsEmployee">{{row.entity.ManagerName}}</a>' +
                            '<div>' +
                        '</div>',
                }
            ],
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.statuses = grid;
        };


        var prevDate, prevDate2;

        ctrl.show = function (date, rowRenderIndex) {
            if (rowRenderIndex == 0)
                prevDate = null;

            if (prevDate == date)
                return false;

            prevDate = date;
            return true;
        }

        ctrl.show2 = function (date, rowRenderIndex) {
            if (rowRenderIndex == 0)
                prevDate2 = null;

            if (prevDate2 == date)
                return false;

            prevDate2 = date;
            return true;
        }
    };

    OrderHistoryCtrl.$inject = ['$http', 'uiGridCustomConfig'];

    ng.module('orderHistory', ['uiGridCustom'])
        .controller('OrderHistoryCtrl', OrderHistoryCtrl)
        .component('orderHistory', {
            templateUrl: '../areas/admin/content/src/order/components/orderHistory/orderHistory.html',
            controller: OrderHistoryCtrl,
            bindings: {
                orderId: '<?',
            }
        });

})(window.angular);