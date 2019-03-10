; (function (ng) {
    'use strict';

    var CustomerLeadsCtrl = function ($http, uiGridCustomConfig) {
        var ctrl = this;

        ctrl.gridLeadsOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: [
                {
                    name: 'Id',
                    displayName: '№ Лида',
                    enableCellEdit: false,
                    enableSorting: false,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"> ' +
                            '<div class="m-l-xs"><a href="leads/edit/{{row.entity.Id}}" target="_blank">{{row.entity.Id}}</a></div> ' +
                        '</div>',
                },
                {
                    name: 'DealStatusName',
                    displayName: 'Статус',
                    enableCellEdit: false,
                    enableSorting: false,
                },
                {
                    name: 'FullName',
                    displayName: 'ФИО покупателя',
                    enableCellEdit: false,
                    enableSorting: false,
                },
                {
                    name: 'Sum',
                    displayName: 'Стоимость',
                    enableCellEdit: false,
                    enableSorting: false,
                },
                {
                    name: 'CreatedDateFormatted',
                    displayName: 'Время создания',
                    enableCellEdit: false,
                    enableSorting: false,
                },
                {
                    name: 'ManagerName',
                    displayName: 'Менеджер',
                    enableCellEdit: false,
                    enableSorting: false,
                },
            ],
        });


    };

    CustomerLeadsCtrl.$inject = ['$http', 'uiGridCustomConfig'];

    ng.module('customerLeads', ['uiGridCustom'])
        .controller('CustomerOrdersCtrl', CustomerLeadsCtrl)
        .component('customerLeads', {
            templateUrl: '../areas/admin/content/src/customer/components/customerLeads/customerLeads.html',
            controller: CustomerLeadsCtrl,
            bindings: {
                customerId: '@'
            }
      });

})(window.angular);