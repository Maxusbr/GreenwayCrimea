; (function (ng) {
    'use strict';

    var CustomerSegmentsCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, toaster, SweetAlert, $http, $q) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'Name',
                    displayName: 'Название',
                    filter: {
                        placeholder: 'Название',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Name'
                    },
                    cellTemplate: '<div class="ui-grid-cell-contents"><a ng-href="customersegments/edit/{{row.entity.Id}}">{{COL_FIELD}}</a></div>',
                },
                {
                    name: 'CustomersCount',
                    displayName: 'Кол-во покупателей',
                    width: 120,
                    enableCellEdit: false,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div class="p-l-sm">{{COL_FIELD}}</div></div>',
                },
                {
                    name: 'CreatedDateFormatted',
                    displayName: 'Дата создания',
                    width: 150,
                    enableCellEdit: false,
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 70,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<a ng-href="customersegments/edit/{{row.entity.Id}}" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a>' +
                            '<ui-grid-custom-delete url="customerSegments/deleteSegment" params="{\'id\': row.entity.Id}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'customerSegments/deleteSegments',
                        field: 'Id',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    }
                ]
            }
        });


        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };


    };

    CustomerSegmentsCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'toaster', 'SweetAlert', '$http', '$q'];


    ng.module('customerSegments', ['uiGridCustom', 'urlHelper'])
      .controller('CustomerSegmentsCtrl', CustomerSegmentsCtrl);

})(window.angular);