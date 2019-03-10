; (function (ng) {
    'use strict';


    var SettingsSystemLocationCountryCtrl = function ($q, uiGridConstants, uiGridCustomConfig, SweetAlert) {

        var ctrl = this;

        ctrl.$onInit = function () {
            var columnDefsCountry = [
                    {
                        name: 'Name',
                        displayName: 'Страна',
                        enableCellEdit: false,
                        cellTemplate: '<div class="ui-grid-cell-contents"><a href="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.onSelect({id : row.entity.CountryId, name: row.entity.Name})">{{COL_FIELD}}</a></div>',
                        filter: {
                            placeholder: 'Страна',
                            type: uiGridConstants.filter.INPUT,
                            name: 'Name'
                        }
                    },
                    {
                        name: 'Iso2',
                        displayName: 'ISO2',
                        enableCellEdit: true,
                        uiGridCustomEdit: {
                            attributes: {
                                maxlength: 2
                            }
                        },
                        width: 80,
                        filter: {
                            placeholder: 'ISO2',
                            type: uiGridConstants.filter.INPUT,
                            name: 'Iso2'
                        }
                    },
                    {
                        name: 'Iso3',
                        displayName: 'ISO3',
                        width: 80,
                        enableCellEdit: true,
                        uiGridCustomEdit: {
                            attributes: {
                                maxlength: 3
                            }
                        },
                        filter: {
                            placeholder: 'ISO3',
                            type: uiGridConstants.filter.INPUT,
                            name: 'Iso3'
                        },
                    },
                    {
                        name: 'DisplayInPopup',
                        displayName: 'Основная страна',
                        enableCellEdit: true,
                        type: 'checkbox',
                        width: 80,
                        filter: {
                            placeholder: 'Основная страна',
                            name: 'DisplayInPopup',
                            type: uiGridConstants.filter.SELECT,
                            selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                        }
                    },
                    {
                        name: 'DialCode',
                        displayName: 'Код телефона',
                        type: 'number',
                        enableCellEdit: true,
                        width: 150,
                        filter: {
                            placeholder: 'Код телефона',
                            type: 'number',
                            name: 'DialCode'
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
                                            '<ui-modal-trigger data-controller="\'ModalAddEditCountryCtrl\'" controller-as="ctrl" ' +
                                            'template-url="../areas/admin/content/src/settingsSystem/location/modal/addEditCountry/addEditCountry.html" ' +
                                            'data-resolve="{\'entity\': row.entity}" ' +
                                            'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                            '<a ng-href="" class="ui-grid-custom-service-icon fa fa-pencil">{{COL_FIELD}}</a>' +
                                       '</ui-modal-trigger><ui-grid-custom-delete url="countries/deletecountry" params="{\'Ids\': row.entity.CountryId}"></ui-grid-custom-delete></div></div>'
                    }
            ];

            ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
                columnDefs: columnDefsCountry,
                uiGridCustom: {
                    rowUrl: '', //'countryregioncity/edit/{{row.entity.CountryId}}',
                    selectionOptions: [
                        {
                            text: 'Удалить выделенные',
                            url: 'countries/deletecountry',
                            field: 'CountryId',
                            before: function () {
                                return SweetAlert.confirm("Вы уверены что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                    return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                                });
                            }
                        },
                        {
                            text: 'Отображать при выборе страны',
                            url: 'countries/activatecountry',
                            field: 'CountryId'
                        },
                        {
                            text: 'Не отображать при выборе страны',
                            url: 'countries/disablecountry',
                            field: 'CountryId'
                        }
                    ]
                }
            });
        }

    }

    SettingsSystemLocationCountryCtrl.$inject = ['$q', 'uiGridConstants', 'uiGridCustomConfig', 'SweetAlert'];

    ng.module('settingsSystem')
      .controller('SettingsSystemLocationCountryCtrl', SettingsSystemLocationCountryCtrl);

})(window.angular);