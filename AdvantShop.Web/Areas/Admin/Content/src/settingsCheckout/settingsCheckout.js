; (function (ng) {
    'use strict';

    var SettingsCheckoutCtrl = function ($uibModal, $http, $q, uiGridConstants, uiGridCustomConfig, SweetAlert, toaster) {
        var ctrl = this;
        
        // #region Taxes
            var columnDefsTaxes = [
            {
                name: 'Name',
                displayName: 'Наименование',
                cellTemplate: '<div class="ui-grid-cell-contents"><span class="link">{{COL_FIELD}}</span></div>',
                filter: {
                    placeholder: 'Наименование',
                    type: uiGridConstants.filter.INPUT,
                    name: 'Name',
                }
            },
            {
                name: 'IsDefault',
                displayName: 'Налог по умолчанию',
                enableCellEdit: false,
                enableSorting: false,
                cellTemplate:
                    '<ui-grid-custom-switch row="row" field-name="IsDefault" class="js-grid-not-clicked"></ui-grid-custom-switch>',
                width: 100,
            },
            {
                name: 'Enabled',
                displayName: 'Активно',
                enableCellEdit: false,
                cellTemplate:
                    '<ui-grid-custom-switch row="row" field-name="Enabled" class="js-grid-not-clicked"></ui-grid-custom-switch>',
                width: 100,
                filter: {
                    placeholder: 'Активность',
                    name: 'Enabled',
                    type: uiGridConstants.filter.SELECT,
                    selectOptions: [{ label: 'Активные', value: true }, { label: 'Неактивные', value: false }]
                }
            },
            {
                name: 'TaxTypeFormatted',
                displayName: 'Тип',
                enableCellEdit: false,
                width: 200
            },
            {
                name: 'Rate',
                displayName: 'Ставка',
                width: 100,
            },
            {
                name: '_serviceColumn',
                displayName: '',
                enableSorting: false,
                width: 80,
                cellTemplate:
                    '<div class="ui-grid-cell-contents js-grid-not-clicked"><div>' +
                    '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil" ng-click="grid.appScope.$ctrl.gridExtendCtrl.loadTax(row.entity.TaxId)"></a> ' +
                    '<a href="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.deleteTax(row.entity.CanBeDeleted, row.entity.TaxId)" ng-class="(!row.entity.CanBeDeleted ? \'ui-grid-custom-service-icon fa fa-times link-disabled\' : \'ui-grid-custom-service-icon fa fa-times link-invert\')"></a> ' +
                    '</div></div>'
            }
        ];

        ctrl.gridTaxesOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefsTaxes,
            uiGridCustom: {
                rowClick: function ($event, row) {
                    ctrl.loadTax(row.entity.TaxId);
                },
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'settingsCheckout/deleteItems',
                        field: 'TaxId',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    }
                ]
            }
        });

        ctrl.gridTaxesOnInit = function (grid) {
            ctrl.gridTaxes = grid;
        };

        ctrl.gridTaxesUpdate = function() {
            ctrl.gridTaxes.fetchData();
        }

        ctrl.loadTax = function (id) {
            $uibModal.open({
                bindToController: true,
                controller: 'ModalAddEditTaxCtrl',
                controllerAs: 'ctrl',
                templateUrl: '../areas/admin/content/src/settingsCheckout/modal/addEditTax/AddEditTax.html',
                resolve: {
                    id: function () {
                        return id;
                    }
                }
            }).result.then(function (result) {
                ctrl.gridTaxes.fetchData();
                return result;
            }, function (result) {
                return result;
            });
        };

        ctrl.deleteTax = function (canBeDeleted, taxId) {
            if (canBeDeleted) {
                SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                    if (result === true) {
                        $http.post('settingsCheckout/deletetax', { 'id': taxId }).then(function (response) {
                            ctrl.gridTaxes.fetchData();
                        });
                    }
                });
            } else {
                SweetAlert.alert("Налог используется в товарах или его нельзя удалять", { title: "Удаление невозможно" });
            }
        }

        // #endregion

        ctrl.changeOrderId = function(orderId) {
            $http.post('settingsCheckout/changeOrderId', { orderId: orderId }).then(function (response) {
                var data = response.data;
               if (data.result === true) {
                   toaster.pop('success', '', 'Номер заказа изменен');
               } else {
                   data.errors.forEach(function(error) {
                       toaster.pop('error', '', error);
                   });
               }
            });
        }
    };

    SettingsCheckoutCtrl.$inject = ['$uibModal', '$http', '$q', 'uiGridConstants', 'uiGridCustomConfig', 'SweetAlert', 'toaster'];

    ng.module('settingsCheckout', [])
      .controller('SettingsCheckoutCtrl', SettingsCheckoutCtrl);

})(window.angular);