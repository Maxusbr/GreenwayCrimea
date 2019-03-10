; (function (ng) {
    'use strict';

    var SettingsCatalogCtrl = function ($uibModal, $http, $q, uiGridConstants, uiGridCustomConfig, SweetAlert, toaster) {
        var ctrl = this;

        // #region Currencies
        var columnDefsCurrencies = [
            {
                name: 'Name',
                displayName: 'Наименование',
                filter: {
                    placeholder: 'Наименование',
                    type: uiGridConstants.filter.INPUT,
                    name: 'Name',
                },
                enableCellEdit: true,
            },
            {
                name: 'Symbol',
                displayName: 'Символ',
                enableCellEdit: true,
                width: 80,
            },
            {
                name: 'Rate',
                displayName: 'Значение',
                type: 'number',
                enableCellEdit: true,
                width: 100,
            },
            {
                name: 'Iso3',
                displayName: 'Код ISO3',
                enableCellEdit: true,
                filter: {
                    placeholder: 'Код ISO3',
                    type: uiGridConstants.filter.INPUT,
                    name: 'Iso3',
                },
                uiGridCustomEdit: {
                    attributes: { maxlength: 3 }
                },
                width: 80,
            },
            {
                name: 'NumIso3',
                displayName: 'Числовой код ISO3',
                type: 'number',
                enableCellEdit: true,
                uiGridCustomEdit: {
                    attributes: { max: 999 }
                },
                width: 80,
            },
            {
                name: 'IsCodeBefore',
                displayName: 'Символ впереди',
                type: 'checkbox',
                enableCellEdit: true,
                width: 80,
            },
            {
                name: 'RoundNumbers',
                displayName: 'Порядок огругления',
                enableCellEdit: true,
                width: 220,
                type: 'select',
                uiGridCustomEdit: {
                    editDropdownOptionsArray: [
                        { label: 'Не округлять', value: -1 },
                        { label: 'Округлять до копеек', value: 0.01 },
                        { label: 'Округлять до целого', value: 1 },
                        { label: 'Округлять до десятков', value: 10 },
                        { label: 'Округлять до сотен', value: 100 },
                        { label: 'Округлять до тысяч', value: 1000 }
                    ]
                }
            },
            {
                name: '_serviceColumn',
                displayName: '',
                width: 80,
                cellTemplate:
                    '<div class="ui-grid-cell-contents js-grid-not-clicked"><div>' +
                    '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil" ng-click="alert(1);grid.appScope.$ctrl.gridExtendCtrl.loadCurrency(row.entity.CurrencyId)"></a> ' +
                    '<ui-grid-custom-delete url="settingsCatalog/deleteCurrency" params="{\'Id\': row.entity.CurrencyId}"></ui-grid-custom-delete>' +
                    '</div></div>'
            }
        ];

        ctrl.gridCurrenciesOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefsCurrencies,
            uiGridCustom: {
                rowClick: function ($event, row) {
                    ctrl.loadCurrency(row.entity.CurrencyId);
                },
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'settingsCatalog/deleteItems',
                        field: 'CurrencyId',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    }
                ]
            }
        });

        ctrl.gridCurrenciesOnInit = function (grid) {
            ctrl.gridCurrencies = grid;
        };

        ctrl.loadCurrency = function (id) {
            $uibModal.open({
                bindToController: true,
                controller: 'ModalAddEditCurrencyCtrl',
                controllerAs: 'ctrl',
                templateUrl: '../areas/admin/content/src/settingsCatalog/modal/addEditCurrency/AddEditCurrency.html',
                resolve: {
                    id: function () {
                        return id;
                    }
                }
            }).result.then(function (result) {
                ctrl.gridCurrencies.fetchData();
                return result;
            }, function (result) {
                return result;
            });
        };
        // #endregion
        ctrl.updateCb = function () {
            var url = "settingsCatalog/updateCb";
            $http.post(url).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop("success", "", "Курсы валют обновлены");
                    ctrl.gridCurrencies.fetchData();
                } else {
                    toaster.pop("error", "Ошибка", "Ошибка при обновлении валют");
                    ctrl.btnSleep = false;
                }
            });
        }


        ctrl.reindex = function ()
        {
            
            toaster.pop('success', '', 'Обновление индекса запущено');

            $http.post('settingsCatalog/ReindexLucene').then(function (response) {
                if (response.data.result === true) {
                    toaster.pop('success', '', 'Обновление индекса завершено');
                }
                else {
                    toaster.pop('error', 'Ошибка при обновлении индекса', "");
                }
            });
        }
    };

    SettingsCatalogCtrl.$inject = ['$uibModal', '$http', '$q', 'uiGridConstants', 'uiGridCustomConfig', 'SweetAlert', 'toaster'];

    ng.module('settingsCatalog', [])
      .controller('SettingsCatalogCtrl', SettingsCatalogCtrl);

})(window.angular);