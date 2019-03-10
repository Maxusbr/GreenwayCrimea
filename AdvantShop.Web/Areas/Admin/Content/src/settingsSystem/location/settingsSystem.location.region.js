; (function (ng) {
    'use strict';


    var SettingsSystemLocationRegionCtrl = function ($q, uiGridConstants, uiGridCustomConfig, SweetAlert) {

        var ctrl = this;

        ctrl.$onInit = function () {
            var columnDefsRegion = [
                    {
                        name: 'Name',
                        displayName: 'Регион',
                        enableCellEdit: false,
                        cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.onSelect({id : row.entity.RegionId, name: row.entity.Name})">{{COL_FIELD}}</a></div>',
                        filter: {
                            placeholder: 'Регион',
                            type: uiGridConstants.filter.INPUT,
                            name: 'Name'
                        }
                    },
                    {
                        name: 'RegionCode',
                        displayName: 'Код региона',
                        type: uiGridConstants.filter.INPUT,
                        enableCellEdit: true,
                        width: 150,
                        filter: {
                            placeholder: 'Код региона',
                            type: uiGridConstants.filter.INPUT,
                            name: 'RegionCode'
                        }
                    },
                    {
                        name: 'SortOrder',
                        displayName: 'Порядок',
                        enableCellEdit: true,
                        type: 'number',
                        width: 100,
                        filter: {
                            placeholder: 'Порядок',
                            type: 'range',
                            rangeOptions: {
                                from: {
                                    name: 'SortingFrom'
                                },
                                to: {
                                    name: 'SortingTo'
                                }
                            }
                        }
                    },
                    {
                        name: '_serviceColumn',
                        displayName: '',
                        width: 60,
                        cellTemplate: '<div class="ui-grid-cell-contents"><div class="">' +
                                            '<ui-modal-trigger data-controller="\'ModalAddEditRegionsCtrl\'" controller-as="ctrl" ' +
                                            'template-url="../areas/admin/content/src/settingsSystem/location/modal/addEditRegion/addEditRegions.html" ' +
                                            'data-resolve="{\'entity\': row.entity}" ' +
                                            'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                            '<a ng-href="" class="ui-grid-custom-service-icon fa fa-pencil">{{COL_FIELD}}</a>' +
                                       '</ui-modal-trigger>' +
                                       '<ui-grid-custom-delete url="Regions/DeleteRegion" params="{\'Ids\': row.entity.RegionId}"></ui-grid-custom-delete></div></div>'
                    }
            ];

            ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
                columnDefs: columnDefsRegion,
                uiGridCustom: {
                    rowUrl: '', //'countryregioncity/edit/{{row.entity.CountryId}}',
                    selectionOptions: [
                        {
                            text: 'Удалить выделенные',
                            url: 'Regions/DeleteRegion',
                            field: 'RegionId',
                            before: function () {
                                return SweetAlert.confirm("Вы уверены что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                    return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                                });
                            }
                        }
                    ]
                }
            });
        }

    }

    SettingsSystemLocationRegionCtrl.$inject = ['$q', 'uiGridConstants', 'uiGridCustomConfig', 'SweetAlert'];


    ng.module('settingsSystem')
      .controller('SettingsSystemLocationRegionCtrl', SettingsSystemLocationRegionCtrl);

})(window.angular);