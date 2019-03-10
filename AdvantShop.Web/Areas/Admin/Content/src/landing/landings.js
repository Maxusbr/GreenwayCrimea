; (function (ng) {
    'use strict';

    var LandingsAdminCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'Enabled',
                    displayName: 'Актив.',
                    enableCellEdit: false,
                    cellTemplate: '<ui-grid-custom-switch row="row"></ui-grid-custom-switch>',
                    width: 80,
                    filter: {
                        placeholder: 'Активность',
                        type: uiGridConstants.filter.SELECT,
                        selectOptions: [{ label: 'Активные', value: true }, { label: 'Неактивные', value: false }]
                    }
                },
                {
                    name: 'Name',
                    displayName: 'Название',
                    enableCellEdit: true,
                },
                {
                    name: 'ProductsCount',
                    displayName: 'ProductsCount',
                    width: 100,
                },
                {
                    name: 'Visitors',
                    displayName: 'Visitors',
                    width: 100,
                },
                {
                    name: 'Views',
                    displayName: 'Views',
                    width: 100,
                },
                {
                    name: 'Goals',
                    displayName: 'Goals',
                    width: 100,
                },
                {
                    name: 'Conversion',
                    displayName: 'Conversion',
                    width: 100,
                },
                {
                    name: 'CreatedDateFormatted',
                    displayName: 'Дата и время',
                    width: 150,
                    filter: {
                        placeholder: 'Дата и время',
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
                    }
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 60,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div><a target="_blank" ng-href="../lp/{{row.entity.Url}}?inplace=true" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a>' +
                        '<ui-grid-custom-delete url="landing/landingadmin/deletelanding" params="{\'Id\': row.entity.Id }"></ui-grid-custom-delete></div></div>'
                }
            ];


        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    //{
                    //    text: 'Удалить выделенные',
                    //    url: 'orders/deletelandings',
                    //    field: 'Id'
                    //}
                ]
            }
        });

    };

    LandingsAdminCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig'];


    ng.module('landings', ['uiGridCustom'])
      .controller('LandingsAdminCtrl', LandingsAdminCtrl);

})(window.angular);