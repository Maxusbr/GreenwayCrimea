; (function (ng) {
    'use strict';

    var AnalyticsFilterCtrl = function (uiGridCustomConfig) {

        var ctrl = this,
            columnDefsAbcxyz = [
                {
                    name: 'ArtNo',
                    displayName: 'Артикул',
                    width: 150,
                    enableCellEdit: false,
                    enableSorting: false,
                },
                {
                    name: 'Name',
                    displayName: 'Название',
                    enableCellEdit: false,
                    enableSorting: false,
                },
                {
                    name: 'PriceFormatted',
                    displayName: 'Цена',
                    width: 150,
                    enableCellEdit: false,
                    enableSorting: false,
                },
            ],
            columnDefsRfm = [
                {
                    name: 'Name',
                    displayName: 'Имя',
                    enableCellEdit: false,
                    enableSorting: false,
                },
                {
                    name: 'Email',
                    displayName: 'Email',
                    enableCellEdit: false,
                    enableSorting: false,
                },
                {
                    name: 'Phone',
                    displayName: 'Телефон',
                    enableCellEdit: false,
                    enableSorting: false,
                }
            ];

        ctrl.gridOptionsAbcxyz = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefsAbcxyz,
            uiGridCustom: {
                rowUrl: 'product/edit/{{row.entity.ProductId}}'
            }
        });

        ctrl.gridOptionsRfm = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefsRfm,
            uiGridCustom: {
                rowUrl: 'customers/edit/{{row.entity.CustomerId}}'
            }
        });
    };

    AnalyticsFilterCtrl.$inject = ['uiGridCustomConfig'];

    ng.module('analyticsFilter', ['uiGridCustom'])
      .controller('AnalyticsFilterCtrl', AnalyticsFilterCtrl);

})(window.angular);