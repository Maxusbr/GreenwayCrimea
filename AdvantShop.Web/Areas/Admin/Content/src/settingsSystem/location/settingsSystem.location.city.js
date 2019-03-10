; (function (ng) {
    'use strict';


    var SettingsSystemLocationCityCtrl = function ($q, uiGridConstants, uiGridCustomConfig, SweetAlert) {

        var ctrl = this;

        ctrl.$onInit = function () {
            var columnDefsCity = [
                    {
                        name: 'Name',
                        displayName: 'Город',
                        enableCellEdit: false,
                        filter: {
                            placeholder: 'Город',
                            type: uiGridConstants.filter.INPUT,
                            name: 'Name'
                        }
                    },
                    {
                        name: 'DisplayInPopup',
                        displayName: 'Основной город',
                        enableCellEdit: true,
                        type: 'checkbox',
                        width: 80,
                        filter: {
                            placeholder: 'Основной город',
                            name: 'DisplayInPopup',
                            type: uiGridConstants.filter.SELECT,
                            selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                        }
                    },
                    {
                        name: 'PhoneNumber',
                        displayName: 'Номер телефона',
                        type: uiGridConstants.filter.INPUT,
                        enableCellEdit: true,
                        width: 150,
                        filter: {
                            placeholder: 'Номер телефона',
                            type: uiGridConstants.filter.INPUT,
                            name: 'PhoneNumber'
                        }
                    },

                    {
                        name: 'MobilePhoneNumber',
                        displayName: 'Номер телефона в мобильной версии',
                        type: uiGridConstants.filter.INPUT,
                        enableCellEdit: true,
                        width: 150,
                        filter: {
                            placeholder: 'Номер телефона в мобильной версии',
                            type: uiGridConstants.filter.INPUT,
                            name: 'MobilePhoneNumber'
                        }
                    },
                    {
                        name: 'CitySort',
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
                                            '<ui-modal-trigger data-controller="\'ModalAddEditCitysCtrl\'" controller-as="ctrl" ' +
                                            'template-url="../areas/admin/content/src/settingsSystem/location/modal/addEditCitys/addEditCitys.html" ' +
                                            'data-resolve="{\'entity\': row.entity}" ' +
                                            'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                            '<a ng-href="" class="ui-grid-custom-service-icon fa fa-pencil">{{COL_FIELD}}</a>' +
                                       '</ui-modal-trigger>' +
                                       '<ui-grid-custom-delete url="Cities/DeleteCity" params="{\'Ids\': row.entity.CityId}"></ui-grid-custom-delete></div></div>'
                    }
            ];

            ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
                columnDefs: columnDefsCity,
                uiGridCustom: {
                    rowUrl: '', //'countryregioncity/edit/{{row.entity.CountryId}}',
                    selectionOptions: [
                        {
                            text: 'Удалить выделенные',
                            url: 'Cities/DeleteCity',
                            field: 'CityId',
                            before: function () {
                                return SweetAlert.confirm("Вы уверены что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                    return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                                });
                            }
                        },
                        {
                            text: 'Отображать при выборе города',
                            url: 'Cities/ActivateCity',
                            field: 'CityId'
                        },
                        {
                            text: 'Не отображать при выборе города',
                            url: 'Cities/DisableCity',
                            field: 'CityId'
                        }
                    ]
                }
            });
        }

    }

    SettingsSystemLocationCityCtrl.$inject = ['$q', 'uiGridConstants', 'uiGridCustomConfig', 'SweetAlert'];


    ng.module('settingsSystem')
      .controller('SettingsSystemLocationCityCtrl', SettingsSystemLocationCityCtrl);

})(window.angular);